namespace btr.distrib.InventoryContext.StokBalanceRpt
{
    public class StokBalanceInfoDto
    {
        public string Supplier { get; set; }
        public string Kategori { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string Warehouse{ get; set; }
        public int QtyBesar { get => Conversion == 1 ? 0: InPcs / Conversion; }
        public string SatBesar { get; set; }
        public int Conversion { get; set; }
        public int QtyKecil { get => Conversion ==1 ? InPcs: InPcs % Conversion;  }
        public string SatKecil { get; set; }
        public int InPcs { get; set; }
        public decimal Hpp { get; set; }
        public decimal NilaiSediaan { get; set; }
    }
}
