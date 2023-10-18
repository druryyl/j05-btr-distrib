using System;

namespace btr.domain.InventoryContext.KartuStokRpt
{
    public class KartuStokView
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string KategoriId { get; set; }
        public string KategoriName { get; set; }
        public DateTime Tgl { get; set; }
        public int QtyAwal { get; set; }
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public int QtyAkhir { get; set; }
        public decimal Hpp { get; set; }
    }
}
