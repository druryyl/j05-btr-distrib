using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using btr.application.SalesContext.AlokasiFpAgg;
using btr.application.SalesContext.EFakturAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.domain.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Mapster;
using Color = System.Drawing.Color;

namespace btr.distrib.SalesContext.AlokasiFpAgg
{
    public partial class AlokasiFpForm : Form
    {
        private AlokasiFpModel _agg;

        private readonly ITglJamDal _dateTime;
        private readonly IAlokasiFpBuilder _alokasiBuilder;
        private readonly IAlokasiFpWriter _alokasiWriter;
        private readonly IAlokasiFpDal _alokasiFpdal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IFakturAlokasiFpItemDal _fakturAlokasiFpItemDal;
        private readonly IEFakturBuilder _efakturBuilder;
        
        private BindingList<FakturAlokasiFpItemView> _listFaktur;
        
        private ContextMenu _alokasiMenu;

        public AlokasiFpForm(ITglJamDal dateTime,
            IAlokasiFpBuilder builder,
            IAlokasiFpWriter writer,
            IAlokasiFpDal alokasiFpdal,
            IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter, 
            IFakturAlokasiFpItemDal fakturAlokasiFpItemDal, 
            IEFakturBuilder efakturBuilder)
        {
            _dateTime = dateTime;
            _alokasiBuilder = builder;
            _alokasiWriter = writer;
            _alokasiFpdal = alokasiFpdal;
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;
            _fakturAlokasiFpItemDal = fakturAlokasiFpItemDal;
            _efakturBuilder = efakturBuilder;

            _agg = new AlokasiFpModel();

            InitializeComponent();
            RegisterEventHandler();
            InitPeriode();
            InitFakturGrid();
            InitAlokasiGrid();
            InitContextMenu();
        }

        private void RegisterEventHandler()
        {
            
            AlokasiButton.Click += AlokasiButton_Click;

            AlokasiGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            AlokasiGrid.MouseClick += AlokasiGrid_MouseClick;

            ListButton.Click += ListButton_Click;
            
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            
            FakturGrid.ColumnHeaderMouseDoubleClick +=FakturGrid_ColumnHeaderMouseDoubleClick;
            ProsesButton.Click += ProsesButton_Click;
            
            ImportEFakturButton.Click += ImportEFakturButton_Click;
            ExportExcelButton.Click += ExportExcelButton_Click; 
        }

        private void ExportExcelButton_Click(object sender, EventArgs e)
        {

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) 
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Alokasi E-Faktur")
                    .FirstCell()
                    .InsertTable(_listFaktur, false);
                wb.SaveAs(filePath);
            }
        }

        private bool _sortAsc = true;
        private void FakturGrid_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            var colName = grid.Columns[e.ColumnIndex].Name;

            switch (colName)
            {
                case "CustomerName":
                    _listFaktur = _sortAsc ? 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderBy(x => x.CustomerName).ToList()) : 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderByDescending(x => x.CustomerName).ToList());
                    break;
                case "NoFakturPajak":
                    _listFaktur = _sortAsc ? 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderBy(x => x.NoFakturPajak).ToList()) : 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderByDescending(x => x.NoFakturPajak).ToList());
                    break;
                case "Npwp":
                    _listFaktur = _sortAsc ? 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderBy(x => x.Npwp).ToList()) : 
                        new BindingList<FakturAlokasiFpItemView>(_listFaktur.OrderByDescending(x => x.Npwp).ToList());
                    break;
            }


            var binding = new BindingSource();
            binding.DataSource = _listFaktur;
            grid.DataSource = binding;
            _sortAsc = !_sortAsc;
        }

        #region IMPORT-EFAKTUR
        private void ImportEFakturButton_Click(object sender, EventArgs e)
        {
            var listEFaktur = new List<EFakturModel>();
            listEFaktur.AddRange(_listFaktur
                .Where(x => x.NoFakturPajak.Length > 0)
                .Select(item => _fakturBuilder.Load(item).Build())
                .Select(faktur => _efakturBuilder.Create(faktur).Build()));
            SaveToCsv(listEFaktur);
        }

        private static void SaveToCsv(List<EFakturModel> listEFaktur)
        {
            var sb = new StringBuilder();
            //      header
            sb.Append(
                @"FK,KD_JENIS_TRANSAKSI,FG_PENGGANTI,NOMOR_FAKTUR,MASA_PAJAK,TAHUN_PAJAK,TANGGAL_FAKTUR,NPWP,NAMA,ALAMAT_LENGKAP,JUMLAH_DPP,JUMLAH_PPN,JUMLAH_PPNBM,ID_KETERANGAN_TAMBAHAN,FG_UANG_MUKA,UANG_MUKA_DPP,UANG_MUKA_PPN,UANG_MUKA_PPNBM,REFERENSI,KODE_DOKUMEN_PENDUKUNG");
            sb.Append(Environment.NewLine);
            sb.Append(
                @"LT,NPWP,NAMA,JALAN,BLOK,NOMOR,RT,RW,KECAMATAN,KELURAHAN,KABUPATEN,PROPINSI,KODE_POS,NOMOR_TELEPON");
            sb.Append(Environment.NewLine);
            sb.Append(@"OF,KODE_OBJEK,NAMA,HARGA_SATUAN,JUMLAH_BARANG,HARGA_TOTAL,DISKON,DPP,PPN,TARIF_PPNBM,PPNBM");
            sb.Append(Environment.NewLine);
            //      content
            const string fapr = @"FAPR,CV. BINTANG TIMUR RAHAYU,JALAN KALIURANG KM.5 GANG.DURMO NO.18 RT.012 RW.005 CATURTUNGGAL DEPOK  KAB.SLEMAN DAERAH ISTIMEWA YOGYAKARTA,Admin,,967913591542000,,,,,,,,,,,,,";
            foreach (var item in listEFaktur)
            {
                sb.Append("FK,")
                    .Append($"{item.KD_JENIS_TRANSAKSI},")
                    .Append($"{item.FG_PENGGANTI},")
                    .Append($"{item.NOMOR_FAKTUR},")
                    .Append($"{item.MASA_PAJAK},")
                    .Append($"{item.FG_PENGGANTI},")
                    .Append($"{item.TANGGAL_FAKTUR},")
                    .Append($"{item.NPWP},")
                    .Append($"{item.NAMA},")
                    .Append($"{item.ALAMAT_LENGKAP},")
                    .Append($"{item.JUMLAH_DPP},")
                    .Append($"{item.JUMLAH_PPN},")
                    .Append($"{item.ID_KETERANGAN_TAMBAHAN},")
                    .Append($"{item.FG_UANG_MUKA},")
                    .Append($"{item.UANG_MUKA_DPP},")
                    .Append($"{item.UANG_MUKA_PPN},")
                    .Append($"{item.UANG_MUKA_PPNBM},")
                    .Append($"{item.REFERENSI},")
                    .Append($"{item.KODE_DOKUMEN_PENDUKUNG},")
                    .Append($"{Environment.NewLine}");
                sb.Append(fapr)
                    .Append($"{Environment.NewLine}");
                foreach (var item2 in item.ListItem)
                {
                    sb.Append("OF,")
                        .Append($"{item2.KODE_OBJEK},")
                        .Append($"{item2.NAMA},")
                        .Append($"{item2.HARGA_SATUAN},")
                        .Append($"{item2.JUMLAH_BARANG},")
                        .Append($"{item2.HARGA_TOTAL},")
                        .Append($"{item2.DISKON},")
                        .Append($"{item2.DPP},")
                        .Append($"{item2.PPN},")
                        .Append($"{item2.TARIF_PPNBM},")
                        .Append($"{item2.PPNBM}")
                        .Append($"{Environment.NewLine}");
                }
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"CSV Files|*.csv";
                saveFileDialog.Title = @"Save CSV File";
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.AddExtension = true;
                //      Show the SaveFileDialog and get the user's response
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                //      Get the selected file path from the dialog
                var filePath = saveFileDialog.FileName;
                //      Write the CSV string to the selected file
                File.WriteAllText(filePath, sb.ToString());
            }
        }
        #endregion
        
        #region ALOKASI-NOMOR
        private void AlokasiButton_Click(object sender, EventArgs e)
        {
            CreateAlokasi();
            NoAwalText.Text = string.Empty;
            NoAkhirText.Text = string.Empty;
            RefreshAlokasiGrid();
        }

        private void CreateAlokasi()
        {
            var nsfp = _alokasiBuilder
                .Create()
                .NomorSeri(NoAwalText.Text, NoAkhirText.Text)
                .Build();
            _alokasiWriter.Save(nsfp);
        }
        #endregion

        #region GRID-ALOKASI
        private void RemoveAlokasiMenu_Click(object sender, EventArgs e)
        {
            RemoveAlokasi();
        }

        private void AlokasiGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            _alokasiMenu.Show((DataGridView)sender, e.Location);
        }

        private void PilihAlokasiMenu_Click(object sender, EventArgs e)
        {
            if (AlokasiGrid.CurrentRow is null)
                return;
            
            var alokasiId = AlokasiGrid.CurrentRow.Cells["AlokasiId"].Value.ToString();
            _agg = _alokasiBuilder
                .Load(new AlokasiFpModel(alokasiId))
                .Build();

            NoAwalLabel.Text = _agg.NoAwal;
            NoAkhirLabel.Text = _agg.NoAkhir;
        }

        private void InitAlokasiGrid()
        {
            RefreshAlokasiGrid();
            var g = AlokasiGrid.Columns;
            g.SetDefaultCellStyle(Color.LightCyan);
            g.GetCol("AlokasiId").Visible = false;
            g.GetCol("NoSeriFp").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            g.GetCol("NoSeriFp").Width = 130;
            g.GetCol("Sisa").Width = 40;
            AlokasiGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            AlokasiGrid.AutoResizeRows();
        }

        private void InitContextMenu()
        {
            _alokasiMenu = new ContextMenu();
            _alokasiMenu.MenuItems.Add(new MenuItem("Pilih Alokasi", PilihAlokasiMenu_Click));
            _alokasiMenu.MenuItems.Add(new MenuItem("Cancel Alokasi", RemoveAlokasiMenu_Click));

        }


        private void RefreshAlokasiGrid()
        {
            var periode = new Periode(_dateTime.Now.AddDays(-31), _dateTime.Now);
            var listAlokasi = _alokasiFpdal.ListData(periode)?.ToList()
                ?? new List<AlokasiFpModel>();
            var projection = listAlokasi
                .Select(x => new AlokasiFpAlokasiDto(x.AlokasiFpId,
                    $"{x.NoAwal}{Environment.NewLine}{x.NoAkhir}", x.Sisa))
                .ToList();
            AlokasiGrid.DataSource = projection;
            AlokasiGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            AlokasiGrid.AutoResizeRows();
        }

        private void RemoveAlokasi()
        {

            if (AlokasiGrid.CurrentRow is null)
                return;
            
            var alokasiId = AlokasiGrid.CurrentRow.Cells["AlokasiId"].Value.ToString();
            var alokasi = _alokasiBuilder
                .Load(new AlokasiFpModel(alokasiId))
                .Build();

            var listFaktur = new List<FakturModel>();
            foreach(var item in alokasi.ListItem)
            {
                if (item.FakturId.Length == 0)
                    continue;

                var faktur = _fakturBuilder
                    .Load(item)
                    .FakturPajak(string.Empty)
                    .Build();
                listFaktur.Add(faktur);
            }

            using (var trans = TransHelper.NewScope())
            {
                _alokasiWriter.Delete(alokasi);
                foreach (var item in listFaktur)
                    _ = _fakturWriter.Save(item);

                trans.Complete();
            }

            RefreshAlokasiGrid();
            RefreshFakturGrid();

        }
        #endregion

        #region PERIODE
        private void ListButton_Click(object sender, EventArgs e)
        {
            RefreshFakturGrid();
        }

        private void InitPeriode()
        {
            Periode1Date.Value = _dateTime.Now.AddDays(-3);
            Periode2Date.Value = _dateTime.Now;
        }

        #endregion

        #region PROSES
        private void ProsesButton_Click(object sender, EventArgs e)
        {
            ProsesGenNoSeriFaktur();
        }
        private void ProsesGenNoSeriFaktur()
        {
            if (_agg?.ListItem is null)
                throw new InvalidOperationException("Alokasi Faktur belum ada yg terpilih");

            var listUpdatedFaktur = new List<FakturModel>();
            foreach(var item in _listFaktur)
            {
                //      jika sudah terisi nomor-seri-nya, lanjutkan ke faktur berikutnya
                //      (abaikan yang ini)
                if (item.NoFakturPajak.Length > 0)
                    continue;

                //      get available no-seri-faktur
                var itemAlokasi = _agg.ListItem
                    .OrderBy(x => x.NoFakturPajak)
                    .FirstOrDefault(x => x.FakturId.Length == 0);
                
                //      jika alokasi sudah full, maka selesai
                if (itemAlokasi is null)
                    break;

                //      update alokasi
                itemAlokasi.FakturId = item.FakturId;
                itemAlokasi.FakturCode = item.FakturCode;
                
                //      update faktur
                var faktur = _fakturBuilder
                    .Load(item)
                    .FakturPajak(itemAlokasi.NoFakturPajak)
                    .Build();
                listUpdatedFaktur.Add(faktur);

                //      update visualisasi grid
                item.SetNoFakturPajak(itemAlokasi.NoFakturPajak);
                FakturGrid.Refresh();
            }

            //      simpan ke database
            using (var trans = TransHelper.NewScope())
            {
                _ = _alokasiWriter.Save(_agg);
                foreach (var item in listUpdatedFaktur)
                    _ = _fakturWriter.Save(item);
                trans.Complete();
            }

            MessageBox.Show(@"Proses selesai");
        }
        #endregion

        #region GRID-FAKTUR
        private void InitFakturGrid()
        {
            RefreshFakturGrid();
            var g = FakturGrid.Columns;
            g.SetDefaultCellStyle(Color.Wheat);
            g.GetCol("FakturId").Visible = false;

            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = @"Faktur";

            g.GetCol("UserId").Width = 50;
            g.GetCol("UserId").HeaderText = @"Admin";

            g.GetCol("FakturDate").DefaultCellStyle.Format = "dd-MM-yyyy";
            g.GetCol("FakturDate").Width = 80;
            g.GetCol("FakturDate").HeaderText = @"Tgl";

            g.GetCol("CustomerName").Width = 80;
            g.GetCol("CustomerName").HeaderText = @"Customer";

            g.GetCol("Npwp").Width = 100;
            g.GetCol("Npwp").HeaderText = @"NPWP";

            g.GetCol("Address").Width = 100;
            g.GetCol("GrandTotal").Width = 80;
            g.GetCol("NoFakturPajak").Width = 140;

            g.GetCol("VoidDate").Visible = false;
            g.GetCol("UserIdVoid").Visible = false;
        }

        private void RefreshFakturGrid()
        {
            var periode = new Periode(Periode1Date.Value, Periode2Date.Value);
            var listFaktur = _fakturAlokasiFpItemDal.ListData(periode)?.ToList()
                ?? new List<FakturAlokasiFpItemView>();

            var dataSource = listFaktur
                .Where(x => x.VoidDate == new DateTime(3000,1,1))
                .OrderBy(x => x.FakturId)
                .Adapt<List<FakturAlokasiFpItemView>>();
            _listFaktur = new BindingList<FakturAlokasiFpItemView>(dataSource);
            FakturGrid.DataSource = _listFaktur;
            FakturGrid.Refresh();
        }
        #endregion
    }

    public class AlokasiFpAlokasiDto
    {
        public AlokasiFpAlokasiDto(string alokasiId, string noSeriFp, int sisa)
        {
            AlokasiId = alokasiId;
            NoSeriFp = noSeriFp;
            Sisa = sisa;
        }
        public string AlokasiId { get; private set; }
        public string NoSeriFp { get; private set; }
        public int Sisa { get; private set; }
    }
}
