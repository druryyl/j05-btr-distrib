using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.SalesContext.FakturPajakVoidAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturPajakVoidAgg
{
    public interface IFakturPajakVoidDal :
        IInsert<FakturPajakVoidModel>,
        IUpdate<FakturPajakVoidModel>,
        IDelete<INoFakturPajak>,
        IGetData<FakturPajakVoidModel, INoFakturPajak>,
        IListData<FakturPajakVoidModel, Periode>
    {
    }
}
