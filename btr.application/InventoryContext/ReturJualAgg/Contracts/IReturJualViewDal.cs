using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualViewDal :
        IListData<ReturJualView, Periode>
    {
    }

    public class ReturJualView
    {
        public string ReturJualId { get; set; }
        public string ReturJualCode { get; set; }
        public DateTime ReturJualDate { get; set; }
        public string JenisRetur { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string WilayahName { get; set; }
        public string SalesName { get; set; }
        public string WarehouseName { get; set; }

        public decimal Total { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal GrandTotal { get; set; }
    }

}
