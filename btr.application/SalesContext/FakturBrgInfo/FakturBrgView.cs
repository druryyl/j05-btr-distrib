using System;

namespace btr.application.SalesContext.FakturBrgInfo
{
    public class FakturBrgView
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerName { get; set; }
        public string KlasifikasiName { get; set; }
        public string WilayahName { get; set; }
        public string BrgName { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public decimal QtyJual { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }

    }
}
