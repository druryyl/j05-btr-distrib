namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class CustomerType : ICustomerKey
    {
        public CustomerType(string customerId, string customerCode,
            string customerName, string alamat, string noTelp)
        {
            CustomerId = customerId;
            CustomerCode = customerCode;
            CustomerName = customerName;
            Alamat = alamat;
            NoTelp = noTelp;
        }

        public static CustomerType Default => new CustomerType(
            "-", "-", "-", "-", "-");

        public static ICustomerKey Key(string id)
        {
            var result = Default;
            result.CustomerId = id;
            return result;
        }

        public string CustomerId { get; private set; }
        public string CustomerCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Alamat { get; private set; }
        public string NoTelp { get; private set; }
    }

    public interface ICustomerKey
    {
        string CustomerId { get; }
    }
}

