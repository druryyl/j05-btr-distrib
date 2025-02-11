namespace btr.distrib.InventoryContext.PackingAgg
{
    internal class Packing2FakturDto
    {
        public int No { get; private set; }
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public string FakturDate { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Kota { get; set; }
        public decimal GrandTotal { get; set; }
        public bool Pilih { get; set; }
    }
}