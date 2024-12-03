using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.ReturBalanceAgg
{
    public interface IReturBalanceDal :
        IInsert<ReturBalanceModel>,
        IUpdate<ReturBalanceModel>,
        IDelete<IReturJualKey>,
        IGetData<ReturBalanceModel, IReturJualKey>,
        IListData<ReturBalanceModel, ICustomerKey>
    {
    }
}
