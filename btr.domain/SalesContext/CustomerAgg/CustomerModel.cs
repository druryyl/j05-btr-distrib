using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.WilayahAgg;

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
        public string KlasidikasiName { get; set; }
        public string HargaTypeId { get; set; }
        public string HargaTypeName { get; set; }

        public string Address1 {get;set;}
        public string Address2 {get;set;}
        public string Kota {get;set;}
        public string KodePos {get;set;}
        public string NoTelp {get;set;}
        public string NoFax {get;set;}

        public string Npwp {get;set;}
        public string Nppkp {get;set;}
        public string NamaWp { get; set; }
        public string AddressWp { get;set;}
        public string AddressWp2 { get;set;}
        public bool IsKenaPajak {get;set;}

        public bool IsSuspend {get;set;}
        public decimal Plafond {get;set;}
        public decimal CreditBalance { get; set; }
    }
}