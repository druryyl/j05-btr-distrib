using btr.domain.InventoryContext.KartuStokRpt;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.KartuStokRpt
{
    public interface IKartuStokDal :
        IListData<KartuStokView, Periode>
    {
    }
}
