namespace btr.distrib.InventoryContext.WarehouseAgg
{
    public class WarehouseFormGridDto
    {
        public WarehouseFormGridDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }
        public string Name { get; }
    }
}
