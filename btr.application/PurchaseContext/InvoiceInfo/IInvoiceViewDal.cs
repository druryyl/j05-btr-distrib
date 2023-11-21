using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.InvoiceInfo
{
    // create interface for IPurchaseInfoDal
    public interface IInvoiceViewDal
        : IListData<InvoiceView, Periode>
    {
    }
}
