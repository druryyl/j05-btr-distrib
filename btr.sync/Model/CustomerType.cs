using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class CustomerType
    {
        public CustomerType()
        {
        }
        public CustomerType(string customerId, string customerCode, string customerName, 
            string alamat, string wilayah, double latitude, double longitude, double accuracy, 
            DateTime coordinateTimeStamp, string coordinateUser, string serverId)
        {
            CustomerId = customerId;
            CustomerCode = customerCode;
            CustomerName = customerName;
            Alamat = alamat;
            Wilayah = wilayah;
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
            CoordinateTimeStamp = coordinateTimeStamp;
            CoordinateUser = coordinateUser;
            ServerId = serverId;
        }

        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public string Wilayah { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public DateTime CoordinateTimeStamp { get; set; }
        public string CoordinateUser { get; set;  }
        public string ServerId { get; set; }
    }
}
