using btr.infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using btr.nuna.Domain;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using Mapster;

namespace btr.distrib.PrintDocs
{
    public interface IInvoicePrintDoc : IPrintDoc<InvoiceModel> 
    { 
    }
    public class InvoicePrintDoc : IInvoicePrintDoc
    {
        private readonly PrinterOptions _opt;
        private readonly ISupplierDal _supplierDal;
        private string _content = string.Empty;
        private JudeDocument _pd;
        private PrintPreviewDialog _ppv;

        public InvoicePrintDoc(IOptions<PrinterOptions> opt, 
            ISupplierDal supplierDal)
        {
            _opt = opt.Value;
            _supplierDal = supplierDal;

            InitPrintDocument();
            
        }

        private void InitPrintDocument()
        {
            _pd = new JudeDocument();
            _pd.PrintPage += PrintDocument_PrintPage;
            _pd.PrinterSettings = new PrinterSettings { PrinterName = DefaultPrinter };
            
            var customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
            _pd.DefaultPageSettings.PaperSize = customPaperSize;
            
            var margins = new Margins(0, 0, 25, 25);
            _pd.DefaultPageSettings.Margins = margins;
        }

        private void InitPrintPreview()
        {
            _ppv = new PrintPreviewDialog();
            _ppv.Document = _pd;
            _ppv.WindowState = FormWindowState.Maximized;
            var printerButton = new Button { Text = "Printer" };
            //  allow user to change printer
            foreach (Control cont in _ppv.Controls)
            {
                if (cont.GetType() == typeof(ToolStrip))
                {
                    var ts = cont as ToolStrip;
                    ts.Items.Add(new ToolStripButton("Printer Settings", null, (s, e) =>
                    {
                        var printDialog = new PrintDialog();
                        printDialog.Document = _pd;
                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            _pd.PrinterSettings = printDialog.PrinterSettings;
                        }
                    }));
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var printDocument = (PrintDocument)sender;
            var font = new Font(_opt.FontName, _opt.FontSize, FontStyle.Regular);
            var customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
            printDocument.DefaultPageSettings.PaperSize = customPaperSize;

            e.Graphics.DrawString(_content, font, Brushes.Black, e.MarginBounds);
        }

        public void PrintDoc()
        {
            InitPrintPreview();
            _ppv.ShowDialog();
            IsPrinted = _pd.IsPrinting;
            _ppv.Close();
        }

        public void CreateDoc(InvoiceModel model)
        {
            var sb = new StringBuilder();
            var supplier = _supplierDal.GetData(model)??new SupplierModel();
            supplier.RemoveNull();

            var noItem = 1;
            var listItemWithBonus = PindahBonusNewBaris(model.ListItem);
            
            PrintHeader(model, supplier, sb);
            foreach (var item in listItemWithBonus)
            {
                var no = noItem.ToString("D2");
                var brgId = item.BrgCode.Trim().Length > 0 ? item.BrgCode.FixWidth(10) : item.BrgId.FixWidth(10);

                var arrName = item.BrgName.WrapText(27);
                var arrName1 = arrName[0].FixWidth(27);
                var arrName2 = "".FixWidth(27);
                if (arrName.Length > 1)
                    arrName2 = arrName[1].FixWidth(27);

                var arrQty1 = GetQty(item.QtyBesar, item.SatBesar, 7);
                var arrQty2 = GetQty(item.QtyKecil, item.SatKecil, 7);

                var qty1A = arrQty1[0];
                var qty2A = arrQty2[0];

                var qty1B = arrQty1.Length > 1 ? arrQty1[1] : string.Empty.FixWidth(7);
                var qty2B = arrQty2.Length > 1 ? arrQty2[1] : string.Empty.FixWidth(7);

                var hrg = item.HppSatBesar;
                var hrg1 = hrg != 0? $"{hrg:N0}".FixWidthRight(10) : "- ".FixWidthRight(10);
                hrg = item.HppSatKecil;
                var hrg2 = hrg != 0? $"{hrg:N0}".FixWidthRight(8) : "- ".FixWidthRight(8);
                var disc1 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
                var disc2 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
                var disc3 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
                var disc4 = item.ListDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
                var total = (item.SubTotal - item.DiscRp).ToString("N0").FixWidthRight(11);
                //var total2 =  item.Total.ToString("N0").FixWidthRight(11);
                if (item.QtyBesar + item.QtyKecil == 0)
                    continue;

                sb.Append($"{no}│{brgId}│{arrName1.FixWidth(27)}│{qty1A}│{qty2A}│{hrg1}│{hrg2}│{disc1}│{disc2}│{disc3}│{disc4}│{total}\n");
                if ($"{arrName2}{qty1B}{qty2B}".Trim().Length > 0)
                {
                    sb.Append($"  │          │{arrName2.FixWidth(27)}│{qty1B}│{qty2B}│          │        │     │     │   │   │              \n");
                }
                noItem++;
            }

            //  jika kurang dari 10, tambahkan baris kosong
            for (var j = noItem; j <= 5; j++)
                sb.Append($"  │          │                           │       │       │          │        │     │     │   │   │           \n");

            sb.Append($"──┴──────────┴───────────────────────────┴───────┴───────┴──────────┴────────┴─────┴─────┴───┴───┴───────────\n");
            var subTotal = model.ListItem.Sum(x => x.SubTotal);
            var discount = model.ListItem.Sum(x => x.DiscRp);
            var total2 = subTotal - discount;
            var ppn = model.ListItem.Sum(x => x.PpnRp); 
            var grandTotal = model.ListItem.Sum(x => x.Total);
            grandTotal = decimal.Floor(grandTotal);
            var terbilang = $"{grandTotal.Eja()} rupiah".WrapText(70);
            var terbilang2 = terbilang.Length > 1 ? terbilang[1] : string.Empty;
            sb.Append($"{terbilang[0].FixWidth(70)}                SubTotal   :{subTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"{terbilang2.FixWidth(70)}                Discount   :{discount.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                      Total      :{total2.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                      PPN 11%    :{ppn.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                      Grand Total:{grandTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"\n");
            sb.Append($"Tanda Terima,                            Pengirim,                             Yogyakarta, {model.InvoiceDate:dd-MM-yyyy}\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"___________________                      ___________________                   {model.UserId}\n");
            sb.Append($"(Nama Terang/Cap)\n");

            _content = sb.ToString();
        }

        private static string[] GetQty(int qtyParam, string satuan, int length)
        {
            if (qtyParam == 0)
                return new string[] { "     - " };

            var qty = $"{qtyParam:N0}";
            var sat = $"{satuan.ToLower()}";
            var qtySatuan = $"{qty} {sat.Trim()}";
            var lines = qtySatuan.WrapText(7);
            var result = new List<string>();
            foreach (var line in lines)
                result.Add(line.Trim().FixWidthRight(length));
            return result.ToArray();
        }

        private static void PrintHeader(InvoiceModel model, SupplierModel supplier, StringBuilder sb)
        {
            var tgl = model.InvoiceDate.ToString("dd-MM-yyyy");
            var custId = model.SupplierId.PadRight(12, ' ');
            var noFakturrrr = model.InvoiceCode.FixWidth(13);
            var jnsJl = model.TermOfPayment.ToString().FixWidth(7);
            var jatuhTmpo = model.DueDate.ToString("dd MMM yyyy");
            var addr1 = supplier.Address1;
            var addr2 = supplier.Address2.Length > 0 ? supplier.Address2 : supplier.Kota;
            var addr3 = supplier.Address2.Length > 0 ? supplier.Kota : string.Empty ;
            

            var hdr1 = $"CV BINTANG TIMUR RAHAYU                                                      Tgl  : {model.InvoiceDate:dd-MMM-yyyy}\n";
            var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18         INVOICE PEMBELIAN                 Dari Supplier-{custId}\n";
            var hdr3 = $"Yogyakarta 0274-546079                                                       {supplier.SupplierName.FixWidth(40)}\n";
            var hdr4 = $"                                                                             {addr1.FixWidth(40)}\n";
            var hdr5 = $"                                                                             {addr2.FixWidth(40)}\n";
            var hdr6 = $"No.Invoice:{noFakturrrr}                                                     Tempo:{jatuhTmpo}\n";
            var hdr7 = $"──┬──────────┬───────────────────────────┬───────────────┬───────────────────┬───────────────────┬───────────\n";
            var hdr8 = $"No│Kode      │Nama Barang                │   Kwantitas   │       @Harga      │      Discount     │   Total   \n";
            var hdr9 = $"──┼──────────┼───────────────────────────┼───────┬───────┼──────────┬────────┼─────┬─────┬───┬───┼───────────\n";
            sb.Append(hdr1);
            sb.Append(hdr2);
            sb.Append(hdr3);
            sb.Append(hdr4);
            sb.Append(hdr5);
            sb.Append(hdr6);
            sb.Append(hdr7);
            sb.Append(hdr8);
            sb.Append(hdr9);
        }

        private static List<InvoiceItemModel> PindahBonusNewBaris(List<InvoiceItemModel> listItem)
        {
            var result = new List<InvoiceItemModel>();

            foreach(var item in listItem)
            {
                result.Add(item);
                if (item.QtyBonus == 0)
                    continue;
                var newItem = item.Adapt<InvoiceItemModel>();
                newItem.ListDisc.Clear();
                newItem.QtyKecil = item.QtyBonus;
                newItem.SatKecil = item.SatKecil;
                newItem.HppSat = 0;
                newItem.HppSatBesar = 0;
                newItem.HppSatKecil = 0;
                newItem.Total = 0;
                newItem.SubTotal = 0;
                newItem.DiscRp = 0;
                newItem.PpnRp = 0;
                result.Add(newItem);
            }
            return result;
        }

        public bool IsPrinted { get; private set; }
        public string DefaultPrinter { get; set; }
    }
}
