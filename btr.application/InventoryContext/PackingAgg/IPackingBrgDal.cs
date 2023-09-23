using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingBrgDal :
        IInsertBulk<PackingBrgModel>,
        IDelete<IPackingKey>,
        IListData<PackingBrgModel, IPackingKey>
    {
    }
}
