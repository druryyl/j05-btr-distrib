using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgStokViewAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgStokViewAgg.Contracts
{
    public interface IBrgStokViewDal :
        IListData<BrgStokViewModel, IWarehouseKey>
    {
        BrgStokViewModel GetData<T>(T key) where T : IBrgKey, IWarehouseKey;
    }
}
