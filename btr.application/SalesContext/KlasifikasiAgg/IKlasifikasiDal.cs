using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.WilayahAgg
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
