using btr.domain.SalesContext.InfoFakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturInfoAgg
{
    public interface IFakturInfoDal : IListData<FakturInfoDto, Periode>
    {
    }
}
