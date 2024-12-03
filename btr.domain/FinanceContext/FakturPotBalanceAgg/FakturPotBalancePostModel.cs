using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.FakturAgg;
using System;

namespace btr.domain.FinanceContext.ReturBalanceAgg
{
    public class FakturPotBalancePostModel : IFakturKey, IReturJualKey
    {
        public string FakturId { get; set; }
        public int NoUrut { get; set; }
        public DateTime PostDate { get; set; }
        public string UserId { get; set; }
        public string ReturJualId { get; set; }
        public DateTime ReturJualDate { get; set; }
        public decimal NilaiRetur { get; set; }
        public decimal NilaiPost { get; set; }
    }
}
