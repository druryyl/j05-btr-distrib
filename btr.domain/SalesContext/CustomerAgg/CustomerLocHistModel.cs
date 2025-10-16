using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.CustomerAgg
{
    public class CustomerLocHistModel : ICustomerLocHistKey
    {
        public CustomerLocHistModel(string id, string custId, DateTime changeDate, 
            double lat,  double lng, double accuracy, string changeUser)
        {
            LocHistId = id;
            CustomerId = custId;
            ChangeDate = changeDate;
            Latitude = lat;
            Longitude = lng;
            Accuracy = accuracy;
            ChangeUser = changeUser;
        }
        public static CustomerLocHistModel Create(string custId, double lat, double lng, double accuracy, string changeUser)
        {
            var newId = Ulid.NewUlid().ToString();
            var result = new CustomerLocHistModel(newId, custId, DateTime.Now, lat, lng, accuracy, changeUser);
            return result;
        }
        public static ICustomerLocHistKey Key(string id)
        {
            var result = new CustomerLocHistModel(id, "-", new DateTime(3000, 1, 1), 0, 0, 0, "-");
            return result;
        }

        public string LocHistId { get; private set; }
        public string CustomerId { get; private set;  }
        public DateTime ChangeDate { get; private set;  }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double Accuracy { get; private set; }
        public string ChangeUser { get; private set; }

    }

    public interface ICustomerLocHistKey
    {
        string LocHistId { get; }
    }
}
