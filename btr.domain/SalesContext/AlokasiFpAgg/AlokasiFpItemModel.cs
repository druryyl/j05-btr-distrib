using btr.domain.SalesContext.FakturAgg;

namespace btr.domain.SalesContext.AlokasiFpAgg
{
    public class AlokasiFpItemModel : IAlokasiFpKey, IFakturKey, IFakturCode, INoFakturPajak
    {
        public AlokasiFpItemModel()
        {
        }
        public AlokasiFpItemModel(INoFakturPajak noFakturPajak)
        {
            NoFakturPajak = noFakturPajak.NoFakturPajak;
        }
        public string AlokasiFpId { get;set;}
        public string NoFakturPajak { get; set; }
        public long NoUrut { get; set; }
        public string FakturId { get; set;}
        public string FakturCode { get; set;}
        
        public string Npwp { get; set; }
    }
}