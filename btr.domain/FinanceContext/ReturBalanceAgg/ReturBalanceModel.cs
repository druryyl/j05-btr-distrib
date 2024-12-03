using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.ReturBalanceAgg
{
    public class ReturBalanceModel : IReturJualKey, ICustomerKey
    {
        public string ReturJualId { get; set; }
        public DateTime ReturJualDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal NilaiRetur { get; set; }
        public decimal NilaiSumPost { get => ListPost.Sum(x => x.NilaiPost); }

        public List<ReturBalancePostModel> ListPost { get; set; }
    }

}
