using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using System;

namespace btr.domain.InventoryContext.StokAgg
{
    public class StokMutasiModel : IStokKey, IStokMutasiKey, IReffKey, IBrgKey, IWarehouseKey
    {
        public string StokId { get; set; }
        public string StokMutasiId { get; set; }
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }

        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
        public int NoUrut { get; set; }
        public DateTime MutasiDate { get; set; }
        public DateTime PencatatanDate { get; set; }
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public decimal HargaJual { get; set; }
        public string Keterangan { get; set; }
    }
}