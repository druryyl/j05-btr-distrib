using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.FakturPajak
{
    public class AlokasiFpModel : IAlokasiFpKey
    {
        public AlokasiFpModel()
        {
        }
        public AlokasiFpModel(string id)
        {
            AlokasiFpId = id;
        }
        public string AlokasiFpId { get; set; }
        public DateTime AlokasiFpDate { get; set; }

        public string NoAwal { get; set; }
        public string NoAkhir { get; set; }
        public int Kapasitas { get; set; }
        public int Sisa { get; set; }
        public List<AlokasiFpItemModel> ListItem { get; set; }
    }

    public interface IAlokasiFpKey
    {
        string AlokasiFpId { get; }
    }

    public class AlokasiFpItemModel : IAlokasiFpKey, IFakturKey, IFakturCode
    {
        public string AlokasiFpId { get;set;}
        public string NoFakturPajak { get; set; }
        public long NoUrut { get; set; }
        public string FakturId { get; set;}
        public string FakturCode { get; set;}


    }
}
