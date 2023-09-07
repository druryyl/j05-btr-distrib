using btr.nuna.Infrastructure;
using btr.domain.SalesContext.WilayahAgg;

namespace btr.application.SalesContext.WilayahAgg
{
    public interface IWilayahDal :
        IInsert<WilayahModel>,
        IUpdate<WilayahModel>,
        IDelete<IWilayahKey>,
        IGetData<WilayahModel, IWilayahKey>,
        IListData<WilayahModel>
    {
    }
}
