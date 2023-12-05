using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgSatuanDal :
        IInsertBulk<BrgSatuanModel>,
        IDelete<IBrgKey>,
        IListData<BrgSatuanModel, IBrgKey>,
        IListData<BrgSatuanModel, IKategoriKey>
    {
    }
}