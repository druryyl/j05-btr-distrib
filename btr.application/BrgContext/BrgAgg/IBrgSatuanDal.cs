using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Infrastructure;
using System.Collections.Generic;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgSatuanDal :
        IInsertBulk<BrgSatuanModel>,
        IDelete<IBrgKey>,
        IListData<BrgSatuanModel, IBrgKey>,
        IListData<BrgSatuanModel, IKategoriKey>,
        IListData<BrgSatuanModel>,
        IListData<BrgSatuanModel, IEnumerable<IBrgKey>>
    {
    }
}