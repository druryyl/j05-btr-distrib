using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.WarehouseAgg
{
    public interface IWarehouseDal :
        IInsert<WarehouseModel>,
        IUpdate<WarehouseModel>,
        IDelete<IWarehouseKey>,
        IGetData<WarehouseModel, IWarehouseKey>,
        IListData<WarehouseModel>
    {
    }
}