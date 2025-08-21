using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.SalesContext.OrderFeature
{
    public class ListOrderDto
    {
        public ListOrderDto(string orderId, DateTime orderDate,
            string sales, string email, string localID,
            string customerName, string customerCode, string address,
            decimal totalAmount, string syncStatus)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            Sales = sales;
            Email = email;
            LocalID = localID;
            CustomerName = customerName;
            CustomerCode = customerCode;
            Address = address;
            TotalAmount = totalAmount;
            SyncStatus = syncStatus;
        }

        public string ID => $"{Email}\r\n  {LocalID}";
        public string Email { get; private set; }
        public string LocalID { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string Sales { get; private set; }

        public string Customer => $"{CustomerName}\r\n    {Address}";
        public string CustomerName { get; private set; }
        public string CustomerCode { get; private set; }
        public string Address { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string SyncStatus { get; private set; }
        public string OrderId { get; private set; }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string Faktur => $"{FakturCode}\r\n{FakturDate:dd-MM-yyyy}";
        public decimal NilaiFaktur { get; private set; }
        public string UserrName { get; private set; }

        public void SetFakturInfo(string fakturId, string fakturCode, string userrName, DateTime fakturDate, Decimal nilaiFaktur)
        {
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            UserrName = userrName;
            NilaiFaktur = nilaiFaktur;
        }
    }
}
