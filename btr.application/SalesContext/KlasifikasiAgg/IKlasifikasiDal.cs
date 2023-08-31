using btr.domain.SalesContext.KlasifikasiAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.KlasifikasiAgg
{
    public interface IKlasifikasiDal :
        IInsert<KlasifikasiModel>,
        IUpdate<KlasifikasiModel>,
        IDelete<IKlasifikasiKey>,
        IGetData<KlasifikasiModel, IKlasifikasiKey>,
        IListData<KlasifikasiModel>
    {
    }
}
