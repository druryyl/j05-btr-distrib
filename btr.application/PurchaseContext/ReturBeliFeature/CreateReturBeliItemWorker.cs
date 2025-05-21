using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.Helpers;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Application;

namespace btr.application.PurchaseContext.ReturBeliAgg
{
    public class CreateReturBeliItemRequest : IBrgKey
    {
        public CreateReturBeliItemRequest(string brgId,
            string hrgInputStr,
            string qtyInputStr,
            string discInputStr,
            decimal dppProsen,
            decimal ppnProsen,
            bool isUbahHarga)
        {
            BrgId = brgId;
            HrgInputStr = hrgInputStr;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
            DppProsen = dppProsen;
            PpnProsen = ppnProsen;
            IsGetHarga = isUbahHarga;
        }
        public string BrgId { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }
        public decimal DppProsen { get; set; }
        public decimal PpnProsen { get; set; }
        public bool IsGetHarga { get; set; }
    }

    public interface ICreateReturBeliItemWorker : INunaService<ReturBeliItemModel, CreateReturBeliItemRequest>
    {
    }

    public class CreateReturBeliItemWorker : ICreateReturBeliItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;

        public CreateReturBeliItemWorker(IBrgBuilder brgBuilder,
            IStokBalanceBuilder stokBalanceBuilder)
        {
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
        }

        public ReturBeliItemModel Execute(CreateReturBeliItemRequest req)
        {
            var brg = _brgBuilder.Load(req).Build();
            var stok = _stokBalanceBuilder.Load(brg).Build();

            var qtys = ParseStringMultiNumber(req.QtyInputStr, 3);

            var item = new ReturBeliItemModel
            {
                BrgId = req.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                QtyInputStr = req.QtyInputStr,
                HrgInputStr = req.HrgInputStr,
                DiscInputStr = req.DiscInputStr,
            };

            if (req.IsGetHarga)
                item.HrgInputStr = GetCurrentHpp(brg);

            var hrgInputStr = item.HrgInputStr;
            GenHrgBeli(ref hrgInputStr, ref brg, out string hrgDetilStr,
                out decimal hppSatKecil, out decimal hppSatBesar);
            item.HrgDetilStr = hrgDetilStr;
            item.HppSatKecil = hppSatKecil;
            item.HppSatBesar = hppSatBesar;
            item.HppSat = hppSatKecil;
            item.HrgInputStr = hrgInputStr;

            item.QtyKecil = (int)qtys[1];
            item.SatKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

            item.QtyBesar = (int)qtys[0];
            item.SatBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty;
            item.Conversion = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 0;

            //  jika cuman punya satu satuan, pindah jadi satuan kecil
            if (item.Conversion == 0)
                if (item.QtyBesar > 0)
                    if (item.QtyKecil == 0)
                    {
                        item.QtyKecil = item.QtyBesar;
                        item.QtyBesar = 0;
                    }
                    else if (item.QtyKecil > 0)
                    {
                        item.QtyBesar = 0;
                    }

            item.QtyBeli = (item.QtyBesar * item.Conversion) + item.QtyKecil;
            item.SubTotal = item.QtyBeli * item.HppSat;

            item.QtyBonus = (int)qtys[2];
            item.QtyPotStok = item.QtyBeli + item.QtyBonus;

            item.QtyDetilStr = GenQtyDetilStr(item);

            item.DiscInputStr = req.DiscInputStr ?? string.Empty;
            var listDisc = GenListDisc(req.BrgId, item.SubTotal, req.DiscInputStr).ToList();
            item.DiscDetilStr = GenDiscDetilStr(listDisc);
            item.DiscRp = listDisc.Sum(x => x.DiscRp);
            item.ListDisc = listDisc;
            item.DppProsen = req.DppProsen;
            item.DppRp = (item.SubTotal - item.DiscRp) * req.DppProsen / 100;

            item.PpnProsen = req.PpnProsen;
            item.PpnRp = item.DppRp * req.PpnProsen / 100;
            item.Total = item.SubTotal - item.DiscRp + item.PpnRp;

            item.QtyInputStr = $"{item.QtyBesar};{item.QtyKecil};{item.QtyBonus}";

            return item;
        }

        private string GetCurrentHpp(BrgModel brg)
        {
            var hpp = brg.Hpp;
            var konversiSatuan = brg.ListSatuan
                .DefaultIfEmpty(new BrgSatuanModel { Conversion = 0 })
                .Max(x => x.Conversion);
            var hppBesar = hpp * konversiSatuan;
            var hppStr = DecFormatterHelper.ToStr(hpp);
            var hppBesarStr = DecFormatterHelper.ToStr(hppBesar);

            string result = $"{hppBesarStr};{hppStr}";

            return result;
        }

        private void GenHrgBeli(ref string hrgInputStr, ref BrgModel brg,
            out string hrgDetilStr, out decimal hppSatKecil, out decimal hppSatBesar)
        {
            var hrgs = ParseStringMultiNumber(hrgInputStr, 2);
            if (brg.ListSatuan.Count == 0)
                throw new ArgumentException("Barang ini belum punya satuan");
            var conversion = brg.ListSatuan.Max(x => x.Conversion);
            var satKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
            var satBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty;

            //  jika brg cuman punya 1 satuan
            if (conversion == 1)
            {
                //  jika terisi di 2 tempat, ambil yang kanan
                hppSatKecil = hrgs[1] > 0 ? hrgs[1] : hrgs[0];
                hppSatBesar = 0;
                hrgDetilStr = $"{hppSatKecil}/{satKecil}";
            }

            //  jika brg punya 2 satuan
            else
            {
                //  jika hrg besar terisi, maka jadikan acuan (satuan kecil di-replace)
                if (hrgs[0] > 0)
                    hrgs[1] = hrgs[0] / conversion;
                //  jika hrg besar kosong, maka sat kecil jadi acuan
                else
                    hrgs[0] = hrgs[1] * conversion;

                hppSatBesar = hrgs[0];
                hppSatKecil = hrgs[1];
                hrgDetilStr = $"{DecFormatterHelper.ToStr(hppSatBesar)}/{satBesar}{Environment.NewLine}";
                hrgDetilStr += $"{DecFormatterHelper.ToStr(hppSatKecil)}/{satKecil}";
            }
            hrgInputStr = $"{DecFormatterHelper.ToStr(hppSatBesar)};{DecFormatterHelper.ToStr(hppSatKecil)}";
        }

        private string GenQtyDetilStr(ReturBeliItemModel item)
        {
            var result = string.Empty;
            //  normalisasi; jika qty kecil lebih dari item.Conversion maka ubah jadi qty besar;
            if (item.Conversion > 0)
                if (item.QtyKecil > item.Conversion)
                {
                    var qtyBesarAdd = (int)(item.QtyKecil / item.Conversion);
                    item.QtyBesar += qtyBesarAdd;
                    item.QtyKecil -= (qtyBesarAdd * item.Conversion);
                }
            ;

            if (item.QtyBesar > 0)
                result = $"{item.QtyBesar} {item.SatBesar}{Environment.NewLine}";
            if (item.QtyKecil > 0)
                result += $"{item.QtyKecil} {item.SatKecil}{Environment.NewLine}";
            if (item.QtyBonus > 0)
                result += $"Bonus {item.QtyBonus} {item.SatKecil}";

            return result.TrimEnd('\n', '\r');
        }

        private static string GenDiscDetilStr(IReadOnlyCollection<ReturBeliDiscModel> listDisc)
        {
            var listString = new List<string>();
            foreach (var item in listDisc)
                listString.Add($"{item.DiscRp:N0}");

            var result = string.Join(Environment.NewLine, listString);
            return result;
        }

        private static IEnumerable<ReturBeliDiscModel> GenListDisc(string brgId, decimal subTotal,
            string disccountString)
        {
            var discs = ParseStringMultiNumber(disccountString, 4);

            var discRp = new decimal[4];
            discRp[0] = subTotal * discs[0] / 100;
            var newSubTotal = subTotal - discRp[0];
            discRp[1] = newSubTotal * discs[1] / 100;
            newSubTotal -= discRp[1];
            discRp[2] = newSubTotal * discs[2] / 100;
            newSubTotal -= discRp[2];
            discRp[3] = newSubTotal * discs[3] / 100;

            var result = new List<ReturBeliDiscModel>
            {
                new ReturBeliDiscModel(1, brgId, discs[0], discRp[0]),
                new ReturBeliDiscModel(2, brgId, discs[1], discRp[1]),
                new ReturBeliDiscModel(3, brgId, discs[2], discRp[2]),
                new ReturBeliDiscModel(4, brgId, discs[3], discRp[3])
            };
            result.RemoveAll(x => x.DiscProsen == 0);
            return result;
        }

        private static List<decimal> ParseStringMultiNumber(string str, int size)
        {
            if (str is null)
                str = string.Empty;

            var result = new List<decimal>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr.TakeWhile(item => x < result.Count))
            {
                if (decimal.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }

    }
}
