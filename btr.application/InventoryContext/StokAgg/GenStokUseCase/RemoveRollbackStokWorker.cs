using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RemoveRollbackRequest : IReffKey
    {
        public RemoveRollbackRequest(string id, string jenisMutasi, DateTime mutasiDate)
        {
            ReffId = id;
            JenisMutasi = jenisMutasi;
            MutasiDate = mutasiDate;
        }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
        public DateTime MutasiDate { get; }
    }

    public interface IRemoveRollbackStokWorker : INunaServiceVoid<RemoveRollbackRequest>
    {
    }
    
    public class RemoveRollbackStokWorker : IRemoveRollbackStokWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RemoveRollbackStokWorker(IStokDal stokDal,
            IStokMutasiDal stokMutasiDal,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IBrgBuilder brgBuilder,
            IGenStokBalanceWorker stokBalanceWorker)
        {
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _removePriorityStokWorker = removePriorityStokWorker;
            _brgBuilder = brgBuilder;
            _stokBalanceWorker = stokBalanceWorker;
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

                var brg = _brgBuilder.Load(new BrgModel(item.BrgId)).Build();
                var satKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan
                    ?? throw new KeyNotFoundException($"Satuan Kecil {item.BrgId} not found. Rollback Stock failed");

                //  jika sudah ada barang keluar, maka remove priority;
                var req = new RemovePriorityStokRequest(
                    item.BrgId, item.WarehouseId, item.Qty, satKecil, 
                    item.NilaiPersediaan, request.ReffId, request.JenisMutasi, 
                    request.ReffId, "Rollback", request.MutasiDate);
                if (req.Qty == 0)
                    continue;
                _removePriorityStokWorker.Execute(req);


            }
            foreach(var item in listStok.Select(x => new GenStokBalanceRequest(x.BrgId, x.WarehouseId)).Distinct())
            {
                _stokBalanceWorker.Execute(item);
            }
        }

    }
    
}