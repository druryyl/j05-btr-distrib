using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranFakturDal :
        IInsertBulk<FpKeluaranFakturModel>,
        IDelete<IFpKeluaranKey>,
        IListData<FpKeluaranFakturModel, IFpKeluaranKey>
    {
    }
}
