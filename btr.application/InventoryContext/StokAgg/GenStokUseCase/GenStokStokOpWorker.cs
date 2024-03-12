using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokStokOpRequest : IStokOpKey
    {
        public GenStokStokOpRequest(string id)
        {
            StokOpId = id;
        }
        public string StokOpId { get; set; }
    }

    public class GenStokStokOpResult
    {
        public int QtyPcsAwal { get; set; }
        public int QtyPcsAkhir { get; set; }
        public int QtyPcsAdjust { get; set; }
    }
    
    public interface IGenStokStokOpWorker : INunaService<GenStokStokOpResult, GenStokStokOpRequest>
    {
    }
    
    public class GenStokStokOpWorker : IGenStokStokOpWorker
    {
        private readonly IStokOpBuilder _stokOpBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;

        public GenStokStokOpWorker(IStokOpBuilder stokOpBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker,
            IRollBackStokWorker rollBackStokWorker,
            IStokBalanceWarehouseDal stokBalanceWarehouseDal,
            IRemovePriorityStokWorker removePriorityStokWorker)
        {
            _stokOpBuilder = stokOpBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
            _rollBackStokWorker = rollBackStokWorker;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _removePriorityStokWorker = removePriorityStokWorker;
        }

        public GenStokStokOpResult Execute(GenStokStokOpRequest req)
        {
            var stokOp = _stokOpBuilder.Load(req).Build();
            var qty = GetCurrentStok(stokOp);
            if (stokOp.QtyPcsAdjust == 0)
                return new GenStokStokOpResult
                {
                    QtyPcsAwal = qty,
                    QtyPcsAdjust = 0,
                    QtyPcsAkhir = qty
                };
            
            using (var trans = TransHelper.NewScope())
            {
                //  gen stok hanya dilakukan jika ada qtyAdjustment saja
                var qtyAdjust = CalcAdjustment(stokOp, qty);
                var brg = _brgBuilder.Load(stokOp).Build();
                var satuanKecil = brg.ListSatuan?.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
                var nilaiPersediaan = brg.Hpp;
                var ketarangn = $"Stok-Op: {stokOp.PeriodeOp:ddd, dd MMM yyyy}";
                if (qtyAdjust > 0)
                {
                    var cmd = new AddStokRequest(stokOp.BrgId, stokOp.WarehouseId,
                        qtyAdjust, satuanKecil, nilaiPersediaan, stokOp.StokOpId, "STOKOP",ketarangn, stokOp.StokOpDate);
                    _addStokWorker.Execute(cmd);
                }
                else
                {
                    var cmd = new RemoveFifoStokRequest(stokOp.BrgId, stokOp.WarehouseId,
                        -qtyAdjust, satuanKecil, 0, stokOp.StokOpId, "STOKOP", ketarangn, stokOp.StokOpDate);
                    _removeFifoStokWorker.Execute(cmd);
                }
                var result = new GenStokStokOpResult
                {
                    QtyPcsAwal = qty,
                    QtyPcsAdjust = qtyAdjust,
                    QtyPcsAkhir = qty + qtyAdjust
                };

                trans.Complete();
                return result;
            }
            
        }
        private static int CalcAdjustment(StokOpModel stokOp, int qtyAwal)
        {
            var qtyAkhir = stokOp.QtyPcsOpname;
            var result = qtyAkhir - qtyAwal;
            return result;
        }

        private int GetCurrentStok(StokOpModel stokOp)
        {
            var listStok = _stokBalanceWarehouseDal.ListData((IBrgKey)stokOp)?.ToList()
                           ?? new List<StokBalanceWarehouseModel>();
            var result = listStok.FirstOrDefault(x => x.WarehouseId == stokOp.WarehouseId)?.Qty ?? 0;
            return result;
        }
    }
}
