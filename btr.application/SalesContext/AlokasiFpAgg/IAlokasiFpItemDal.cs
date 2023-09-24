using btr.domain.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.AlokasiFpAgg
{
    public interface IAlokasiFpItemDal :
        IInsertBulk<AlokasiFpItemModel>,
        IDelete<IAlokasiFpKey>,
        IGetData<AlokasiFpItemModel, INoFakturPajak>,
        IListData<AlokasiFpItemModel, IAlokasiFpKey>
    {
    }
}
