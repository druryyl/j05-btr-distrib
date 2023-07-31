using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturQtyHargaDal :
        IInsertBulk<FakturQtyHargaModel>,
        IDelete<IFakturKey>,
        IListData<FakturQtyHargaModel, IFakturKey>
    {
    }
}