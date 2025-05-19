using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.ReturBeliFeature
{
    public interface IReturBeliDal :
        IInsert<ReturBeliModel>,
        IUpdate<ReturBeliModel>,
        IDelete<IReturBeliKey>,
        IGetData<ReturBeliModel, IReturBeliKey>,
        IListData<ReturBeliModel, Periode>
    {
    }
}
