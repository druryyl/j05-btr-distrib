using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturItemKlaimDal :
        IInsertBulk<FakturItemModel>,
        IDelete<IFakturKey>,
        IListData<FakturItemModel, IFakturKey>
    {
    }
}
