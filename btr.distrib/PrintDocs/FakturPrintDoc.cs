using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
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
                var hrg1 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1)?.HargaJual.ToString(CultureInfo.InvariantCulture).FixWidth(8) ?? string.Empty;
                var hrg2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2)?.HargaJual.ToString(CultureInfo.InvariantCulture).FixWidth(8) ?? string.Empty;
                var disc1 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 1)?.DiscountProsen.ToString(CultureInfo.InvariantCulture).FixWidth(3) ?? string.Empty;
                var disc2 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 2)?.DiscountProsen.ToString(CultureInfo.InvariantCulture).FixWidth(3) ?? string.Empty;
                var disc3 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 3)?.DiscountProsen.ToString(CultureInfo.InvariantCulture).FixWidth(3) ?? string.Empty;
                var disc4 = item.ListDiscount.FirstOrDefault(x => x.NoUrut == 4)?.DiscountProsen.ToString(CultureInfo.InvariantCulture).FixWidth(3) ?? string.Empty;
                var total = item.Total.ToString("N").FixWidth(14);
                await sw.WriteLineAsync($"{no}|{brgId}|{brgName}|{bonus}|{qty1}|{qty2}|{hrg1}|{hrg2}|{disc1}|{disc2}|{disc3}|{disc4}|{total}|");
                i++;
            }
            sw.Close();
        }

        public void PrintDoc(string doc)
        {
            throw new NotImplementedException();
        }
    }
}
