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
    public interface IInvoiceBrgInfoDal
        : IListData<InvoiceBrgInfoDto, Periode>
    {
    }

    public class InvoiceBrgInfoDto
    {
        public string InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime Tgl { get; set; }
        public string BrgName { get; set; }
        public string BrgCode { get; set; }
        public string SupplierName { get; set; }
        public string Kategori { get; set; }

        public string Satuan { get; set; }
        public decimal Hpp { get; set; }
        public int Qty { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Disc { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
