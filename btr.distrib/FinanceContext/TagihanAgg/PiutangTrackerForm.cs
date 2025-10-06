using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.FinanceContext.TagihanAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
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

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class PiutangTrackerForm : Form
    {
        private readonly IFakturDal _fakturDal;
        private readonly IPiutangTrackerDal _piutangTrackerDal;

        public PiutangTrackerForm(IFakturDal fakturDal, 
            IPiutangTrackerDal piutangTrackerDal)
        {
            InitializeComponent();

            _fakturDal = fakturDal;

            RegEventHandler();
            _piutangTrackerDal = piutangTrackerDal;
        }

        private void RegEventHandler()
        {
            TrackButton.Click += TrackButton_Click;
        }

        #region HEADER
        private void TrackButton_Click(object sender, EventArgs e)
        {
            IFakturCode fakturCode = new FakturModel { FakturCode = FakturCodeText.Text };
            var faktur = _fakturDal.GetData(fakturCode) ?? new FakturModel(string.Empty);
            CustomerText.Text = faktur.CustomerName;
            AddressText.Text = faktur.Address;
            FakturIdText.Text = faktur.FakturId;
            SalesText.Text = faktur.SalesPersonName;
            TglFakturText2.Text = faktur.FakturDate.ToString("dd-MM-yyyy");
            JatuhTempoText2.Text = faktur.DueDate.ToString("dd-MM-yyyy");

            var listTracker = _piutangTrackerDal.ListData(new PiutangModel(faktur.FakturId))?.ToList() 
                ?? new List<PiutangTrackerDto>();
            var datasource = listTracker.Select(x => new PiutangTrackerView(x))?.ToList();
            TrackerGrid.DataSource = datasource;
            TrackerGrid.Refresh();
            SetGridLayout();
        }
        private void SetGridLayout()
        {
            var grid = TrackerGrid.Columns;
            grid.SetDefaultCellStyle(Color.White);
            grid["Tgl"].Width = 80;
            grid["Aktifitas"].Width = 120;
            grid["Piutang"].Width = 80;
            grid["Tagihan"].Width = 80;
            grid["Pelunasan"].Width = 80;
            grid["Aktifitas"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            TrackerGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        #endregion
    }

    internal class PiutangTrackerView
    {
        public PiutangTrackerView(PiutangTrackerDto dto)
        {
            Tgl = dto.Tgl.ToString("dd-MM-yyyy");
            Aktifitas = $"{dto.Keterangan}\n{dto.ReffId}";
            Piutang = dto.Piutang;
            Tagihan = dto.Tagihan;
            Pelunasan = dto.Pelunasan;
        }
        public string Tgl { get; private set; }
        public string Aktifitas { get; private set; }
        public decimal Piutang { get; private set; }
        public decimal Tagihan { get; private set; }
        public decimal Pelunasan { get; private set; }
    }

}
