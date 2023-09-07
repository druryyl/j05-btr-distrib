using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.InventoryContext.StokAgg.UseCases
{
    public class RollBackStokRequest : IBrgKey, IReffKey, IWarehouseKey
    {
        public RollBackStokRequest(string brgId, string warehouseId,
            string reffId)
        {
            BrgId = brgId;
            WarehouseId = warehouseId;
            ReffId = reffId;
        }

        public string BrgId { get; set; }
        public string ReffId { get; set; }
        public string WarehouseId { get; set; }
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
            foreach(var item in listStokMutasi)
            {
                var stok = _stokBuilder.Load(item).Build();
                if (stok.BrgId != request.BrgId)
                    continue;
                if (stok.WarehouseId != request.WarehouseId)
                    continue;

                stok = _stokBuilder
                    .Attach(stok)
                    .RollBack(request)
                    .Build();
                result.Add(stok);
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                foreach(var item in result)
                    _ = _stokWriter.Save(item);
                
                var stokBalanceReq = new GenStokBalanceRequest(request.BrgId, request.WarehouseId);
                _stokBalanceWorker.Execute(stokBalanceReq);

                trans.Complete();
            }
        }
    }
}
