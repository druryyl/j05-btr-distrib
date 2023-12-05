using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgDal :
        IInsert<BrgModel>,
        IUpdate<BrgModel>,
        IDelete<IBrgKey>,
        IGetData<BrgModel, IBrgKey>,
        IGetData<BrgModel, string>,
        IListData<BrgModel>,
        IListData<BrgModel, IKategoriKey>
    {
    }
}