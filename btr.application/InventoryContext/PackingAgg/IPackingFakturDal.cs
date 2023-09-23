using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.PackingAgg
{

    public interface IPackingFakturDal :
        IInsertBulk<PackingFakturModel>,
        IDelete<IPackingKey>,
        IListData<PackingFakturModel, IPackingKey>
    {
    }
}
