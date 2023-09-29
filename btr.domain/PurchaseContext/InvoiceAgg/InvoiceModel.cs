using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.PurchaseContext.InvoiceAgg
{
    public class InvoiceModel : IInvoiceKey, ISupplierKey, IUserKey, IWarehouseKey
    {
        public InvoiceModel()
        {
        }

        public string InvoiceId { get; set; }   
        public DateTime InvoiceDate { get; set; }
        public string InvoiceCode { get; set; }

        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }


        public string NoFakturPajak { get; set; }
        public TermOfPaymentEnum TermOfPayment { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public decimal Disc { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserId { get; set; }

        public DateTime VoidDate { get; set; }
        public string UserIdVoid { get; set; }
        public bool IsVoid { get => VoidDate != new DateTime(3000, 1, 1); }


        public List<InvoiceItemModel> ListItem { get; set; }
    }
}
 