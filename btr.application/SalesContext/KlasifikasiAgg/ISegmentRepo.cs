using btr.application.SupportContext.RoleFeature;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.domain.SupportContext.RoleFeature;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.KlasifikasiAgg
{
    public interface ISegmentRepo :
        ISaveChange<SegmentType>,
        IDeleteEntity<ISegmentKey>,
        ILoadEntity<SegmentType, ISegmentKey>
    {
        IEnumerable<SegmentType> ListData();
    }
}
