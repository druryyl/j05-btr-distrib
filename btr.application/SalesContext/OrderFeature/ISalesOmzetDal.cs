using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.OrderFeature
{
    public interface ISalesOmzetDal :
        IListData<SalesOmzetView, Periode>
    {
    }

    public class SalesOmzetView
    {
        public string OrderId { get;set ; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerName { get; set; }
        public decimal FakturTotal { get; set; }
        public DateTime OmzetDate { get; set; }
        public string SalesPersonName { get; set; }
    }
}
