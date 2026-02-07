namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class LocationReff
    {
        public LocationReff(double latitude, double longitude, double accuracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }

        public static LocationReff Default => new LocationReff(
            0, 0, 0);

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double Accuracy { get; private set; }
    }
}

