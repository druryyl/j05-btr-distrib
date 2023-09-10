using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturControlAgg
{
    public interface IFakturControlStatusDal :
        IInsertBulk<FakturControlStatusModel>,
        IDelete<IFakturKey>,
        IListData<FakturControlStatusModel, IFakturKey>,
        IListData<FakturControlStatusModel, Periode>
    {
    }
}
