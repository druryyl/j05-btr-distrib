using System;
using btr.domain.PurchaseContext.SupplierAgg;

namespace btr.domain.PurchaseContext.KlaimAgg
{
    public class KlaimModel : IKlaimKey, ISupplierKey
    {
        public string KlaimId { get; set; }
        public DateTime KlaimDate { get; set; }
        public string SupplierId { get; set; }
    }
}