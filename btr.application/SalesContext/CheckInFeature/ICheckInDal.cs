using btr.domain.SalesContext.CheckInFeature;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.CheckInFeature
{
    public interface ICheckInDal :
        IInsert<CheckInModel>,
        IUpdate<CheckInModel>,
        IDelete<ICheckInKey>,
        IGetData<CheckInModel, ICheckInKey>,
        IListData<CheckInModel, Periode>
    {
    }
}
