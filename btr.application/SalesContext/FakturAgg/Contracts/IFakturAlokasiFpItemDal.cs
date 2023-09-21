using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturAlokasiFpItemDal :
        IListData<FakturAlokasiFpItemView, Periode>
    {
        
    }
}