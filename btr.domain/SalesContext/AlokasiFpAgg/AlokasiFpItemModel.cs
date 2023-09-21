using btr.domain.SalesContext.FakturAgg;

namespace btr.domain.SalesContext.FakturPajak
{
    public class AlokasiFpItemModel : IAlokasiFpKey, IFakturKey, IFakturCode
    {
        public string AlokasiFpId { get;set;}
        public string NoFakturPajak { get; set; }
        public long NoUrut { get; set; }
        public string FakturId { get; set;}
        public string FakturCode { get; set;}


    }
}