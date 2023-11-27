using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public enum JenisQtyEnum
    {
        SatuanBesar,
        SatuanKecil
    }   

    public class ReturJualItemQtyHrgModel
    {
        public string ReturJualId { get; set; }
        public string ReturJualItemId { get; set; }
        public string ReturJualItemQtyHrgId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public JenisQtyEnum JenisQty { get; set; }
        public int Conversion { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
    }
}
