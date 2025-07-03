using System;
using System.Text;

namespace btr.domain.InventoryContext.KartuStokRpt
{
    public class KartuStokView
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string JenisMutasi { get; set; }
        public string ReffId { get; set; }
        public DateTime MutasiDate { get; set; }
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public decimal Hpp { get; set; }
        public decimal HargaJual { get; set; }
        public string Keterangan { get; set; }
        public string ReffCode { get; set; }
    }

    public class KartuStokStokAwalView
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int QtyMasuk { get; set; }
        public int QtyKeluar { get; set; }
        public int QtyAkhir { get; set; }
    }
}
