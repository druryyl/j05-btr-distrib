using btr.domain.InventoryContext.MutasiAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiDal : 
        IInsert<MutasiModel>,
        IUpdate<MutasiModel>,
        IDelete<IMutasiKey>,
        IGetData<MutasiModel, IMutasiKey>,
        IListData<MutasiModel, Periode>
    {
    }

    public interface IMutasiItemDal :
        IInsertBulk<MutasiItemModel>,
        IDelete<IMutasiKey>,
        IListData<MutasiItemModel, IMutasiKey>
    {
    }

}
