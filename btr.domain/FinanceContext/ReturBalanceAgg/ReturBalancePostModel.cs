using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.FakturAgg;
using System;

namespace btr.domain.FinanceContext.ReturBalanceAgg
{
    public class ReturBalancePostModel : IReturJualKey, IFakturKey
    {
        public string ReturJualId { get; set; }
        public int NoUrut { get; set; }
        public DateTime PostDate { get; set; }
        public string UserId { get; set; }
        
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }

        public decimal NilaiFaktur { get; set; }
        public decimal NilaiPotong { get; set; }
        public decimal NilaiPost { get; set; }
    }
}
