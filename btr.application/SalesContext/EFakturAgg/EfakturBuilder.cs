using System;
using System.Collections.Generic;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.EFakturAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.EFakturAgg
{
    
    public interface IEFakturBuilder : INunaBuilder<EFakturModel>
    {
        IEFakturBuilder Create(FakturModel faktur);
        
    }
    public class EfakturBuilder : IEFakturBuilder
    {
        private EFakturModel _aggregate;
        private readonly ICustomerDal _customerDal;

        public EfakturBuilder(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public EFakturModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IEFakturBuilder Create(FakturModel faktur)
        {
            if (faktur is null)
                throw new ArgumentException("Faktur invalid (null)");
            var customer = _customerDal.GetData(faktur)
                ?? throw new KeyNotFoundException("Customer not found");
            var alamatWp = $"{customer.AddressWp} {customer.AddressWp2}";
            _aggregate = new EFakturModel
            {
                KD_JENIS_TRANSAKSI = "01",
                FG_PENGGANTI = 0,
                NOMOR_FAKTUR = faktur.NoFakturPajak.Replace("-", "").Replace(".","").Replace(",",""),
                MASA_PAJAK = faktur.FakturDate.Month,
                TAHUN_PAJAK = faktur.FakturDate.Year,
                TANGGAL_FAKTUR = faktur.FakturDate.ToString("dd/MM/yyyy"),
                NPWP = faktur.Npwp.Length != 0 ? faktur.Npwp : "000000000000000",
                NAMA = customer.NamaWp.Length != 0 ? customer.NamaWp : customer.CustomerName,
                ALAMAT_LENGKAP = alamatWp.Length != 0 ? alamatWp : $"{customer.Address1} {customer.Address2}",
                JUMLAH_DPP = faktur.Total - faktur.Discount,
                JUMLAH_PPN = faktur.Tax,
                JUMLAH_PPNBM = 0,
                ID_KETERANGAN_TAMBAHAN = string.Empty,
                FG_UANG_MUKA = 0,
                UANG_MUKA_DPP = 0,
                UANG_MUKA_PPN = 0,
                UANG_MUKA_PPNBM = 0,
                REFERENSI = faktur.FakturCode,
                KODE_DOKUMEN_PENDUKUNG = 0,
                ListItem = new List<EFakturItemModel>()
            };

            foreach (var item in faktur.ListItem)
            {
                var newItem = new EFakturItemModel
                {
                    KODE_OBJEK = item.BrgCode,
                    NAMA = item.BrgName,
                    HARGA_SATUAN = item.HrgSat,
                    JUMLAH_BARANG = item.QtyJual,
                    HARGA_TOTAL = item.SubTotal,
                    DISKON = item.DiscRp,
                    DPP = item.SubTotal - item.DiscRp,
                    PPN = item.PpnRp,
                    TARIF_PPNBM = 0,
                    PPNBM = 0
                };
                _aggregate.ListItem.Add(newItem);
            }
            return this;
        }
    }
}