using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.SalesPersonAgg.Contracts
{
    public interface ISalesPersonDal :
        IInsert<SalesPersonModel>,
        IUpdate<SalesPersonModel>,
        IDelete<ISalesPersonKey>,
        IGetData<SalesPersonModel, ISalesPersonKey>,
        IListData<SalesPersonModel>
    {
    }
}