using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturItemDto
    {
        private string _brgId;
        private string _qty;
        private string _disc;
        private decimal _ppn;

        private int _qtyBesar;
        private string _satBesar ;
        private int _conversion ;
        private decimal _hrgSatBesar ;
        private int _qtyKecil ;
        private string _satKecil ;
        private decimal _hrgSatKecil ;
        private int _qtyJual ;
        private decimal _hrgSat ;
        private decimal _subTotal ;
        private int _qtyBonus ;
        private int _qtyPotStok ;

        private BrgModel _brgModel;


        private List<FakturItemDtoStokHargaSatuan> _listStokHargaSatuan;

        public FakturItemDto()
        {
            _listStokHargaSatuan = new List<FakturItemDtoStokHargaSatuan>();
        }

        public BrgModel Brg { get; set; }
        public int Stok { get; set; }
        public string HargaTypeId { get; set; }

        public string BrgId { get => Brg?.BrgId ?? string.Empty; }
        public string Code { get => Brg?.BrgCode ?? string.Empty; }
        public string BrgName { get => Brg?.BrgName ?? string.Empty; }
        public string StokHarga { get; private set; }

        public string Qty
        {
            get => _qty;
            set
            {
                _qty = value;
                ReCalc();
            }
        }        
        

        public string QtyDetilStr { get; private set;}

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
        }        
        public decimal PpnRp { get; private set; }
        public decimal Total { get; private set; }

        public List<FakturItemDtoStokHargaSatuan> ListStokHargaSatuan 
        { 
            get => _listStokHargaSatuan;
            set
            {
                _listStokHargaSatuan = value;
                SetStokHarga(string.Join(Environment.NewLine,
                    _listStokHargaSatuan
                    .Select(x => $"{x.Stok} {x.Satuan} @{x.Harga:N0}")));
            }
        }

        public void SetStokHarga(string stokHarga) => StokHarga = stokHarga;
        
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
            var satBesar = Brg?.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty;
            var satKecil = Brg?.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
            var conversion = Brg?.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 0;
            _qtyBesar = (int)qtys[0];
            _qtyKecil = (int)qtys[1];
            _qtyBonus = (int)qtys[2];

            //  normalisasi; jika qty kecil lebih dari conversion maka ubah jadi qty besar;
            if (conversion > 0)
                if (_qtyKecil > conversion)
                {
                    var qtyBesarAdd = (int)(_qtyKecil / conversion);
                    _qtyBesar += qtyBesarAdd;
                    _qtyKecil -= (qtyBesarAdd * conversion);
                }

            QtyDetilStr = string.Empty;
            if (_qtyBesar > 0)
                QtyDetilStr = $"{_qtyBesar} {satBesar}";
            if (_qtyKecil > 0)
                QtyDetilStr += $"{Environment.NewLine}{_qtyKecil} {satKecil}";
            if (_qtyBonus > 0)
                QtyDetilStr = $"{Environment.NewLine}nBonus {_qtyBonus} {satKecil}";
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

            var result = string.Empty;
            if (discRp[0] > 0)
                result += $"1: {discFormated1}";
            if (discRp[1] > 0)
                result += $"{Environment.NewLine}2: {discFormated2}";
            if (discRp[2] > 0)
                result += $"{Environment.NewLine}3: {discFormated3}";
            if (discRp[3] > 0)
                result += $"{Environment.NewLine}4: {discFormated4}";
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

    public class FakturItemDtoStokHargaSatuan
    {
        public FakturItemDtoStokHargaSatuan(int stok, decimal harga, string satuan)
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
