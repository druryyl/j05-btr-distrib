using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.BrgAgg
{
    public class BrgSatuanHargaModel : IBrgKey
    {
        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
        public double HargaJual { get; set; }
    }
}