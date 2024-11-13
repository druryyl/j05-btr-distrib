using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    public class ReturJualItemDto
    {
        public ReturJualItemDto()
        {
        }

        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; private set; }
        
        //  input
        public string QtyInputStr { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyHrgDetilStr { get; private set; }
        public string DiscInputStr { get; set; }

        //  detil tampilan
        public string DiscDetilStr { get; private set; }
        
        //  in-pcs (satuan kecil)
        public int Qty { get; private set; }
        public decimal HrgSat { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal DiscRp { get; private set; }
        public decimal PpnRp { get; private set; }
        public decimal Total { get; private set; }
    }
}
