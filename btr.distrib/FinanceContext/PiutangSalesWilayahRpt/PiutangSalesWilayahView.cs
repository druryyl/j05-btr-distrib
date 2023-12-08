using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.FinanceContext.PiutangPerSalesPerWilayahRpt
{
    public class PiutangSalesWilayahView
    {
        public string SalesId { get; set; }
        public string SalesName { get; set; }
        public List<PiutangSalesWilayahWilayahView> ListWilayah { get; set; }
    }

    public class PiutangSalesWilayahWilayahView
    {
        public string WilayahId { get; set; }
        public string WilayahName { get; set; }
        public List<PiutangSalesWilayahPiutangView> ListPiutang { get; set; }
    }

    public class PiutangSalesWilayahPiutangView
    {
        public int NoUrut { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }

        public string CustomerName { get; set; }
        public string JatuhTempo { get; set; }
        public decimal TotalJual{ get; set; }

        public decimal BayarTunai { get; set; }
        public decimal BayarGiro { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal MateraiAdmin { get; set; }

        public decimal KurangBayar { get; set; }
    }
}
