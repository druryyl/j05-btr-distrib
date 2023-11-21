using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.InvoiceBrgInfo
{
    // create interface for IPurchaseInfoDal
    public interface IInvoiceBrgViewDal
        : IListData<InvoiceBrgViewDto, Periode>
    {
    }
}
