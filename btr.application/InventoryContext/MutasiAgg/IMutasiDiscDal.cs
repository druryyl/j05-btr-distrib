using btr.domain.InventoryContext.MutasiAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiDiscDal : 
        IInsertBulk<MutasiDiscModel>,
        IDelete<IMutasiKey>,
        IListData<MutasiDiscModel, IMutasiKey>
    {
    }
}
