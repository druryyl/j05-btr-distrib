using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public interface IInvoiceDiscDal :
        IInsertBulk<InvoiceDiscModel>,
        IDelete<IInvoiceKey>,
        IListData<InvoiceDiscModel, IInvoiceKey>
    {
    }
}