using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using btr.distrib.Helpers;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturPrintOutDto
    {
        public FakturPrintOutDto(FakturModel faktur, CustomerModel customer)
        {
            FakturCode = $"No.Faktur: {faktur.FakturCode}";
            FakturDate = $"Tgl: {faktur.FakturDate:dd MMM yyyy}";
            CustomerId = $"Kepada Yth Customer-{faktur.CustomerId}";
            CustomerName = $"{faktur.CustomerName}";
            Address1 = $"{faktur.Address}";
            Address2 = customer.Address2.Length != 0 ?
                $"{customer.Address2}-{customer.Kota}" : $"{customer.Kota}";
            JatuhTempo = $"Tempo: {faktur.DueDate:dd MMM yyyy}";
            SalesName = $"Sales: {faktur.SalesPersonName}";
            JenisBayar = $"Jenis:{faktur.TermOfPayment}";
            DriverName = $"{faktur.DriverName}";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Terbilang = $"Terbilang #{Math.Round(faktur.GrandTotal, 0).Eja()} rupiah#";
            Terbilang = textInfo.ToTitleCase(Terbilang);

            Note = $"Note: {faktur.Note}";

            SubTotal = $"{faktur.Total:N0}";
            Discount = $"{faktur.Discount:N0}";
            Ppn = $"{faktur.ListItem.Sum(x => x.PpnRp):N0}";
            GrandTotal = $"{faktur.GrandTotal:N0}";
            UserName = faktur.UserId;

            ListItem = new List<FakturPrintOutItemDto>();
            var noUrut = 1;
            foreach(var item in faktur.ListItem)
            {
                if (item.QtyBesar != 0 || item.QtyKecil != 0)
                {
                    var qtyBesar = item.QtyBesar == 0 ? "-" :
                        $"{item.QtyBesar:N0} {item.SatBesar}";
                    var qtyKecil = item.QtyKecil == 0 ? "-" :
                        $"{item.QtyKecil:N0} {item.SatKecil}";
                    var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen ?? 0;
                    var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen ?? 0;
                    var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen ?? 0;
                    var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen ?? 0;
                    var newItem = new FakturPrintOutItemDto
                    {
                        NoUrut = $"{noUrut}",
                        BrgCode = item.BrgCode,
                        BrgName = item.BrgName,
                        QtyBesar = qtyBesar,
                        QtyKecil = qtyKecil,
                        HrgBesar = item.HrgSatBesar == 0 ? "-" : $"{item.HrgSatBesar:N0}",
                        HrgKecil = item.HrgSatKecil == 0 ? "-" : $"{item.HrgSatBesar:N0}",
                        Disc1 = disc1 == 0 ? "-" : $"{DecFormatter.ToStr(disc1)}%",
                        Disc2 = disc2 == 0 ? "-" : $"{DecFormatter.ToStr(disc2)}%",
                        Disc3 = disc3 == 0 ? "-" : $"{DecFormatter.ToStr(disc3)}%",
                        Disc4 = disc4 == 0 ? "-" : $"{DecFormatter.ToStr(disc4)}%",
                        Total = $"{DecFormatter.ToStr(item.Total)}",
                    };
                    ListItem.Add(newItem);
                    noUrut++;
                }

                if (item.QtyBonus != 0)
                {
                    var itemBonus = new FakturPrintOutItemDto
                    {
                        NoUrut = $"{noUrut}",
                        BrgCode = item.BrgCode,
                        BrgName = $"{item.BrgName}",
                        QtyBesar = "-",
                        QtyKecil = $"{item.QtyBonus:N0} {item.SatKecil}",
                        HrgBesar = "-",
                        HrgKecil = "-",
                        Disc1 = "-",
                        Disc2 = "-",
                        Disc3 = "-",
                        Disc4 = "-".PadLeft(8),
                        Total = "-",
                    };
                    ListItem.Add(itemBonus);
                    noUrut++;
                }
            }
        }

        public string FakturCode { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string JatuhTempo { get; set; }

        public string SalesName { get; set; }
        public string JenisBayar { get; set; }
        public string DriverName { get; set; }
        
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Ppn { get; set; }
        public string GrandTotal { get; set; }

        public string Terbilang { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }

        public List<FakturPrintOutItemDto> ListItem { get; set; }

    }

    public class FakturPrintOutItemDto
    {
        public string NoUrut { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string QtyBesar { get; set; }
        public string QtyKecil { get; set; }
        public string HrgBesar { get; set; }
        public string HrgKecil { get; set; }
        public string Disc1 { get; set; }
        public string Disc2 { get; set; }
        public string Disc3 { get; set; }
        public string Disc4 { get; set; }
        public string Total { get; set; }
    }
}
