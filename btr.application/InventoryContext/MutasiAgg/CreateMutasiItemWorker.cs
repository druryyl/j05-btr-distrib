using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace btr.application.InventoryContext.MutasiAgg
{
    public class CreateMutasiItemRequest : IBrgKey, IWarehouseKey
    {
        public CreateMutasiItemRequest(string brgId, string warehouseId, string qtyInputStr, string discInputStr)
        {
            BrgId = brgId;
            WarehouseId = warehouseId;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
        }

        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }

    }

    public interface ICreateMutasiItemWorker : INunaService<MutasiItemModel, CreateMutasiItemRequest>
    {
    }
    public class CreateMutasiItemWorker : ICreateMutasiItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;

        public CreateMutasiItemWorker(IBrgBuilder brgBuilder, 
            IStokBalanceBuilder stokBalanceBuilder)
        {
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
        }

        public MutasiItemModel Execute(CreateMutasiItemRequest req)
        {
            var brg = _brgBuilder.Load(req).Build();
            var stok = _stokBalanceBuilder.Load(req).Build();
            var qtys = ParseStringMultiNumber(req.QtyInputStr, 2);

            var item = new MutasiItemModel
            {
                BrgId = req.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                QtyInputStr = req.QtyInputStr,
                DiscInputStr = req.DiscInputStr,
            };

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
            item.QtyDetilStr = GenQtyDetilStr(item);
            item.QtyInputStr = $"{item.QtyBesar};{item.QtyKecil}";
            item.Qty = (item.QtyBesar * item.Conversion) + item.QtyKecil;
            
            //  stok
            var stokInPcs = stok.ListWarehouse.FirstOrDefault(x => x.WarehouseId == req.WarehouseId)?.Qty ?? 0;
            if (item.Conversion == 0)
            {
                item.StokBesar = 0;                
                item.StokKecil = stokInPcs;
                item.StokDetilStr = $"{stokInPcs} {item.SatKecil}";
            }
            else
            {
                item.StokBesar = (int)(stokInPcs / item.Conversion);
                item.StokKecil = stokInPcs % item.Conversion;
                item.StokDetilStr = $"{item.StokBesar} {item.SatBesar}{Environment.NewLine}";
                item.StokDetilStr += $"{item.StokKecil} {item.SatKecil}";
            }
            
            item.Hpp = brg.Hpp;
            if (item.Conversion == 0)
            {
                item.HppBesar = 0;
                item.HppKecil = item.Hpp;
                item.HppDetilStr = $"{item.Hpp:N0}";
            }
            else
            {
                item.HppBesar = item.Hpp * item.Conversion;
                item.HppKecil = item.Hpp;
                item.HppDetilStr = $"{item.HppBesar:N0}{Environment.NewLine}{item.Hpp:N0}";
            }

            //  discount
            var discInputStr = UbahKomaJadiTitik(req.DiscInputStr);
            item.DiscInputStr = discInputStr;
            var listDisc = GenListDiscount(req.BrgId, item.Qty * item.Hpp, item.DiscInputStr).ToList();
            item.DiscDetilStr = GenDiscDetilStr(listDisc);
            item.DiscRp = listDisc.Sum(x => x.DiscRp);
            item.ListDisc = listDisc;

            item.Sat = item.SatKecil;
            item.NilaiSediaan = (item.Qty * item.Hpp) - item.DiscRp;
            return item;
        }

        private string UbahKomaJadiTitik(string discInputStr)
        {
            if (discInputStr is null)
                return string.Empty;

            var result = discInputStr.Replace(',', '.');
            return result;
        }

        private static IEnumerable<MutasiDiscModel> GenListDiscount(string brgId, decimal subTotal,
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

            var result = new List<MutasiDiscModel>
            {
                new MutasiDiscModel(1, brgId, discs[0], discRp[0]),
                new MutasiDiscModel(2, brgId, discs[1], discRp[1]),
                new MutasiDiscModel(3, brgId, discs[2], discRp[2]),
                new MutasiDiscModel(4, brgId, discs[3], discRp[3])
            };
            result.RemoveAll(x => x.DiscProsen == 0);
            return result;
        }
        private static string GenDiscDetilStr(IReadOnlyCollection<MutasiDiscModel> listDisc)
        {
            var listString = new List<string>();
            foreach (var item in listDisc)
            {
                listString.Add($"{item.DiscRp:N0}");
            }
            var result = string.Join(Environment.NewLine, listString);
            return result;
        }

        private static string GenQtyDetilStr(MutasiItemModel item)
        {
            var result = string.Empty;
            //  normalisasi; jika qty kecil lebih dari item.Conversion maka ubah jadi qty besar;
            if (item.Conversion > 0)
                if (item.QtyKecil > item.Conversion)
                {
                    var qtyBesarAdd = (int)(item.QtyKecil / item.Conversion);
                    item.QtyBesar += qtyBesarAdd;
                    item.QtyKecil -= (qtyBesarAdd * item.Conversion);
                };

            if (item.QtyBesar > 0)
                result = $"{item.QtyBesar} {item.SatBesar}{Environment.NewLine}";
            if (item.QtyKecil > 0)
                result += $"{item.QtyKecil} {item.SatKecil}{Environment.NewLine}";

            return result.TrimEnd('\n', '\r');
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
                if (decimal.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }
    }
}