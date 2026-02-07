using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.WilayahAgg;
using System;
using System.Linq;

namespace btr.domain.SalesContext.CustomerAgg
{
    public class CustomerModel : ICustomerKey, IWilayahKey, IKlasifikasiKey, IHargaTypeKey
    {
        public CustomerModel()
        {
        }
        public CustomerModel(string id) => CustomerId = id;

        public string CustomerId {get;set;}
        public string CustomerName {get;set;}
        public string CustomerCode { get; set; }
        
        public string WilayahId {get;set;}
        public string WilayahName { get; set;}
        public string KlasifikasiId { get; set; }
        public string KlasifikasiName { get; set; }
        public string HargaTypeId { get; set; }
        public string HargaTypeName { get; set; }

        public string Address1 {get;set;}
        public string Address2 {get;set;}
        public string Kota {get;set;}
        public string KodePos {get;set;}
        public string NoTelp {get;set;}
        public string NoFax {get;set;}
        public string Email { get; set; }
        public string Nitku { get; set; }

        public string Npwp {get;set;}
        public string Nik { get; set; }
        public string Nppkp {get;set;}
        public string NamaWp { get; set; }
        public string AddressWp { get;set;}
        public string AddressWp2 { get;set;}
        public bool IsKenaPajak {get;set;}

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public DateTime CoordinateTimestamp { get; set; }
        public string CoordinateUser { get; set; }

        public string JenisIdentitasPajak { get; set; }
        public bool IsSuspend {get;set;}
        public decimal Plafond {get;set;}
        public decimal CreditBalance { get; set; }

        public CustomerReff ToReff()
            => new CustomerReff(CustomerId, CustomerCode, CustomerName,
                JoinNonEmpty(",", Address1, Address2, Kota), NoTelp);
        private static string JoinNonEmpty(string separator, params string[] values)
            => string.Join(separator, values.Where(v => !string.IsNullOrWhiteSpace(v)));

    }
}