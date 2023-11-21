using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangLunasView
    {
        public string PiutangId { get; set; }
        public string FakturCode { get; set; }
        public DateTime PiutangDate { get; set; }

        public string Sales { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        
        public decimal Total { get; set; }
        public decimal Terbayar { get; set; }
        public decimal Sisa { get; set; }
    }
}
