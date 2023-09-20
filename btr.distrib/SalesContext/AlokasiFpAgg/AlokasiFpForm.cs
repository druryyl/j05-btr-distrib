using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.NomorFpAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturPajak;
using btr.nuna.Domain;
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
        private readonly IAlokasiFpBuilder _builder;
        private readonly IAlokasiFpWriter _writer;
        private readonly IAlokasiFpDal _alokasiFpdal;

        public AlokasiFpForm(IFakturDal fakturDal,
            ITglJamDal dateTime,
            IAlokasiFpBuilder builder,
            IAlokasiFpWriter writer,
            IAlokasiFpDal alokasiFpdal)
        {
            _fakturDal = fakturDal;
            _dateTime = dateTime;
            _builder = builder;
            _writer = writer;
            _alokasiFpdal = alokasiFpdal;

            _agg = new AlokasiFpModel();

            InitializeComponent();
            RegisterEventHandler();
            InitPeriode();
            InitFakturKiriGrid();
            InitAlokasiGrid();
        }

        private void InitFakturKiriGrid()
        {
            RefreshFakturKiriGrid();
            var g = FakturKiriGrid.Columns;
            g.SetDefaultCellStyle(Color.MistyRose);
            g.GetCol("FakturId").Visible = false;

            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = "Faktur";

            g.GetCol("UserId").Width = 50;
            g.GetCol("UserId").HeaderText = "Admin";

            g.GetCol("FakturDate").DefaultCellStyle.Format = "dd-MM-yyyy";
            g.GetCol("FakturDate").Width = 80;
            g.GetCol("FakturDate").HeaderText = "Tgl";

            g.GetCol("CustomerName").Width = 90;
            g.GetCol("CustomerName").HeaderText = "Customer";

            g.GetCol("GrandTotal").Width = 80;
            g.GetCol("NoFakturPajak").Width = 100;

        }

        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ListButton.Click += ListButton_Click;
            FakturKiriGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            AlokasiButton.Click += AlokasiButton_Click;
            AlokasiGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }

        private void AlokasiButton_Click(object sender, EventArgs e)
        {
            CreateAlokasi();
            NoAwalText.Text = string.Empty;
            NoAkhirText.Text = string.Empty;
            RefreshAlokasiGrid();
        }

        private void InitAlokasiGrid()
        {
            RefreshAlokasiGrid();
            var g = AlokasiGrid.Columns;
            g.SetDefaultCellStyle(Color.Beige);

        }
        private void RefreshAlokasiGrid()
        {
            var periode = new Periode(_dateTime.Now().AddDays(-31), _dateTime.Now());
            var listAlokasi = _alokasiFpdal.ListData(periode)?.ToList() 
                ?? new List<AlokasiFpModel>();
            var projection = listAlokasi
                .Select(x => new AlokasiFpAlokasiDto(x.AlokasiFpId, 
                    $"{x.NoAwal}{Environment.NewLine}{x.NoAkhir}",x.Sisa))
                .ToList();
            AlokasiGrid.DataSource = projection;
        }

        private void CreateAlokasi()
        {
            var nsfp = _builder
                .Create()
                .NomorSeri(NoAwalText.Text, NoAkhirText.Text)
                .Build();
            _writer.Save(nsfp);
        }
        private void ListButton_Click(object sender, EventArgs e)
        {
            RefreshFakturKiriGrid();
        }

        private void RefreshFakturKiriGrid()
        {
            var periode = new Periode(Periode1Date.Value, Periode2Date.Value);
            var listFaktur = _fakturDal.ListData(periode)?.ToList()
                ?? new List<FakturModel>();
            var dataSource = listFaktur
                .Where(x => x.IsVoid == false)
                .OrderBy(x => x.FakturId)
                .Adapt<List<AlokasiFpFakturDto>>();
            FakturKiriGrid.DataSource = dataSource;
            FakturKiriGrid.Refresh();
        }
        private void InitPeriode()
        {
            Periode1Date.Value = _dateTime.Now().AddDays(-3);
            Periode2Date.Value = _dateTime.Now();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            ProsesGenNoSeriFaktur();
        }

        private void ProsesGenNoSeriFaktur()
        {
            
        }
    }

    public class AlokasiFpFakturDto
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public decimal GrandTotal { get; private set; }
        public string NoFakturPajak { get; private set; }
        public string UserId { get; private set; }
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
