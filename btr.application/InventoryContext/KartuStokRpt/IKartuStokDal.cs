using btr.domain.InventoryContext.KartuStokRpt;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.KartuStokRpt
{
    public interface IKartuStokDal :
        IListData<KartuStokView, Periode>
    {
    }
}
