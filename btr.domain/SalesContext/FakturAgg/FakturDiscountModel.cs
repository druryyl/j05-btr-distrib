namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturDiscountModel : IFakturKey, 
        IFakturItemKey, IFakturDiscountKey
    {
        public FakturDiscountModel()
        {
        }

        public FakturDiscountModel(int discountNo, string brgId, double discountProsen, double discountRp)
        {
            NoUrut = discountNo;
            BrgId = brgId;
            DiscountProsen = discountProsen;
            DiscountRp = discountRp;
        }
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturDiscountId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public double DiscountProsen { get; set; }
        public double DiscountRp { get; set; }
    }
}