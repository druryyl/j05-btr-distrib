using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class CheckInModel : ICheckInKey
    {
        public CheckInModel()
        {
        }

        public CheckInModel(
            string checkInId,
            string checkInDate,        // yyyy-MM-dd
            string checkInTime,        // HH:mm:ss
            string userEmail,
            double checkInLatitude,
            double checkInLongitude,
            float accuracy,
            string customerId,
            string customerCode,
            string customerName,
            string customerAddress,
            double customerLatitude,
            double customerLongitude,
            string statusSync = "DRAFT")
        {
            CheckInId = checkInId;
            CheckInDate = checkInDate;
            CheckInTime = checkInTime;
            UserEmail = userEmail;
            CheckInLatitude = checkInLatitude;
            CheckInLongitude = checkInLongitude;
            Accuracy = accuracy;
            CustomerId = customerId;
            CustomerCode = customerCode;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerLatitude = customerLatitude;
            CustomerLongitude = customerLongitude;
            StatusSync = statusSync;
        }

        public string CheckInId { get; set; }
        public string CheckInDate { get; set; }        // yyyy-MM-dd
        public string CheckInTime { get; set; }        // HH:mm:ss
        public string UserEmail { get; set; }
        public double CheckInLatitude { get; set; }
        public double CheckInLongitude { get; set; }
        public float Accuracy { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double CustomerLatitude { get; set; }
        public double CustomerLongitude { get; set; }
        public string StatusSync { get; set; }

        public static ICheckInKey Key(string id) => new CheckInModel(
            id, "", "", "", 0, 0, 0, "", "", "", "", 0, 0, "DRAFT");
    }

    public interface ICheckInKey
    {
        string CheckInId { get; }
    }
}
