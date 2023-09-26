using System.Collections.Generic;

public class EFakturModel
{
    public string KD_JENIS_TRANSAKSI { get; set; }
    public int FG_PENGGANTI { get; set; }
    public string NOMOR_FAKTUR { get; set; }
    public int MASA_PAJAK { get; set; }
    public int TAHUN_PAJAK { get; set; }
    public string TANGGAL_FAKTUR { get; set; }
    public string NPWP { get; set; }
    public string NAMA { get; set; }
    public string ALAMAT_LENGKAP { get; set; }
    public decimal JUMLAH_DPP { get; set; }
    public decimal JUMLAH_PPN { get; set; }
    public decimal JUMLAH_PPNBM { get; set; }
    public string ID_KETERANGAN_TAMBAHAN { get; set; }
    public decimal FG_UANG_MUKA { get; set; }
    public decimal UANG_MUKA_DPP { get; set; }
    public decimal UANG_MUKA_PPN { get; set; }
    public decimal UANG_MUKA_PPNBM { get; set; }
    public string REFERENSI { get; set; }
    public int KODE_DOKUMEN_PENDUKUNG { get; set; }
    public List<EFakturItemModel> ListItem { get; set; }
}

public class EFakturItemModel
{
    public string KODE_OBJEK { get; set; }
    public string NAMA { get; set; }
    public decimal HARGA_SATUAN { get; set; }
    public decimal JUMLAH_BARANG { get; set; }
    public decimal HARGA_TOTAL { get; set; }
    public decimal DISKON { get; set; }
    public decimal DPP { get; set; }
    public decimal PPN { get; set; }
    public decimal TARIF_PPNBM { get; set; }
    public decimal PPNBM { get; set; }
}
