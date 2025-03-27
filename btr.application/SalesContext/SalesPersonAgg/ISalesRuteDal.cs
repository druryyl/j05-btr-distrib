using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.SalesPersonAgg
{
    public interface ISalesRuteDal :
        IInsert<SalesRuteModel>,
        IUpdate<SalesRuteModel>,
        IDelete<ISalesRuteKey>,
        IGetData<SalesRuteModel, ISalesRuteKey>,
        IListData<SalesRuteModel, ISalesPersonKey>
    {
    }
}
