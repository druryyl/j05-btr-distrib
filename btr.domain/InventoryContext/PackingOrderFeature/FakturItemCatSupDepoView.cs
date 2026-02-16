using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class FakturItemCatSupDepoView
    {
        public string BrgId { get; set; }
        public string KategoriName { get; set; }
        public string SupplierName { get; set; }
        public string DepoId { get; set; }
        public string DepoName { get; set; }

        public static FakturItemCatSupDepoView Default => new FakturItemCatSupDepoView
        {
            BrgId = "-",
            KategoriName = "-",
            SupplierName = "-",
            DepoId = "-",
            DepoName = "-"
        };
    }
}
