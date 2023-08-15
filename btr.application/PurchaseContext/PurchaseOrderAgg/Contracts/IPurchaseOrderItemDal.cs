using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.Contracts
{
    public interface IPurchaseOrderItemDal :
        IInsertBulk<PurchaseOrderItemModel>,
        IDelete<IPurchaseOrderKey>,
        IListData<PurchaseOrderItemModel, IPurchaseOrderKey>
    {
    }
}
