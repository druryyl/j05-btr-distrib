namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormBrgDto
    {
        public BrgFormBrgDto(string id, string code, string name, string cat, string supplier) 
            => (Id, Code, BrgName, Kategori, SupplierName) = (id, code, name, cat, supplier);
        
        public string Id { get; private set; }
        public string Code { get; private set; }
        public string BrgName { get; private set; }
        public string Kategori { get; private set; }
        public string SupplierName { get; set; }
    }
}