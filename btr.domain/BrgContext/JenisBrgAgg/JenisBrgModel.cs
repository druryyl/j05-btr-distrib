using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.BrgContext.JenisBrgAgg
{
    public class JenisBrgModel : IJenisBrgKey
    {
        public JenisBrgModel()
        {
        }

        public JenisBrgModel(string id) => JenisBrgId = id;

        public string JenisBrgId { get; set; }
        public string JenisBrgName { get; set; }
    }
}
