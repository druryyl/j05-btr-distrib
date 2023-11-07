using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualItemDal :
        IInsertBulk<ReturJualItemModel>,
        IDelete<IReturJualKey>,
        IListData<ReturJualItemModel, IReturJualKey>
    {
    }
}