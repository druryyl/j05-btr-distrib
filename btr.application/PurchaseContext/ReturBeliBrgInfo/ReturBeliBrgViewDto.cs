using System;
using System.Globalization;

namespace btr.application.PurchaseContext.InvoiceBrgInfo
{
    public class ReturBeliBrgViewDto
    {
        private string _discProsen;
        public string ReturBeliId { get; set; }
        public string ReturBeliCode { get; set; }
        public DateTime Tgl { get; set; }
        public string BrgName { get; set; }
        public string BrgCode { get; set; }
        public string SupplierName { get; set; }
        public string Kategori { get; set; }
        public string Warehouse { get; set; }

        public decimal Hpp { get; set; }
        public int QtyBesar { get; set; }
        public string SatuanBesar { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal SubTotal { get; set; }
        
        public string DiscProsen 
        { 
            get => _discProsen; 
            set 
            { 
                _discProsen = value;
                var listDisc = DiscProsen?.Split(';') ?? Array.Empty<string>();
                Disc1 = listDisc.Length > 0 ? decimal.TryParse(listDisc[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var disc1) ? disc1 : 0 : 0;
                Disc2 = listDisc.Length > 1 ? decimal.TryParse(listDisc[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var disc2) ? disc2 : 0 : 0;
                Disc3 = listDisc.Length > 2 ? decimal.TryParse(listDisc[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var disc3) ? disc3 : 0 : 0;
                Disc4 = listDisc.Length > 3 ? decimal.TryParse(listDisc[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var disc4) ? disc4 : 0 : 0;
            } 
        }

        public decimal Disc1 { get; private set; }
        public decimal Disc2 { get; private set; }
        public decimal Disc3 { get; private set; }
        public decimal Disc4 { get; private set; }




        public decimal Disc { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}