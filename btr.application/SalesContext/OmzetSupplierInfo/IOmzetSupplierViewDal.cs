using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System.Collections;
using System.Collections.Generic;

namespace btr.application.SalesContext.OmzetSupplierInfo
{
    public interface IOmzetSupplierViewDal :
        IListData<OmzetSupplierView, Periode>
    {
        IEnumerable<OmzetSupplierView> ListDataRetur(Periode periode);
    }
}
