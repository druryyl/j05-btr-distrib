using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranModel : IFpKeluaranKey
    {
        public FpKeluaranModel()
        {
        }
        public FpKeluaranModel(string id)
        {
            FpKeluaranId = id;
        }
        public string FpKeluaranId { get; set; }
        public DateTime FpKeluaranDate { get; set; }
        public string UserId { get; set; }

        public List<FpKeluaranFakturModel> ListFaktur { get; set; }
    }
}
