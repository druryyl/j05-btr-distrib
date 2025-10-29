using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System.Collections.Generic;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangDal :
        IInsert<PiutangModel>,
        IUpdate<PiutangModel>,
        IDelete<IPiutangKey>,
        IGetData<PiutangModel, IPiutangKey>,
        IListData<PiutangModel, Periode>,
        IListData<PiutangModel, IEnumerable<IPiutangKey>>,
        IListData<PiutangModel, ICustomerKey, Periode>,
        IListData<PiutangModel, ISalesPersonKey, Periode>
    {
    }

}