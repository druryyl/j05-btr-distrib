using btr.domain.SalesContext.AlokasiFpAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.AlokasiFpAgg
{
    public interface IAlokasiFpDal :
        IInsert<AlokasiFpModel>,
        IUpdate<AlokasiFpModel>,
        IDelete<IAlokasiFpKey>,
        IGetData<AlokasiFpModel, IAlokasiFpKey>,
        IListData<AlokasiFpModel, Periode>
    {
    }
}
