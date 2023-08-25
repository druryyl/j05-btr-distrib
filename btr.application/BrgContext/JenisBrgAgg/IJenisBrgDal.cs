using btr.domain.BrgContext.JenisBrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.JenisBrgAgg
{
    public interface IJenisBrgDal :
        IInsert<JenisBrgModel>,
        IUpdate<JenisBrgModel>,
        IDelete<IJenisBrgKey>,
        IGetData<JenisBrgModel, IJenisBrgKey>,
        IListData<JenisBrgModel>
    {
    }
}
