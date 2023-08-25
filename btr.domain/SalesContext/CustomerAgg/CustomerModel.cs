namespace btr.domain.SalesContext.CustomerAgg
{
    public class CustomerModel : ICustomerKey
    {
        public CustomerModel()
        {
        }
        public CustomerModel(string id) => CustomerId = id;

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Plafond { get; set; }
        public double CreditBalance { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}