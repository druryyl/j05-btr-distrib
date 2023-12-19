using btr.domain.InventoryContext.AdjustmentAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.AdjustmentAgg
{
    public interface  IAdjustmentDal :
        IInsert<AdjustmentModel>,
        IUpdate<AdjustmentModel>,
        IDelete<IAdjustmentKey>,
        IGetData<AdjustmentModel, IAdjustmentKey>,
        IListData<AdjustmentModel, Periode>
    {
    }
}