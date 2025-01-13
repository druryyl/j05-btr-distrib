using btr.domain.BrgContext.BrgAgg;

namespace btr.application.InventoryContext.StokPeriodikInfo
{
    public class StokPeriodikDto : IBrgKey
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }

        public string KategoriId { get; set; }
        public string KategoriName { get; set; }

        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }

        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public int Qty { get; set; }
        public decimal Hpp { get; set; }
        public decimal NilaiSediaan { get; set; }
    }
}