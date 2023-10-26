using System.Collections;
using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangLunasDal :
        IInsertBulk<PiutangLunasModel>,
        IDelete<IPiutangKey>,
        IListData<PiutangLunasModel, IPiutangKey>
    {
    }
}