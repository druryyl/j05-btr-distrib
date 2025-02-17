using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturCodeOpenDal :
        IInsert<string>,
        IDelete<string>,
        IListData<string>
    {
    }
}
