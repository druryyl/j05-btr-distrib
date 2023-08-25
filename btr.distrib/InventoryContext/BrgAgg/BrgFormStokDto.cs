namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormStokDto
    {
        public BrgFormStokDto(string id, string name, int qty) 
            => (Id, WarehouseName, Qty) = (id, name, qty);

        public BrgFormStokDto()
        {
        }
        public string Id { get; private set; }
        public string WarehouseName { get; private set; }
        public int Qty { get; private set; }
        public void SetQty(int qty) => Qty = qty;
    }
}