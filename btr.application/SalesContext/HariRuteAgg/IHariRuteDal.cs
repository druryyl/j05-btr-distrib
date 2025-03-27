using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.RuteAgg
{
    public interface IHariRuteDal :
        IInsert<HariRuteModel>,
        IUpdate<HariRuteModel>,
        IDelete<IHariRuteKey>,
        IGetData<HariRuteModel, IHariRuteKey>,
        IListData<HariRuteModel>
    {
    }
}
