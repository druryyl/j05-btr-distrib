using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using btr.nuna.Domain;

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

            async void PrintHeader()
            {
                var tgl = model.FakturDate.ToString("dd-MM-yyyy");
                var custId = model.CustomerId.PadRight(12, ' ');
                var noFakt = model.FakturId.FixWidth(8);
                var jnsJl = model.TermOfPayment.FixWidth(7);
                var salesNameeeee = model.SalesPersonName.FixWidth(13);
                var jatuhTmpo = string.Empty; // jatuh temp

                //  print area : 112 char x 29 baris Consolas 8
                //          "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789  2"
                var hdr1 = $"CV BINTANG TIMUR RAHAYU                                   {tgl}";
                var hdr2 = $"Jl.Kaliurang Km 5.5 Gg Durmo No 18                        Kepada Yth Customer-{custId}";
                var hdr3 = $"0274-546079                                               {cust.CustomerName}";
                var hdr4 = $"Yogyakarta                                                {cust.Address1}";
                var hdr5 = $"FAKTUR PENJUALAN                                          {cust.Address2}";
                var hdr7 = $"No.Faktur: {noFakt}   Jns Jual: {jnsJl}   Sales: {salesNameeeee}   Jth Tempo: {jatuhTmpo}";
                var hdr6 = $" ";
                var hdr8 = $"--+-------+-------------------------+--+-------------+-----------------+---------------+--------------";
                var hdr9 = $"No|Kode   |Nama Barang              |  |  Kwantitas  |      @Harga     |    Discount   | Total Harga  ";
                var hdrA = $"--+-------+-------------------------+--+------+------+--------+--------+---+---+---+---+--------------";
                
                await sw.WriteLineAsync(hdr1);
                await sw.WriteLineAsync(hdr2);
                await sw.WriteLineAsync(hdr3);
                await sw.WriteLineAsync(hdr4);
                await sw.WriteLineAsync(hdr5);
                await sw.WriteLineAsync(hdr6);
                await sw.WriteLineAsync(hdr7);
                await sw.WriteLineAsync(hdr8);
                await sw.WriteLineAsync(hdr9);
                await sw.WriteLineAsync(hdrA);
            }

            var i = 1;
            foreach(var item in model.ListItem)
            {
                if (i % 5 == 1)
                    PrintHeader();
                var no = i.ToString("D2");
                var brgId = item.BrgId.FixWidth(7);
                var brgName = item.BrgName.FixWidth(25);
                var bonus = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3)?.Qty.ToString() ?? "0";
                var qty1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.Qty.ToString().FixWidth(6) ?? string.Empty;
                var qty2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.Qty.ToString().FixWidth(6) ?? string.Empty;
                var hrg1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.HargaJual.ToString().FixWidth(8) ?? string.Empty;
                var hrg2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.HargaJual.ToString().FixWidth(8) ?? string.Empty;
                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscountProsen.ToString().FixWidth(3) ?? string.Empty;
                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscountProsen.ToString().FixWidth(3) ?? string.Empty;
                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscountProsen.ToString().FixWidth(3) ?? string.Empty;
                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscountProsen.ToString().FixWidth(3) ?? string.Empty;
                var total = item.Total.ToString("N").FixWidth(14);
                await sw.WriteLineAsync($"{no}|{brgId}|{brgName}|{bonus}|{qty1}|{qty2}|{hrg1}|{hrg2}|{disc1}|{disc2}|{disc3}|{disc4}|{total}|");
                i++;
            }
            sw.Close();
            
            //sw.WriteLine(kiri0.PadRight(51, ' ') + ' ' + kanan0);
            //sw.WriteLine(kiri1.PadRight(51, ' ') + ' ' + kanan1);
            //sw.WriteLine(kiri2.PadRight(51, ' ') + ' ' + kanan2);
            //sw.WriteLine(kiri3.PadRight(51, ' ') + ' ' + kanan3);
            //sw.WriteLine(kiri4.PadRight(51, ' ') + ' ' + kanan4);
            //sw.WriteLine(kiri5.PadRight(51, ' ') + ' ' + kanan5);

            ////  header
            ////           "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 12345678 0"
            //sw.WriteLine("------------------------------------------------------------------------------------------");
            //sw.WriteLine("No| Nama Barang                               | Qty | Harga     | Diskon    | Sub Total   ");
            //sw.WriteLine("------------------------------------------------------------------------------------------");

            ////  content
            //var lineCounter = 0;
            //foreach (var item in _penjualan.ListBrg.OrderBy(x => x.NoUrut))
            //{
            //    var no = (item.NoUrut + 1).ToString().PadLeft(2, ' ');
            //    var namaBrg = item.BrgName.PadRight(43, ' ');
            //    //if (namaBrg.ToLower().Contains("jasa"))
            //    //    namaBrg = GetNamaBrgJasa(item.NoUrut);
            //    var qty = item.Qty.ToString().PadLeft(5, ' ');
            //    var harga = item.Harga.ToString("N0").PadLeft(11);
            //    var diskon = item.Diskon.ToString("N0").PadLeft(11);
            //    var subTotal = item.SubTotal.ToString("N0").PadLeft(11);
            //    sw.WriteLine(no + '|' + namaBrg + '|' + qty + '|' + harga + '|' + diskon + '|' + subTotal);
            //    lineCounter++;
            //}

            //if (lineCounter < 8)
            //    for (int i = lineCounter; i <= 10; i++)
            //        sw.WriteLine("  |                                           |     |           |           |             ");
            //sw.WriteLine("------------------------------------------------------------------------------------------");
            ////               "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 12345678 0"
            //var nilaiTotal = "Total      : " + _penjualan.NilaiTotal.ToString("N0").PadLeft(11, ' ');
            //var nilaiDiskn = "Diskon Lain: " + _penjualan.NilaiDiskonLain.ToString("N0").PadLeft(11, ' ');
            //var nilaiBiaya = "Biaya Lain : " + _penjualan.NilaiBiayaLain.ToString("N0").PadLeft(11, ' ');
            //var nilaiGrand = "Grand Total: " + _penjualan.NilaiGrandTotal.ToString("N0").PadLeft(11, ' ');
            ////sw.WriteLine("                                                                --------------------------");
            //sw.WriteLine(nilaiTotal.PadLeft(88));
            //sw.WriteLine(nilaiDiskn.PadLeft(88));
            //sw.WriteLine(nilaiBiaya.PadLeft(88));
            //sw.WriteLine(nilaiGrand.PadLeft(88));
            //sw.WriteLine(" ");
            //sw.WriteLine("                                                                Hormat Kami,");
            //sw.WriteLine("Terimakasih seudah berbelanja di tempat kami.");
            //sw.WriteLine("Brg yg sudah dibeli, tidak dapat dikembalikan.                  Kasir: DIAS ");

            //sw.Close();
        }

        public void PrintDoc(string doc)
        {
            throw new NotImplementedException();
        }
    }
}
