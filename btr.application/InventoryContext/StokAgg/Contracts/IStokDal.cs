using System.Collections.Generic;
using btr.domain.InventoryContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokAgg.Contracts
{
    public interface IStokDal :
        IInsert<StokModel>,
        IUpdate<StokModel>,
        IDelete<IStokKey>,
        IGetData<StokModel, IStokKey>,
        IListData<StokModel, IBrgKey, IWarehouseKey>
    {
        IEnumerable<StokModel> ListDataAll(IBrgKey brg, IWarehouseKey warehouse);
    }
}