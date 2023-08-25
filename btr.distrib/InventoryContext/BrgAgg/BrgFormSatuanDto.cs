using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormSatuanDto
    {
        public BrgFormSatuanDto()
        {
        }

        public BrgFormSatuanDto(string id, int conversion) => (Satuan, Conversion) = (id, conversion);
        public string Satuan { get; set; }
        public int Conversion { get; set; }
    }
}
