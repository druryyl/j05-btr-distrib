using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturItemDto
    {
        private string _qty;
        private string _disc;
        private decimal _ppn;

        private string _brgId;

        public FakturItemDto()
        {
            ListStokHargaSatuan = new List<FakturItem2DtoStokHargaSatuan>();
        }

        public string BrgId 
        { 
            get => _brgId; 
            set => _brgId = value;
        }

        public string Code { get; private set; }

        public string BrgName { get; private set; }
        public string StokHarga
        {
            get
            {
                return string.Join(Environment.NewLine,
                    ListStokHargaSatuan
                        .Select(x => $"{x.Stok} {x.Satuan} @{x.Harga:N0}"));
            }
        }
        public string Qty
        {
            get => _qty;
            set
            {
                _qty = value;
                ReCalc();
            }
        }        
        
        public string QtyDetil { get; private set;}
        public decimal SubTotal { get; private set; }

        public string Disc
        {
            get => _disc;
            set
            {
                _disc = value;
                ReCalc();
            }
        }
        public string DiscRp { get; private set; }
        public decimal DiscTotal { get; private set; }
        public decimal Ppn
        {
            get => _ppn;
            set
            {
                _ppn = value;
                ReCalc();
            }
        }        public decimal PpnRp { get; private set; }
        public decimal Total { get; private set; }

        public List<FakturItem2DtoStokHargaSatuan> ListStokHargaSatuan { get; set; }


        public void SetBrgName(string name) => BrgName = name;
        public void SetCode(string code) => Code = code;
        
        private static List<decimal> ParseStringMultiNumber(string str, int size)
        {
            var result = new List<decimal>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr)
            {
                if (x >= result.Count) break;

                if (decimal.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }
            return result;
        }
        private void ReCalcQty()
        {
            if (Qty is null) return;
            var qtys = ParseStringMultiNumber(Qty, 3);
            var satBesar = ListStokHargaSatuan.FirstOrDefault()?.Satuan ?? string.Empty;
            var satKecil = ListStokHargaSatuan.LastOrDefault()?.Satuan ?? string.Empty;
            QtyDetil = $"{(int)qtys[0]} {satBesar}\n{(int)qtys[1]} {satKecil}\nBonus {(int)qtys[2]} {satKecil}";
        }        
        
        private void ReCalcDisc()
        {
            if (Disc is null) return;
            var discs = ParseStringMultiNumber(Disc, 4);

            var discRp = new decimal[4];
            discRp[0] = SubTotal * discs[0] / 100;
            var newSubTotal = SubTotal - discRp[0];
            discRp[1] = newSubTotal * discs[1] / 100;
            newSubTotal -= discRp[1];
            discRp[2] = newSubTotal * discs[2] / 100;
            newSubTotal -= discRp[2];
            discRp[3] = newSubTotal * discs[3] / 100;
            DiscTotal = discRp[0] + discRp[1] + discRp[2] + discRp[3];

            var discFormated1 = discRp[0] == 0 ? "-" : $"{discRp[0]:#,##0.00}";
            var discFormated2 = discRp[1] == 0 ? "-" : $"{discRp[1]:#,##0.00}";
            var discFormated3 = discRp[2] == 0 ? "-" : $"{discRp[2]:#,##0.00}";
            var discFormated4 = discRp[3] == 0 ? "-" : $"{discRp[3]:#,##0.00}";
            var result = $"1: {discFormated1}\n";
            result += $"2: {discFormated2}\n";
            result += $"3: {discFormated3}\n";
            result += $"4: {discFormated4}";
            DiscRp = result;
        }    
        
        private void ReCalcSubTotal()
        {
            if (Qty is null) return;
            var qtys = ParseStringMultiNumber(Qty, 3);
            var result = qtys[0] * ListStokHargaSatuan.FirstOrDefault()?.Harga ?? 0;
            result += qtys[1] * ListStokHargaSatuan.LastOrDefault()?.Harga ?? 0;
            SubTotal = result;
        }        
        public void ReCalc()
        {
            ReCalcQty();
            ReCalcSubTotal();
            ReCalcDisc();
            PpnRp = (SubTotal - DiscTotal) * Ppn / 100;
            Total = SubTotal - DiscTotal + PpnRp;
        }        
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
