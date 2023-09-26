using btr.domain.InventoryContext.KartuStokRpt;
using btr.nuna.Infrastructure;
using System;

namespace btr.application.InventoryContext.KartuStokRpt
{
    public interface IKartuStokSaldoDal :
        IListData<KartuStokSaldoView, DateTime>
    { }
}
