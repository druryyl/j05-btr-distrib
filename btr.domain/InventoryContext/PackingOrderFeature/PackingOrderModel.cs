using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.WarehouseAgg;
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
        private readonly List<DepoType> _listDepo;

        public PackingOrderModel(
            string packingOrderId,
            DateTime packingOrderDate,
            CustomerReff customer,
            LocationReff location,
            FakturReff faktur,
            DriverReff driver,
            string note,
            IEnumerable<PackingOrderItemModel> listItem,
            IEnumerable<DepoType> listDepo)
        {
            PackingOrderId = packingOrderId;
            PackingOrderDate = packingOrderDate;
            Customer = customer;
            Location = location;
            Faktur = faktur;
            Driver = driver;
            Note = note;
            _listItem = listItem.ToList();
            _listDepo = listDepo.ToList();
        }

        public static PackingOrderModel CreateFromFaktur(FakturModel faktur, CustomerModel customer, 
            IEnumerable<FakturItemCatSupDepoView> listBrgDepo)
        {
            var newId = Ulid.NewUlid().ToString();
            var address = JoinNonEmpty(",", customer.Address1, customer.Address2, customer.Kota); 
            var customerReff = new CustomerReff(faktur.CustomerId, customer.CustomerCode, customer.CustomerName, address, customer.NoTelp);
            var location = new LocationReff(
                customer.Latitude, 
                customer.Longitude, 
                customer.Accuracy);
            var fakturReff = new FakturReff(faktur.FakturId, faktur.FakturCode, faktur.FakturDate, faktur.UserId, faktur.GrandTotal);
            var driverReff = new DriverReff(faktur.DriverId, faktur.DriverName);
            var listBrg = faktur.ListItem.Select((x,idx) =>
            {
                var depoView = listBrgDepo.FirstOrDefault(y => y.BrgId == x.BrgId) ?? FakturItemCatSupDepoView.Default;
                return new PackingOrderItemModel(idx,
                    new BrgReff(x.BrgId, x.BrgCode, x.BrgName, depoView.KategoriName, depoView.SupplierName),
                    new QtyType(x.QtyBesar, x.SatBesar),
                    new QtyType(x.QtyKecil + x.QtyBonus, x.SatKecil),
                    depoView.DepoId, depoView.DepoName);
            });

            var listDepo = listBrg
                .GroupBy(x => new { x.DepoId, x.DepoName })
                .Select(g => new DepoType(g.Key.DepoId, g.Key.DepoName))
                .ToList();
            var result = new PackingOrderModel(newId, DateTime.Now, customerReff, location, fakturReff, driverReff, faktur.Note, listBrg, listDepo);
            return result;
        }

        public static PackingOrderModel UpdateFromFaktur(PackingOrderModel packingOrder, FakturModel faktur, 
            CustomerModel customer, IEnumerable<FakturItemCatSupDepoView> listBrgDepo)
        {
            var result = CreateFromFaktur(faktur, customer, listBrgDepo);
            result.PackingOrderId = packingOrder.PackingOrderId;
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
            DriverReff.Default,
            "-",
            Enumerable.Empty<PackingOrderItemModel>(),
            Enumerable.Empty<DepoType>());

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
        public DriverReff Driver { get; private set; }
        public string Note { get; private set; }

        public IEnumerable<PackingOrderItemModel> ListItem => _listItem;
        public IEnumerable<DepoType> ListDepo => _listDepo;

        public void AddDepo(DepoType depo)
        {
            if (!_listDepo.Any(x => x.DepoId == depo.DepoId))
            {
                _listDepo.Add(depo);
            }
        }
    }

    public interface IPackingOrderKey
    {
        string PackingOrderId { get; }
    }
}
