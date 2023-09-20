using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.FakturPajak
{
    public class NomorSeriFpModel : INomorSeriFpKey
    {
        public string NomorSeriFpId { get; set; }
        public DateTime NomorSeriFpDate { get; set; }

        public string NoAwal { get; set; }
        public string NoAkhir { get; set; }
        public List<NomorSeriFpItemModel> ListItem { get; set; }
    }

    public interface INomorSeriFpKey
    {
        string NomorSeriFpId { get; }
    }

    public class NomorSeriFpItemModel
    {
        public string NomorFpId {get;set;}
        public string NomorSeri { get; set; }
        public string FakturId { get; set;}
        public string FakturCode { get; set;}


    }
}
