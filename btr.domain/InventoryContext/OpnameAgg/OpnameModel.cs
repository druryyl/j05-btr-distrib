using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.OpnameAgg
{
    public class OpnameModel
    {
        public string OpnameId { get; set; }
        public DateTime OpnameDate { get; set; }
        public string UserId { get; set; }
        public string BrgId { get; set; }
        public int QtyAwal { get; set; }
        public int QtyOpname { get; set; }
        public int QtyAdjust { get; set; }
        public decimal Nilai { get; set; }
        public string Satuan { get; set; }
        public string Asdf { get; set; }
    }
}
