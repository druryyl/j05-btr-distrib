namespace btr.domain.InventoryContext.KartuStokRpt
{
    public class KartuStokSaldoView
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string KategoriId { get; set; }
        public string KategoriName { get; set; }
        public int QtySaldo { get; set; }
        public decimal NilaiSediaanSaldo { get; set; }
    }
}
