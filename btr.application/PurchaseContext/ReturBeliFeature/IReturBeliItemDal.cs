using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.ReturBeliFeature
{
    public interface IReturBeliItemDal :
        IInsertBulk<ReturBeliItemModel>,
        IDelete<IReturBeliKey>,
        IListData<ReturBeliItemModel, IReturBeliKey>
    {
    }
}
