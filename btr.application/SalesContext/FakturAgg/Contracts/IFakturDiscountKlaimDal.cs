using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg
{
    public interface IFakturDiscountKlaimDal :
        IInsertBulk<FakturDiscountModel>,
        IDelete<IFakturKey>,
        IListData<FakturDiscountModel, IFakturKey>
    {
    }
}
