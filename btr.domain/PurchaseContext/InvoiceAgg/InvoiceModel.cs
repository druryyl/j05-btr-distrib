namespace btr.domain.PurchaseContext.InvoiceAgg
{
    public class InvoiceModel : IInvoiceKey
    {
        public string InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string UserId { get; set; }
        public string PurchaseOrderId { get; set; }
    }
}