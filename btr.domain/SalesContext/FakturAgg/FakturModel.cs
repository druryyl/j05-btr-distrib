using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.PackingOrderFeature;
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

    public class FakturModel : IFakturKey, ISalesPersonKey, ICustomerKey, IWarehouseKey, IUserKey, IFakturCode, INoFakturPajak, IDriverKey
    {
        public FakturModel()
        {
        }
        public FakturModel(string id) => FakturId = id;

        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string FakturCode { get; set; }
        public string FakturCodeOri { get; set; }

        public string OrderId { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Npwp { get; set; }
        public string Nitku { get; set; }
        public decimal Plafond { get; set; }
        public decimal CreditBalance { get; set; }
        public string Address { get; set; } 
        public string Kota { get; set; }

    
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime TglRencanaKirim { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }


        public TermOfPaymentEnum TermOfPayment { get; set; }
        public DateTime DueDate { get; set; }

        public string HargaTypeId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Dpp { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal TotalKlaim { get; set; }
        public decimal DiscountKlaim { get; set; }
        public decimal DppKlaim { get; set; }
        public decimal TaxKlaim { get; set; }
        public decimal GrandTotalKlaim { get; set; }

        public decimal UangMuka { get; set; }
        public decimal KurangBayar { get; set; }
        public string NoFakturPajak { get; set; }
        public string FpKeluaranId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserId { get; set; }

        public DateTime VoidDate { get; set; }
        public string UserIdVoid { get; set; }
        public bool IsVoid { get => VoidDate != new DateTime(3000,1,1); }
        public string Note { get; set; }
        public bool IsHasKlaim { get; set; }
    
        public List<FakturItemModel> ListItem { get; set; }
        public List<FakturItemModel> ListItemKlaim { get; set; }

        public FakturReff ToReff()
            => new FakturReff(FakturId, FakturCode, FakturDate, UserId);
    }
}