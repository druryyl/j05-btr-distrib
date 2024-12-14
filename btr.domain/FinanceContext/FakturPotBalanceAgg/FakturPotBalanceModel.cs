using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.domain.FinanceContext.ReturBalanceAgg
{
    public class FakturPotBalanceModel : IFakturKey, ICustomerKey
    {
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string FakturCode { get; set; }
        public bool IsHeapFaktur { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal NilaiFaktur { get; set; }
        public decimal NilaiPotong { get; set; }
        public decimal NilaiSumPost { get; set; }

        public List<FakturPotBalancePostModel> ListPost { get; set; }
    }
}
