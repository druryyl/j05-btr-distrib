using btr.distrib.Helpers;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    public class ReturJualPrintOutDto
    {
        public ReturJualPrintOutDto(ReturJualModel retJual, CustomerModel customer)
        {
            ReturJualCode = $"No.Faktur: {retJual.ReturJualCode}";
            ReturJualDate = $"Tgl: {retJual.ReturJualDate:dd MMM yyyy}";
            CustomerId = $"Kepada Yth Customer-{retJual.CustomerId}";
            CustomerName = $"{customer.CustomerName}";
            Address1 = $"{customer.Address1}";
            Address2 = customer.Address2.Length != 0 ?
                $"{customer.Address2}-{customer.Kota}" : $"{customer.Kota}";
            SalesName = $"Sales: {retJual.SalesPersonName}";
            JenisRetur = $"Jenis Retur: BRG {retJual.JenisRetur}";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Terbilang = $"Terbilang #{Math.Round(retJual.GrandTotal, 0).Eja()} rupiah#";
            Terbilang = textInfo.ToTitleCase(Terbilang);

            SubTotal = $"{retJual.Total:N0}";
            Discount = $"{retJual.DiscRp:N0}";
            Ppn = $"{retJual.ListItem.Sum(x => x.PpnRp):N0}";
            PpnProsen = $"PPN {DecFormatter.ToStr(retJual.ListItem.FirstOrDefault().PpnProsen)}% :";
            GrandTotal = $"{retJual.GrandTotal:N0}";
            UserName = retJual.UserId;

            ListItem = new List<ReturJualPrintOutItemDto>();
            var noUrut = 1;
            foreach (var item in retJual.ListItem)
            {
                var qty = item.Qty == 0 ? "-" :
                    $"{item.Qty:N0} PCS";
                var disc1 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen ?? 0;
                var disc2 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen ?? 0;
                var disc3 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen ?? 0;
                var disc4 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen ?? 0;
                var newItem = new ReturJualPrintOutItemDto
                {
                    NoUrut = $"{noUrut}",
                    BrgCode = item.BrgCode,
                    BrgName = item.BrgName,
                    Qty = qty,
                    Hrg = item.HrgSat == 0 ? "-" : $"{DecFormatter.ToStr(item.HrgSat)}",
                    Disc1 = disc1 == 0 ? "-" : $"{DecFormatter.ToStr(disc1)}%",
                    Disc2 = disc2 == 0 ? "-" : $"{DecFormatter.ToStr(disc2)}%",
                    Disc3 = disc3 == 0 ? "-" : $"{DecFormatter.ToStr(disc3)}%",
                    Disc4 = disc4 == 0 ? "-" : $"{DecFormatter.ToStr(disc4)}%",
                    Total = $"{item.Total:N0}",
                };
                ListItem.Add(newItem);
                noUrut++;
            }
        }

        public string ReturJualCode { get; set; }
        public string ReturJualDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string SalesName { get; set; }
        public string JenisRetur { get; set; }

        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Ppn { get; set; }
        public string PpnProsen { get; set; }
        public string GrandTotal { get; set; }

        public string Terbilang { get; set; }
        public string UserName { get; set; }

        public List<ReturJualPrintOutItemDto> ListItem { get; set; }

    }

    public class ReturJualPrintOutItemDto
    {
        public string NoUrut { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string Qty { get; set; }
        public string Hrg { get; set; }
        public string Disc1 { get; set; }
        public string Disc2 { get; set; }
        public string Disc3 { get; set; }
        public string Disc4 { get; set; }
        public string Total { get; set; }
    }
}
