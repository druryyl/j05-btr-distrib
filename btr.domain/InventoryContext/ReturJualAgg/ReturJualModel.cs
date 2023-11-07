using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public class ReturJualModel : IReturJualKey, ICustomerKey, ISalesPersonKey, IWarehouseKey
    {
        public string ReturJualId { get; set; }
        public DateTime ReturJualDate { get; set; }
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string UserId { get; set; }
        
        public decimal Total { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public interface IReturJualKey
    {
        string ReturJualId { get; }
    }
}
