namespace btr.domain.PurchaseContext.ReturBeliFeature
{
    public class ReturBeliDiscModel : IReturBeliKey
    {
        public ReturBeliDiscModel(int noUrut,
            string brgId,
            decimal discProsen, decimal discRp)
        {
            NoUrut = noUrut;
            BrgId = brgId;
            DiscProsen = discProsen;
            DiscRp = discRp;
        }
        public ReturBeliDiscModel()
        {
        }

        public string ReturBeliId { get; set; }
        public string ReturBeliItemId { get; set; }
        public string ReturBeliDiscId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public decimal DiscProsen { get; set; }
        public decimal DiscRp { get; set; }
    }
}