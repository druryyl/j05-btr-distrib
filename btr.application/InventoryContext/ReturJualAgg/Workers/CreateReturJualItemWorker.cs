using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public class CreateReturJualItemRequest : IBrgKey
    {
        public CreateReturJualItemRequest(string brgId, 
            string customerId,
            string hrgInputStr, 
            string qtyInputStr, 
            string discInputStr,
            decimal ppnProsen) 
        {
            BrgId = brgId;
            CustomerId = customerId;

            HrgInputStr = hrgInputStr;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
            PpnProsen = ppnProsen;
        }
        public string BrgId { get; set; }
        public string CustomerId { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public interface ICreateReturJualItemWorker : INunaService<ReturJualItemModel, CreateReturJualItemRequest>
    {
    }

    public class CreateReturJualItemWorker : ICreateReturJualItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IFindHrgReturJualHandler _findHrgReturJualHandler;

        public CreateReturJualItemWorker(IBrgBuilder brgBuilder, 
            IFindHrgReturJualHandler findHrgReturJualHandler)
        {
            _brgBuilder = brgBuilder;
            _findHrgReturJualHandler = findHrgReturJualHandler;
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
            var brg = _brgBuilder.Load(req).Build();

            result.BrgCode = brg.BrgCode;
            result.BrgName = brg.BrgName;
            result = NormalizeInput(result, brg, req.CustomerId);
            result.ListQtyHrg = GetQtyHrg(result.QtyInputStr, result.HrgInputStr, brg).ToList();
            result.ListDisc = GenListDisc(result.DiscInputStr,
                result.ListQtyHrg.Sum(x => x.SubTotal),
                brg).ToList();

            result.QtyHrgDetilStr = result.ListQtyHrg
                .Select(x => $"{x.Qty:N0} {x.JenisQty} @ {x.HrgSat:N0}")
                .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
            result.DiscDetilStr = result.ListDisc.Count > 0 ?
                result.ListDisc
                    .Select(x => $"{x.DiscProsen:N0}% = {x.DiscRp:N0}")
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}"):
                string.Empty;

            result.Qty = result.ListQtyHrg.Sum(x => x.Qty * x.Conversion);
            result.HrgSat = result.ListQtyHrg.First(x => x.Conversion == 1).HrgSat;
            result.SubTotal = result.ListQtyHrg.Sum(x => x.SubTotal);
            result.DiscRp = result.ListDisc.Sum(x => x.DiscRp);
            result.PpnRp = result.SubTotal * req.PpnProsen / 100;
            result.Total = result.SubTotal - result.DiscRp + result.PpnRp;

            return result;
        }

        private IEnumerable<ReturJualItemQtyHrgModel> GetQtyHrg(string qtyInputStr,
            string hrgInputStr, BrgModel brgModel)
        {
            var listQty = qtyInputStr.Split(';').ToList();
            var listHrg = hrgInputStr.Split(';').ToList();
            var item1 = new ReturJualItemQtyHrgModel
            {
                NoUrut = 1,
                BrgId = brgModel.BrgId,
                JenisQty = JenisQtyEnum.SatuanBesar,
                Conversion = brgModel.ListSatuan.Max(x => x.Conversion),
                Qty = int.Parse(listQty[0]),
                HrgSat = decimal.Parse(listHrg[0]),
            };
            var item2 = new ReturJualItemQtyHrgModel
            {
                NoUrut = 2,
                BrgId = brgModel.BrgId,
                JenisQty = JenisQtyEnum.SatuanKecil,
                Conversion = 1,
                Qty = int.Parse(listQty[1]),
                HrgSat = decimal.Parse(listHrg[1])
            };
            var result = new List<ReturJualItemQtyHrgModel> { item1, item2 };
            result.ForEach(x => x.SubTotal = x.Qty * x.HrgSat);
            return result;
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
            var item2 = new ReturJualItemDiscModel(1, brg.BrgId, newSubTotal, discs[0], discRp[0]);
            newSubTotal -= discRp[1];

            discRp[2] = newSubTotal * discs[2] / 100;
            var item3 = new ReturJualItemDiscModel(1, brg.BrgId, newSubTotal, discs[0], discRp[0]);
            newSubTotal -= discRp[2];

            discRp[3] = newSubTotal * discs[3] / 100;
            var item4 = new ReturJualItemDiscModel(1, brg.BrgId, newSubTotal, discs[0], discRp[0]);


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
                : ParsingHrg(inputStr.HrgInputStr);

            //      qtyInput
            if (inputStr.HrgInputStr.IsNullOrEmpty())
                inputStr.HrgInputStr = "0;0";
            inputStr.QtyInputStr = ParsingQty(inputStr.QtyInputStr);

            //      discInput
            if (inputStr.DiscInputStr.IsNullOrEmpty())
                inputStr.DiscInputStr = "0;0;0;0";
            inputStr.DiscInputStr = ParsingDisc(inputStr.DiscInputStr);

            return inputStr;
        }

        private static string ParsingDisc(string discInputStr)
        {
            var listNumber = ParseStringMultiNumber(discInputStr, 4);
            var disc1 = listNumber[0];
            var disc2 = listNumber[1];
            var disc3 = listNumber[2];
            var disc4 = listNumber[3];
            var result = $"{disc1:N0};{disc2:N0};{disc3:N0};{disc4:N0}";
            return result;
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
                : $"{hrg.Harga:N0} * conversion;{hrg.Harga:N0}";
        }

        private static string ParsingHrg(string hargaInputStr)
        {
            var listNumber = ParseStringMultiNumber(hargaInputStr, 2);
            var hrgBesar = listNumber.OrderBy(x => x).First();
            var hrgKecil = listNumber.OrderBy(x => x).Last();
            var result = $"{hrgBesar:N0};{hrgKecil:N0}";
            return result;
        }

        private static string ParsingQty(string qtyInputStr)
        {
            var listNumber = ParseStringMultiNumber(qtyInputStr, 2);
            var qtyBesar = listNumber.OrderBy(x => x).First();
            var qtyKecil = listNumber.OrderBy(x => x).Last();
            var result = $"{qtyBesar:N0};{qtyKecil:N0}";
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
