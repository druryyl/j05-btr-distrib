namespace btr.domain.InventoryContext.WarehouseAgg
{
    public class WarehouseModel : IWarehouseKey
    {
        public WarehouseModel()
        {
        }

        public WarehouseModel(string id) => WarehouseId = id;
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public bool IsSpecial { get; set; }
    }
}