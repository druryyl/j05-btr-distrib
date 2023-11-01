using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangDal :
        IInsert<PiutangModel>,
        IUpdate<PiutangModel>,
        IDelete<IPiutangKey>,
        IGetData<PiutangModel, IPiutangKey>,
        IListData<PiutangModel, Periode>
    {
    }
}