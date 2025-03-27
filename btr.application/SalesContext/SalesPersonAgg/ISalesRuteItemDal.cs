using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.SalesPersonAgg
{
    public interface ISalesRuteItemDal:
        IInsertBulk<SalesRuteItemModel>,
        IDelete<ISalesRuteKey>,
        IListData<SalesRuteItemModel, ISalesRuteKey>
    { }
}
