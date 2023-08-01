using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.application.InventoryContext.BrgAgg.UseCases;
using MediatR;
using Polly;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturItemDto
    {
        private string _brgId = string.Empty;
        private string _brgName = string.Empty;
        private string _disc = string.Empty;
        private string _discRp = string.Empty;
        private double _discTotal = 0;

        private string _qty = string.Empty;
        private int[] _qtyInt = new int[3];
        private string _qtyDetil = string.Empty;
        private double _subTotal = 0;
        private double _ppn = 0;
        private double _ppnRp = 0;
        private double _total = 0;

        private readonly IMediator _mediator;

        public FakturItemDto(IMediator mediator)
        {
            ListStokHargaSatuan = new List<FakturItemStokHargaSatuan>();
            _mediator = mediator;
        }

        public string BrgId
        {
            get => _brgId;
            set => BrgIdSetterAsync(value).Wait();
        }

        private async Task BrgIdSetterAsync(string value)
        {
            _brgId = value;

            var defBrgHarga = new GetBrgHargaResponse
            {
                BrgId = value,
                BrgName = string.Empty,
                ListSatuanHarga = new List<GetBrgHargaResponseSatuanHrg>()
            };
            var policy = Policy<GetBrgHargaResponse>
                .Handle<KeyNotFoundException>()
                .FallbackAsync(defBrgHarga);
            var query = new GetBrgHargaQuery(value);
            Task<GetBrgHargaResponse> queryTask() => _mediator.Send(query);
            var result = await policy.ExecuteAsync(queryTask);

            //var service = new GetBrgHargaService();
            //var response = service.Execute(_brgId);
            _brgName = result.BrgName;
            ListStokHargaSatuan = result.ListSatuanHarga
                .Select(x => new FakturItemStokHargaSatuan(x.Stok, x.HargaJual, x.Satuan))
                .ToList();
        }
        public string BrgName { get => _brgName; }

        public string StokHarga
        {
            get
            {
                return string.Join(Environment.NewLine,
                    ListStokHargaSatuan
                    .Select(x => $"{x.Stok} {x.Satuan} @{x.Harga}"));
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
        public string QtyDetil { get => _qtyDetil; }

        public double SubTotal { get => _subTotal; }

        public string Disc
        {
            get => _disc;
            set
            {
                _disc = value;
                ReCalc();
            }
        }
        public string DiscRp { get => _discRp; }
        public double DiscTotal { get => _discTotal; }
        public double Ppn
        {
            get => _ppn;
            set
            {
                _ppn = value;
                ReCalc();
            }
        }
        public double PpnRp { get => _ppnRp; }
        public double Total { get => _total; }
        public List<FakturItemStokHargaSatuan> ListStokHargaSatuan { get; set; }
        public void ReCalc()
        {
            ReCalcQty();
            ReCalcSubTotal();
            ReCalcDisc();
            _ppnRp = (SubTotal - DiscTotal) * Ppn / 100;
            _total = SubTotal - DiscTotal + PpnRp;
        }
        public void SetBrgName(string name) => _brgName = name;

        #region PRIVATE-HELPER
        private List<double> ParseStringMultiNumber(string str, int size)
        {
            var result = new List<double>();
            for (int i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr)
            {
                if (x >= result.Count) break;

                if (int.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }
            return result;
        }
        private void ReCalcQty()
        {
            var qtys = ParseStringMultiNumber(_qty, 3);
            _qtyInt[0] = (int)qtys[0];
            _qtyInt[1] = (int)qtys[1];
            _qtyInt[2] = (int)qtys[2];
            var satBesar = ListStokHargaSatuan.FirstOrDefault()?.Satuan ?? string.Empty;
            var satKecil = ListStokHargaSatuan.LastOrDefault()?.Satuan ?? string.Empty;
            _qtyDetil = $"{_qtyInt[0]} {satBesar}\n{_qtyInt[1]} {satKecil}\nBonus {_qtyInt[2]} {satKecil}";
        }

        private void ReCalcDisc()
        {
            var discs = ParseStringMultiNumber(_disc, 4);

            double[] discRp = new double[4];
            discRp[0] = SubTotal * discs[0] / 100;
            var newSubTotal = SubTotal - discRp[0];
            discRp[1] = newSubTotal * discs[1] / 100;
            newSubTotal -= discRp[1];
            discRp[2] = newSubTotal * discs[2] / 100;
            newSubTotal -= discRp[2];
            discRp[3] = newSubTotal * discs[3] / 100;
            _discTotal = discRp[0] + discRp[1] + discRp[2] + discRp[3];

            var discFormated1 = discRp[0] == 0 ? "-" : $"{discRp[0]:#,##0.00}";
            var discFormated2 = discRp[1] == 0 ? "-" : $"{discRp[1]:#,##0.00}";
            var discFormated3 = discRp[2] == 0 ? "-" : $"{discRp[2]:#,##0.00}";
            var discFormated4 = discRp[3] == 0 ? "-" : $"{discRp[3]:#,##0.00}";
            var result = $"1: {discFormated1}\n";
            result += $"2: {discFormated2}\n";
            result += $"3: {discFormated3}\n";
            result += $"4: {discFormated4}";
            _discRp = result;
        }
        private void ReCalcSubTotal()
        {
            var result = _qtyInt[0] * ListStokHargaSatuan.FirstOrDefault()?.Harga ?? 0;
            result += _qtyInt[1] * ListStokHargaSatuan.LastOrDefault()?.Harga ?? 0;
            _subTotal = result;
        }
        #endregion
    }

    public class FakturItemStokHargaSatuan
    {
        public FakturItemStokHargaSatuan(int stok, double harga, string satuan)
        {
            Stok = stok;
            Harga = harga;
            Satuan = satuan;
        }
        public int Stok { get; set; }
        public double Harga { get; set; }
        public string Satuan { get; set; }
    }

}
