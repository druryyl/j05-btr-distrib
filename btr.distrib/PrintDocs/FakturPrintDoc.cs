using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using btr.nuna.Domain;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace btr.distrib.PrintDocs
{
    public interface IPrintDoc<T>
    {
        void CreateDoc(T model);
        void PrintDoc();
        bool IsPrinted { get; }
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
            InitPrintPreview();
        }

        private void InitPrintDocument()
        {
            pd = new JudeDocument();
            pd.PrintPage += PrintDocument_PrintPage;
            pd.PrinterSettings = new PrinterSettings { PrinterName = _opt.Faktur };
            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
            pd.DefaultPageSettings.PaperSize = customPaperSize;
            Margins margins = new Margins(50, 50, 50, 50);
            pd.DefaultPageSettings.Margins = margins;
        }

        private void InitPrintPreview()
        {
            ppv = new PrintPreviewDialog();
            ppv.Document = pd;
            ppv.WindowState = FormWindowState.Maximized;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            e.Graphics.DrawString(_content, font, Brushes.Black, e.MarginBounds);
        }

        public void PrintDoc()
        {
            ppv.ShowDialog();
            IsPrinted = pd.IsPrinting;
        }

        public async void CreateDoc(FakturModel model)
        {
            var sb = new StringBuilder();
            var cust = await _mediator.Send(new GetCustomerQuery(model.CustomerId));

            void PrintHeader()
            {
                var tgl = model.FakturDate.ToString("dd-MM-yyyy");
                var custId = model.CustomerId.PadRight(12, ' ');
                var noFakturrrr = model.FakturId.FixWidth(13);
                var jnsJl = model.TermOfPayment.FixWidth(7);
                var salesNameeeee = model.SalesPersonName.FixWidth(13);
                var jatuhTmpo = "12 Sep 2023";

                var hdr1 = $"CV BINTANG TIMUR RAHAYU                                              {tgl}\n";
                var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18                                   Kepada Yth Customer-{custId}\n";
                var hdr3 = $"Yogyakarta 0274-546079                                               {cust.CustomerName}\n";
                var hdr4 = $"                                                                     {cust.Address1} \n";
                var hdr5 = $"FAKTUR PENJUALAN                                                     {cust.Address2} \n";
                var hdr6 = $" \n";
                var hdr7 = $"No.Faktur: {noFakturrrr}            Sales: {salesNameeeee}           Jenis: {jnsJl}      Tempo:{jatuhTmpo}\n";
                var hdr8 = $"──┬───────┬──────────────────────────────┬──┬─────────────┬───────────────────┬───────────────┬───────────\n";
                var hdr9 = $"No│Kode   │Nama Barang                   │  │  Kwantitas  │       @Harga      │    Discount   │   Total   \n";
                var hdrA = $"──┼───────┼──────────────────────────────┼──┼──────┬──────┼──────────┬────────┼───┬───┬───┬───┼───────────\n";
                sb.Append(hdr1);
                sb.Append(hdr2);
                sb.Append(hdr3);
                sb.Append(hdr4);
                sb.Append(hdr5);
                sb.Append(hdr6);
                sb.Append(hdr7);
                sb.Append(hdr8);
                sb.Append(hdr9);
                sb.Append(hdrA);
            }

            var i = 1;
            foreach(var item in model.ListItem)
            {
                if (i % 5 == 1)
                    PrintHeader();
                
                var no = i.ToString("D2");
                var brgId = item.BrgId.FixWidth(7);
                var brgName = item.BrgName.FixWidth(30);
                var bonus = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3)?.Qty.ToString().FixWidthRight(2) ?? string.Empty.FixWidth(2);
                var qty1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.Qty.ToString().FixWidthRight(6) ?? string.Empty.FixWidth(6);
                var qty2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.Qty.ToString().FixWidthRight(6) ?? string.Empty.FixWidth(6);
                var hrg1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.HargaJual.ToString("N0").FixWidthRight(10) ?? string.Empty.FixWidth(8);
                var hrg2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.HargaJual.ToString("N0").FixWidthRight(8) ?? string.Empty.FixWidth(8);
                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscountProsen.ToString("N0").FixWidthRight(3) ?? string.Empty.FixWidth(3);
                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscountProsen.ToString("N0").FixWidthRight(3) ?? string.Empty.FixWidth(3);
                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscountProsen.ToString("N0").FixWidthRight(3) ?? string.Empty.FixWidth(3);
                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscountProsen.ToString("N0").FixWidthRight(3) ?? string.Empty.FixWidth(3);
                var total = item.Total.ToString("N0").FixWidthRight(11);
                
                sb.Append($"{no}|{brgId}|{brgName}|{bonus}|{qty1}|{qty2}|{hrg1}|{hrg2}|{disc1}|{disc2}|{disc3}|{disc4}|{total}\n");
                i++;
            }

            for(var j = 1; j <= i % 5; j++)
                sb.Append($"  │       │                              │  │      │      │          │        │   │   │   │   │              \n");
            
            sb.Append($"──┴───────┴──────────────────────────────┴──┴──────┴──────┴──────────┴────────┴───┴───┴───┴───┴───────────\n");
            var subTotal = model.ListItem.Sum(x => x.SubTotal);
            var discount = model.ListItem.Sum(x => x.DiscountRp) + model.DiscountLain;
            var total2 = subTotal + discount;
            var ppn = model.ListItem.Sum(x => x.PpnRp);
            var grandTotal = model.ListItem.Sum(x => x.Total);
            sb.Append($"                                                                                   SUBTOTAL   :{subTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                   DISCOUNT   :{discount.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                   TOTAL      :{total2.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                   PPN 11%    :{ppn.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                   GRAND TOTAL:{grandTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"\n");
            sb.Append($"Tanda Terima,                             Pengirim,                             Yogyakarta, {model.FakturDate:dd-MM-yyyy}\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"___________________                       ___________________                   Mayang\n");
            sb.Append($"(Nama Terang/Cap)\n");

            _content = sb.ToString();
        }

        public bool IsPrinted { get; private set; }
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
