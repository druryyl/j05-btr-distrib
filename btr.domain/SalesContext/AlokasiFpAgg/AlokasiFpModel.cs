using System;
using System.Collections.Generic;

namespace btr.domain.SalesContext.AlokasiFpAgg
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
}
