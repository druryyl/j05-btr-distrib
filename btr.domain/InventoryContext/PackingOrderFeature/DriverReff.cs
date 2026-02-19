using btr.domain.InventoryContext.DriverAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class DriverReff : IDriverKey
    {
        public DriverReff(string driverId, string driverName)
        {
            DriverId = driverId;
            DriverName = driverName;
        }

        public static DriverReff Default => new DriverReff("-", "-");

        public static IDriverKey Key(string id)
        {
            return new DriverReff(id, "-");
        }

        public string DriverId { get; private set; }
        public string DriverName { get; private set; }
    }

}
