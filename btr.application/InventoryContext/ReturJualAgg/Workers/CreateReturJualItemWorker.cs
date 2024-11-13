using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Polly;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public class CreateReturJualItemRequest : IBrgKey
    {
        public CreateReturJualItemRequest(string brgId,
            string customerId,
            string hrgInputStr,
            string qtyInputStr,
            string discInputStr,
            decimal ppnProsen,
            string jenisRetur) 
        {
            BrgId = brgId;
            CustomerId = customerId;

            HrgInputStr = hrgInputStr;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
            PpnProsen = ppnProsen;
            JenisRetur = jenisRetur;
        }
        public string BrgId { get; set; }
        public string CustomerId { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }
        public decimal PpnProsen { get; set; }
        public string JenisRetur { get; set; }
    }

    public interface ICreateReturJualItemWorker : INunaService<ReturJualItemModel, CreateReturJualItemRequest>
    {
    }

    public class CreateReturJualItemWorker : ICreateReturJualItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IFindHrgReturJualHandler _findHrgReturJualHandler;
        private readonly IBrgDal _brgDal;

        public CreateReturJualItemWorker(IBrgBuilder brgBuilder, 
            IFindHrgReturJualHandler findHrgReturJualHandler, 
            IBrgDal brgDal)
        {
            _brgBuilder = brgBuilder;
            _findHrgReturJualHandler = findHrgReturJualHandler;
            _brgDal = brgDal;
        }

        public ReturJualItemModel Execute(CreateReturJualItemRequest req)
        {
            var result = new ReturJualItemModel
            {
                BrgId = req.BrgId,
                QtyInputStr = req.QtyInputStr,
                HrgInputStr = req.HrgInputStr,
                DiscInputStr = req.DiscInputStr,
            };

            //      load brg
            var fallback = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as BrgModel);
            var brg = fallback.Execute(() => _brgBuilder.Load(req).Build());
            if (brg == null)
            {
                if (req.BrgId.IsNullOrEmpty())
                    return result;
                var brg2 = _brgDal.GetData(req.BrgId);
                if (brg2 is null)
                    return result;

                brg = fallback.Execute(() => _brgBuilder.Load(brg2).Build())
                    ?? new BrgModel();
            }
            
            result.BrgId = brg.BrgId ?? string.Empty;
            result.BrgCode = brg.BrgCode ?? string.Empty;
            result.BrgName = brg.BrgName ?? string.Empty;
            result = NormalizeInput(result, brg, req.CustomerId);
            
            result.ListQtyHrg = GetQtyHrg(result.QtyInputStr, result.HrgInputStr, brg, req.JenisRetur).ToList();
            result.ListDisc = GenListDisc(result.DiscInputStr,
                result.ListQtyHrg.Sum(x => x.SubTotal),
                brg).ToList();

            result.QtyHrgDetilStr = result.ListQtyHrg
                .Select(x => $"{x.Qty:N0} {x.Satuan} @{x.HrgSat:N0}")
                .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
            result.DiscDetilStr = result.ListDisc.Count > 0 ?
                result.ListDisc
                    .Select(x => $"{x.DiscProsen}% = {x.DiscRp:N0}")
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}"):
                string.Empty;

            result.Qty = result.ListQtyHrg
                .Where(x => x.JenisQty == JenisQtyEnum.SatuanBesar || x.JenisQty == JenisQtyEnum.SatuanKecil)
                .Sum(x => x.Qty * x.Conversion);
            result.HrgSat = result.ListQtyHrg.First(x => x.Conversion == 1).HrgSat;
            result.SubTotal = result.ListQtyHrg.Sum(x => x.SubTotal);
            result.DiscRp = result.ListDisc.Sum(x => x.DiscRp);
            result.PpnRp = result.SubTotal * req.PpnProsen / 100;
            result.Total = result.SubTotal - result.DiscRp + result.PpnRp;

            return result;
        }

        private static IEnumerable<ReturJualItemQtyHrgModel> GetQtyHrg(string qtyInputStr,
            string hrgInputStr, BrgModel brgModel, string jenisRetur)
        {
            var listQty = qtyInputStr.Split(';').ToList();
            var listHrg = hrgInputStr.Split(';').ToList();
            
            listQty.ForEach(x => HilangkanKomaRibuan(x));
            listHrg.ForEach(x => HilangkanKomaRibuan(x));
            
            var item1 = new ReturJualItemQtyHrgModel
            {
                NoUrut = 1,
                BrgId = brgModel.BrgId,
                JenisQty = JenisQtyEnum.SatuanBesar,
                Conversion = brgModel.ListSatuan.Max(x => x.Conversion),
                Qty = int.Parse(listQty[0]),
                HrgSat = jenisRetur == "BAGUS" ? decimal.Parse(listHrg[0]) : 0,
                Satuan = brgModel.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty,
            };
            var item2 = new ReturJualItemQtyHrgModel
            {
                NoUrut = 2,
                BrgId = brgModel.BrgId,
                JenisQty = JenisQtyEnum.SatuanKecil,
                Conversion = 1,
                Qty = int.Parse(listQty[1]),
                HrgSat = jenisRetur == "BAGUS" ? decimal.Parse(listHrg[1]) : 0,
                Satuan = brgModel.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty,
            };


            var result = new List<ReturJualItemQtyHrgModel> { item1, item2};
            if (item1.Satuan == string.Empty)
                result.RemoveAll(x => x.JenisQty == JenisQtyEnum.SatuanBesar);
            
            result.ForEach(x => x.SubTotal = x.Qty * x.HrgSat);
            return result;

            string HilangkanKomaRibuan(string str)
            {
                // check current culture
                var culture = CultureInfo.CurrentCulture;
                var groupSeparator = culture.NumberFormat.NumberGroupSeparator;
                // remove group separator            
                var thisResult = str.Replace(groupSeparator, "");
                return thisResult;
            }
        }
        
        
        private static IEnumerable<ReturJualItemDiscModel> GenListDisc(string disccountString, 
            decimal subTotal, IBrgKey brg)
        {
            var discs = ParseStringMultiNumber(disccountString, 4);

            var discRp = new decimal[4];
            discRp[0] = subTotal * discs[0] / 100;
            var newSubTotal = subTotal - discRp[0];
            var item1 = new ReturJualItemDiscModel(1, brg.BrgId, subTotal, discs[0], discRp[0]);

            discRp[1] = newSubTotal * discs[1] / 100;
            var item2 = new ReturJualItemDiscModel(2, brg.BrgId, newSubTotal, discs[1], discRp[1]);
            newSubTotal -= discRp[1];

            discRp[2] = newSubTotal * discs[2] / 100;
            var item3 = new ReturJualItemDiscModel(3, brg.BrgId, newSubTotal, discs[2], discRp[2]);
            newSubTotal -= discRp[2];

            discRp[3] = newSubTotal * discs[3] / 100;
            var item4 = new ReturJualItemDiscModel(4, brg.BrgId, newSubTotal, discs[3], discRp[3]);


            var result = new List<ReturJualItemDiscModel>
            {
                item1, item2, item3, item4
            };
            result.RemoveAll(x => x.DiscProsen == 0);
            return result;
        }

        private ReturJualItemModel NormalizeInput(ReturJualItemModel inputStr, 
            BrgModel brg, string customerId)
        {
            //      hrgInput (jika diisi)
            if (inputStr.HrgInputStr.IsNullOrEmpty())
                inputStr.HrgInputStr = "0;0";
            inputStr.HrgInputStr = inputStr.HrgInputStr == "0;0" 
                ? GetHargaJualStr(brg, new CustomerModel(customerId)) 
                : ParsingHrg(inputStr.HrgInputStr, brg);

            //      qtyInput
            if (inputStr.HrgInputStr.IsNullOrEmpty())
                inputStr.HrgInputStr = "0;0";
            inputStr.QtyInputStr = ParsingQty(inputStr.QtyInputStr, brg);

            //      discInput
            if (inputStr.DiscInputStr.IsNullOrEmpty())
                inputStr.DiscInputStr = "0;0;0;0";
            inputStr.DiscInputStr = ParsingDisc(inputStr.DiscInputStr);

            return inputStr;
        }

        private static string ParsingDisc(string discInputStr)
        {
            var listNumber = ParseStringMultiNumber(discInputStr, 4);
            var disc1 = ConvertToString(listNumber[0]);
            var disc2 = ConvertToString(listNumber[1]);
            var disc3 = ConvertToString(listNumber[2]);
            var disc4 = ConvertToString(listNumber[3]);
            
            var result = $"{disc1};{disc2};{disc3};{disc4}";
            return result;

            string ConvertToString(decimal valDecimal)
            {
                if (valDecimal % 1 == 0)
                    return $"{valDecimal:N0}";
                
                // determining how many decimal places is valDecimal
                var str = valDecimal.ToString(CultureInfo.InvariantCulture);
                var decimalPlaces = str.Substring(str.IndexOf('.') + 1).Length;
                var format = decimalPlaces == 1 ? "N1" : "N2";
                return $"{valDecimal.ToString(format)}";
            }
        }

        private string GetHargaJualStr(BrgModel brg, ICustomerKey customerKey)
        {
            var hrg = _findHrgReturJualHandler.Execute(new FindHrgReturJualRequest(brg.BrgId, customerKey.CustomerId));

            var conversion = brg.ListSatuan
                .DefaultIfEmpty(new BrgSatuanModel{ Conversion = 1})
                .Max(x => x.Conversion);
            if (conversion == 0) conversion = 1;

            return conversion == 1 
                ? $"0;{hrg.Harga:N0}" 
                : $"{(hrg.Harga * conversion):N0} ;{hrg.Harga:N0}";
        }

        private static string ParsingHrg(string hargaInputStr, BrgModel brg)
        {
            var listNumber = ParseStringMultiNumber(hargaInputStr, 2);
            var hrgBesar = listNumber.OrderByDescending(x => x).First();
            var hrgKecil = listNumber.OrderByDescending(x => x).Last();
            
            var maxConv = brg.ListSatuan.Max(x => x.Conversion);
            var result = maxConv > 1
                ? GenDuaSatuan() 
                : GenSatuSatuan();
            return result;

            string GenDuaSatuan()
            {
                //  nilai lebih besar dianggap hrg besar
                hrgBesar = hrgKecil > hrgBesar ? hrgKecil : hrgBesar;
                //  hrg kecil dihitung berdasarkan hrg kecil-nya
                hrgKecil = hrgBesar / maxConv;
            
                return  $"{hrgBesar:N0};{hrgKecil:N0}";
            }

            string GenSatuSatuan()
            {
                //  jika terisi salah satu, ambil yang bukan nol
                hrgKecil = listNumber[0] != 0 
                    ? listNumber[0] 
                    : listNumber[1];
                
                //  jika terisi keduanya, ambil yang kecil
                if (listNumber[0] != 0 && listNumber[1] != 0)
                {
                    hrgKecil = listNumber[0] < listNumber[1] 
                        ? listNumber[0] 
                        : listNumber[1];
                }
                hrgBesar = 0;
                
                return  $"{hrgBesar:N0};{hrgKecil:N0}";
            }
        }

        private static string ParsingQty(string qtyInputStr, BrgModel brg)
        {
            var listNumber = ParseStringMultiNumber(qtyInputStr, 2);
            var qtyBesar = (int)listNumber[0];
            var qtyKecil = (int)listNumber[1];

            var maxConv = brg.ListSatuan.Max(x => x.Conversion);
            var result = maxConv > 1 
                ? GenDuaSatuan() 
                : GenSatuSatuan();
            return result;
            
            string GenDuaSatuan()
            {
                //  qty kecil yang melebihi conversion akan diubah ke qty besar   
                if (qtyKecil <= maxConv) 
                    return $"{qtyBesar};{qtyKecil}";
                
                var toQtyBesar = qtyKecil / maxConv;
                qtyKecil = qtyKecil - (toQtyBesar * maxConv);
                qtyBesar += toQtyBesar;
                return $"{qtyBesar};{qtyKecil}";
            }

            string GenSatuSatuan()
            {
                qtyKecil = qtyKecil != 0? qtyKecil : qtyBesar;
                qtyBesar = 0;
                return $"{qtyBesar};{qtyKecil}";
            }
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
