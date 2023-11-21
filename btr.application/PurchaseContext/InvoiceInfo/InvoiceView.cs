using System;

namespace btr.application.PurchaseContext.InvoiceInfo
{
    public class InvoiceView
    {
        public string InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime Tgl { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }
        public decimal Total { get; set; }
        public decimal Disc{ get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
    }
}