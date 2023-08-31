using btr.domain.SalesContext.WilayahAgg;

namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class SalesPersonModel : ISalesPersonKey, IWilayahKey
    {
        public SalesPersonModel(string id) => SalesPersonId = id;
        public SalesPersonModel()
        {
        }

        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string WilayahId { get; set; }
        public string WilayahName { get; set; }
    }
}