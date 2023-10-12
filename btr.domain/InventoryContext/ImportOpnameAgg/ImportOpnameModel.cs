using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.ImportOpnameAgg
{
    public class ImportOpnameModel
    {
        public string BrgCode { get; set; }
        public string WarehouseId { get; set; }
        public int Qty { get; set; }
    }
}
