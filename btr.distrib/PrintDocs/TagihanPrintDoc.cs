//using btr.application.SalesContext.CustomerAgg.UseCases;
//using btr.domain.SalesContext.CustomerAgg;
//using btr.infrastructure.Helpers;
//using btr.nuna.Domain;
//using MediatR;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Drawing.Printing;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using btr.domain.FinanceContext.TagihanAgg;
//using btr.application.SalesContext.SalesPersonAgg.Contracts;
//using btr.application.FinanceContext.TagihanAgg;
//using btr.domain.SalesContext.SalesPersonAgg;

//namespace btr.distrib.PrintDocs
//{
//    public interface ITagihanPrintDoc : IPrintDoc<TagihanModel>
//    {
//    }
//    public class TagihanPrintDoc : ITagihanPrintDoc
//    {
//        private readonly PrinterOptions _opt;
//        private readonly IMediator _mediator;
//        private string _content = string.Empty;
//        private JudeDocument pd;
//        private PrintPreviewDialog ppv;

//        private readonly ISalesPersonDal _salesPersonDal;
//        private readonly ITagihanDal _tagihanDal;
//        private readonly ITagihanBuilder _tagihanBuilder;


//        public TagihanPrintDoc(IOptions<PrinterOptions> opt,
//            IMediator mediator,
//            ISalesPersonDal salesPersonDal,
//            ITagihanDal tagihanDal,
//            ITagihanBuilder tagihanBuilder)
//        {
//            _opt = opt.Value;
//            _mediator = mediator;

//            InitPrintDocument();
//            _salesPersonDal = salesPersonDal;
//            _tagihanDal = tagihanDal;
//            _tagihanBuilder = tagihanBuilder;
//        }

//        private void InitPrintDocument()
//        {
//            pd = new JudeDocument();
//            pd.PrintPage += PrintDocument_PrintPage;
//            pd.PrinterSettings = new PrinterSettings { PrinterName = DefaultPrinter };

//            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
//            pd.DefaultPageSettings.PaperSize = customPaperSize;

//            Margins margins = new Margins(0, 0, 25, 25);
//            pd.DefaultPageSettings.Margins = margins;
//        }

//        private void InitPrintPreview()
//        {
//            ppv = new PrintPreviewDialog();
//            ppv.Document = pd;
//            ppv.WindowState = FormWindowState.Maximized;
//        }

//        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
//        {
//            var pd = (PrintDocument)sender;
//            Font font = new Font("Courier New", 8.25f, FontStyle.Regular);
//            PaperSize customPaperSize = new PaperSize("Custom", Convert.ToInt32(9.5 * 100), Convert.ToInt32(11 * 100));
//            pd.DefaultPageSettings.PaperSize = customPaperSize;

//            e.Graphics.DrawString(_content, font, Brushes.Black, e.MarginBounds);
//        }

//        public void PrintDoc()
//        {
//            InitPrintPreview();
//            ppv.ShowDialog();
//            IsPrinted = pd.IsPrinting;
//            ppv.Close();
//        }

//        public void CreateDoc(TagihanModel model)
//        {
//            var sb = new StringBuilder();

//            var noItem = 1;

//            PrintHeader(model, cust, sb);
//            foreach (var item in listItemWithBonus)
//            {
//                var no = noItem.ToString("D2");
//                var brgId = item.BrgCode.Trim().Length > 0 ? item.BrgCode.FixWidth(10) : item.BrgId.FixWidth(10);

//                var arrName = item.BrgName.WrapText(27);
//                var arrName1 = arrName[0].FixWidth(27);
//                var arrName2 = "".FixWidth(27);
//                if (arrName.Length > 1)
//                    arrName2 = arrName[1].FixWidth(27);

//                var arrQty1 = GetQty(item.QtyBesar, item.SatBesar, 7);
//                var arrQty2 = GetQty(item.QtyKecil, item.SatKecil, 7);

//                var qty1A = arrQty1[0];
//                var qty2A = arrQty2[0];

//                var qty1B = arrQty1.Length > 1 ? arrQty1[1] : string.Empty.FixWidth(7);
//                var qty2B = arrQty2.Length > 1 ? arrQty2[1] : string.Empty.FixWidth(7);

//                var hrg = item.HrgSatBesar;
//                var hrg1 = hrg != 0 ? $"{hrg:N0}".FixWidthRight(10) : "- ".FixWidthRight(10);
//                hrg = item.HrgSatKecil;
//                var hrg2 = hrg != 0 ? $"{hrg:N0}".FixWidthRight(8) : "- ".FixWidthRight(8);
//                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
//                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscProsen.ToString("N2").FixWidthRight(5) ?? "-".FixWidthRight(5);
//                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
//                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscProsen.ToString("N2").FixWidthRight(3) ?? "-".FixWidthRight(3);
//                var total = (item.SubTotal - item.DiscRp).ToString("N0").FixWidthRight(11);
//                //var total2 =  item.Total.ToString("N0").FixWidthRight(11);

//                sb.Append($"{no}│{brgId}│{arrName1.FixWidth(27)}│{qty1A}│{qty2A}│{hrg1}│{hrg2}│{disc1}│{disc2}│{disc3}│{disc4}│{total}\n");
//                if ($"{arrName2}{qty1B}{qty2B}".Trim().Length > 0)
//                {
//                    sb.Append($"  │          │{arrName2.FixWidth(27)}│{qty1B}│{qty2B}│          │        │     │     │   │   │              \n");
//                }
//                noItem++;
//            }

//            //  jika kurang dari 10, tambahkan baris kosong
//            for (var j = noItem; j <= 5; j++)
//                sb.Append($"  │          │                           │       │       │          │        │     │     │   │   │           \n");

//            sb.Append($"──┴──────────┴───────────────────────────┴───────┴───────┴──────────┴────────┴─────┴─────┴───┴───┴───────────\n");
//            var subTotal = model.ListItem.Sum(x => x.SubTotal);
//            var discount = model.ListItem.Sum(x => x.DiscRp);
//            var total2 = subTotal - discount;
//            var ppn = model.ListItem.Sum(x => x.PpnRp);
//            var grandTotal = model.ListItem.Sum(x => x.Total);
//            grandTotal = decimal.Floor(grandTotal);
//            var terbilang = $"{grandTotal.Eja()} rupiah".WrapText(70);
//            var terbilang2 = terbilang.Length > 1 ? terbilang[1] : string.Empty;
//            sb.Append($"{terbilang[0].FixWidth(70)}                SubTotal   :{subTotal.ToString("N0").FixWidthRight(11)}\n");
//            sb.Append($"{terbilang2.FixWidth(70)}                Discount   :{discount.ToString("N0").FixWidthRight(11)}\n");
//            sb.Append($"                                                                                      Total      :{total2.ToString("N0").FixWidthRight(11)}\n");
//            sb.Append($"                                                                                      PPN 11%    :{ppn.ToString("N0").FixWidthRight(11)}\n");
//            sb.Append($"                                                                                      Grand Total:{grandTotal.ToString("N0").FixWidthRight(11)}\n");
//            sb.Append($"\n");
//            sb.Append($"Tanda Terima,                            Pengirim,                             Yogyakarta, {model.TagihanDate:dd-MM-yyyy}\n");
//            sb.Append($"\n");
//            sb.Append($"\n");
//            sb.Append($"\n");
//            sb.Append($"\n");
//            sb.Append($"___________________                      ___________________                   {model.UserId}\n");
//            sb.Append($"(Nama Terang/Cap)\n");

//            _content = sb.ToString();
//        }

//        private static string[] GetQty(int qtyParam, string satuan, int length)
//        {
//            if (qtyParam == 0)
//                return new string[] { "     - " };

//            var qty = $"{qtyParam:N0}";
//            var sat = $"{satuan.ToLower()}";
//            var qtySatuan = $"{qty} {sat.Trim()}";
//            var lines = qtySatuan.WrapText(7);
//            var result = new List<string>();
//            foreach (var line in lines)
//                result.Add(line.Trim().FixWidthRight(length));
//            return result.ToArray();
//        }

//        private static void PrintHeader(TagihanModel model, SalesPersonModel cust, StringBuilder sb)
//        {
//            var hdr1 = $"CV BINTANG TIMUR RAHAYU                ┌───────────────────────┐                             Tgl: {model.TagihanDate:dd-MMM-yyyy}\n";
//            var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18     │   TAGIHAN PENJUALAN   │             Kepada Yth Customer-{custId}\n";
//            var hdr3 = $"Yogyakarta 0274-546079                 └───────────────────────┘             {cust.CustomerName.FixWidth(40)}\n";
//            var hdr4 = $"                                                                             {addr1.FixWidth(40)}\n";
//            var hdr5 = $"                                                                             {addr2.FixWidth(40)}\n";
//            var hdr6 = $"No.Tagihan: {noTagihanrrr}                 Sales: {salesNameee}                Jenis: {jnsJl} Tempo:{jatuhTmpo}\n";
//            var hdr7 = $"──┬──────────┬───────────────────────────┬───────────────┬───────────────────┬───────────────────┬───────────\n";
//            var hdr8 = $"No│Kode      │Nama Barang                │   Kwantitas   │       @Harga      │      Discount     │   Total   \n";
//            var hdr9 = $"──┼──────────┼───────────────────────────┼───────┬───────┼──────────┬────────┼─────┬─────┬───┬───┼───────────\n";
//            sb.Append(hdr1);
//            sb.Append(hdr2);
//            sb.Append(hdr3);
//            sb.Append(hdr4);
//            sb.Append(hdr5);
//            sb.Append(hdr6);
//            sb.Append(hdr7);
//            sb.Append(hdr8);
//            sb.Append(hdr9);
//        }

//        private List<TagihanItemModel> PindahBonusNewBaris(List<TagihanItemModel> listItem)
//        {
//            var result = new List<TagihanItemModel>();

//            foreach (var item in listItem)
//            {
//                result.Add(item);
//                if (item.QtyBonus == 0)
//                    continue;
//                var newItem = item.Adapt<TagihanItemModel>();
//                newItem.ListDiscount.Clear();
//                newItem.QtyKecil = item.QtyBonus;
//                newItem.SatKecil = item.SatKecil;
//                newItem.HrgSat = 0;
//                newItem.HrgSatBesar = 0;
//                newItem.HrgSatKecil = 0;
//                newItem.Total = 0;
//                newItem.SubTotal = 0;
//                newItem.DiscRp = 0;
//                newItem.PpnRp = 0;
//                result.Add(newItem);
//            }
//            return result;
//        }

//        public bool IsPrinted { get; private set; }
//        public string DefaultPrinter { get; set; }
//    }
//}
