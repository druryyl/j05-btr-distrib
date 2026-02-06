using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.WarehouseAgg
{
    public class DepoModel : IDepoKey
    {
        public DepoModel(string depoId, string depoName)
        {
            DepoId = depoId;
            DepoName = depoName;
        }
        public string DepoId { get; private set; }
        public string DepoName { get; private set; }
    }
}
