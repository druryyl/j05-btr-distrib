using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgSatuanDal :
        IInsertBulk<BrgSatuanModel>,
        IDelete<IBrgKey>,
        IListData<BrgSatuanModel, IBrgKey>
    {
    }
}