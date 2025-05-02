using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.FinanceContext.FpKeluaranAgg;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.SalesReplacementFeat
{
    public partial class SalesReplacementForm : Form
    {
        private readonly BindingList<FakturSalesReplacementDto> _listFaktur;
        private readonly BindingSource _bindingSource;

        private readonly IFakturDal _fakturDal;
        private readonly IPiutangDal _piutangDal;
        private readonly ISalesPersonDal _salesPersonDal;

        public SalesReplacementForm(IFakturDal fakturDal,
            IPiutangDal piutangDal,
            ISalesPersonDal salesPersonDal)
        {
            InitializeComponent();
            _fakturDal = fakturDal;
            _piutangDal = piutangDal;

            _listFaktur = new BindingList<FakturSalesReplacementDto>();
            _bindingSource = new BindingSource(_listFaktur, null);
            _salesPersonDal = salesPersonDal;

            InitCombo();
            InitGrid();
            InitPeriode();
            RegisterControlEventHandler();
        }

        private void RegisterControlEventHandler()
        {
            LoadFakturButton.Click += LoadFakturButton_Click;
            SearchFakturButton.Click += LoadFakturButton_Click;
            ReplaceButton.Click += ReplaceButton_Click;

            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            ReplaceSales();
            LoadFaktur();
        }

        private void LoadFakturButton_Click(object sender, EventArgs e)
        {
            LoadFaktur();
        }

        private void InitCombo()
        {
            var listSales = _salesPersonDal.ListData()?.ToList() 
                ?? new List<SalesPersonModel>();
            var salesDatasource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesAsalCombo.DataSource = salesDatasource;
            SalesAsalCombo.DisplayMember = "SalesPersonName";
            SalesAsalCombo.ValueMember = "SalesPersonId";

            var salesPenggantiDatasource = listSales
                .OrderBy(x => x.SalesPersonName)
                .ToList();
            SalesPenggantiCombo.DataSource = salesPenggantiDatasource;
            SalesPenggantiCombo.DisplayMember = "SalesPersonName";
            SalesPenggantiCombo.ValueMember = "SalesPersonId";

        }

        private void InitGrid()
        {
            FakturGrid.DataSource = _bindingSource;
            LoadFaktur();
            FakturGrid.DefaultCellStyle.Font = new Font("Lucida Console", 8);

            var grid = FakturGrid.Columns;
            grid.SetDefaultCellStyle(Color.LightSteelBlue);
            grid["FakturId"].Visible = false;

            grid["FakturCode"].Width = 70;
            grid["FakturDate"].Width = 80;
            grid["CustomerCode"].Width = 80;
            grid["CustomerName"].Width = 110;
            grid["Address"].Width = 200;
            grid["TotalFaktur"].Width = 80;
            grid["Terbayar"].Width = 80;
            grid["SisaTagihan"].Width = 80;
            grid["IsPilih"].Width = 40;

            grid["FakturId"].HeaderText = "ID";
            grid["FakturCode"].HeaderText = "Faktur-ID";
            grid["FakturDate"].HeaderText = "Tanggal";
            grid["CustomerCode"].HeaderText = "Cust-ID";
            grid["CustomerName"].HeaderText = "Customer";
            grid["Address"].HeaderText = "Alamat";
            grid["TotalFaktur"].HeaderText = "Total";
            grid["Terbayar"].HeaderText = "Bayar";
            grid["SisaTagihan"].HeaderText = "Sisa";
            grid["IsPilih"].HeaderText = "Pilih";

            grid["FakturDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
            grid["TotalFaktur"].DefaultCellStyle.Format = "N0";
            grid["Terbayar"].DefaultCellStyle.Format = "N0";
            grid["SisaTagihan"].DefaultCellStyle.Format = "N0";
            grid["TotalFaktur"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["Terbayar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["SisaTagihan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grid["CustomerName"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            grid["Address"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            FakturGrid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0)
                    return;
                var row = FakturGrid.Rows[e.RowIndex];
                var item = (FakturSalesReplacementDto)row.DataBoundItem;
                if (item.IsPilih)
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            };
        }

        private void InitPeriode()
        {
            Periode1DatePicker.Value = DateTime.Now.AddDays(-7);
            Periode2DatePicker.Value = DateTime.Now;
        }

        private Periode GetPeriode()
        {
            if (Periode2DatePicker.Value.Subtract(Periode1DatePicker.Value).TotalDays > 31)
                throw new Exception("Periode tidak boleh lebih dari 1 bulan");

            return new Periode(Periode1DatePicker.Value, Periode2DatePicker.Value);
        }

        private void LoadFaktur()
        {
            var listFakturAll = _fakturDal.ListData(GetPeriode())?.ToList() 
                ?? new List<FakturModel>(); 
            var listFakturSalesAsal = listFakturAll
                .Where(x => x.SalesPersonId == SalesAsalCombo.SelectedValue.ToString())
                .ToList();
            if (SearchText.Text != string.Empty)
                listFakturSalesAsal = FilterSearch(listFakturSalesAsal, SearchText.Text);

            _listFaktur.Clear();
            foreach (var faktur in listFakturSalesAsal)
            {
                var (terbayar, sisaTagihan) = GetTerbayarSisaTagihan(faktur.FakturId);
                var fakturDto = new FakturSalesReplacementDto(
                    faktur.FakturId, faktur.FakturCode, faktur.FakturDate,
                    faktur.CustomerCode, faktur.CustomerName, faktur.Address,
                    faktur.GrandTotal, terbayar, sisaTagihan)
                {
                    IsPilih = false
                };
                _listFaktur.Add(fakturDto);
            }
        }

        private void ReplaceSales()
        {
            var listFaktur = _listFaktur
                .Where(x => x.IsPilih)
                .ToList();
            if (listFaktur.Count == 0)
            {
                MessageBox.Show("Tidak ada faktur yang dipilih");
                return;
            }
            var salesAsal = SalesAsalCombo.SelectedValue.ToString();
            var salesPengganti = SalesPenggantiCombo.SelectedValue.ToString();
            foreach (var faktur in listFaktur)
            {
                IFakturKey fakturKey = new FakturModel(faktur.FakturId);
                var fakturModel = _fakturDal.GetData(fakturKey);
                fakturModel.SalesPersonId = salesPengganti;
                _fakturDal.Update(fakturModel);
            }
            MessageBox.Show("Berhasil mengganti sales");
        }

        private (decimal, decimal) GetTerbayarSisaTagihan(string FakturId)
        {
            var piutang = _piutangDal.GetData(new PiutangModel(FakturId));
            if (piutang is null)
                return (0, 0);

            return (piutang.Terbayar + piutang.Potongan, piutang.Sisa);
        }

        private List<FakturModel> FilterSearch(List<FakturModel> listFaktur, string searchText)
        {
            searchText = searchText.ToLower();
            return listFaktur
                .Where(x => x.FakturCode.ToLower().Contains(searchText) ||
                            x.CustomerCode.ToLower().Contains(searchText) ||
                            x.CustomerName.ToLower().Contains(searchText))
                .ToList();
        }

    }

    public class FakturSalesReplacementDto
    {
        public string FakturId { get; }

        public FakturSalesReplacementDto(string fakturId, string fakturCode, 
            DateTime fakturDate, string customerCode, string customerName, 
            string address, decimal totalFaktur, decimal terbayar, decimal sisaTagihan)
        {
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            CustomerCode = customerCode;
            CustomerName = customerName;
            Address = address;
            TotalFaktur = totalFaktur;
            Terbayar = terbayar;
            SisaTagihan = sisaTagihan;
        }

        public string FakturCode { get; }
        public DateTime FakturDate { get; }
        public string CustomerCode { get; }
        public string CustomerName { get; }
        public string Address{ get; }
        public decimal TotalFaktur { get; }
        public decimal Terbayar { get; }
        public decimal SisaTagihan { get; }
        public bool IsPilih { get; set; }
    }
}
