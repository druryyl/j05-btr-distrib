using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangLunasViewDal :
        IListData<PiutangLunasView, Periode>
    {
    }
}
