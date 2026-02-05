using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class OrderModel : IOrderKey
    {
        public OrderModel()
        {
        }
        public OrderModel(string orderId, string orderLocalCode,
            string customerId, string customerCode, string customerName, string customerAddress,
            string orderDate, string salesId, string salesName, decimal totalAmount,
            string userEmail, string statusSync, string fakturCode, string orderNote)
        {
            OrderId = orderId;
            OrderLocalId = orderLocalCode;
            CustomerId = customerId;
            CustomerCode = customerCode;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            OrderDate = orderDate;
            SalesId = salesId;
            SalesName = salesName;
            TotalAmount = totalAmount;
            UserEmail = userEmail;
            StatusSync = statusSync;
            FakturCode = fakturCode;
            ListItems = new List<OrderItemType>();
            OrderNote = orderNote;
        }

        public string OrderId { get;  set; }
        public string OrderLocalId { get;  set; }
        public string CustomerId { get;  set; }
        public string CustomerCode { get;  set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

        public string OrderDate { get; set; }
        public string SalesId { get; set; }
        public string SalesName { get; set; }
        public decimal TotalAmount { get; set; }

        public string UserEmail { get; set; }
        public string StatusSync { get; set; }

        public string FakturCode { get; set; }
        public string OrderNote { get; set; }

        public List<OrderItemType> ListItems { get; set; }

        public static IOrderKey Key(string id) => new OrderModel(id, "", "", "", "", "", "", "", "", 0, "", "", "", "");
    }

    public interface IOrderKey
    {
        string OrderId { get; }
    }
}
