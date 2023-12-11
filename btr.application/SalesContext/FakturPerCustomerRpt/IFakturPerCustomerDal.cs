using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturPerCustomerRpt
{
    public interface IFakturPerCustomerDal : IListData<FakturPerCustomerView, Periode>
    {
    }

    public class FakturPerCustomerView
    {
        public string FakturCode {get;set;}
        public DateTime FakturDate {get;set;}
        public DateTime DueDate {get;set;}
        public string SupplierCode {get;set;}
        public string SupplierName {get;set;}
        public string KategoriName {get;set;}
        public string BrgCode {get;set;}
        public string BrgName {get;set;}
        public string CustomerCode {get;set;}
        public string CustomerName {get;set;}
        public string CustomerAddress {get;set;}
        public string CustomerKota {get;set;}
        public string WilayahName {get;set;}
        public string SalesPersonName {get;set;}
        
        public int QtyBesar {get;set;}
        public string SatBesar {get;set;}
        public decimal HrgSatBesar {get;set;}
        public int QtyKecil {get;set;}
        public string SatKecil {get;set;}
        public decimal HrgSatKecil {get;set;}
        public int QtyBonus {get;set;}
        public int QtyPotStok {get;set;}

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
