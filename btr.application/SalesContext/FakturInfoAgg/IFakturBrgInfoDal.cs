using btr.domain.SalesContext.FakturInfoAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturInfoAgg
{
    public interface IFakturBrgInfoDal 
        : IListData<FakturBrgInfoDto, Periode>
    {
    }
}
