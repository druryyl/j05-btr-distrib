using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;

namespace btr.domain.InventoryContext.OpnameAgg
{
    public class StokOpModel : IStokOpKey, IBrgKey, IWarehouseKey
    {
        public string StokOpId { get; set; }
        public DateTime StokOpDate { get; set; }
        public DateTime PeriodeOp { get; set; }
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public int QtyBesarAwal { get; set; }
        public int QtyKecilAwal { get; set; }
        public int QtyPcsAwal { get; set; }

        public int QtyBesarOpname { get; set; }
        public int QtyKecilOpname { get; set; }
        public int QtyPcsOpname { get; set; }

        public int QtyBesarAdjust { get; set; }
        public int QtyKecilAdjust { get; set; }
        public int QtyPcsAdjust { get; set; }

        public string QtyOpnameInputStr { get; set; }
        public string UserId { get; set; }
    }
}
