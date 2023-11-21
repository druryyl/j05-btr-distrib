namespace btr.application.InventoryContext.StokBalanceInfo
{
    public class StokBalanceView
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }

        public string KategoriId { get; set; }
        public string KategoriName { get; set; }

        public string BrgId{ get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }


        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int Conversion { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public int Qty { get; set; }
        public decimal Hpp { get; set; }
        public decimal NilaiSediaan { get; set; }
    }
}
