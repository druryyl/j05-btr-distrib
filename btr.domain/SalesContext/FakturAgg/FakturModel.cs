using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.SalesContext.FakturAgg
{ 
    public enum TermOfPaymentEnum
    {
        Credit, Cash
    }

    public class FakturModel : IFakturKey, ISalesPersonKey, ICustomerKey, IWarehouseKey, IUserKey
    {
        public FakturModel()
        {
        }
        public FakturModel(string id) => FakturId = id;

        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string FakturCode { get; set; }

        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Plafond { get; set; }
        public decimal CreditBalance { get; set; }
    
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime TglRencanaKirim { get; set; }

        public TermOfPaymentEnum TermOfPayment { get; set; }
        public DateTime DueDate { get; set; }

        public string HargaTypeId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
    
        public decimal UangMuka { get; set; }
        public decimal KurangBayar { get; set; }
    
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserId { get; set; }
    
        public List<FakturItemModel> ListItem { get; set; }
    }
}