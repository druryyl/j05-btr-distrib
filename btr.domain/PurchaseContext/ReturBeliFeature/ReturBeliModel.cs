using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.PurchaseContext.ReturBeliFeature
{
    public class ReturBeliModel : IReturBeliKey, ISupplierKey, IUserKey, IWarehouseKey
    {
        public ReturBeliModel()
        {
        }
        public ReturBeliModel(string id) => ReturBeliId = id;

        public string ReturBeliId { get; set; }
        public DateTime ReturBeliDate { get; set; }
        public string ReturBeliCode { get; set; }

        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }


        public string NoFakturPajak { get; set; }
        public TermOfPaymentEnum TermOfPayment { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public decimal Disc { get; set; }
        public decimal Dpp { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal UangMuka { get; set; }
        public decimal KurangBayar { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserId { get; set; }

        public DateTime VoidDate { get; set; }
        public string UserIdVoid { get; set; }
        public bool IsVoid { get => VoidDate != new DateTime(3000, 1, 1); }
        public bool IsStokPosted { get; set; }

        public List<InvoiceItemModel> ListItem { get; set; }
    }
}
