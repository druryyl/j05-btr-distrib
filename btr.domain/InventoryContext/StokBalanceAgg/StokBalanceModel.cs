using btr.domain.BrgContext.BrgAgg;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.StokBalanceAgg
{
    public class StokBalanceModel : IBrgKey
    {
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public List<StokBalanceWarehouseModel> ListWarehouse { get; set; }
    }
}
