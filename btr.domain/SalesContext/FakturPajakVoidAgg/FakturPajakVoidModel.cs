using System;
using btr.domain.SalesContext.FakturAgg;

namespace btr.domain.SalesContext.FakturPajakVoidAgg
{
    public class FakturPajakVoidModel : INoFakturPajak
    {
        public string NoFakturPajak { get; set; }
        public DateTime VoidDate { get; set; }
        public string AlasanVoid { get; set; }
        public string UserId { get; set; }

    }
}
