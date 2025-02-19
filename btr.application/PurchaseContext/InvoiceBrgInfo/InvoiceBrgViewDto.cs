using System;

namespace btr.application.PurchaseContext.InvoiceBrgInfo
{
    public class InvoiceBrgViewDto
    {
        public string InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime Tgl { get; set; }
        public string BrgName { get; set; }
        public string BrgCode { get; set; }
        public string SupplierName { get; set; }
        public string Kategori { get; set; }

        public decimal Hpp { get; set; }
        public int QtyBesar { get; set; }
        public string SatuanBesar { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Disc { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}