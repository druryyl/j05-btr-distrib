using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranItemDto
    {
        public FpKeluaranItemDto(string id, string code, DateTime tgl, 
            string customer, string npwp, string address, decimal grandTotal)
        {
            FakturId = id;
            FakturCode = code;
            FakturDate = tgl;
            CustomerName = customer;
            Npwp = npwp;
            Address = address;
            GrandTotal = grandTotal;
        }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string Npwp { get; private set; }
        public string Address { get; private set; }
        public decimal GrandTotal { get; private set; }
        public bool IsPilih { get; set; }
    }
}
