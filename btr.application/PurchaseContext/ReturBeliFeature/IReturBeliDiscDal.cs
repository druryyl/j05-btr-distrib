using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.ReturBeliFeature
{
    public interface IReturBeliDiscDal :
        IInsertBulk<ReturBeliDiscModel>,
        IDelete<IReturBeliKey>,
        IListData<ReturBeliDiscModel, IReturBeliKey>
    {
    }
}
