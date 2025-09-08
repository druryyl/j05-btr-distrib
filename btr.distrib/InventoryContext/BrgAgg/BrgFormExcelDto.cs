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
        public decimal HrgJual31 { get; set; }
        public decimal HrgJual41 { get; set; }

        public string Satuan2 { get; set; }
        public decimal Hpp2 => Hpp1 * Conversion;
        public decimal HrgJual2Gt => HrgJual1Gt * Conversion;
        public decimal HrgJual2Mt => HrgJual1Mt * Conversion;
        public decimal HrgJual32 => HrgJual31 * Conversion;
        public decimal HrgJual42 => HrgJual41 * Conversion;
        public int Conversion { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public bool Aktif { get; set; }
        
        
    }
}