﻿using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using btr.distrib.Helpers;
using btr.domain.SupportContext.UserAgg;

namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturPrintOutDto
    {
        public FakturPrintOutDto(FakturModel faktur, CustomerModel customer, UserModel user, bool isKlaim)
        {
            FakturCode = $"No.Faktur: {faktur.FakturCode}";
            FakturDate = $"Tgl: {faktur.FakturDate:dd MMMM yyyy}";
            CustomerId = $"Kepada Yth Customer-{faktur.CustomerId}";
            CustomerName = $"{faktur.CustomerName}";
            Address1 = $"{faktur.Address}";
            Address2 = customer.Address2.Length != 0 ?
                $"{customer.Address2}-{customer.Kota}" : $"{customer.Kota}";
            JatuhTempo = $"Tempo: {faktur.DueDate:dd-MM-yyyy}";
            SalesName = $"Sales: {faktur.SalesPersonName}";
            JenisBayar = $"Jenis: {faktur.TermOfPayment}";
            DriverName = $"{faktur.DriverName}";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Note = faktur.Note.Trim().Length == 0 ? "" : $"Note: {faktur.Note}";

            decimal dppProsen;
            if (!isKlaim)
            {
                SubTotal = $"{faktur.Total:N0}";
                Discount = $"{faktur.Discount:N0}";
                Total = $"{faktur.Total - faktur.Discount:N0}";
                Dpp = $"{faktur.ListItem.Sum(x => x.DppRp):N0}";
                dppProsen = faktur.ListItem.FirstOrDefault().DppProsen;
                Ppn = $"{faktur.ListItem.Sum(x => x.PpnRp):N0}";
                PpnProsen = $"PPN {DecFormatter.ToStr(faktur.ListItem.FirstOrDefault().PpnProsen)}% :";
                GrandTotal = $"{faktur.GrandTotal:N0}";
                Terbilang = $"Terbilang #{Math.Round(faktur.GrandTotal, 0).Eja()} rupiah#";
                Terbilang = textInfo.ToTitleCase(Terbilang);
            }
            else
            {
                SubTotal = $"{faktur.ListItemKlaim.Sum(x => x.SubTotal):N0}";
                Discount = $"{faktur.ListItemKlaim.Sum(x => x.DiscRp):N0}";
                Total = $"{faktur.ListItemKlaim.Sum(x => x.Total):N0}";
                Dpp = $"{faktur.ListItemKlaim.Sum(x => x.DppRp):N0}";
                dppProsen = faktur.ListItemKlaim.FirstOrDefault().DppProsen;
                Ppn = $"{faktur.ListItemKlaim.Sum(x => x.PpnRp):N0}";
                PpnProsen = $"PPN {DecFormatter.ToStr(faktur.ListItemKlaim.FirstOrDefault().PpnProsen)}% :";
                GrandTotal = $"{faktur.GrandTotalKlaim:N0}";
                Terbilang = $"Terbilang #{Math.Round(faktur.GrandTotalKlaim, 0).Eja()} rupiah#";
                Terbilang = textInfo.ToTitleCase(Terbilang);
            }


            if (dppProsen == 100M)
                DppProsen = $"DPP :";
            else 
                DppProsen = "DPP 11/12 :";

            UserName = user.UserName;

            ListItem = new List<FakturPrintOutItemDto>();
            var noUrut = 1;
            List<FakturItemModel>  listItem;
            if (isKlaim)
                listItem = faktur.ListItemKlaim;
            else
                listItem = faktur.ListItem;

            foreach(var item in listItem.OrderBy(x => x.NoUrut))
            {
                if (item.QtyBesar != 0 || item.QtyKecil != 0)
                {
                    var qtyBesar = item.QtyBesar == 0 ? "-      " :
                        $"{item.QtyBesar:N0} {item.SatBesar}";
                    var qtyKecil = item.QtyKecil == 0 ? "-      " :
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
                        HrgBesar = item.HrgSatBesar == 0 ? "-      " : $"{item.HrgSatBesar:N0}",
                        HrgKecil = item.HrgSatKecil == 0 ? "-      " : $"{item.HrgSatKecil:N0}",
                        Disc1 = disc1 == 0 ? "-" : $"{DecFormatter.ToStr(disc1)}%",
                        Disc2 = disc2 == 0 ? "-" : $"{DecFormatter.ToStr(disc2)}%",
                        Disc3 = disc3 == 0 ? "-" : $"{DecFormatter.ToStr(disc3)}%",
                        Disc4 = disc4 == 0 ? "-" : $"{DecFormatter.ToStr(disc4)}%",
                        Total = $"{(item.Total - item.PpnRp + item.DiscRp):N0}",
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
        public string Total { get; set; }
        public string Dpp { get; set; }
        public string DppProsen { get; set; }
        public string Ppn { get; set; }
        public string PpnProsen { get; set; }
        public string GrandTotal { get; set; }

        public string Terbilang { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }

        public List<FakturPrintOutItemDto> ListItem { get; set; }

    }
}
