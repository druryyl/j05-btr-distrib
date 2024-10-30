using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public class CreateFakturItemRequest : IBrgKey
    {
        public CreateFakturItemRequest(string brgId, 
            string qtyInputStr, 
            string discInputStr,
            string hrgInputStr,
            decimal ppnProsen, 
            string hargaTypeId,
            string warehouseId) 
        {
            BrgId = brgId;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
            HrgInputStr = hrgInputStr;
            PpnProsen = ppnProsen;
            HargaTypeId = hargaTypeId;
            WarehouseId = warehouseId;
        }
        public string BrgId { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }
        public string HrgInputStr { get; set; }
        public decimal PpnProsen { get; set; }
        public string HargaTypeId { get; set; }
        public string WarehouseId { get; set; }
    }

    public interface ICreateFakturItemWorker : INunaService<FakturItemModel, CreateFakturItemRequest>
    {
    }

    public class CreateInvoiceItemWorker : ICreateFakturItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;

        public CreateInvoiceItemWorker(IBrgBuilder brgBuilder, 
            IStokBalanceBuilder stokBalanceBuilder)
        {
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
        }

        public FakturItemModel Execute(CreateFakturItemRequest req)
        {
            var brg = _brgBuilder.Load(req).Build();
            var stok = _stokBalanceBuilder.Load(brg).Build();

            var qtys = ParseStringMultiNumber(req.QtyInputStr, 3);
            var hrgs = ParseStringMultiNumber(req.HrgInputStr, 2);
            
            
            var item = new FakturItemModel
            {
                BrgId = req.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                QtyInputStr = req.QtyInputStr,
                HrgInputStr = req.HrgInputStr,
                
                QtyKecil = (int)qtys[1],
                SatKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty,
                //HrgSatKecil = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == req.HargaTypeId)?.Harga ?? 0,
                QtyBesar = (int)qtys[0],
                SatBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty,
                Conversion = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 0,
            };

            //  prioritas ambil hrgSat Kecil:
            //  1. dari input hrg kecil
            //  2. dari input hrg besar / conversion
            //  3. dari master barang
            var pembagi = item.Conversion == 0 ? 1 : item.Conversion;
            item.HrgSatKecil = hrgs[1] != 0
                ? hrgs[1]
                : hrgs[0] != 0
                    ? hrgs[0] / pembagi : 0M;
            item.HrgSatKecil = item.HrgSatKecil == 0
                ? brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == req.HargaTypeId)?.Harga ?? 0M
                : item.HrgSatKecil ;
            item.HrgSatBesar = item.HrgSatKecil * item.Conversion;
            item.HrgInputStr = $"{item.HrgSatBesar:N0}; {item.HrgSatKecil:N2}";

            //  jika cuman punya satu satuan, pindah qty jadi satuan kecil
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
            //  jika cuman punya satu harg, pindah qty jadi harga kecil
            if (item.Conversion == 0)
                if (item.HrgSatBesar > 0)
                    if (item.HrgSatKecil == 0)
                    {
                        item.HrgSatKecil = item.HrgSatBesar;
                        item.HrgSatBesar = 0;
                    }
                    else if (item.HrgSatKecil > 0)
                    {
                        item.HrgSatBesar = 0;
                    }
            

            item.QtyJual = (item.QtyBesar * item.Conversion) + item.QtyKecil;
            item.HrgSat = item.HrgSatKecil;
            item.SubTotal = item.QtyJual * item.HrgSat;

            item.QtyBonus = (int)qtys[2];
            item.QtyPotStok = item.QtyJual + item.QtyBonus;

            item.QtyDetilStr = GenQtyDetilStr(item);

            item.DiscInputStr = req.DiscInputStr ?? string.Empty;
            var listDisc = GenListDiscount(req.BrgId, item.SubTotal, req.DiscInputStr).ToList();
            item.DiscDetilStr = GenDiscDetilStr(listDisc);
            item.DiscRp = listDisc.Sum(x => x.DiscRp);
            item.ListDiscount = listDisc;

            item.PpnProsen = req.PpnProsen;
            item.PpnRp = (item.SubTotal - item.DiscRp) * req.PpnProsen / 100;
            item.Total = item.SubTotal - item.DiscRp + item.PpnRp;

            var stokKecil = stok.ListWarehouse.FirstOrDefault(x => x.WarehouseId == req.WarehouseId)?.Qty ?? 0;
            item.StokHargaStr = $"{stokKecil:N0} {item.SatKecil}@{item.HrgSatKecil:N0}";
            if (item.Conversion > 0)
            {
                var stokBesar = (int)(stokKecil / item.Conversion);
                stokKecil -= (stokBesar * item.Conversion);
                item.StokHargaStr = $"{stokBesar:N0} {item.SatBesar} @{item.HrgSatBesar:N0}{Environment.NewLine}";
                item.StokHargaStr += $"{stokKecil:N0} {item.SatKecil} @{item.HrgSatKecil:N0}";
            }
            item.QtyInputStr = $"{item.QtyBesar};{item.QtyKecil};{item.QtyBonus}";

            return item;
        }

        private string GenQtyDetilStr(FakturItemModel item)
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
            if (item.QtyBonus > 0)
                result += $"Bonus {item.QtyBonus} {item.SatKecil}";

            return result.TrimEnd('\n', '\r');
        }

        private static string GenDiscDetilStr(IReadOnlyCollection<FakturDiscountModel> listDisc)
        {
            var listString = new List<string>();
            foreach (var item in listDisc)
            {
                listString.Add($"{item.DiscRp:N0}");
            }
            
            //{
            //    $"{listDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscRp:N0 ?? 0}",
            //    $"{listDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscRp:N0 ?? 0}",
            //    $"{listDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscRp:N0 ?? 0}",
            //    $"{listDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscRp:N0 ?? 0}"
            //};
            var result = string.Join(Environment.NewLine, listString);
            return result;
        }

        private static IEnumerable<FakturDiscountModel> GenListDiscount(string brgId, decimal subTotal,
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

            var result = new List<FakturDiscountModel>
            {
                new FakturDiscountModel(1, brgId, discs[0], discRp[0]),
                new FakturDiscountModel(2, brgId, discs[1], discRp[1]),
                new FakturDiscountModel(3, brgId, discs[2], discRp[2]),
                new FakturDiscountModel(4, brgId, discs[3], discRp[3])
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
