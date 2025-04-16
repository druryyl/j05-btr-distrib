using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPelunasanInfoDal :
        IListData<PelunasanInfoDto, Periode>
    {
    }

    public class PelunasanInfoDto
    {
        public string SalesPersonName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address1 { get; set; }
        public string Kota { get; set; }
        public string FakturCode { get; set; }
        public decimal TotalFaktur { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal Materai { get; set; }
        public decimal BiayaAdmin { get; set; }
        public string PelunasanId { get; set; }
        public DateTime TglBayar { get; set; }
        public decimal Cash { get; set; }
        public decimal BgTransfer { get; set; }
        public decimal Sisa { get; set; }
    }

}
