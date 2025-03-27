using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.HariRuteAgg
{
    public class HariRuteModel : IHariRuteKey
    {
        public HariRuteModel()
        {
        }
        public HariRuteModel(string id)
        {
            HariRuteId = id;
        }
        public string HariRuteId { get; set; }
        public string HariRuteName { get; set; }
        public string ShortName { get; set; }
    }

    public interface IHariRuteKey
    {
        string HariRuteId { get; }
    }

}
