using btr.domain.SalesContext.FakturPajak;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.NomorFpAgg
{
    public interface IAlokasiFpDal :
        IInsert<AlokasiFpModel>,
        IUpdate<AlokasiFpModel>,
        IDelete<IAlokasiFpKey>,
        IGetData<AlokasiFpModel, IAlokasiFpKey>,
        IListData<AlokasiFpModel, Periode>
    {
    }

    public interface IAlokasiFpItemDal :
        IInsertBulk<AlokasiFpItemModel>,
        IDelete<IAlokasiFpKey>,
        IListData<AlokasiFpItemModel, IAlokasiFpKey>
    {
    }
}
