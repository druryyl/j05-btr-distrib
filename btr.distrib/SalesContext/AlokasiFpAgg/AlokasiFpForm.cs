using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using btr.application.SalesContext.AlokasiFpAgg;
using btr.application.SalesContext.EFakturAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.FakturPajakVoidAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.EFakturAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Mapster;
using Polly;
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
        private readonly IAlokasiFpItemDal _alokasiFpItemDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IFakturAlokasiFpItemDal _fakturAlokasiFpItemDal;
        private readonly IEFakturBuilder _efakturBuilder;
        private readonly IFakturPajakVoidBuilder _fakturPajakVoidBuilder;
        private readonly IFakturPajakVoidWriter _fakturPajakVoidWriter;
        private readonly IFakturPajakVoidDal _fakturPajakVoidDal;

        private BindingList<FakturAlokasiFpItemView> _listFaktur;
        
        private ContextMenu _alokasiMenu;
        private ContextMenu _fakturMenu;

        public AlokasiFpForm(ITglJamDal dateTime,
            IAlokasiFpBuilder builder,
            IAlokasiFpWriter writer,
            IAlokasiFpDal alokasiFpdal,
            IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter,
            IFakturAlokasiFpItemDal fakturAlokasiFpItemDal,
            IEFakturBuilder efakturBuilder,
            IFakturPajakVoidBuilder fakturPajakVoidBuilder,
            IFakturPajakVoidWriter fakturPajakVoidWriter,
            IFakturPajakVoidDal fakturPajakVoidDal,
            IAlokasiFpItemDal alokasiFpItemDal)
        {
            _dateTime = dateTime;
            _alokasiBuilder = builder;
            _alokasiWriter = writer;
            _alokasiFpdal = alokasiFpdal;
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;
            _fakturAlokasiFpItemDal = fakturAlokasiFpItemDal;
            _efakturBuilder = efakturBuilder;
            _fakturPajakVoidBuilder = fakturPajakVoidBuilder;
            _fakturPajakVoidWriter = fakturPajakVoidWriter;
            _fakturPajakVoidDal = fakturPajakVoidDal;
            _alokasiFpItemDal = alokasiFpItemDal;

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
            SearchButton.Click += SearchButton_Click;

            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            FakturGrid.ColumnHeaderMouseDoubleClick +=FakturGrid_ColumnHeaderMouseDoubleClick;
            FakturGrid.MouseClick += FakturGrid_MouseClick;
            FakturGrid.CellContentClick += FakturGrid_CellContentClick;

            ExportEFakturButton.Click += ExportEFakturButton_Click;
            ExportExcelButton.Click += ExportExcelButton_Click;

            Digit16Option.CheckedChanged += Digit1316Option_CheckedChanged;
            Digit13Option.CheckedChanged += Digit1316Option_CheckedChanged;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchText.Text.Length == 0) 
            {
                LoadFakturGrid();
                return; 
            }
            var filterFakturCode = _listFaktur.Where(x => x.FakturCode == SearchText.Text)?.ToList();
            var filterCustomerName = _listFaktur.Where(x => x.CustomerName.ContainMultiWord(SearchText.Text))?.ToList();
            var result = filterFakturCode.Union(filterCustomerName);
            RefreshFakturGrid(result);
        }

        private void Digit1316Option_CheckedChanged(object sender, EventArgs e)
        {
            if (Digit16Option.Checked)
                NoAwalText.Mask = "000.000-00.00000000";
            else
                NoAwalText.Mask = "000.00.00000000";

            NoAkhirText.Mask = NoAwalText.Mask;
        }


        #region EXPORT EXCEL
        private void ExportExcelButton_Click(object sender, EventArgs e)
        {

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"faktur-pajak-{_dateTime.Now:yyyy-MM-dd-HHmm}";
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
            System.Diagnostics.Process.Start(filePath);
        }
        #endregion

        #region EXPORT-CSV-EFAKTUR
        private void ExportEFakturButton_Click(object sender, EventArgs e)
        {
            var listEFaktur = new List<EFakturModel>();
            listEFaktur.AddRange(_listFaktur
                .Where(x => x.NoFakturPajak.Length > 0)
                .Select(item => _fakturBuilder.Load(item).Build())
                .Select(faktur => _efakturBuilder.Create(faktur).Build()));
            SaveToCsv(listEFaktur);
        }

        private  void SaveToCsv(List<EFakturModel> listEFaktur)
        {
            var toBeExported = listEFaktur.Where(x => x.NOMOR_FAKTUR.Length > 0).ToList();
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
            foreach (var item in toBeExported)
            {
                //  DPP dijumlah ulang karena ada pembulatan ke bawah
                var jumlahDpp = item.ListItem.Sum(x => Math.Floor(x.DPP));
                var jumlahPpn = item.ListItem.Sum(x => Math.Floor(x.PPN));
                sb.Append("FK,")
                    .Append($"{item.KD_JENIS_TRANSAKSI},")
                    .Append($"{item.FG_PENGGANTI},")
                    .Append($"{item.NOMOR_FAKTUR.Substring(3)},")
                    .Append($"{item.MASA_PAJAK},")
                    .Append($"{item.TAHUN_PAJAK},")
                    .Append($"{item.TANGGAL_FAKTUR},")
                    .Append($"{item.NPWP},")
                    .Append($"{item.NAMA},")
                    .Append($"{item.ALAMAT_LENGKAP.Replace(",", " ")},")
                    .Append(ConvertTo0Zero(jumlahDpp)).Append(",")
                    .Append(ConvertTo0Zero(jumlahPpn)).Append(",")
                    .Append(ConvertTo0Zero(item.JUMLAH_PPNBM)).Append(",")
                    .Append($"{item.ID_KETERANGAN_TAMBAHAN},")
                    .Append(ConvertTo0Zero(item.FG_UANG_MUKA)).Append(",")
                    .Append(ConvertTo0Zero(item.UANG_MUKA_DPP)).Append(",")
                    .Append(ConvertTo0Zero(item.UANG_MUKA_PPN)).Append(",")
                    .Append(ConvertTo0Zero(item.UANG_MUKA_PPNBM)).Append(",")
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
                        .Append(ConvertTo1Zero(item2.HARGA_SATUAN)).Append(",")
                        .Append(ConvertTo1Zero(item2.JUMLAH_BARANG)).Append(",")
                        .Append(ConvertTo1Zero(item2.HARGA_TOTAL)).Append(",")
                        .Append(ConvertTo1Zero(item2.DISKON)).Append(",")
                        .Append(ConvertTo1ZeroFloor(item2.DPP)).Append(",")
                        .Append(ConvertTo1ZeroFloor(item2.PPN)).Append(",");
                      sb.Append(ConvertTo0Zero(item2.TARIF_PPNBM)).Append(",")
                        .Append(ConvertTo1Zero(item2.PPNBM))
                        .Append($"{Environment.NewLine}");
                }
            }

            using (var saveFileDialog 
                = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"CSV Files|*.csv";
                saveFileDialog.Title = @"Save CSV File";
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"efaktur-{_dateTime.Now:yyyy-MM-dd-HHmm}";
                //      Show the SaveFileDialog and get the user's response
                if (saveFileDialog.ShowDialog() != DialogResult.OK) 
                    return;
                //      Get the selected file path from the dialog
                var filePath = saveFileDialog.FileName;
                //      Write the CSV string to the selected file
                File.WriteAllText(filePath, sb.ToString());

                string selectedFolder = Path.GetDirectoryName(filePath);
                Process.Start("explorer.exe", selectedFolder);
            }

            return;

            string ConvertTo1Zero(decimal number)
            {
                //var result =
                //    // Number has fractions
                //    number.ToString(number == Math.Floor(number) ?
                //    // Number is round (integer)
                //    "0" : "0.0", CultureInfo.InvariantCulture);
                var result = number.ToString("0.0", CultureInfo.InvariantCulture);
                return result;
            }
            string ConvertTo1ZeroFloor(decimal number)
            {
                //var result =
                //    // Number has fractions
                //    number.ToString(number == Math.Floor(number) ?
                //    // Number is round (integer)
                //    "0" : "0.0", CultureInfo.InvariantCulture);
                var resultInt = Math.Floor(number);
                var result = resultInt.ToString("0.0", CultureInfo.InvariantCulture);
                return result;
            }
            string ConvertTo0Zero(decimal number)
            {
                var resultInt = Math.Floor(number);
                var result = resultInt.ToString("0", CultureInfo.InvariantCulture);

                return result;
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
            var formatFlag = Digit16Option.Checked ? "16" : "13";
            var nsfp = _alokasiBuilder
                .Create()
                .NomorSeri(NoAwalText.Text, NoAkhirText.Text, formatFlag)
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
            SisaFpLabel.Text = _agg.Sisa.ToString();
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

            _fakturMenu = new ContextMenu();
            _fakturMenu.MenuItems.Add(new MenuItem("Void Nomor Seri", VoidNomorSeriFakturMenu_Click));
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

            SisaFpLabel.Text = _agg.Sisa.ToString();
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
            LoadFakturGrid();
        }
        #endregion

        #region PERIODE
        private void ListButton_Click(object sender, EventArgs e)
        {
            LoadFakturGrid();
        }

        private void InitPeriode()
        {
            Periode1Date.Value = _dateTime.Now.AddDays(-3);
            Periode2Date.Value = _dateTime.Now;
        }
        #endregion

        #region GRID-FAKTUR

        #region --UPDATE-CONTENT
        private void InitFakturGrid()
        {
            _listFaktur = new BindingList<FakturAlokasiFpItemView>();
            var binding = new BindingSource();
            binding.DataSource = _listFaktur;
            FakturGrid.DataSource = binding;

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

            g.GetCol("IsSet").Width = 30;
            g.GetCol("IsSet").HeaderText= "Set";

            g.GetCol("NoFakturPajak").Width = 140;

            g.GetCol("VoidDate").Visible = false;
            g.GetCol("UserIdVoid").Visible = false;
        }
        private void LoadFakturGrid()
        {
            List<FakturAlokasiFpItemView> listFaktur = null;
            var periode = new Periode(Periode1Date.Value, Periode2Date.Value);

            if (AllOutstandingCheckBox.Checked)
                listFaktur = _fakturAlokasiFpItemDal.ListData()?.ToList()
                    ?? new List<FakturAlokasiFpItemView>();
            else
                listFaktur = _fakturAlokasiFpItemDal.ListData(periode)?.ToList()
                    ?? new List<FakturAlokasiFpItemView>();

            var dataSource = listFaktur
                .Where(x => x.VoidDate == new DateTime(3000, 1, 1))
                .OrderBy(x => x.FakturId)
                .Adapt<List<FakturAlokasiFpItemView>>();

            RefreshFakturGrid(dataSource);
        }
        private void RefreshFakturGrid(IEnumerable<FakturAlokasiFpItemView> listFakturAlokasiItem)
        {
            _listFaktur = new BindingList<FakturAlokasiFpItemView>(listFakturAlokasiItem.ToList());
            var binding = new BindingSource
            {
                DataSource = _listFaktur
            };
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
        }

        #endregion

        #region --SET-UNSET-NOMOR-FAKTUR-PAJAK
        private void FakturGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            if (grid.CurrentCell.ColumnIndex != grid.Columns["IsSet"].Index)
                return;
            grid.EndEdit();

            try
            {
                if ((bool)grid.CurrentCell.Value == true)
                    ProsesSetNoSerFaktur(e.RowIndex);
                else
                    ProsesUnsetNoSeriFaktur(e.RowIndex);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                LoadFakturGrid();
                return;
            }
        }
        private void ProsesSetNoSerFaktur(int rowIndex)
        {
            //  GUARD
            var item = _listFaktur[rowIndex];
            if (item.NoFakturPajak.Length > 0)
                return;
            if (_agg.ListItem is null)
            {
                _listFaktur[rowIndex].IsSet = false;
                throw new ArgumentException("Alokasi Faktur Pajak belum terpilih");
            }


            //  BUILD
            //      claim alokasi
            _agg = _alokasiBuilder
                .Attach(_agg)
                .SetFaktur(item)
                .Build();
            //      update faktur
            var noFakturPajak = _agg.ListItem
                .FirstOrDefault(x => x.FakturId == item.FakturId)
                .NoFakturPajak;
            var faktur = _fakturBuilder
                .Load(item)
                .FakturPajak(noFakturPajak)
                .Build();


            //  APPLY
            //      write to database
            using (var trans = TransHelper.NewScope())
            {
                _ = _alokasiWriter.Save(_agg);
                _ = _fakturWriter.Save(faktur);
                trans.Complete();
            }
            //      update visualisasi grid
            item.SetNoFakturPajak(noFakturPajak);
            FakturGrid.Refresh();
            RefreshAlokasiGrid();
        }
        private void ProsesUnsetNoSeriFaktur(int rowIndex)
        {
            //  GUARD
            var item = _listFaktur[rowIndex];
            if (item.NoFakturPajak.Length == 0)
                return;
            var noSeriRemove = item.NoFakturPajak;

            //  BUILD
            //      1. build aggregate;
            var alokasi = BuildAlokasi(noSeriRemove);
            var faktur = _fakturBuilder.Load(item).Build();

            //      2. modify aggregate
            if (alokasi.AlokasiFpId == (_agg.AlokasiFpId ?? string.Empty))
                alokasi = _agg;

            alokasi = _alokasiBuilder
                .Attach(alokasi)
                .UnSetFaktur(faktur)
                .Build();
            faktur.NoFakturPajak = string.Empty;

            //  APPLY
            using (var trans = TransHelper.NewScope())
            {
                _alokasiWriter.Save(alokasi);
                _fakturWriter.Save(faktur);
                trans.Complete();
            }
            item.SetNoFakturPajak(string.Empty);
            FakturGrid.Refresh();
            RefreshAlokasiGrid();

            //  INNER-HELPER
            AlokasiFpModel BuildAlokasi(string noSeriFaktur)
            {
                var alokasiItem = _alokasiFpItemDal.GetData(new FakturModel { NoFakturPajak = noSeriFaktur })
                    ?? new AlokasiFpItemModel();
                alokasiItem.RemoveNull();
                var fallbackAlokasi = Policy<AlokasiFpModel>
                    .Handle<KeyNotFoundException>()
                    .Fallback(new AlokasiFpModel { ListItem = new List<AlokasiFpItemModel>() });
                var result = fallbackAlokasi.Execute(
                    () => _alokasiBuilder
                        .Load(alokasiItem)
                        .Build());
                return result;
            }
        }
        #endregion

        #region --SORT-COLUMN
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

            RefreshFakturGrid(_listFaktur);
            _sortAsc = !_sortAsc;
        }
        #endregion

        #region--VOID-NOMOR-SERI-FAKTUR-PAJAK
        private void FakturGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            _fakturMenu.Show((DataGridView)sender, e.Location);
        }
        private void VoidNomorSeriFakturMenu_Click(object sender, EventArgs e)
        {
            if (FakturGrid.CurrentRow is null)
                return;

            if (FakturGrid.CurrentRow.Cells["NoFakturPajak"].Value.ToString().Length == 0)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            //      input alasan void
            var inputAlasan = new VoidReasonForm();
            var dialogResult = inputAlasan.ShowDialog();
            var alasan = string.Empty;
            if (dialogResult == DialogResult.OK)
                alasan = inputAlasan.Alasan;

            //      add data faktur-pajak-void
            var noFakturPajak = FakturGrid.CurrentRow.Cells["NoFakturPajak"].Value.ToString();
            var noFakturPajakKey = new FakturModel
            {
                NoFakturPajak = noFakturPajak
            };
            var voidFaktur = _fakturPajakVoidBuilder
                .LoadOrCreate(new AlokasiFpItemModel(noFakturPajakKey))
                .Alasan(alasan)
                .Build();
            _fakturPajakVoidWriter.Save(voidFaktur);

            //      unset alokasi
            ProsesUnsetNoSeriFaktur(FakturGrid.CurrentRow.Index);
            _listFaktur[FakturGrid.CurrentRow.Index].IsSet = false;

            //      update tampilan grid
            _listFaktur[FakturGrid.CurrentRow.Index].SetNoFakturPajak(string.Empty);
            FakturGrid.Refresh();
        }
        #endregion

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
