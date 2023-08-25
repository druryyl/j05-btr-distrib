using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Infrastructure;
using FluentValidation.Resources;

namespace btr.application.BrgContext.KategoriAgg.Contracts
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