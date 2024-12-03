using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.ReturBalanceAgg
{
    public interface IReturBalancePostDal:
        IInsertBulk<ReturBalancePostModel>,
        IDelete<IReturJualKey>,
        IListData<ReturBalancePostModel, IReturJualKey> 
    { 
    }
}
