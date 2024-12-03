using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.FakturPotBalanceAgg
{
    public interface IFakturPotBalancePostDal :
        IInsertBulk<FakturPotBalancePostModel>,
        IDelete<IFakturKey>,
        IListData<FakturPotBalancePostModel, IFakturKey>
    {
    }
}
