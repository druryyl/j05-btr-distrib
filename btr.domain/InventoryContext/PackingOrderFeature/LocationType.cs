namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class LocationType
    {
        public LocationType(decimal latitude, decimal longitude, int accuracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }

        public static LocationType Default => new LocationType(
            0, 0, 0);

        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public int Accuracy { get; private set; }
    }
}

