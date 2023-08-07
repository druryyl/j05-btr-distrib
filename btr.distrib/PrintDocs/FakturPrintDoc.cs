using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.PrintDocs
{
    public interface IPrintDoc<T>
    {
        void CreateDoc(T model);
        void PrintDoc(string  doc);
    }
    public interface IFakturPrintDoc : IPrintDoc<FakturModel> 
    { 
    }
    public class FakturPrintDoc : IFakturPrintDoc
    {
        private readonly PrinterOptions _opt;
        private readonly IMediator _mediator;

        public FakturPrintDoc(IOptions<PrinterOptions> opt, 
            IMediator mediator)
        {
            _opt = opt.Value;
            _mediator = mediator;
        }

        public async void CreateDoc(FakturModel model)
        {
            var sw = new StreamWriter(_opt.TempFile);
            var cust = await _mediator.Send(new GetCustomerQuery(model.CustomerId));

            var tgl___ = model.FakturDate.ToString("dd-MM-yyyy");
            var custId____ = model.CustomerId.PadRight(12, ' ');
            var custName______________________ = model.CustomerId.PadRight(32, ' ');
            var custAddr______________________ = cust.

            //  print area : 90 char x 29 baris Courier New 8
            //           "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 12345678 0"
            var hdr1 = $"CV BINTANG TIMUR RAHAYU                                                           {tgl___}";
            var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18                        Kepada Yth Customer-{custId____}";
            var hdr3 = $"0274-546079                                               {custName______________________}";
            var hdr4 = $"Yogyakarta                                                {custAddr______________________}";


            var kiri1 = "Jl.Kol.Sugiyono 65"; var kanan1 = " ";
            var kiri2 = "Yogyakarta 55153"; var kanan2 = "Kepada Yth,";
            var kiri3 = "No.Telp : (0274) 123-456"; var kanan3 = "Bp/Ibu. " + _penjualan.BuyerName;
            var kiri4 = ""; var kanan4 = _penjualan.Alamat;
            var kiri5 = "No.Nota : " + _penjualan.PenjualanID; var kanan5 = _penjualan.NoTelp;

            sw.WriteLine(kiri0.PadRight(51, ' ') + ' ' + kanan0);
            sw.WriteLine(kiri1.PadRight(51, ' ') + ' ' + kanan1);
            sw.WriteLine(kiri2.PadRight(51, ' ') + ' ' + kanan2);
            sw.WriteLine(kiri3.PadRight(51, ' ') + ' ' + kanan3);
            sw.WriteLine(kiri4.PadRight(51, ' ') + ' ' + kanan4);
            sw.WriteLine(kiri5.PadRight(51, ' ') + ' ' + kanan5);

            //  header
            //           "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 12345678 0"
            sw.WriteLine("------------------------------------------------------------------------------------------");
            sw.WriteLine("No| Nama Barang                               | Qty | Harga     | Diskon    | Sub Total   ");
            sw.WriteLine("------------------------------------------------------------------------------------------");

            //  content
            var lineCounter = 0;
            foreach (var item in _penjualan.ListBrg.OrderBy(x => x.NoUrut))
            {
                var no = (item.NoUrut + 1).ToString().PadLeft(2, ' ');
                var namaBrg = item.BrgName.PadRight(43, ' ');
                //if (namaBrg.ToLower().Contains("jasa"))
                //    namaBrg = GetNamaBrgJasa(item.NoUrut);
                var qty = item.Qty.ToString().PadLeft(5, ' ');
                var harga = item.Harga.ToString("N0").PadLeft(11);
                var diskon = item.Diskon.ToString("N0").PadLeft(11);
                var subTotal = item.SubTotal.ToString("N0").PadLeft(11);
                sw.WriteLine(no + '|' + namaBrg + '|' + qty + '|' + harga + '|' + diskon + '|' + subTotal);
                lineCounter++;
            }

            if (lineCounter < 8)
                for (int i = lineCounter; i <= 10; i++)
                    sw.WriteLine("  |                                           |     |           |           |             ");
            sw.WriteLine("------------------------------------------------------------------------------------------");
            //               "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 12345678 0"
            var nilaiTotal = "Total      : " + _penjualan.NilaiTotal.ToString("N0").PadLeft(11, ' ');
            var nilaiDiskn = "Diskon Lain: " + _penjualan.NilaiDiskonLain.ToString("N0").PadLeft(11, ' ');
            var nilaiBiaya = "Biaya Lain : " + _penjualan.NilaiBiayaLain.ToString("N0").PadLeft(11, ' ');
            var nilaiGrand = "Grand Total: " + _penjualan.NilaiGrandTotal.ToString("N0").PadLeft(11, ' ');
            //sw.WriteLine("                                                                --------------------------");
            sw.WriteLine(nilaiTotal.PadLeft(88));
            sw.WriteLine(nilaiDiskn.PadLeft(88));
            sw.WriteLine(nilaiBiaya.PadLeft(88));
            sw.WriteLine(nilaiGrand.PadLeft(88));
            sw.WriteLine(" ");
            sw.WriteLine("                                                                Hormat Kami,");
            sw.WriteLine("Terimakasih seudah berbelanja di tempat kami.");
            sw.WriteLine("Brg yg sudah dibeli, tidak dapat dikembalikan.                  Kasir: DIAS ");

            sw.Close();
        }

        public void PrintDoc(string doc)
        {
            throw new NotImplementedException();
        }
    }
}
