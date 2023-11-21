using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturBrgInfo
{
    public interface IFakturBrgViewDal 
        : IListData<FakturBrgView, Periode>
    {
    }
}
