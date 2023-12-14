using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using JetBrains.Annotations;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TagihanForm : Form
    {
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<TagihanDto> _listTagihan = new BindingList<TagihanDto>();
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturDal _fakturDal;
        private readonly IPiutangBuilder _piutangBuilder;
        
        private readonly ISalesPersonDal _salesDal; 
        public TagihanForm(ISalesPersonDal salesDal, IFakturBuilder fakturBuilder, IPiutangBuilder piutangBuilder, IFakturDal fakturDal)
        {
            _salesDal = salesDal;
            _fakturBuilder = fakturBuilder;
            _piutangBuilder = piutangBuilder;
            _fakturDal = fakturDal;
            InitializeComponent();
            InitGrid();
            InitCombo();
            TglTagihText.Value = DateTime.Now;

            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            FakturGrid.CellValidated += FakturGridOnCellValidated;
        }

        private void FakturGridOnCellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != FakturGrid.Columns.GetCol("FakturCode").Index) return;
            
            var fakturCode = _listTagihan[e.RowIndex].FakturCode;
            var faktur = GetFaktur(fakturCode);
            var piutang = GetPiutang(fakturCode);
            
            if (piutang == null) return;
            if (piutang.Sisa <= 0) return;
            
            if (faktur.SalesPersonId != SalesCombo.SelectedValue.ToString())
            {
                MessageBox.Show(@"Sales tidak sesuai dengan faktur");
                _listTagihan[e.RowIndex].FakturCode  = "";
                _listTagihan[e.RowIndex].FakturId  = "";
                _listTagihan[e.RowIndex].CustomerId  = "";
                _listTagihan[e.RowIndex].CustomerName  = "";
                _listTagihan[e.RowIndex].Alamat = "";
                _listTagihan[e.RowIndex].Nilai = 0;
                return;
            }
            
            _listTagihan[e.RowIndex].FakturId  = faktur.FakturId;
            _listTagihan[e.RowIndex].CustomerId  = faktur.CustomerId;
            _listTagihan[e.RowIndex].CustomerName  = faktur.CustomerName;
            _listTagihan[e.RowIndex].Alamat  = faktur.Address;
            _listTagihan[e.RowIndex].Nilai  = faktur.Total;
            FakturGrid.Refresh();
        }
        
        private FakturModel GetFaktur(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var fakturHdr = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            var faktur = _fakturBuilder.Load(fakturHdr).Build();
            return faktur;
        }
        private PiutangModel GetPiutang(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var faktur = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            
            var piutangKey = new PiutangModel{PiutangId = faktur.FakturId};
            var piutang = _piutangBuilder.Load(piutangKey).Build();

            return piutang;
        }

        private void InitCombo()
        {
            var listSales = _salesDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            SalesCombo.DataSource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesCombo.DisplayMember = "SalesPersonName";
            SalesCombo.ValueMember = "SalesPersonId";
        }
        
        private void InitGrid()
        {
            _bindingSource.DataSource = _listTagihan;
            FakturGrid.DataSource = _bindingSource;
            FakturGrid.Refresh();
            
            var col = FakturGrid.Columns;
            col.GetCol("FakturCode").HeaderText = @"Faktur";
            col.GetCol("FakturDate").HeaderText = @"Tgl Faktur";
            col.GetCol("CustomerId").HeaderText = @"Cust Id";
            col.GetCol("CustomerName").HeaderText = @"Customer";
            col.GetCol("Alamat").HeaderText = @"Alamat";
            col.GetCol("Nilai").HeaderText = @"Total Tagihan";

            col.GetCol("FakturId").Visible = false;
            
            col.GetCol("FakturDate").DefaultCellStyle.Format = "dd MMM yyyy";
            col.GetCol("Nilai").DefaultCellStyle.Format = "N0";
            col.GetCol("Nilai").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            col.GetCol("FakturCode").Width = 80;
            col.GetCol("FakturDate").Width = 80;
            col.GetCol("CustomerId").Width = 80;
            col.GetCol("CustomerName").Width = 200;
            col.GetCol("Alamat").Width = 200;
            col.GetCol("Nilai").Width = 100;
            
            col.GetCol("FakturDate").ReadOnly = true;
            col.GetCol("CustomerId").ReadOnly = true;
            col.GetCol("CustomerName").ReadOnly = true;
            col.GetCol("Alamat").ReadOnly = true;
            col.GetCol("Nilai").ReadOnly = true;
            
            col.GetCol("FakturDate").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("CustomerId").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("CustomerName").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("Alamat").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("Nilai").DefaultCellStyle.BackColor = Color.Beige;
        }
    }

    [PublicAPI]
    public class TagihanDto
    {
        public string FakturCode { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public decimal Nilai { get; set; }
        public string FakturId { get; set; }
    }
}
