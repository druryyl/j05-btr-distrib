using System;
using System.Collections.Generic;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public class ReturJualModel : IReturJualKey, ICustomerKey, IWarehouseKey, IUserKey
    {
        public ReturJualModel(string id) => ReturJualId = id;

        public ReturJualModel()
        {
        }
        public string ReturJualId { get;  set; }
        public DateTime ReturJualDate { get;  set; }
        public string UserId { get;  set; }
        public string JenisRetur { get; set; }
        public string ReturJualCode { get; set; }

        public string CustomerId { get;  set; }
        public string CustomerName { get;  set; }
        public string Address { get; set; }
        public string WarehouseId { get;  set; }
        public string WarehouseName { get;  set; }


        public string SalesPersonId { get;  set; }
        public string SalesPersonName { get;  set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }


        public decimal Total { get;  set; }
        public decimal DiscRp { get;  set; }
        public decimal PpnRp { get;  set; }
        public decimal GrandTotal { get;  set; }


        public DateTime VoidDate { get; set; }
        public string UserIdVoid { get; set; }
        public bool IsVoid { get => VoidDate != new DateTime(3000, 1, 1); }


        public List<ReturJualItemModel> ListItem { get;  set; }
    }

    public interface IReturJualKey
    {
        string ReturJualId { get; }
    }
}
