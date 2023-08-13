using System;

namespace btr.domain.InventoryContext.TerimaBrgAgg
{
    public class TerimaBrgModel : ITerimaBrgKey
    {
        public string TerimaBrgId { get; set; }
        public DateTime TerimaBrgDate { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }

    public class TerimaBrgItemModel
    {
        public string TerimaBrgId { get; set; }
        public string TerimaBrgItemId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal  HargaSatuan { get; set; }
    }
}