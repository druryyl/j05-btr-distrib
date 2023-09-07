using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokDal :
        IInsert<StokModel>,
        IUpdate<StokModel>,
        IDelete<IStokKey>,
        IGetData<StokModel, IStokKey>,
        IListData<StokModel, IBrgKey, IWarehouseKey>
    {
    }
}