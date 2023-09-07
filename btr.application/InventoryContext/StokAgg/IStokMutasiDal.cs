using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokMutasiDal :
        IInsertBulk<StokMutasiModel>,
        IDelete<IStokKey>,
        IListData<StokMutasiModel, IStokKey>,
        IListData<StokMutasiModel, IReffKey>
    {
    }
}