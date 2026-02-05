using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class PackingOrderModel : IPackingOrderKey
    {
        private readonly List<PackingOrderItemModel> _listItem;

        public PackingOrderModel(
            string packingOrderId,
            DateTime packingOrderDate,
            CustomerReff customer,
            LocationReff location,
            FakturReff faktur,
            IEnumerable<PackingOrderItemModel> listItem)
        {
            PackingOrderId = packingOrderId;
            PackingOrderDate = packingOrderDate;
            Customer = customer;
            Location = location;
            Faktur = faktur;
            _listItem = listItem.ToList();
        }

        public static PackingOrderModel CreateFromFaktur(FakturModel faktur, CustomerModel customer)
        {
            var newId = Ulid.NewUlid().ToString();
            var address = JoinNonEmpty(",", customer.Address1, customer.Address2, customer.Kota); 
            var customerReff = new CustomerReff(faktur.CustomerId, faktur.CustomerCode, faktur.CustomerName, address, customer.NoTelp);
            var location = new LocationReff(
                Convert.ToDecimal(customer.Latitude), 
                Convert.ToDecimal(customer.Longitude), 
                (int)customer.Accuracy);
            var fakturReff = new FakturReff(faktur.FakturId, faktur.FakturCode, faktur.FakturDate, faktur.UserId);
            var listBrg = faktur.ListItem.Select((x,idx) => new PackingOrderItemModel(idx, 
                new BrgReff(x.BrgId, x.BrgCode, x.BrgName), 
                new QtyType(x.QtyBesar, x.SatBesar), 
                new QtyType(x.QtyKecil + x.QtyBonus, x.SatKecil)));
            var result = new PackingOrderModel(newId, DateTime.Now, customerReff, location, fakturReff, listBrg);
            return result;
        }

        private static string JoinNonEmpty(string separator, params string[] values)
            => string.Join(separator, values.Where(v => !string.IsNullOrWhiteSpace(v)));

        public static PackingOrderModel Default => new PackingOrderModel(
            "-",
            new DateTime(3000, 1, 1),
            CustomerReff.Default,
            LocationReff.Default,
            FakturReff.Default,
            Enumerable.Empty<PackingOrderItemModel>());

        public static IPackingOrderKey Key(string id)
        {
            var result = Default;
            result.PackingOrderId = id;
            return result;
        }

        public string PackingOrderId { get; private set; }
        public DateTime PackingOrderDate { get; private set; }
        public string PackingOrderCode { get; private set; }
        public CustomerReff Customer { get; private set; }
        public FakturReff Faktur { get; private set; }
        public LocationReff Location { get; private set; }
        public IEnumerable<PackingOrderItemModel> ListItem => _listItem;
    }

    public interface IPackingOrderKey
    {
        string PackingOrderId { get; }
    }
}
