using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.BrgAgg.Contracts
{
    public interface IBrgSatuanDal :
        IInsertBulk<BrgSatuanModel>,
        IDelete<IBrgKey>,
        IListData<BrgSatuanModel, IBrgKey>
    {
    }
}