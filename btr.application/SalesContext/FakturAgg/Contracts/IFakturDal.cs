using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturDal :
        IInsert<FakturModel>,
        IUpdate<FakturModel>,
        IDelete<IFakturKey>,
        IGetData<FakturModel, IFakturKey>,
        IListData<FakturModel, Periode>
    {
    }
}