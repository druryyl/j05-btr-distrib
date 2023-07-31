using btr.domain.InventoryContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.BrgAgg.Contracts
{
    public interface IBrgDal :
        IInsert<BrgModel>,
        IUpdate<BrgModel>,
        IDelete<IBrgKey>,
        IGetData<BrgModel, IBrgKey>,
        IListData<BrgModel>
    {
    }
}