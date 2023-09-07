using System.Collections.Generic;
using System.Linq;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RollBackStokRequest : IReffKey
    {
        public RollBackStokRequest(string reffId)
        {
            ReffId = reffId;
        }

        public string ReffId { get; set; }
    }

    public interface IRollBackStokWorker : INunaServiceVoid<RollBackStokRequest>
    {
    }

    public class RollBackStokWorker : IRollBackStokWorker
    {
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RollBackStokWorker(IStokMutasiDal stokMutasiDal,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter,
            IGenStokBalanceWorker genStokBalanceWorker)
        {
            _stokMutasiDal = stokMutasiDal;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
            _stokBalanceWorker = genStokBalanceWorker;
        }

        public void Execute(RollBackStokRequest request)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.ReffId, y => y.NotEmpty());

            //  BUILD
            var listStokMutasi = _stokMutasiDal.ListData(request)?.ToList();
            if (listStokMutasi is null)
                return;

            //      rollback hanya untuk pembatalan stok keluar
            listStokMutasi.RemoveAll(x => x.QtyOut == 0);
            var result = new List<StokModel>();
            var resultBalance = new List<GenStokBalanceRequest>();
            
            foreach(var item in listStokMutasi)
            {
                var stok = _stokBuilder.Load(item).Build();
                //  stok-controler
                stok = _stokBuilder
                    .Attach(stok)
                    .RollBack(request)
                    .Build();
                result.Add(stok);
                
                //  stok-balancer
                var stokBalance = new GenStokBalanceRequest(stok.BrgId, stok.WarehouseId);
                resultBalance.Add(stokBalance);
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                foreach(var item in result)
                    _ = _stokWriter.Save(item);
                
                foreach(var item in resultBalance)
                    _stokBalanceWorker.Execute(item);

                trans.Complete();
            }
        }
    }
}
