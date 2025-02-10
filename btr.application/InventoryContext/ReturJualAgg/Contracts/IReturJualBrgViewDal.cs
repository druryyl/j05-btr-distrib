using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;

namespace btr.application.SalesContext.FakturBrgInfo
{
    public interface IReturJualBrgViewDal 
        : IListData<ReturJualBrgView, Periode>
    {
    }
    public class ReturJualBrgView
    {
        public string ReturJualId { get; set; }
        public DateTime ReturJualDate { get; set; }
        public string JenisRetur { get; set; }
        public string SalesName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address{ get; set; }
        public string WilayahName { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public int InPcs { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }

    }

}
