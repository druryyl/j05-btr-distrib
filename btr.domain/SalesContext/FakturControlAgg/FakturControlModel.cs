using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.SalesContext.FakturControlAgg
{
    public class FakturControlModel : IFakturKey
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Npwp { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Bayar { get; set; }
        public decimal Sisa { get; set; }
        public string NoFakturPajak { get; set; }
        public string UserId { get; set; }
        
        public List<FakturControlStatusModel> ListStatus { get; set; }
    }
}
