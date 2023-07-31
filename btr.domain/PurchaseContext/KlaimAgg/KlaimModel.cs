using System;
using btr.domain.PurchaseContext.PrinsipalAgg;

namespace btr.domain.PurchaseContext.KlaimAgg
{
    public class KlaimModel : IKlaimKey, IPrinsipalKey
    {
        public string KlaimId { get; set; }
        public DateTime KlaimDate { get; set; }
        public string PrinsipalId { get; set; }
    }
}