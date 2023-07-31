using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.BrgAgg
{
    public class BrgModel : IBrgKey
    {
        public BrgModel()
        {
        }

        public BrgModel(string id) => BrgId = id;

        public string BrgId { get; set; }
        public string BrgName { get; set;}
        public List<BrgSatuanHargaModel> ListSatuanHarga { get; set; }
    }
}