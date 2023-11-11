using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.PuchaseInfoRpt
{
    // create interface for IPurchaseInfoDal
    public interface IInvoiceInfoDal
        : IListData<InvoiceInfoDto, Periode>
    {
    }

    public class InvoiceInfoDto
    {
        public string InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime Tgl { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }
        public decimal Total { get; set; }
        public decimal Disc{ get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
