using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.domain.SalesContext.FakturInfoAgg;

namespace btr.application.SalesContext.FakturInfoAgg
{
    public interface IFakturInfoDal : IListData<FakturInfoDto, Periode>
    {
    }
}
