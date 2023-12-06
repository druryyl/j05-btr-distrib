using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.KartuStokRpt;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.KartuStokRpt
{
    public interface IKartuStokDal :
        IListData<KartuStokView, Periode, IBrgKey>
    {
        KartuStokStokAwalView GetSaldoAwal(Periode filter, IBrgKey brgKey, IWarehouseKey warehouseKey);
    }
}
