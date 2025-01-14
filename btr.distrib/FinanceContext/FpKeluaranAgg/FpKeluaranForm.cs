using btr.application.FinanceContext.FpKeluaragAgg;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.distrib.Helpers;
using btr.distrib.InventoryContext.StokBalanceRpt;
using btr.distrib.SharedForm;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    public partial class FpKeluaranForm : Form
    {
        private readonly IFakturDal _fakturDal;
        private readonly IFpKeluaranBuilder _fpKeluaranBuilder;
        private readonly IFpKeluaranWriter _fpKeluaranWriter;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly ICustomerBuilder _customerBuilder;


        private readonly BindingList<FpKeluaranFakturDto> _listFaktur;
        private readonly BindingList<FpKeluaranFakturDto> _listFakturPilih;
        private readonly BindingSource _fakturBindingSource;

        private const string NPWP_PENJUAL = "1288723065240000";
        private const string ID_TKU_PENJUAL = "1288723065240000000000";



        public FpKeluaranForm(IFakturDal fakturDal,
            IFpKeluaranBuilder fpKeluaranBuilder,
            IFpKeluaranWriter fpKeluaranWriter,
            IFakturBuilder fakturBuilder,
            ICustomerBuilder customerBuilder,
            IFakturWriter fakturWriter)
        {
            InitializeComponent();

            _fakturDal = fakturDal;
            _listFaktur = new BindingList<FpKeluaranFakturDto>();
            _listFakturPilih = new BindingList<FpKeluaranFakturDto>();
            _fakturBindingSource = new BindingSource(_listFaktur, null);

            RegisterEventHandler();
            InitGrid();
            InitCalender();
            _fpKeluaranBuilder = fpKeluaranBuilder;
            _fpKeluaranWriter = fpKeluaranWriter;
            _fakturBuilder = fakturBuilder;
            _customerBuilder = customerBuilder;
            _fakturWriter = fakturWriter;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SaveButton.Click += SaveButton_Click;
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var fpKeluaran = Save();
            ClearScreen();
            LastIdLabel.Text = fpKeluaran.FpKeluaranId;
            GenerateExcel(fpKeluaran);
        }

        private void GenerateExcel(FpKeluaranModel fpKeluaran)
        {
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"fp-keluaran-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var listToExcel = fpKeluaran
                .ListFaktur
                .Select(x => new
                {
                    Baris = x.Baris.ToString(),
                    x.TanggalFaktur,
                    x.JenisFaktur,
                    x.KodeTransaksi,
                    x.KeteranganTambahan,
                    x.DokumenPendukung,
                    x.Referensi,
                    x.CapFasilitas,
                    x.IdTkuPenjual,
                    x.NpwpNikPembeli,
                    x.JenisIdPembeli,
                    x.NegaraPembeli,
                    x.NomorDokumenPembeli,
                    x.NamaPembeli,
                    x.AlamatPembeli,
                    x.EmailPembeli,
                    x.IdTkuPembeli,
                });

            var listToExcel2 = fpKeluaran.ListFaktur
                .SelectMany(x => x.ListBrg)
                .Select(x => new
                {
                    Baris = x.Baris.ToString(),
                    x.BarangJasa,
                    x.KodeBarangJasa,
                    x.NamaBarangJasa,
                    x.NamaSatuanUkur,
                    x.HargaSatuan,
                    x.JumlahBarangJasa,
                    x.TotalDiskon,
                    x.Dpp,
                    x.DppLain,
                    x.TarifPpn,
                    x.Ppn,
                    x.TarifPpnBm,
                    x.PpnBm
                });

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet();
                var ws1 = wb.Worksheets.First();
                ws1.Name = "Faktur";
                ws1.Cell($"A1").Value = "NPWP Penjual";
                ws1.Cell($"C1").Value = NPWP_PENJUAL;
                ws1.Cell($"A3").InsertTable(listToExcel, false);
                
                ws1.Cell($"B3").Value = "Tanggal Faktur";
                ws1.Cell($"C3").Value = "Jenis Faktur";
                ws1.Cell($"D3").Value = "Kode Transaksi";
                ws1.Cell($"E3").Value = "Keterangan Tambahan";
                ws1.Cell($"F3").Value = "Dokumen Pendukung";
                ws1.Cell($"G3").Value = "Referensi";
                ws1.Cell($"H3").Value = "Cap Fasilitas";
                ws1.Cell($"I3").Value = "ID TKU Penjual";
                ws1.Cell($"J3").Value = "NPWP/NIK Pembeli";
                ws1.Cell($"K3").Value = "Jenis ID Pembeli";
                ws1.Cell($"L3").Value = "Negara Pembeli";
                ws1.Cell($"M3").Value = "Nomor Dokumen Pembeli";
                ws1.Cell($"N3").Value = "Nama Pembeli";
                ws1.Cell($"O3").Value = "Alamat Pembeli";
                ws1.Cell($"P3").Value = "Email Pembeli";
                ws1.Cell($"Q3").Value = "ID TKU Pembeli";
                ws1.Columns().AdjustToContents();
                var barisAkhir = listToExcel.Count() + 4;
                ws1.Cell($"A{barisAkhir}").Value = "END";

                wb.AddWorksheet();
                var ws2 = wb.Worksheets.Last();
                ws2.Name = "DetailFaktur";
                ws2.Cell($"A1").InsertTable(listToExcel2, false);

                ws2.Cell($"A1").Value = "Baris";
                ws2.Cell($"B1").Value = "Barang/Jasa";
                ws2.Cell($"C1").Value = "Kode Barang Jasa";
                ws2.Cell($"D1").Value = "Nama Barang/Jasa";
                ws2.Cell($"E1").Value = "Nama Satuan Ukur";
                ws2.Cell($"F1").Value = "Harga Satuan";
                ws2.Cell($"G1").Value = "Jumlah Barang Jasa";
                ws2.Cell($"H1").Value = "Total Diskon";
                ws2.Cell($"I1").Value = "DPP";
                ws2.Cell($"J1").Value = "DPP Lain";
                ws2.Cell($"K1").Value = "Tarif PPN";
                ws2.Cell($"L1").Value = "PPN";
                ws2.Cell($"M1").Value = "Tarif PPnBM";
                ws2.Cell($"N1").Value = "PPnBM";
                barisAkhir = listToExcel2.Count() + 2;
                ws2.Cell($"A{barisAkhir}").Value = "END";


                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void ClearScreen()
        {
            FpKeluaranText.Clear();
            FpKeluaranDateText.Value = DateTime.Now;
            _listFaktur.Clear();
        }

        private FpKeluaranModel Save()
        {
            FpKeluaranModel fpKeluaran;
            if (FpKeluaranText.Text == string.Empty)
            {
                fpKeluaran = _fpKeluaranBuilder
                    .Create()
                    .Build();
            }
            else
                fpKeluaran = _fpKeluaranBuilder
                    .Load(new FpKeluaranModel(FpKeluaranText.Text))
                    .Build();

            var mainform = (MainForm)this.Parent.Parent;
            var userId = mainform.UserId;
            fpKeluaran = _fpKeluaranBuilder
                .Attach(fpKeluaran)
                .Tanggal(FpKeluaranDateText.Value)
                .User(userId)
                .Build();

            var listFaktur = _listFaktur
                .Where(x => x.IsPilih).ToList();

            foreach(var item in listFaktur)
            {
                var faktur = _fakturBuilder.Load(item).Build();
                var customer = _customerBuilder.Load(faktur).Build();
                var fpFaktur = new FpKeluaranFakturModel();
                fpFaktur.CreateFrom(faktur, customer);
                fpFaktur.ListBrg = new List<FpKeluaranBrgModel>();
                foreach (var brg in faktur.ListItem)
                {
                    var newBrg = new FpKeluaranBrgModel();
                    newBrg.CreateFrom(brg);
                    fpFaktur.ListBrg.Add(newBrg);
                }

                fpKeluaran.ListFaktur.Add(fpFaktur);
            }

            FpKeluaranModel result;
            using(var trans = TransHelper.NewScope())
            {
                result = _fpKeluaranWriter.Save(fpKeluaran);
                foreach (var item in result.ListFaktur)
                {
                    var faktur = _fakturBuilder
                        .Load(item)
                        .FpKeluaran(result.FpKeluaranId)
                        .Build();
                    _fakturWriter.Save(faktur);
                }
                trans.Complete();
            }

            return result;
        }
        #endregion

        private void InitCalender()
        {
            PeriodeCalender.MaxSelectionCount = 31;
            PeriodeCalender.SelectionStart = DateTime.Now;
            PeriodeCalender.SelectionEnd = DateTime.Now;
        }

        #region GRID
        private void InitGrid()
        {
            FakturGrid.DataSource = _fakturBindingSource;
            FakturGrid.DefaultCellStyle.Font = new Font("Lucida Console", 8);

            var grid = FakturGrid.Columns;
            grid["FakturId"].Visible = false;

            grid["FakturCode"].Width = 70;
            grid["FakturDate"].Width = 80;
            grid["CustomerName"].Width = 130;
            grid["Npwp"].Width = 125;
            grid["Address"].Width = 220;
            grid["GrandTotal"].Width = 80;
            grid["IsPilih"].Width = 30;

            grid["FakturId"].HeaderText = "ID";
            grid["FakturCode"].HeaderText = "Kode";
            grid["FakturDate"].HeaderText = "Tanggal";
            grid["CustomerName"].HeaderText = "Customer";
            grid["Npwp"].HeaderText = "NPWP";
            grid["Address"].HeaderText = "Alamat";
            grid["GrandTotal"].HeaderText = "Total";
            grid["IsPilih"].HeaderText = "Pilih";

            grid["FakturDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
            grid["GrandTotal"].DefaultCellStyle.Format = "N0";
            grid["GrandTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["CustomerName"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            grid["Address"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
        }
        #endregion


        #region SEARCH
        private void SearchButton_Click(object sender, EventArgs e)
        {
            PreserveTerpilih();
            SearchFaktur();
            RestoreTerpilih();
        }

        private void PreserveTerpilih()
        {
            var listTerpilih = _listFaktur.Where(x => x.IsPilih).ToList();
            _listFakturPilih.Clear();
            foreach (var item in listTerpilih)
                _listFakturPilih.Add(item);
        }

        private void RestoreTerpilih()
        {
            //  remove _listFaktur which is already in _listFakturPilih
            foreach (var item in _listFakturPilih)
            {
                var faktur = _listFaktur.FirstOrDefault(x => x.FakturId == item.FakturId);
                if (faktur != null)
                    _listFaktur.Remove(faktur);
            }
            int i = 0;
            foreach (var item in _listFakturPilih)
            {
                _listFaktur.Insert(i, item);
                i++;
            }
        }

        public void SearchFaktur()
        {
            var tgl1 = PeriodeCalender.SelectionStart;
            var tgl2 = PeriodeCalender.SelectionEnd;
            var periode = new Periode(tgl1, tgl2);
            var listFaktur = _fakturDal.ListData(periode)?.ToList() ?? new List<FakturModel>();
            listFaktur.RemoveAll(x => x.FpKeluaranId != string.Empty);

            var searchText = SearchText.Text.ToUpper();
            if (searchText != string.Empty)
                listFaktur = listFaktur.Where(x => x.FakturCode.ToUpper().Contains(searchText) || x.CustomerName.ToUpper().Contains(searchText)).ToList();

            _listFaktur.Clear();
            foreach(var item in listFaktur)
            {
                var newItem = new FpKeluaranFakturDto(item.FakturId, item.FakturCode, item.FakturDate, item.CustomerName, item.Npwp, item.Address, item.GrandTotal, false);
                _listFaktur.Add(newItem);
            }
        }
        #endregion
    }
}
