using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.InvoiceAgg.Workers
{
    public class CreateInvoiceItemRequest : IBrgKey
    {
        public CreateInvoiceItemRequest(string brgId, 
            string qtyInputStr, 
            string discInputStr, 
            decimal ppnProsen, 
            string warehouseId) 
        {
            BrgId = brgId;
            QtyInputStr = qtyInputStr;
            DiscInputStr = discInputStr;
            PpnProsen = ppnProsen;
            WarehouseId = warehouseId;
        }
        public string BrgId { get; set; }
        public string QtyInputStr { get; set; }
        public string DiscInputStr { get; set; }
        public decimal PpnProsen { get; set; }
        public string WarehouseId { get; set; }
    }

    public interface ICreateInvoiceItemWorker : INunaService<InvoiceItemModel, CreateInvoiceItemRequest>
    {
    }

    public class CreateInvoiceItemWorker : ICreateInvoiceItemWorker
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;

        public CreateInvoiceItemWorker(IBrgBuilder brgBuilder, 
            IStokBalanceBuilder stokBalanceBuilder)
        {
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
        }

        public InvoiceItemModel Execute(CreateInvoiceItemRequest req)
        {
            var brg = _brgBuilder.Load(req).Build();
            var stok = _stokBalanceBuilder.Load(brg).Build();

            var qtys = ParseStringMultiNumber(req.QtyInputStr, 3);

            var item = new InvoiceItemModel
            {
                BrgId = req.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                QtyInputStr = req.QtyInputStr,
            };
            
            item.QtyKecil = (int)qtys[1];
            item.SatKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
            item.HppSatKecil = brg.Hpp;

            item.QtyBesar = (int)qtys[0];
            item.SatBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Satuan ?? string.Empty;
            item.Conversion = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 0;
            item.HppSatBesar = item.HppSatKecil * item.Conversion;

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
            item.HppSat = item.HppSatKecil;
            item.SubTotal = item.QtyBeli * item.HppSat;

            item.QtyBonus = (int)qtys[2];
            item.QtyPotStok = item.QtyBeli + item.QtyBonus;

            item.QtyDetilStr = GenQtyDetilStr(item);

            item.DiscInputStr = req.DiscInputStr ?? string.Empty;
            var listDisc = GenListDisc(req.BrgId, item.SubTotal, req.DiscInputStr).ToList();
            item.DiscDetilStr = GenDiscDetilStr(listDisc);
            item.DiscRp = listDisc.Sum(x => x.DiscRp);
            item.ListDisc = listDisc;

            item.PpnProsen = req.PpnProsen;
            item.PpnRp = (item.SubTotal - item.DiscRp) * req.PpnProsen / 100;
            item.Total = item.SubTotal - item.DiscRp + item.PpnRp;

            var stokKecil = stok.ListWarehouse.FirstOrDefault(x => x.WarehouseId == req.WarehouseId)?.Qty ?? 0;
            item.StokHargaStr = $"{stokKecil:N0} {item.SatKecil}@{item.HppSatKecil:N0}";
            int stokBesar = 0;
            if (item.Conversion > 0)
            {
                stokBesar = (int)(stokKecil / item.Conversion);
                stokKecil -= (stokBesar * item.Conversion);
                item.StokHargaStr = $"{stokBesar:N0} {item.SatBesar} @{item.HppSatKecil:N0}{Environment.NewLine}";
                item.StokHargaStr += $"{stokKecil:N0} {item.SatKecil} @{item.HppSatBesar:N0}";
            }
            item.QtyInputStr = $"{item.QtyBesar};{item.QtyKecil};{item.QtyBonus}";

            return item;
        }

        private string GenQtyDetilStr(InvoiceItemModel item)
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

        private static string GenDiscDetilStr(IReadOnlyCollection<InvoiceDiscModel> listDisc)
        {
            var listString = new List<string>();
            foreach (var item in listDisc)
                listString.Add($"{item.DiscRp:N0}");

            var result = string.Join(Environment.NewLine, listString);
            return result;
        }

        private static IEnumerable<InvoiceDiscModel> GenListDisc(string brgId, decimal subTotal,
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

            var result = new List<InvoiceDiscModel>
            {
                new InvoiceDiscModel(1, brgId, discs[0], discRp[0]),
                new InvoiceDiscModel(2, brgId, discs[1], discRp[1]),
                new InvoiceDiscModel(3, brgId, discs[2], discRp[2]),
                new InvoiceDiscModel(4, brgId, discs[3], discRp[3])
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
