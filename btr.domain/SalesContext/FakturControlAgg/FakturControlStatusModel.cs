using System;

namespace btr.domain.SalesContext.FakturControlAgg
{
    public class FakturControlStatusModel
    {
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public StatusFakturEnum StatusFaktur { get; set; }
        public DateTime StatusDate { get; set; }
        public string Keterangan { get; set; }
        public string UserId { get; set; }
    }
}
