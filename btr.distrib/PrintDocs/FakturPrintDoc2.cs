//using btr.application.SalesContext.CustomerAgg.UseCases;
//using btr.domain.SalesContext.CustomerAgg;
//using btr.domain.SalesContext.FakturAgg;
//using btr.nuna.Domain;
//using MediatR;
//using Polly.Caching;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Printing;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace btr.distrib.PrintDocs
//{
//    public interface IPrintDoc2<T>
//    {

//    }
//    public class FakturPrintDoc2 : IPrintDoc2<FakturModel>
//    {
//        private FakturModel _agg;
//        private CustomerModel _customer;

//        private readonly IMediator _mediator;
//        public FakturPrintDoc2(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        public async void PrintDoc(FakturModel faktur)
//        {
//            _agg = faktur;
//            _customer = await _mediator.Send(new GetCustomerQuery(_agg.CustomerId));

//            PrintDocument printDocument = new PrintDocument();
//            printDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            
//            PrinterSettings printerSettings = new PrinterSettings
//            {
//                PrinterName = _opt.Faktur
//            };
//            pd.PrinterSettings = printerSettings;
//            printDocument.Print();
//        }

//        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
//        {
//            var printFont = new Font("Lucida Console", 9);
//            float fontHeight = ev.Graphics.MeasureString("H", printFont).Height;

//            var tgl = _agg.FakturDate.ToString("dd-MM-yyyy");
//            var custId = _agg.CustomerId.PadRight(12, ' ');
//            var noFakt = _agg.FakturId.FixWidth(8);
//            var jnsJl = _agg.TermOfPayment.FixWidth(7);
//            var salesNameeeee = _agg.SalesPersonName.FixWidth(13);
//            var jatuhTmpo = string.Empty; // jatuh temp

//            var hdr1 = $"CV BINTANG TIMUR RAHAYU                                   {tgl}";
//            var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18                        Kepada Yth Customer-{custId}";
//            var hdr3 = $"0274-546079                                               {_customer.CustomerName}";
//            var hdr4 = $"Yogyakarta                                                {_customer.Address1}";
//            var hdr5 = $"FAKTUR PENJUALAN                                          {_customer.Address2}";
//            var hdr7 = $"No.Faktur: {noFakt}   Jns Jual: {jnsJl}   Sales: {salesNameeeee}   Jth Tempo: {jatuhTmpo}";
//            var hdr6 = $" ";
//            var hdr8 = $"--+-------+-------------------------+--+-------------+-----------------+---------------+--------------";
//            var hdr9 = $"No|Kode   |Nama Barang              |  |  Kwantitas  |      @Harga     |    Discount   | Total Harga  ";
//            var hdrA = $"--+-------+-------------------------+--+------+------+--------+--------+---+---+---+---+--------------";


//            // Draw the Purchase Order document.
//            ev.Graphics.DrawString(hdr1, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 0, new StringFormat());
//            ev.Graphics.DrawString(hdr2, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 1, new StringFormat());
//            ev.Graphics.DrawString(hdr3, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 2, new StringFormat());
//            ev.Graphics.DrawString(hdr4, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 3, new StringFormat());
//            ev.Graphics.DrawString(hdr5, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 4, new StringFormat());
//            ev.Graphics.DrawString(hdr6, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 5, new StringFormat());
//            ev.Graphics.DrawString(hdr7, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 6, new StringFormat());
//            ev.Graphics.DrawString(hdr8, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 7, new StringFormat());
//            ev.Graphics.DrawString(hdr9, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 8, new StringFormat());
//            ev.Graphics.DrawString(hdrA, printFont, Brushes.Black, ev.MarginBounds.Left, fontHeight * 9, new StringFormat());

//        }
//    }
//}
