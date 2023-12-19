using System;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;

namespace btr.domain.InventoryContext.AdjustmentAgg
{
    public class AdjustmentModel : IAdjustmentKey, IWarehouseKey, IBrgKey
    {
        public string AdjustmentId { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string Alasan { get; set; }

        public int QtyAwalBesar { get; set; }
        public int QtyAwalKecil { get; set; }
        public int QtyAwalInPcs { get; set; }
        
        public int QtyAdjustBesar { get; set; }
        public int QtyAdjustKecil { get; set; }
        public int QtyAdjustInPcs { get; set; }
        
        public int QtyAkhirBesar { get; set; }
        public int QtyAkhirKecil { get; set; }
        public int QtyAkhirInPcs { get; set; }
    }
}