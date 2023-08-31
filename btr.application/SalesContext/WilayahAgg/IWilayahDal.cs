using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.domain.SalesContext.WilayahAgg;

namespace btr.application.SalesContext.WilayahAgg
{
    public interface IWilayahDal :
        IInsert<WilayahModel>,
        IUpdate<WilayahModel>,
        IDelete<IWilayahKey>,
        IGetData<WilayahModel, IWilayahKey>,
        IListData<WilayahModel>
    {
    }
}
