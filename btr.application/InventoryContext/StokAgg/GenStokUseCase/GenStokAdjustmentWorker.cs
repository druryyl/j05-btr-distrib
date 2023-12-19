using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.AdjustmentAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.AdjustmentAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokAdjustmentRequest : IAdjustmentKey
    {
        public GenStokAdjustmentRequest(string id)
        {
            AdjustmentId = id;
        }

        public string AdjustmentId { get; set; }
    }

    public class GenStokAdjustmentResult
    {
        public int QtyPcsAwal { get; set; }
        public int QtyPcsAkhir { get; set; }
        public int QtyPcsAdjust { get; set; }
    }
    
    public interface IGenStokAdjustmentWorker : INunaService<GenStokAdjustmentResult, GenStokAdjustmentRequest>
    {
    }
    
    public class GenStokAdjustmentWorker : IGenStokAdjustmentWorker
    {
        private readonly IAdjustmentBuilder _adjustmentBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;

        public GenStokAdjustmentWorker(IAdjustmentBuilder adjustmentBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker, 
            IRollBackStokWorker rollBackStokWorker, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal)
        {
            _adjustmentBuilder = adjustmentBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
            _rollBackStokWorker = rollBackStokWorker;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
        }

        public GenStokAdjustmentResult Execute(GenStokAdjustmentRequest req)
        {
            var adjustment = _adjustmentBuilder.Load(req).Build();
            using (var trans = TransHelper.NewScope())
            {
                RollBackAdjustment(adjustment);
                var qty = GetCurrentStok(adjustment);
                var qtyAdjust = adjustment.QtyAdjustInPcs;
                var brg = _brgBuilder.Load(adjustment).Build();
                var satuanKecil = brg.ListSatuan?.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
                var nilaiPersediaan = brg.Hpp;
                var ket = adjustment.Alasan.Length > 120 ? adjustment.Alasan.Substring(0, 120) : adjustment.Alasan;
                var ketarangn = $"Adjust: {ket}";
                if (qtyAdjust > 0)
                {
                    var cmd = new AddStokRequest(adjustment.BrgId, adjustment.WarehouseId,
                        qtyAdjust, satuanKecil, nilaiPersediaan, adjustment.AdjustmentId, "ADJUST",ketarangn);
                    _addStokWorker.Execute(cmd);
                }
                else
                {
                    var cmd = new RemoveFifoStokRequest(adjustment.BrgId, adjustment.WarehouseId,
                        -qtyAdjust, satuanKecil, 0, adjustment.AdjustmentId, "ADJUST", ketarangn);
                    _removeFifoStokWorker.Execute(cmd);
                }
                trans.Complete();

                var result = new GenStokAdjustmentResult
                {
                    QtyPcsAwal = qty,
                    QtyPcsAdjust = qtyAdjust,
                    QtyPcsAkhir = qty + qtyAdjust
                };
                return result;
            }
            
        }
        private int GetCurrentStok(AdjustmentModel Adjustment)
        {
            var listStok = _stokBalanceWarehouseDal.ListData((IBrgKey)Adjustment)?.ToList()
                           ?? new List<StokBalanceWarehouseModel>();
            var result = listStok.FirstOrDefault(x => x.WarehouseId == Adjustment.WarehouseId)?.Qty ?? 0;
            return result;
        }

        private void RollBackAdjustment(IAdjustmentKey adjustment)
        {
            var req = new RollBackStokRequest(adjustment.AdjustmentId);
            _rollBackStokWorker.Execute(req);
        }
    }
}