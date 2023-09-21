using System;

namespace btr.domain.SalesContext.FakturAgg
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
        public DateTime VoidDate { get; private set; }
        public string UserIdVoid { get; private set; }

        public void SetNoFakturPajak(string noFakturPajak) => NoFakturPajak = noFakturPajak;
    }
}