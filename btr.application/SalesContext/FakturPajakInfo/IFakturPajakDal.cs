using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturPajakInfo
{
    public interface IFakturPajakViewDal : IListData<FakturPajakView, Periode>
    {
    }

    public class FakturPajakView
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime Tgl { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public string Npwp { get; set; }
        public string NoFakturPajak { get; set; }

    }
}
