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
    public interface INomorSeriFpDal :
        IInsert<NomorSeriFpModel>,
        IUpdate<NomorSeriFpModel>,
        IDelete<INomorSeriFpKey>,
        IGetData<NomorSeriFpModel, INomorSeriFpKey>,
        IListData<NomorSeriFpModel, Periode>
    {
    }

    public interface INomorSeriFpItemDal :
        IInsertBulk<NomorSeriFpItemModel>,
        IDelete<INomorSeriFpKey>,
        IListData<NomorSeriFpItemModel, INomorSeriFpKey>
    {
    }
}
