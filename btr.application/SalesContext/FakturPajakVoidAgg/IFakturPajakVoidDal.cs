using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using btr.domain.SalesContext.FakturPajakVoidAgg;

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
