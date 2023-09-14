namespace btr.domain.InventoryContext.DriverAgg
{
    public class DriverModel : IDriverKey
    {
        public DriverModel(string id) => DriverId = id;
        public DriverModel()
        {
        }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
    }
}
