namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class WilayahModel : IWilayahKey
    {
        public WilayahModel(string id) => WilayahId = id;
        public WilayahModel()
        {
        }

        public string WilayahId { get; set; }
        public string WilayahName { get; set; }
    }
}