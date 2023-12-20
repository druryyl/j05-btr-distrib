namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormExcelDto
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        public string Satuan1 { get; set; }
        public decimal Hpp1 { get; set; }
        public decimal HrgJual1Gt { get; set; }
        public decimal HrgJual1Mt { get; set; }
        
        public string Satuan2 { get; set; }
        public decimal Hpp2 => Hpp1 * Conversion;
        public decimal HrgJual2Gt => HrgJual1Gt * Conversion;
        public decimal HrgJual2Mt => HrgJual1Mt * Conversion;
        public int Conversion { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public bool Aktif { get; set; }
        
        
    }
}