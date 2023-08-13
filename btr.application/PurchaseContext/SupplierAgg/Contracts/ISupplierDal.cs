using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Infrastructure;

namespace btr.application.PurchaseContext.SupplierAgg.Contracts
{
    public interface ISupplierDal :
        IInsert<SupplierModel>,
        IUpdate<SupplierModel>,
        IDelete<ISupplierKey>,
        IGetData<SupplierModel, ISupplierKey>,
        IListData<SupplierModel>
    {
    }
}