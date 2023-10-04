using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RemoveRollbackRequest : IReffKey
    {
        public RemoveRollbackRequest(string id, string jenisMutasi)
        {
            ReffId = id;
            JenisMutasi = jenisMutasi;
        }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
    }

    public interface IRemoveRollbackStokWorker : INunaServiceVoid<RemoveRollbackRequest>
    {
    }
    
    public class RemoveRollbackStokWorker : IRemoveRollbackStokWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;

        public RemoveRollbackStokWorker(IStokDal stokDal, 
            IStokMutasiDal stokMutasiDal, 
            IRemovePriorityStokWorker removePriorityStokWorker)
        {
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _removePriorityStokWorker = removePriorityStokWorker;
        }

        public void Execute(RemoveRollbackRequest request)
        {
            var listStok = _stokDal.ListData(new StokModel { ReffId = request.ReffId });
            if (listStok == null) 
                return;

            foreach(var item in listStok)
            {
                //  jika belum ada barang keluar, maka hapus
                if (item.Qty == item.QtyIn)
                {
                    _stokDal.Delete(item);
                    _stokMutasiDal.Delete(item);
                    continue;
                }

                //  jika sudah ada barang keluar, maka remove priority;
                var req = new RemovePriorityStokRequest(
                    item.BrgId, item.WarehouseId, item.Qty, string.Empty, 
                    item.NilaiPersediaan, request.ReffId, request.JenisMutasi, request.ReffId);
                _removePriorityStokWorker.Execute(req);
            }
        }

    }
    
}