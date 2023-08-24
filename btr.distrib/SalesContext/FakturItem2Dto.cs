using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturItem2Dto
    {
        public FakturItem2Dto()
        {
            ListStokHargaSatuan = new List<FakturItem2DtoStokHargaSatuan>();
        }

        public string BrgId { get; set; }
        public string BrgName { get; private set; }
        public string StokHarga { get; private set; }
        public string Qty { get; set; }
        public string QtyDetil { get; private set;}
        public decimal SubTotal { get; private set; }

        public string Disc { get; set; }
        public string DiscRp { get; private set; }
        public decimal DiscTotal { get; private set; }
        public decimal Ppn { get; set; }
        public decimal PpnRp { get; private set; }
        public decimal Total { get; private set; }

        public List<FakturItem2DtoStokHargaSatuan> ListStokHargaSatuan { get; set; }


        public void SetBrgName(string name) => BrgName = name;
    }

    public class FakturItem2DtoStokHargaSatuan
    {
        public FakturItem2DtoStokHargaSatuan(int stok, decimal harga, string satuan)
        {
            Stok = stok;
            Harga = harga;
            Satuan = satuan;
        }
        public int Stok { get; set; }
        public decimal Harga { get; set; }
        public string Satuan { get; set; }
    }
}
