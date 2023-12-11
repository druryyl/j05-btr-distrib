using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.InvoiceHarianDetilRpt
{
    public interface IInvoiceHarianDetilDal : IListData<InvoiceHarianDetilView, Periode>
    {
    }

    public class InvoiceHarianDetilView
    {
        public string InvoiceCode {get;set;}
        public DateTime InvoiceDate {get;set;}
        public string SupplierName {get;set;}
        public string KategoriName {get;set;}
        public string BrgCode {get;set;}
        public string BrgName {get;set;}
        public int QtyBesar {get;set;}
        public string SatBesar {get;set;}
        public decimal HppSatBesar { get; set; }

        public int QtyKecil { get;set;}
        public string SatKecil {get;set;}
        public decimal HppSatKecil { get; set; }
        public int QtyBonus { get;set;}

        public decimal SubTotal {get;set;}
        public decimal DiscProsen1 {get;set;}
        public decimal DiscProsen2 {get;set;}
        public decimal DiscProsen3 {get;set;}
        public decimal DiscProsen4 {get;set;}
        public decimal TotalDisc {get;set;}
        public decimal TotalSebelumTax {get;set;}
        public decimal PpnRp {get;set;}
        public decimal Total { get; set; }
    }
}
