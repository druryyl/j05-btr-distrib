using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualItemDiscDal :
        IInsert<ReturJualItemDiscModel>,
        IDelete<IReturJualKey>,
        IListData<ReturJualItemDiscModel, IReturJualKey>
    {
    }
}