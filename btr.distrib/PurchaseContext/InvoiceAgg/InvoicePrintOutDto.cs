using btr.distrib.Helpers;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace btr.distrib.PurchaseContext.InvoiceAgg
{
    public class InvoicePrintOutDto
    {
        public InvoicePrintOutDto(InvoiceModel invoice, SupplierModel supplier)
        {
            InvoiceCode = $"No.Invoice: {invoice.InvoiceCode}";
            InvoiceDate = $"Tgl: {invoice.InvoiceDate:dd MMMM yyyy}";
            SupplierId = $"Dari Prinsipal {invoice.SupplierId}";
            SupplierName = $"{invoice.SupplierName}";
            Address1 = $"{supplier.Address1}";
            Address2 = supplier.Address2.Length != 0 ?
                $"{supplier.Address2}-{supplier.Kota}" : $"{supplier.Kota}";
            JatuhTempo = $"Tempo: {invoice.DueDate:dd MMM yyyy}";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Terbilang = $"Terbilang #{Math.Round(invoice.GrandTotal, 0).Eja()} rupiah#";
            Terbilang = textInfo.ToTitleCase(Terbilang);

            SubTotal = $"{invoice.Total:N0}";
            Discount = $"{invoice.Disc:N0}";
            Total = $"{(invoice.Total - invoice.Disc):N0}";

            Dpp = $"{invoice.ListItem.Sum(x => x.DppRp):N0}";
            var dppProsen = invoice.ListItem.FirstOrDefault().DppProsen;

            if (dppProsen == 100M)
                DppProsen = $"DPP :";
            else 
                DppProsen = "DPP 11/12 :";

            Ppn = $"{invoice.ListItem.Sum(x => x.PpnRp):N0}";
            PpnProsen = $"PPN {DecFormatter.ToStr(invoice.ListItem.FirstOrDefault().PpnProsen)}% :";
            GrandTotal = $"{invoice.GrandTotal:N0}";
            UserName = invoice.UserId;

            ListItem = new List<InvoicePrintOutItemDto>();
            var noUrut = 1;
            foreach (var item in invoice.ListItem)
            {
                if (item.QtyBesar != 0 || item.QtyKecil != 0)
                {
                    var qtyBesar = item.QtyBesar == 0 ? "-" :
                        $"{item.QtyBesar:N0} {item.SatBesar}";
                    var qtyKecil = item.QtyKecil == 0 ? "-" :
                        $"{item.QtyKecil:N0} {item.SatKecil}";
                    var disc1 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen ?? 0;
                    var disc2 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen ?? 0;
                    var disc3 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen ?? 0;
                    var disc4 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen ?? 0;
                    var newItem = new InvoicePrintOutItemDto
                    {
                        NoUrut = $"{noUrut}",
                        BrgCode = item.BrgCode,
                        BrgName = item.BrgName,
                        QtyBesar = qtyBesar,
                        QtyKecil = qtyKecil,
                        HrgBesar = item.HppSatBesar== 0 ? "-" : $"{item.HppSatBesar:N0}",
                        HrgKecil = item.HppSatKecil == 0 ? "-" : $"{item.HppSatKecil:N0}",
                        Disc1 = disc1 == 0 ? "-" : $"{DecFormatter.ToStr(disc1)}%",
                        Disc2 = disc2 == 0 ? "-" : $"{DecFormatter.ToStr(disc2)}%",
                        Disc3 = disc3 == 0 ? "-" : $"{DecFormatter.ToStr(disc3)}%",
                        Disc4 = disc4 == 0 ? "-" : $"{DecFormatter.ToStr(disc4)}%",
                        Total = $"{(item.SubTotal):N0}",
                    };
                    ListItem.Add(newItem);
                    noUrut++;
                }

                if (item.QtyBonus != 0)
                {
                    var itemBonus = new InvoicePrintOutItemDto
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


        public string InvoiceCode { get; set; }
        public string InvoiceDate { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName {get;set;}
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string JatuhTempo { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }

        public string Dpp { get; set; }
        public string DppProsen { get; set; }
        public string Ppn { get; set; }
        public string PpnProsen { get; set; }
        public string GrandTotal { get; set; }

        public string Terbilang { get; set; }
        public string UserName { get; set; }
        public List<InvoicePrintOutItemDto> ListItem { get; set; }
    }

    public class InvoicePrintOutItemDto
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
