using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangSalesWilayahDal : IListData<PiutangSalesWilayahDto, Periode>
    {
    }

    public class PiutangSalesWilayahDto
    {
        public string SalesName { get; set; }
        public string WilayahName { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }

        public string CustomerName { get; set; }
        public DateTime JatuhTempo { get; set; }

        public decimal TotalJual { get; set; }
        public decimal BayarTunai { get; set; }
        public decimal BayarGiro { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal MateraiAdmin { get; set; }

        public decimal KurangBayar { get; set; }
    }

}
