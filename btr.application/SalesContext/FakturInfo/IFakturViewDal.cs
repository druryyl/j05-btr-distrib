using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System.Collections;
using System.Collections.Generic;

namespace btr.application.SalesContext.FakturInfo
{
    public interface IFakturViewDal : 
        IListData<FakturView, Periode>
    {
        IEnumerable<FakturView> ListTerhapus(Periode periode);
    }
}
