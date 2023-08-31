namespace btr.domain.SalesContext.WilayahAgg
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