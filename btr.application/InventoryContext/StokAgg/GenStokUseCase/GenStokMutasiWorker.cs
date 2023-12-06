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
            else
                GenMutasiMasuk(req);
        }

        private void GenMutasiKeluar(GenStokMutasiRequest req)
        {
            //  nnt buatkan balikannya dulu
            //      void mutasi keluar-nya
            var rollbackReq = new RollBackStokRequest(req.MutasiId);
            _rollBackStok.Execute(rollbackReq);
            //      void mutasi masuk-nya
            var removeRollbackReq = new RemoveRollbackRequest(req.MutasiId, "MUTASI-MASUK-VOID");
            _removeRollbackStokWorker.Execute(removeRollbackReq);
            
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
                    mutasi.Keterangan));

                _addStok.Execute(new AddStokRequest(
                    item.BrgId, 
                    "IWS", 
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-MASUK", mutasi.Keterangan));
            }
        }
        private void GenMutasiMasuk(GenStokMutasiRequest req)
        {
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
                    "MUTASI-KELUAR", mutasi.Keterangan));
                
                _addStok.Execute(new AddStokRequest(
                    item.BrgId, 
                    mutasi.WarehouseId, 
                    item.Qty, 
                    item.Sat, 
                    item.Hpp, 
                    req.MutasiId,
                    "MUTASI-MASUK", mutasi.Keterangan));
            }
        }
    }
}