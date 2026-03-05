using btr.application.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.nuna.Application;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.KlasifikasiAgg
{
    public partial class SegmentRepo : ISegmentRepo
    {
        private readonly ISegmentDal _segmentDal;

        public SegmentRepo(ISegmentDal segmentDal)
        {
            _segmentDal = segmentDal;
        }

        public void SaveChanges(SegmentType model)
        {
            var existing = LoadEntity(SegmentType.Key(model.SegmentId));
            if (existing.HasValue)
            {
                _segmentDal.Update(model);
            }
            else
            {
                _segmentDal.Insert(model);
            }

        }

        public void DeleteEntity(ISegmentKey key)
        {
            _segmentDal.Delete(key);
        }

        public MayBe<SegmentType> LoadEntity(ISegmentKey key)
        {
            var result = _segmentDal.GetData(key);
            if (result != null)
            {
                return MayBe<SegmentType>.Some(result);
            }
            else
            {
                return MayBe<SegmentType>.None;
            }
        }

        public IEnumerable<SegmentType> ListData()
        {
            return _segmentDal.ListData()?.ToList() 
                ?? new List<SegmentType>();
        }
    }
}
