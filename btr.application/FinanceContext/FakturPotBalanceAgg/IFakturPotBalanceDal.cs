using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.FakturPotBalanceAgg
{
    public interface IFakturPotBalanceDal : 
        IInsert<FakturPotBalanceModel>,
        IUpdate<FakturPotBalanceModel>,
        IDelete<IFakturKey>,
        IGetData<FakturPotBalanceModel, IFakturKey>,
        IListData<FakturPotBalanceModel, Periode>,
        IListData<FakturPotBalanceModel, ICustomerKey>
    {
    }
}
