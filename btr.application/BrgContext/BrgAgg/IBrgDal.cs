using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgDal :
        IInsert<BrgModel>,
        IUpdate<BrgModel>,
        IDelete<IBrgKey>,
        IGetData<BrgModel, IBrgKey>,
        IListData<BrgModel>
    {
    }
}