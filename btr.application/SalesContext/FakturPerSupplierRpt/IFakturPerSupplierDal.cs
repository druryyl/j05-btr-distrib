using System;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturPerSupplierRpt
{
    public interface  IFakturPerSupplierDal :
        IListData<FakturPerSupplierView, Periode>
    {
        
    }
    public class FakturPerSupplierView
    {
        public string SupplierName {get;set;}
        public string SupplierAddress {get;set;}
        public string SupplierKota {get;set;}
        public string FakturCode {get;set;}
        public DateTime FakturDate {get;set;}
        public string SalesPersonName {get;set;}
        public string CustomerCode {get;set;}
        public string CustomerName {get;set;}
        public string Klasifikasi {get;set; }
        public string CustomerAddress {get;set;}
        public string CustomerKota {get;set;}
        public string BrgCode {get;set;}
        public string BrgName {get;set;}
        public int QtyBesar {get;set;}
        public decimal HrgSatBesar {get;set;}
        public int QtyKecil {get;set;}
        public decimal HrgSatKecil {get;set;}
        public int QtyJual {get;set;}
        public decimal SubTotal {get;set;}
        public decimal DiscProsen1 {get;set;}
        public decimal DiscProsen2 {get;set;}
        public decimal DiscProsen3 {get;set;}
        public decimal DiscProsen4 {get;set;}
        public decimal TotalDisc {get;set;}
        public decimal TotalSebelumTax {get;set;}
        public decimal PpnRp {get;set;}
        public decimal Total {get;set;}
    }
    
}