using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public class ReturJualItemDiscModel
    {
        public string ReturJualId { get; set; }
        public string ReturJualItemId { get; set; }
        public string ReturJualItemDiscId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public int DiscNo { get; set; }
        public decimal BaseHrg { get; set; }
        public decimal DiscProsen { get; set; }
        public decimal DiscRp { get; set; }
    }
}
