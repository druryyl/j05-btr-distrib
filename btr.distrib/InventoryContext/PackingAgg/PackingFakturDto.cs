namespace btr.distrib.InventoryContext.PackingAgg
{
    public class PackingFakturDto
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Address { get; private set; }
        public string Kota { get; private set; }
        public string DriverName { get; private set; }
        public decimal GrandTotal { get; private set; }
        public string PackingId { get; private set; }
    }
}
