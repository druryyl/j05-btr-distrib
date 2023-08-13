using System;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.StokAgg
{
    public class StokModel : IStokKey
    {
        public string StokId { get; set; }
        public DateTime StokDate { get; set; }
        public string ReffId { get; set; }
        
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int QtyIn { get; set; }
        public int Qty { get; set; }
        public double NilaiPersediaan { get; set; }

        public List<StokMutasiModel> ListMutasi { get; set; }
    }
}