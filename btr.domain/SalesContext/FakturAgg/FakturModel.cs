using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.SalesContext.FakturAgg
{ 
    public class FakturModel : IFakturKey, ISalesPersonKey, ICustomerKey, IWarehouseKey
    {
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
    
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Plafond { get; set; }
        public double CreditBalance { get; set; }
    
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime TglRencanaKirim { get; set; }

        public string TermOfPayment { get; set; }

        public decimal Total { get; set; }
        public decimal DiscountLain { get; set; }
        public decimal BiayaLain { get; set; }
        public decimal GrandTotal { get; set; }
    
        public decimal UangMuka { get; set; }
        public decimal KurangBayar { get; set; }
    
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserId { get; set; }
    
        public List<FakturItemModel> ListItem { get; set; }
    }
}