using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.nuna.Application;
using JetBrains.Annotations;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    [PublicAPI]
    public class GenStokMutasiRequest : IMutasiKey
    {
        public GenStokMutasiRequest(string id) => MutasiId = id;
        public string MutasiId { get; set; }
    }

    public interface IGenStokMutasiWorker : INunaServiceVoid<GenStokMutasiRequest>
    {
        void ExecuteVoid(GenStokMutasiRequest req);
    }

    public class GenStokMutasiWorker : IGenStokMutasiWorker
    {
        private readonly IMutasiBuilder _mutasiBuilder;
        private readonly IRemoveFifoStokWorker _removeFifoStok;
        private readonly IAddStokWorker _addStok;
        private readonly IRollBackStokWorker _rollBackStok;
        private readonly IRemoveRollbackStokWorker _removeRollbackStokWorker;
        public GenStokMutasiWorker(IMutasiBuilder mutasiBuilder, 
            IRemoveFifoStokWorker removeFifoStok, 
            IAddStokWorker addStok, 
            IRollBackStokWorker rollBackStok, 
            IRemoveRollbackStokWorker removeRollbackStokWorker)
        {
            _mutasiBuilder = mutasiBuilder;
            _removeFifoStok = removeFifoStok;
            _addStok = addStok;
            _rollBackStok = rollBackStok;
            _removeRollbackStokWorker = removeRollbackStokWorker;
        }

        public void Execute(GenStokMutasiRequest req)
        {
            var mutasi = _mutasiBuilder.Load(req).Build();
            if (mutasi.JenisMutasi == JenisMutasiEnum.MutasiKeluar)
                GenMutasiKeluar(req);
            else if (mutasi.JenisMutasi == JenisMutasiEnum.MutasiMasuk)
                GenMutasiMasuk(req);
            else
                GenMutasiKlaimSupplier(req);
        }

        public void ExecuteVoid(GenStokMutasiRequest req)
        {
            var mutasi = _mutasiBuilder.Load(req).Build();
            if (mutasi.JenisMutasi == JenisMutasiEnum.MutasiKeluar)
                GenMutasiKeluarVoid(req);
            else if (mutasi.JenisMutasi == JenisMutasiEnum.MutasiMasuk)
                GenMutasiMasukVoid(req);
            else
                GenMutasiKlaimSupplierVoid(req);
        }

        private void GenMutasiKeluar(GenStokMutasiRequest req)
        {
            GenMutasiKeluarVoid(req);

            var mutasi = _mutasiBuilder.Load(req).Build();
            foreach (var item in mutasi.ListItem)
            {
                _removeFifoStok.Execute(new RemoveFifoStokRequest(
                    item.BrgId, 
                    mutasi.WarehouseId,  
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-KELUAR",
                    mutasi.Keterangan,
                    mutasi.MutasiDate));

                _addStok.Execute(new AddStokRequest(
                    item.BrgId, 
                    "IWS", 
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-MASUK", mutasi.Keterangan, mutasi.MutasiDate));
            }
        }

        private void GenMutasiKeluarVoid(GenStokMutasiRequest req)
        {
            var mutasiOri = _mutasiBuilder.Load(req).Build();
            //      void mutasi keluar-nya
            var rollbackReq = new RollBackStokRequest(req.MutasiId);
            _rollBackStok.Execute(rollbackReq);
            //      void mutasi masuk-nya
            var removeRollbackReq = new RemoveRollbackRequest(req.MutasiId, "MUTASI-KELUAR-VOID", mutasiOri.MutasiDate);
            _removeRollbackStokWorker.Execute(removeRollbackReq);
        }

        private void GenMutasiKlaimSupplier(GenStokMutasiRequest req)
        {
            GenMutasiKlaimSupplierVoid(req);

            var mutasi = _mutasiBuilder.Load(req).Build();
            foreach (var item in mutasi.ListItem)
            {
                _removeFifoStok.Execute(new RemoveFifoStokRequest(
                    item.BrgId,
                    mutasi.WarehouseId,
                    item.Qty,
                    item.Sat,
                    item.Hpp,
                    req.MutasiId,
                    "MUTASI-KLAIM",
                    mutasi.Keterangan,
                    mutasi.MutasiDate));
            }
        }
        
        private void GenMutasiKlaimSupplierVoid(GenStokMutasiRequest req)
        {
            var rollbackReq = new RollBackStokRequest(req.MutasiId);
            _rollBackStok.Execute(rollbackReq);
        }

        private void GenMutasiMasuk(GenStokMutasiRequest req)
        {
            //  rollback dulu mutasi masuk-nya
            GenMutasiMasukVoid(req);

            //  nnt buatkan balikannya dulu
            var mutasi = _mutasiBuilder.Load(req).Build();
            foreach (var item in mutasi.ListItem)
            {
                _removeFifoStok.Execute(new RemoveFifoStokRequest(
                    item.BrgId, 
                    "IWS", 
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-KELUAR", mutasi.Keterangan, mutasi.MutasiDate));
                
                _addStok.Execute(new AddStokRequest(
                    item.BrgId, 
                    mutasi.WarehouseId, 
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-MASUK", mutasi.Keterangan, mutasi.MutasiDate));
            }
        }

        private void GenMutasiMasukVoid(GenStokMutasiRequest req)
        {
            var mutasiOri = _mutasiBuilder.Load(req).Build();
            //      void mutasi masuk-nya
            var removeRollbackReq = new RemoveRollbackRequest(req.MutasiId, "MUTASI-MASUK-VOID", mutasiOri.MutasiDate);
            _removeRollbackStokWorker.Execute(removeRollbackReq);
            //      void mutasi keluar-nya
            var rollbackReq = new RollBackStokRequest(req.MutasiId);
            _rollBackStok.Execute(rollbackReq);
        }

    }
}