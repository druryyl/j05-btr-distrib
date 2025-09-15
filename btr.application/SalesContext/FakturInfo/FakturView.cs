using System;

namespace btr.application.SalesContext.FakturInfo
{
    public class FakturView
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime Tgl { get; set; }
        public string Admin { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string WilayahName { get; set; }
        public string KlasifikasiName { get; set; }
        public string SalesPersonName { get; set; }
        public string WarehouseName { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public int StatusFaktur { get; set; }
        public bool Kembali => StatusFaktur == 2 ? true : false;
    }
}
