using btr.domain.SalesContext.FakturControlAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturStatusDal : 
        IListData<FakturControlStatusModel, Periode>
    {
    }
}
