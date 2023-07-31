using btr.domain.InventoryContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.BrgAgg.Contracts
{
    public interface IBrgSatuanHargaDal :
        IInsertBulk<BrgSatuanHargaModel>,
        IDelete<IBrgKey>,
        IListData<BrgSatuanHargaModel, IBrgKey>
    {
    }
}