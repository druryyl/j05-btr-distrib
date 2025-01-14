using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranFakturDto : IFakturKey
    {
        public FpKeluaranFakturDto(string id, string code, DateTime tgl, 
            string customer, string npwp, string address, 
            decimal grandTotal, decimal ppn, bool isPilih)
        {
            FakturId = id;
            FakturCode = code;
            FakturDate = tgl;
            CustomerName = customer;
            Npwp = npwp;
            Address = address;
            GrandTotal = grandTotal;
            IsPilih = isPilih;
            Ppn = ppn;
        }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string Npwp { get; private set; }
        public string Address { get; private set; }
        public decimal GrandTotal { get; private set; }
        public decimal Ppn { get; private set; }
        public bool IsPilih { get; set; }
    }
}
