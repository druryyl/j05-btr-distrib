namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturDiscountModel : IFakturKey, 
        IFakturItemKey, IFakturDiscountKey
    {
        public FakturDiscountModel()
        {
        }

        public FakturDiscountModel(int discountNo, string brgId, decimal discProsen, decimal discountRp)
        {
            NoUrut = discountNo;
            BrgId = brgId;
            DiscProsen = discProsen;
            DiscRp = discountRp;
        }
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturDiscountId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public decimal DiscProsen { get; set; }
        public decimal DiscRp { get; set; }
    }
}