using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.Contracts
{
    public interface IPurchaseOrderDal : 
        IInsert<PurchaseOrderModel>,
        IUpdate<PurchaseOrderModel>,
        IDelete<IPurchaseOrderKey>,
        IGetData<PurchaseOrderModel, IPurchaseOrderKey>,
        IListData<PurchaseOrderModel, Periode>,
        IListData<PurchaseOrderModel, ISupplierKey>
    {
    }
}
