using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturItemDal :
        IInsertBulk<FakturItemModel>,
        IDelete<IFakturKey>,
        IListData<FakturItemModel, IFakturKey>
    {
    }
}