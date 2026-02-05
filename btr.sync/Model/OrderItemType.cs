using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class OrderItemType : IOrderKey
    {
        public string OrderId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string KategoriName { get; set; }
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public int QtyBonus { get; set; }
        public int Konversi { get; private set; }
        public double UnitPrice { get; set; }
        public double Disc1 { get; set; }
        public double Disc2 { get; set; }
        public double Disc3 { get; set; }
        public double Disc4 { get; set; }

        public double LineTotal { get; set; }
    }
}
