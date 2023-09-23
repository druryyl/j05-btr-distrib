using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturControlAgg
{
    public interface IFakturControlStatusDal :
        IInsertBulk<FakturControlStatusModel>,
        IDelete<IFakturKey>,
        IListData<FakturControlStatusModel, IFakturKey>,
        IListData<FakturControlStatusModel, Periode>
    {
    }
}
