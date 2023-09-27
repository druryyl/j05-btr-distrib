using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public interface IInvoiceItemDal :
        IInsertBulk<InvoiceItemModel>,
        IDelete<IInvoiceKey>,
        IListData<InvoiceItemModel, IInvoiceKey>
    {
    }
}