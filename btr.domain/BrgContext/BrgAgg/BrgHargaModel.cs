using System;
using btr.domain.BrgContext.HargaTypeAgg;

namespace btr.domain.BrgContext.BrgAgg
{
    public class BrgHargaModel : IBrgKey, IHargaTypeKey
    {
        public string BrgId { get; set; }
        public string HargaTypeId { get; set;}
        public decimal Harga { get; set; }
        public decimal Hpp { get; set; }
        public DateTime HargaTimestamp { get; set; }
    }
}