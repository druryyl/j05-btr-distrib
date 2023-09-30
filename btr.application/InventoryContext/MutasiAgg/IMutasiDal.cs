using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiDal : 
        IInsert<MutasiModel>,
        IUpdate<MutasiModel>,
        IDelete<IMutasiKey>,
        IGetData<MutasiModel, IMutasiKey>,
        IListData<MutasiModel, Periode>
    {
    }

    public interface IMutasiItemDal :
        IInsertBulk<MutasiItemModel>,
        IDelete<IMutasiKey>,
        IListData<MutasiItemModel, IMutasiKey>
    {
    }

}
