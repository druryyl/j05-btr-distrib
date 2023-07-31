namespace btr.domain.FinanceContext.HutangAgg
{
    public class HutangModel : IHutangKey
    {
        public string HUtangId { get; set; }
        public string HutangDate { get; set; }
        public string UserId { get; set; }
    }
}