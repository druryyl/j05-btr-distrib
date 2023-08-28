using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.KategoriAgg
{
    public interface IKategoriDal :
        IInsert<KategoriModel>,
        IUpdate<KategoriModel>,
        IDelete<IKategoriKey>,
        IGetData<KategoriModel, IKategoriKey>,
        IListData<KategoriModel>
    {
    }
}