using btr.domain.BrgContext.JenisBrgAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.BrgContext.JenisBrgAgg.Contracts
{
    public interface IJenisBrgDal :
        IInsert<JenisBrgModel>,
        IUpdate<JenisBrgModel>,
        IDelete<IJenisBrgKey>,
        IGetData<JenisBrgModel, IJenisBrgKey>,
        IListData<JenisBrgModel>
    {
    }
}
