using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.distrib.SalesContext.FakturAgg;

namespace btr.distrib.InventoryContext.MutasiAgg
{
    public class MutasiPrintOutDto
    {
        public MutasiPrintOutDto(MutasiModel mutasi)
        {
            MutasiId = $"No.Mutasi: {mutasi.MutasiId}";
            MutasiDate = $"Tgl: {mutasi.MutasiDate:dd MMMM yyyy}";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Terbilang = $"Terbilang #{Math.Round(mutasi.NilaiSediaan, 0).Eja()} rupiah#";
            Terbilang = textInfo.ToTitleCase(Terbilang);

            SubTotal = $"{mutasi.NilaiSediaan:N0}";
            var disc = mutasi.ListItem.Sum(x => x.DiscRp);
            Discount = $"{disc:N0}";
            Total = $"{mutasi.NilaiSediaan - disc:N0}";
            UserName = mutasi.UserId;

            ListItem = new List<MutasiPrintOutItemDto>();
            var noUrut = 1;
            foreach (var item in mutasi.ListItem.OrderBy(x => x.NoUrut))
            {
                if (item.QtyBesar != 0 || item.QtyKecil != 0)
                {
                    var qtyBesar = item.QtyBesar == 0 ? "-      " :
                        $"{item.QtyBesar:N0} {item.SatBesar}";
                    var qtyKecil = item.QtyKecil == 0 ? "-      " :
                        $"{item.QtyKecil:N0} {item.SatKecil}";
                    var disc1 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen ?? 0;
                    var disc2 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen ?? 0;
                    var disc3 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen ?? 0;
                    var disc4 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen ?? 0;
                    var newItem = new MutasiPrintOutItemDto
                    {
                        NoUrut = $"{noUrut}",
                        BrgCode = item.BrgCode,
                        BrgName = item.BrgName,
                        QtyBesar = qtyBesar,
                        QtyKecil = qtyKecil,
                        HrgBesar = item.HppBesar == 0 ? "-      " : $"{item.HppBesar:N0}",
                        HrgKecil = item.Hpp == 0 ? "-      " : $"{item.Hpp:N0}",
                        Disc1 = disc1 == 0 ? "-" : $"{DecFormatter.ToStr(disc1)}%",
                        Disc2 = disc2 == 0 ? "-" : $"{DecFormatter.ToStr(disc2)}%",
                        Disc3 = disc3 == 0 ? "-" : $"{DecFormatter.ToStr(disc3)}%",
                        Disc4 = disc4 == 0 ? "-" : $"{DecFormatter.ToStr(disc4)}%",
                        Total = $"{item.NilaiSediaan:N0}",
                    };
                    ListItem.Add(newItem);
                    noUrut++;
                }
            }
        }

        public string MutasiId { get; set; }
        public string MutasiDate { get; set; }

        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }

        public string Terbilang { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }

        public List<MutasiPrintOutItemDto> ListItem { get; set; }
    }
}
