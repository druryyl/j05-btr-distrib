using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.KlasifikasiAgg
{
    public class SegmentType : ISegmentKey
    {
        public SegmentType(string segmentId, string segmentName)
        {
            SegmentName = segmentName;
            SegmentId = segmentId;
        }
        public static ISegmentKey Key(string id) => new SegmentType(id, string.Empty);
        public string SegmentId { get; private set; }
        public string SegmentName { get; private set; }
    }
    public interface ISegmentKey
    {
        string SegmentId { get; }
    }
}
