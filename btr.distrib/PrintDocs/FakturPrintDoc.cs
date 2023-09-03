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
using System.Collections.Generic;

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
            
        }

        private void InitPrintDocument()
        {
            pd = new JudeDocument();
            pd.PrintPage += PrintDocument_PrintPage;
            pd.PrinterSettings = new PrinterSettings { PrinterName = _opt.Faktur };
            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(5.5 * 100));
            pd.DefaultPageSettings.PaperSize = customPaperSize;
            Margins margins = new Margins(0, 0, 25, 25);
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
            Font font = new Font("Courier New", 8.25f, FontStyle.Regular);
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

            void PrintHeader()
            {
                var tgl = model.FakturDate.ToString("dd-MM-yyyy");
                var custId = model.CustomerId.PadRight(12, ' ');
                var noFakturrrr = model.FakturId.FixWidth(13);
                var jnsJl = model.TermOfPayment.FixWidth(7);
                var salesNameeeee = model.SalesPersonName.FixWidth(13);
                var jatuhTmpo = "12 Sep 2023";

                var hdr1 = $"CV BINTANG TIMUR RAHAYU                                                      Kepada Yth Customer-{custId}\n";
                var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18                                           {cust.CustomerName.FixWidth(40)}\n";
                var hdr3 = $"Yogyakarta 0274-546079                   ┌───────────────────────┐           {cust.Address1.FixWidth(40)}\n";
                var hdr4 = $"                                         │   FAKTUR PENJUALAN    │           {cust.Address2.FixWidth(40)}\n";
                var hdr5 = $"                                         └───────────────────────┘\n";
                var hdr6 = $"No.Faktur: {noFakturrrr}                 Sales: {salesNameeeee}               Jenis: {jnsJl}      Tempo:{jatuhTmpo}\n";
                var hdr7 = $"──┬──────────┬───────────────────────────┬───────────────────────┬───────────────────┬───────────────────┬───────────\n";
                var hdr8 = $"No│Kode      │Nama Barang                │       Kwantitas       │       @Harga      │      Discount     │   Total   \n";
                var hdr9 = $"──┼──────────┼───────────────────────────┼───────┬───────┬───────┼──────────┬────────┼────┬────┬────┬────┼───────────\n";
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

            string[] GetQty(FakturQtyHargaModel item, int length)
            {
                var qty = (item?.Qty ?? 0) != 0 ? $"{item.Qty:N0}" : "-";
                var sat = item?.Satuan.ToLower() ?? string.Empty;
                var qtySatuan = $"{qty} {sat.Trim()}";
                var lines = qtySatuan.WrapText(7);
                var result = new List<string>();
                foreach (var line in lines)
                    result.Add(line.Trim().FixWidthRight(length));
                return result.ToArray();
            }

            var noItem = 1;
            var noBaris = 0;
            foreach(var item in model.ListItem)
            {
                if (noItem % 10 == 1)
                    PrintHeader();
                
                var no = noItem.ToString("D2");
                var brgId = item.BrgCode.Trim().Length > 0 ? item.BrgCode.FixWidth(10) : item.BrgId.FixWidth(10);

                var arrName = item.BrgName.WrapText(27);
                var arrName1 = arrName[0].FixWidth(27);
                var arrName2 = "".FixWidth(27);
                if (arrName.Length > 1)
                    arrName2 = arrName[1].FixWidth(27);


                var arrQty1 = GetQty(item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1), 7);
                var arrQty2 = GetQty(item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2), 7);
                var arrQty3 = GetQty(item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3), 7);

                var qty1a = arrQty1[0]; 
                var qty2a = arrQty2[0]; 
                var qty3a = arrQty3[0];

                var qty1b = arrQty1.Length > 1 ? arrQty1[1] : string.Empty.FixWidth(7);
                var qty2b = arrQty2.Length > 1 ? arrQty2[1] : string.Empty.FixWidth(7);
                var qty3b = arrQty3.Length > 1 ? arrQty3[1] : string.Empty.FixWidth(7);

                var hrg1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.HargaJual.ToString("N0").FixWidthRight(10) ?? "-".FixWidthRight(8);
                var hrg2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.HargaJual.ToString("N0").FixWidthRight(8) ?? "-".FixWidthRight(8);
                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscountProsen.ToString("N1").FixWidthRight(4) ?? "-".FixWidthRight(4);
                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscountProsen.ToString("N1").FixWidthRight(4) ?? "-".FixWidthRight(4);
                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscountProsen.ToString("N1").FixWidthRight(4) ?? "-".FixWidthRight(4);
                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscountProsen.ToString("N1").FixWidthRight(4) ?? "-".FixWidthRight(4);
                var total = item.Total.ToString("N0").FixWidthRight(11);

                sb.Append($"{no}│{brgId}│{arrName1.FixWidth(27)}│{qty1a}│{qty2a}│{qty3a}│{hrg1}│{hrg2}│{disc1}│{disc2}│{disc3}│{disc4}│{total}\n");
                if ($"{arrName2}{qty1b}{qty2b}{qty3b}".Trim().Length > 0)
                {
                    sb.Append($"  │          │{arrName2.FixWidth(27)}│{qty1b}│{qty2b}│{qty3b}│          │        │    │    │    │    │              \n");
                    noBaris++;
                }
                noItem++;
                noBaris++;
            }

            //  jika kurang dari 10, tambahkan baris kosong
            for(var j = noItem; j <= 5; j++)
                sb.Append($"  │          │                           │       │       │       │          │        │    │    │    │    │           \n");

            sb.Append($"──┴──────────┴───────────────────────────┴───────┴───────┴───────┴──────────┴────────┴────┴────┴────┴────┴───────────\n");
            var subTotal = model.ListItem.Sum(x => x.SubTotal);
            var discount = model.ListItem.Sum(x => x.DiscountRp) + model.DiscountLain;
            var total2 = subTotal + discount;
            var ppn = model.ListItem.Sum(x => x.PpnRp);
            var grandTotal = model.ListItem.Sum(x => x.Total);
            sb.Append($"                                                                                              SubTotal   :{subTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                              Discount   :{discount.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                              Total      :{total2.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                              PPN 11%    :{ppn.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"                                                                                              Grand Total:{grandTotal.ToString("N0").FixWidthRight(11)}\n");
            sb.Append($"\n");
            sb.Append($"Tanda Terima,                            Pengirim,                             Yogyakarta, {model.FakturDate:dd-MM-yyyy}\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"\n");
            sb.Append($"___________________                      ___________________                   Mayang\n");
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
