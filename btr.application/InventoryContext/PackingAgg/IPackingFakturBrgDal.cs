using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingFakturBrgDal :
        IInsertBulk<PackingBrgModel>,
        IDelete<IPackingKey>,
        IListData<PackingBrgModel, IPackingKey>
    {
    }
}
