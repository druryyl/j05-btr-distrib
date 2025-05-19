using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using System;
using System.Collections.Generic;

namespace btr.domain.PurchaseContext.InvoiceAgg
{
    public class ReturBeliItemModel : IReturBeliKey, IBrgKey
    {
        public string ReturBeliId { get; set; }
        public string ReturBeliItemId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }

        public string HrgInputStr { get; set; }
        public string HrgDetilStr { get; set; }

        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; set; }

        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int Conversion { get; set; }
        public decimal HppSatBesar { get; set; }

        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HppSatKecil { get; set; }

        public int QtyBeli { get; set; }
        public decimal HppSat { get; set; }
        public decimal SubTotal { get; set; }

        public int QtyBonus { get; set; }
        public int QtyPotStok { get; set; }

        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; set; }
        public decimal DiscRp { get; set; }

        public decimal DppProsen { get; set; }
        public decimal DppRp { get; set; }
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }

        public List<InvoiceDiscModel> ListDisc { get; set; }
    }


    public class PenjelasanReturVsThrow
    {
        public void Method1()
        {
            Console.WriteLine("Method2 called");
            Method2();
            Method3();
        }

        public void Method2()
        {
            return;
            Console.WriteLine("Method2 called");
        }
        public void Method3() {
            Console.WriteLine("Method3 called");
        }
    }
}