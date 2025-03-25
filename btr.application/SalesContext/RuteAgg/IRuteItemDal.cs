using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.RuteAgg
{
    public interface IRuteItemDal :
        IInsertBulk<RuteItemModel>,
        IDelete<IRuteKey>,
        IListData<RuteItemModel, IRuteKey>
    {
    }
}
