using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturCashRpt
{
    public interface IFakturCashViewDal :
        IListData<FakturCashView, Periode>
    {
    }

    public class FakturCashView
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime Tgl { get; set; }
        public string Admin { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string WilayahName { get; set; }
        public string SalesPersonName { get; set; }
        public string WarehouseName { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Cash { get; set; }
        public decimal KurangBayar { get; set; }
    }

}
