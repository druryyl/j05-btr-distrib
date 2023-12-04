using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokBalanceAgg
{
    public interface IStokBalanceWarehouseDal  :
        IInsertBulk<StokBalanceWarehouseModel>,
        IDelete<IBrgKey>,
        IListData<StokBalanceWarehouseModel, IBrgKey>
    {
    }
}
