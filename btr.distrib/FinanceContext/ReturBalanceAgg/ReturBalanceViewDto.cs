namespace btr.distrib.FinanceContext.ReturBalanceAgg
{
    public class ReturBalanceViewDto
    {
        public ReturBalanceViewDto(string id, string code, string tgl,
            string customer, string address, string jenis, string sales,
            decimal grandTotal, decimal posting)
        {
            ReturJualId = id;
            ReturJualCode = code;
            ReturJualDate = tgl;
            Customer = customer;
            Address = address;
            JenisRetur = jenis;
            Sales = sales;
            GrandTotal = grandTotal;
            Posting = posting;
        }
        public string ReturJualId { get; private set; }
        public string ReturJualCode { get; private set; }
        public string ReturJualDate { get; private set; }
        public string Customer { get; private set; }
        public string Address { get; private set; }
        public string Sales { get; private set; }
        public string JenisRetur { get; private set; }
        public decimal GrandTotal { get; private set; }
        public decimal Posting { get; private set; }
    }
}
