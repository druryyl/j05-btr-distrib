namespace btr.domain.PurchaseContext.InvoiceAgg
{
    public class InvoiceDiscModel : IInvoiceKey
    {
        public string InvoiceId { get; set; }
        public string InvoiceItemId { get; set; }
        public string InvoiceDiscId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public decimal DiscProsen { get; set; }
        public decimal DiskRp { get; set; }
    }
}