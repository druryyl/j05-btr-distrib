namespace btr.application.InventoryContext.StokAgg.UseCases
{
    public class RemoveStokCommand
    {
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
    }
}