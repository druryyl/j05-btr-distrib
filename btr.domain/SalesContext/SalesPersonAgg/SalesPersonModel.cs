namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class SalesPersonModel : ISalesPersonKey
    {
        public SalesPersonModel(string id) => SalesPersonId = id;
        public SalesPersonModel()
        {
        }

        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }
}