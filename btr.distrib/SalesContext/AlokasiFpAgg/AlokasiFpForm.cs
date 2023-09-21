using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.NomorFpAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturPajak;
using btr.nuna.Application;
using btr.nuna.Domain;
using DocumentFormat.OpenXml.Bibliography;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.NomorSeriFpAgg
{
    public partial class AlokasiFpForm : Form
    {
        private AlokasiFpModel _agg;

        private readonly IFakturDal _fakturDal;
        private readonly ITglJamDal _dateTime;
        private readonly IAlokasiFpBuilder _alokasiBuilder;
        private readonly IAlokasiFpWriter _alokasiWriter;
        private readonly IAlokasiFpDal _alokasiFpdal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;

        private BindingList<AlokasiFpFakturDto> _listFaktur;
        
        private ContextMenu _alokasiMenu;

        public AlokasiFpForm(IFakturDal fakturDal,
            ITglJamDal dateTime,
            IAlokasiFpBuilder builder,
            IAlokasiFpWriter writer,
            IAlokasiFpDal alokasiFpdal,
            IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter)
        {
            _fakturDal = fakturDal;
            _dateTime = dateTime;
            _alokasiBuilder = builder;
            _alokasiWriter = writer;
            _alokasiFpdal = alokasiFpdal;
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;

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
            ProsesButton.Click += ProsesButton_Click;
            ListButton.Click += ListButton_Click;
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            AlokasiButton.Click += AlokasiButton_Click;
            AlokasiGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            AlokasiGrid.MouseClick += AlokasiGrid_MouseClick;
        }

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
            g.SetDefaultCellStyle(Color.Beige);
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
            var periode = new Periode(_dateTime.Now().AddDays(-31), _dateTime.Now());
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
            //  
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
            Periode1Date.Value = _dateTime.Now().AddDays(-3);
            Periode2Date.Value = _dateTime.Now();
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

            MessageBox.Show("Proses selesai");
        }
        #endregion

        #region GRID-FAKTUR
        private void InitFakturGrid()
        {
            RefreshFakturGrid();
            var g = FakturGrid.Columns;
            g.SetDefaultCellStyle(Color.MistyRose);
            g.GetCol("FakturId").Visible = false;

            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = "Faktur";

            g.GetCol("UserId").Width = 50;
            g.GetCol("UserId").HeaderText = "Admin";

            g.GetCol("FakturDate").DefaultCellStyle.Format = "dd-MM-yyyy";
            g.GetCol("FakturDate").Width = 80;
            g.GetCol("FakturDate").HeaderText = "Tgl";

            g.GetCol("CustomerName").Width = 80;
            g.GetCol("CustomerName").HeaderText = "Customer";

            g.GetCol("Address").Width = 100;
            g.GetCol("GrandTotal").Width = 80;
            g.GetCol("NoFakturPajak").Width = 140;

        }

        private void RefreshFakturGrid()
        {
            var periode = new Periode(Periode1Date.Value, Periode2Date.Value);
            var listFaktur = _fakturDal.ListData(periode)?.ToList()
                ?? new List<FakturModel>();

            var dataSource = listFaktur
                .Where(x => x.IsVoid == false)
                .OrderBy(x => x.FakturId)
                .Adapt<List<AlokasiFpFakturDto>>();
            _listFaktur = new BindingList<AlokasiFpFakturDto>(dataSource);
            FakturGrid.DataSource = _listFaktur;
            FakturGrid.Refresh();
        }
        #endregion
    }

    public class AlokasiFpFakturDto : IFakturKey, IFakturCode
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string Address { get; private set; }
        public decimal GrandTotal { get; private set; }
        public string NoFakturPajak { get; private set; }
        public string UserId { get; private set; }

        public void SetNoFakturPajak(string noFakturPajak) => NoFakturPajak = noFakturPajak;
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
