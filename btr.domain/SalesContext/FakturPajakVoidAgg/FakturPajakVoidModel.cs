using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.FakturPajakVoidAgg
{
    public class FakturPajakVoidModel : INoFakturPajak
    {
        public string NoFakturPajak { get; set; }
        public DateTime VoidDate { get; set; }
        public string AlasanVoid { get; set; }
        public string UserId { get; set; }

    }
}
