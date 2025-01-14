using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranDal : 
        IInsert<FpKeluaranModel>,
        IUpdate<FpKeluaranModel>,
        IDelete<FpKeluaranModel>,
        IGetData<FpKeluaranModel, IFpKeluaranKey>,
        IListData<FpKeluaranModel, Periode>
    {
    }
}
