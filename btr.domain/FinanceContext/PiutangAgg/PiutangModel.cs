using System;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangModel : IPiutangKey
    {
        public string PiutangId { get; set; }
        public DateTime PiutangDate { get; set; }
        public string UserId { get; set; }
    }
}