using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranBrgDal :
        IInsertBulk<FpKeluaranBrgModel>,
        IDelete<IFpKeluaranKey>,
        IListData<FpKeluaranBrgModel, IFpKeluaranKey>
    {
    }
}
