namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormBrgDto
    {
        public BrgFormBrgDto(string id, string code, string name, string cat, string supplier,
            decimal hpp1, decimal hargaGt1, decimal hargaMt1, decimal hpp2, decimal hargaGt2, decimal hargaMt2) 
            => (Id, Code, BrgName, Kategori, SupplierName,Hpp1, HargaGt1, HargaMt1, Hpp2, HargaGt2, HargaMt2) 
             = (id, code, name, cat, supplier, hpp1, hargaGt1, hargaMt1, hpp2, hargaGt2, hargaMt2);
        
        public string Id { get; private set; }
        public string Code { get; private set; }
        public string BrgName { get; private set; }
        public string Kategori { get; private set; }
        public string SupplierName { get; set; }

        public decimal Hpp1 { get; set; }
        public decimal HargaGt1 { get; set; }
        public decimal HargaMt1 { get; set; }
        public decimal Hpp2 { get; set; }
        public decimal HargaGt2 { get; set; }
        public decimal HargaMt2 { get; set; }
    }
}