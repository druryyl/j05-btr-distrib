using System;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangElementModel
    {
        public string PiutangId { get; set; }
        public int NoUrut { get; set; }
        public string ElementName { get; set; }
        public PiutangElementEnum ElementTag { get; set; }
        public decimal NilaiPlus { get; set; }
        public decimal NilaiMinus { get; set; }
        public DateTime ElementDate {get; set; }
}
}