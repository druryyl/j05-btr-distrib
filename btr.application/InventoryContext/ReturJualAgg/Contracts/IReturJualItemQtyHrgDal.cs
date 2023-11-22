using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualItemQtyHrgDal :
        IInsert<ReturJualItemQtyHrgModel>,
        IDelete<IReturJualKey>,
        IListData<ReturJualItemQtyHrgModel, IReturJualKey>
    {
    }
}