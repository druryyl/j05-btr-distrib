using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokAgg.Contracts
{
    public interface IStokMutasiDal :
        IInsertBulk<StokMutasiModel>,
        IDelete<IStokKey>,
        IListData<StokMutasiModel, IStokKey>
    {
    }
}