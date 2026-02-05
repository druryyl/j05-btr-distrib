namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class LocationReff
    {
        public LocationReff(decimal latitude, decimal longitude, int accuracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }

        public static LocationReff Default => new LocationReff(
            0, 0, 0);

        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public int Accuracy { get; private set; }
    }
}

