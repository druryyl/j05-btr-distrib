namespace btr.distrib.InventoryContext.PackingAgg
{
    internal class Packing2SupplierDto
    {
        public Packing2SupplierDto(string id, string name, int jumItem)
        {
            SupplierId = id;
            SupplierName = name;
            JumItem = jumItem;
        }
        public string SupplierId { get; private set; }  
        public string SupplierName { get; private set; }
        public int JumItem { get; private set; }
    }
}