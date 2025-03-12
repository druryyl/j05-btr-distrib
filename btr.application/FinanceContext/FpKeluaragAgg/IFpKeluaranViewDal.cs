using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranViewDal :
        IListData<FpKeluaranViewDto, Periode>
    {
    }
    public class FpKeluaranViewDto
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerName { get; set; }
        public DateTime FpKeluaranDate { get; set; }
        public string FpKeluaranId { get; set; }
        public string NpwpNikPembeli { get; set; }
        public string JenisIdPembeli { get; set; }
        public string NamaPembeli { get; set; }
        public string AlamatPembeli { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Ppn { get; set; }
        public string Status { get; set; }
    }

}