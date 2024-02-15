using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.domain.SalesContext.FakturAgg;
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
using btr.domain.SalesContext.CustomerAgg;
using Mapster;

namespace btr.distrib.PrintDocs
{
    public interface IPrintDoc<T>
    {
        void CreateDoc(T model);
        void PrintDoc();
        bool IsPrinted { get; }
        string DefaultPrinter { get; set; }
    }
    public interface IFakturPrintDoc : IPrintDoc<FakturModel> 
    { 
    }
    public class FakturPrintDoc : IFakturPrintDoc
    {
        private readonly PrinterOptions _opt;
        private readonly IMediator _mediator;
        private string _content = string.Empty;
        private JudeDocument pd;
        private PrintPreviewDialog ppv;

        public FakturPrintDoc(IOptions<PrinterOptions> opt, 
            IMediator mediator)
        {
            _opt = opt.Value;
            _mediator = mediator;

            InitPrintDocument();
            
        }

        private void InitPrintDocument()
        {
            pd = new JudeDocument();
            pd.PrintPage += PrintDocument_PrintPage;
            pd.PrinterSettings = new PrinterSettings { PrinterName = DefaultPrinter };
            
            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
            pd.DefaultPageSettings.PaperSize = customPaperSize;
            
            Margins margins = new Margins(0, 0, 25, 25);
            pd.DefaultPageSettings.Margins = margins;
        }

        private void InitPrintPreview()
        {
            ppv = new PrintPreviewDialog();
            ppv.Document = pd;
            ppv.WindowState = FormWindowState.Maximized;
            var printerButton = new Button { Text = "Printer"};
            //  allow user to change printer
            foreach (Control cont in ppv.Controls)
            {
                if (cont.GetType() == typeof(ToolStrip))
                {
                    var ts = cont as ToolStrip;
                    ts.Items.Add(new ToolStripButton("Printer Settings", null, (s, e) =>
                    {
                        var printDialog = new PrintDialog();
                        printDialog.Document = pd;
                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            pd.PrinterSettings = printDialog.PrinterSettings;
                        }
                    }));
                }
            }
        }


        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pd = (PrintDocument)sender;
            Font font = new Font(_opt.FontName, _opt.FontSize, FontStyle.Regular);
            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
            pd.DefaultPageSettings.PaperSize = customPaperSize;

            e.Graphics.DrawString(_content, font, Brushes.Black, e.MarginBounds);
        }

        public void PrintDoc()
        {
            InitPrintPreview();

            ppv.ShowDialog();
            IsPrinted = pd.IsPrinting;
            ppv.Close();
        }

        public async void CreateDoc(FakturModel model)
        {
            var sb = new StringBuilder();
            var cust = await _mediator.Send(new GetCustomerQuery(model.CustomerId));

            var noItem = 1;
            var listItemWithBonus = PindahBonusNewBaris(model.ListItem);
            
            PrintHeader(model, cust, sb);
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

                var hrg = item.HrgSatBesar;
                var hrg1 = hrg != 0? $"{hrg:N0}".FixWidthRight(10) : "- ".FixWidthRight(10);
                hrg = item.HrgSatKecil;
                var hrg2 = hrg != 0? $"{hrg:N0}".FixWidthRight(8) : "- ".FixWidthRight(8);
                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
                var total = (item.SubTotal - item.DiscRp).ToString("N0").FixWidthRight(11);
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
            sb.Append($"Tanda Terima,                            Pengirim,                             Yogyakarta, {model.FakturDate:dd-MM-yyyy}\n");
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

        private static void PrintHeader(FakturModel model, CustomerModel cust, StringBuilder sb)
        {
            var tgl = model.FakturDate.ToString("dd-MM-yyyy");
            var custId = model.CustomerId.PadRight(12, ' ');
            var noFakturrrr = model.FakturCode.FixWidth(13);
            var jnsJl = model.TermOfPayment.ToString().FixWidth(7);
            var salesNameee = model.SalesPersonName.FixWidth(13);
            var jatuhTmpo = model.DueDate.ToString("dd MMM yyyy");
            var addr1 = cust.Address1;
            var addr2 = cust.Address2.Length > 0 ? cust.Address2 : cust.Kota;
            var addr3 = cust.Address2.Length > 0 ? cust.Kota : string.Empty ;
            

            var hdr1 = $"CV BINTANG TIMUR RAHAYU                                                      Tgl: {model.FakturDate:dd-MMM-yyyy}\n";
            var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18         FAKTUR PENJUALAN                  Kepada Yth Customer-{custId}\n";
            var hdr3 = $"Yogyakarta 0274-546079                                                       {cust.CustomerName.FixWidth(40)}\n";
            var hdr4 = $"                                                                             {addr1.FixWidth(40)}\n";
            var hdr5 = $"                                                                             {addr2.FixWidth(40)}\n";
            var hdr6 = $"No.Faktur: {noFakturrrr}      Sales: {salesNameee}       Jenis: {jnsJl}      Tempo:{jatuhTmpo}\n";
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

        private List<FakturItemModel> PindahBonusNewBaris(List<FakturItemModel> listItem)
        {
            var result = new List<FakturItemModel>();

            foreach(var item in listItem)
            {
                result.Add(item);
                if (item.QtyBonus == 0)
                    continue;
                var newItem = item.Adapt<FakturItemModel>();
                newItem.ListDiscount.Clear();
                newItem.QtyKecil = item.QtyBonus;
                newItem.SatKecil = item.SatKecil;
                newItem.HrgSat = 0;
                newItem.HrgSatBesar = 0;
                newItem.HrgSatKecil = 0;
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

    public class JudeDocument : PrintDocument
    {
        private bool isPrinting = false;

        public bool IsPrinting => isPrinting;
        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            isPrinting = (e.PrintAction == PrintAction.PrintToPrinter);
        }
    }
}
