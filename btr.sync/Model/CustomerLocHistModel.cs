using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class CustomerLocHistModel 
    {
        public CustomerLocHistModel(string id, string custId, DateTime changeDate,
            double lat, double lng, double accuracy, string changeUser)
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

        public static CustomerLocHistModel Create(CustomerType customer)
        {
            var result = Create(customer.CustomerId, customer.Latitude, customer.Longitude, customer.Accuracy, customer.CoordinateUser);
            return result;
        }

        public string LocHistId { get; private set; }
        public string CustomerId { get; private set; }
        public DateTime ChangeDate { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double Accuracy { get; private set; }
        public string ChangeUser { get; private set; }

    }
}
