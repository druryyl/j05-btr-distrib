using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.DriverFakturRpt
{
    public interface IDriverFakturInfoDal :
        IListData<DriverFakturDto, Periode, IDriverKey>
    {
    }

    public class DriverFakturDto
    {
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string FakturId { get; set; }
        public string FakturName { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Kota { get; set; }
    }
}
