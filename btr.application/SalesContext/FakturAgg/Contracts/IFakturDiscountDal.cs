using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturDiscountDal :
        IInsertBulk<FakturDiscountModel>,
        IDelete<IFakturKey>,
        IListData<FakturDiscountModel, IFakturKey>
    {
    }
}