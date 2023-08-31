using System;

namespace btr.domain.InventoryContext.StokAgg
{
    public class StokMutasiModel : IStokKey, IStokMutasiKey
    {
        public string StokId { get; set; }
        public string StokMutasiId { get; set; }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
        public int NoUrut { get; set; }
        public DateTime MutasiDate { get; set; }
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public decimal HargaJual { get; set; }
    }
}