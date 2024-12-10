namespace btr.distrib.FinanceContext.ReturBalanceAgg
{
    public class ReturItemViewDto
    {
        public ReturItemViewDto(int no, string brgId, string brgCode, string brgName, 
            int qty, string satuan, decimal subTotal)
        {
            No = no;
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
            Qty = qty;
            Satuan = satuan;
            SubTotal = subTotal;
        }
        public int No { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal SubTotal { get; set; }
    }
}
