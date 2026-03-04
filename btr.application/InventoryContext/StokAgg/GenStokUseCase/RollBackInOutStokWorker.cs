using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using Dawn;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RollBackInOutStokRequest : IReffKey
    {
        public RollBackInOutStokRequest(string reffId)
        {
            ReffId = reffId;
        }
        public string ReffId { get; set; }
    }

    public interface IRollBackInOutStokWorker : INunaServiceVoid<RollBackInOutStokRequest>
    {
    }

    public class RollBackInOutStokWorker : IRollBackInOutStokWorker
    {
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RollBackInOutStokWorker(IStokMutasiDal stokMutasiDal,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter,
            IGenStokBalanceWorker genStokBalanceWorker)
        {
            _stokMutasiDal = stokMutasiDal;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
            _stokBalanceWorker = genStokBalanceWorker;
        }

        public void Execute(RollBackInOutStokRequest req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.ReffId, y => y.NotEmpty());

            //  BUILD
            var listStokMutasi = _stokMutasiDal.ListData(req)?.ToList();
            if (listStokMutasi is null)
                return;

            var result = new List<StokModel>();
            var resultBalance = new List<GenStokBalanceRequest>();

            foreach (var item in listStokMutasi)
            {
                var stok = _stokBuilder.Load(item).Build();
                //  stok-controler
                stok = _stokBuilder
                    .Attach(stok)
                    .RollBack(req)
                    .Build();
                result.Add(stok);

                //  stok-balancer
                var stokBalance = new GenStokBalanceRequest(stok.BrgId, stok.WarehouseId);
                resultBalance.Add(stokBalance);
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                foreach (var item in result)
                    _ = _stokWriter.Save(item);

                foreach (var item in resultBalance)
                    _stokBalanceWorker.Execute(item);

                trans.Complete();
            }

        }
    }
}
