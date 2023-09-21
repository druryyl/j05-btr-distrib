using System;
using btr.domain.SalesContext.FakturAgg;

namespace btr.domain.SalesContext.AlokasiFpAgg
{
    public class FakturAlokasiFpItemView : IFakturKey, IFakturCode
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string  Npwp { get; private set; }
        public string Address { get; private set; }
        public decimal GrandTotal { get; private set; }
        public string NoFakturPajak { get; private set; }
        public string UserId { get; private set; }

        public void SetNoFakturPajak(string noFakturPajak) => NoFakturPajak = noFakturPajak;
    }
}