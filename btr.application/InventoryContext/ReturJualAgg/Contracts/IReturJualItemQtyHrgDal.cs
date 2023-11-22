using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualItemQtyHrgDal :
        IInsertBulk<ReturJualItemQtyHrgModel>,
        IDelete<IReturJualKey>,
        IListData<ReturJualItemQtyHrgModel, IReturJualKey>
    {
    }
}