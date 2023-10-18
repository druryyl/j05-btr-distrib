using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public interface IInvoiceDal :
        IInsert<InvoiceModel>,
        IUpdate<InvoiceModel>,
        IDelete<IInvoiceKey>,
        IGetData<InvoiceModel, IInvoiceKey>,
        IListData<InvoiceModel, Periode>
    {
    }
}