using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.InventoryContext.WarehouseAgg;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace btr.infrastructure.InventoryContext.PackingOrderFeature
{
    public class PackingOrderDto 
    {
        public string PackingOrderId { get; set; }
        public DateTime PackingOrderDate { get; set; }

        // Flattened CustomerType properties
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public string NoTelp { get; set; }

        // Flattened FakturType properties
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string AdminName { get; set; }
        public decimal GrandTotal { get; set; }

        // Flattened LocationType properties
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }

        public string DriverId { get; set; }
        public string DriverName { get; set; }

        public static PackingOrderDto FromModel(PackingOrderModel model)
        {
            return new PackingOrderDto
            {
                PackingOrderId = model.PackingOrderId,
                PackingOrderDate = model.PackingOrderDate,

                CustomerId = model.Customer.CustomerId,
                CustomerCode = model.Customer.CustomerCode,
                CustomerName = model.Customer.CustomerName,
                Alamat = model.Customer.Alamat,
                NoTelp = model.Customer.NoTelp,
                Latitude = model.Location.Latitude,
                Longitude = model.Location.Longitude,
                Accuracy = model.Location.Accuracy,

                FakturId = model.Faktur.FakturId,
                FakturCode = model.Faktur.FakturCode,
                FakturDate = model.Faktur.FakturDate,
                AdminName = model.Faktur.AdminName,
                GrandTotal = model.Faktur.GrandTotal,

                DriverId = model.Driver.DriverId,
                DriverName = model.Driver.DriverName
            };
        }

        public PackingOrderModel ToModel(IEnumerable<PackingOrderItemModel> listItem,
            IEnumerable<DepoType> listDepo)
        {
            var customer = new CustomerReff(
                CustomerId, CustomerCode, CustomerName, Alamat, NoTelp);
            var faktur = new FakturReff(
                FakturId, FakturCode, FakturDate, AdminName, GrandTotal);
            var location = new LocationReff(
                Latitude, Longitude, Accuracy);
            var driver = new DriverReff(DriverId, DriverName);
            return new PackingOrderModel(
                PackingOrderId,
                PackingOrderDate,
                customer,
                location,
                faktur,
                driver,
                listItem,
                listDepo);
        }
    }
}
