using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.distrib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using btr.nuna.Domain;
using btr.domain.SalesContext.CustomerAgg;
using btr.distrib.Browsers;
using btr.application.SalesContext.WilayahAgg;
using btr.application.BrgContext.HargaTypeAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using System.Drawing;
using btr.application.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.WilayahAgg;
using ClosedXML.Excel;
using Syncfusion.DataSource.Extensions;

namespace btr.distrib.SalesContext.CustomerAgg
{
    public partial class CustomerForm : Form
    {
        private readonly ICustomerDal _customerDal;
        private readonly ICustomerBuilder _customerBuilder;
        private IEnumerable<CustomerFormGridDto> _listCust;
        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IBrowser<WilayahBrowserView> _wilayahBrowser;

        private readonly IKlasifikasiDal _klasifikasiDal;
        private readonly IHargaTypeDal _hargaTypeDal;
        private readonly ICustomerWriter _customerWriter;
        private readonly IWilayahDal _wilayahDal;

        public CustomerForm(ICustomerDal customerDal,
            ICustomerBuilder customerBuilder,
            IBrowser<CustomerBrowserView> customerBrowser,
            IKlasifikasiDal klasifikasiDal,
            IHargaTypeDal hargaTypeDal,
            ICustomerWriter customerWriter,
            IWilayahDal wilayahDal,
            IBrowser<WilayahBrowserView> wilayahBrowser)
        {
            InitializeComponent();

            _customerDal = customerDal;
            _klasifikasiDal = klasifikasiDal;
            _hargaTypeDal = hargaTypeDal;
            _wilayahDal = wilayahDal;

            _customerBrowser = customerBrowser;
            _wilayahBrowser = wilayahBrowser;

            _customerBuilder = customerBuilder;
            _customerWriter = customerWriter;

            RegisterEventHandler();
            InitGrid();
            InitKlasifikasi();
            InitTipeHarga();

            PlafondText.Maximum = 9999999999;
            CreditBalanceText.Maximum = 9999999999;
            CreditBalanceText.Minimum = -9999999999;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            CustIdText.Validated += CustIdText_Validated;
            CustButton.Click += CustButton_Click;

            SaveButton.Click += SaveButton_Click;
            CustGrid.CellDoubleClick += CustGrid_CellDoubleClick;
            CustGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;

            WilayahIdText.Validated += WilayahIdText_Validated;
            WilayahButton.Click += WilayahButton_Click;

            NewButton.Click += NewButton_Click;
            
            ExcelButton.Click += ExcelButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            var listCust = _customerDal.ListData()?.ToList()
                ?? new List<CustomerModel>();

            var listCustomer = _customerDal.ListData()?.ToList() ?? new List<CustomerModel>();
            var listWilayah = _wilayahDal.ListData()?.ToList() ?? new List<WilayahModel>();
            var listKlasifikasi = _klasifikasiDal.ListData()?.ToList() ?? new List<KlasifikasiModel>();
            var listHargaType = _hargaTypeDal.ListData()?.ToList() ?? new List<HargaTypeModel>();
            
            //  projection listCutomer to CustomerFormExcelDto
            var listCustomerExcel = listCustomer
                .Where(x => x.CustomerName.Length > 0)
                .OrderBy(x => x.CustomerName)
                .Select(x => new CustomerFormExcelDto
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    Kota = x.Kota,
                    NoTelp = x.NoTelp,
                    NoFax = x.NoFax,
                    WilayahName = listWilayah.FirstOrDefault(y => y.WilayahId == x.WilayahId)?.WilayahName,
                    KlasifikasiName = listKlasifikasi.FirstOrDefault(y => y.KlasifikasiId == x.KlasifikasiId)?.KlasifikasiName,
                    Plafond = x.Plafond,
                    Npwp = x.Npwp,
                    NamaWp = x.NamaWp,
                    AddressWp = x.AddressWp,
                    IsKenaPajak = x.IsKenaPajak,
                    HargaTypeName = listHargaType.FirstOrDefault(y => y.HargaTypeId == x.HargaTypeId)?.HargaTypeName,
                    IsSuspend = x.IsSuspend
                }).ToList();
            
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"customer-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("customer-info")
                    .Cell($"B1")
                    .InsertTable(listCustomerExcel, false);
                var ws = wb.Worksheets.First();
                //  add row number at column A
                ws.Cell("A1").Value = "No";
                for (var i = 0; i < listCustomerExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;

                //  border header
                ws.Range("A1:R1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  font bold header and background color light blue
                ws.Range("A1:R1").Style.Font.SetBold();
                ws.Range("A1:R1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                //  freeze header
                ws.SheetView.FreezeRows(1);
                //  border table
                ws.Range($"A2:R{listCustomerExcel.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A2:R{listCustomerExcel.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;
                
                //  format number thousand separator and zero decimal place
                ws.Range($"L2:L{listCustomerExcel.Count + 1}").Style.NumberFormat.Format = "#,##";
                
                //  set font to consolas 8.25f
                ws.Range($"A1:R{listCustomerExcel.Count + 1}").Style.Font.SetFontName("Consolas");
                ws.Range($"A1:R{listCustomerExcel.Count + 1}").Style.Font.SetFontSize(9f);
                
                //  set backcolor column E to H as light yellow
                ws.Range($"M2:P{listCustomerExcel.Count + 1}").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range($"L2:L{listCustomerExcel.Count + 1}").Style.Fill.BackgroundColor = XLColor.LightGreen;

                //  auto fit column
                ws.Columns().AdjustToContents();
                //  set column D width to 20 character
                ws.Column(4).Width = 35;
                ws.Column(5).Width = 35;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(15).Width = 35;
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        #region GRID-CUSTOMER
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FilterCustGrid(SearchText.Text);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FilterCustGrid(SearchText.Text);
        }

        private void CustGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var grid = (DataGridView)sender;
            var custId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var cust = _customerBuilder.Load(new CustomerModel(custId)).Build();
            ShowData(cust);
            CustomerTab.SelectedTab = DetilPage;
        }

        private void InitGrid()
        {
            var listCust = _customerDal.ListData()?.ToList()
                ?? new List<domain.SalesContext.CustomerAgg.CustomerModel>();

            _listCust = listCust
                .Select(x => new CustomerFormGridDto(x.CustomerId,
                    x.CustomerCode,
                    x.CustomerName,
                    x.Address1,
                    x.WilayahName,
                    x.Plafond,
                    x.CreditBalance,
                    x.Npwp,
                    x.NamaWp)).ToList();
            CustGrid.DataSource = _listCust;

            CustGrid.Columns.SetDefaultCellStyle(Color.Beige);
            CustGrid.Columns.GetCol("Id").Width = 50;
            CustGrid.Columns.GetCol("Name").Width = 150;
            CustGrid.Columns.GetCol("Alamat").Width = 220;
            CustGrid.Columns.GetCol("Plafond").Width = 100;
            CustGrid.Columns.GetCol("CreditBalance").Width = 100;
            CustGrid.Columns.GetCol("Npwp").Width = 100;
            CustGrid.Columns.GetCol("NamaWp").Width = 150;
        }

        private void FilterCustGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                CustGrid.DataSource = _listCust;
                return;
            }
            var listFilter = _listCust.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            var listByAlamat = _listCust.Where(x => x.Alamat.ContainMultiWord(keyword)).ToList();
            listFilter.AddRange(listByAlamat);
            CustGrid.DataSource = listFilter;
        }
        #endregion

        #region CUSTOMER-ID
        private void CustButton_Click(object sender, EventArgs e)
        {
            CustIdText.Text = _customerBrowser.Browse(CustIdText.Text);
            CustIdText_Validated(CustIdText, null);
        }

        private void CustIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var customer = _customerBuilder
                .Load(new CustomerModel(textbox.Text))
                .Build();

            ShowData(customer);
        }

        private void ShowData(CustomerModel customer)
        {
            CustIdText.Text = customer.CustomerId;
            CustNameText.Text = customer.CustomerName;
            CustCodeText.Text = customer.CustomerCode;
            KlasifikasiCombo.SelectedValue = customer.KlasifikasiId;

            TipeHargaCombo.SelectedValue = customer.HargaTypeId;
            PlafondText.Value = customer.Plafond;
            CreditBalanceText.Value = customer.CreditBalance;

            WilayahIdText.Text = customer.WilayahId;
            WilayahNameText.Text = customer.WilayahName;
            Alamat1Text.Text = customer.Address1;
            Alamat2Text.Text = customer.Address2;
            KotaText.Text = customer.Kota;
            KodePosText.Text = customer.KodePos;
            NoTelponText.Text = customer.NoTelp;
            NoFaxText.Text = customer.NoFax;

            IsKenaPajakCheck.Checked = customer.IsKenaPajak;
            NpwpText.Text = customer.Npwp;
            NppkpText.Text = customer.Nppkp;
            NamaWpText.Text = customer.NamaWp;
            Alamat1WpText.Text = customer.AddressWp;
            Alamat2WpText.Text = customer.AddressWp2;

            EmailText.Text = customer.Email;
            NitkuText.Text = customer.Nitku;
            JenisIdentitasCombo.Text = customer.JenisIdentitasPajak;
        }

        private void ClearForm()
        {
            CustIdText.Clear(); ;
            CustNameText.Clear(); ;
            CustCodeText.Clear(); ;
            PlafondText.Value = 0;
            CreditBalanceText.Value = 0;
            WilayahIdText.Clear();
            WilayahNameText.Clear();
            Alamat1Text.Clear();
            Alamat2Text.Clear();
            KotaText.Clear();
            KodePosText.Clear();
            NoTelponText.Clear();
            NoFaxText.Clear();
            IsKenaPajakCheck.Checked = true;
            NpwpText.Clear();
            NppkpText.Clear();
            NamaWpText.Clear();
            Alamat1WpText.Clear();
            Alamat2WpText.Clear();
            EmailText.Clear();
            NitkuText.Clear();
        }
        #endregion

        #region KLASIFIKASI
        private void InitKlasifikasi()
        {
            var listAll = _klasifikasiDal.ListData()?.ToList() ?? new List<KlasifikasiModel>();
            KlasifikasiCombo.DataSource = listAll;
            KlasifikasiCombo.DisplayMember = "KlasifikasiName";
            KlasifikasiCombo.ValueMember = "KlasifikasiId";
        }
        #endregion

        #region HARGA-TYPE
        private void InitTipeHarga()
        {
            var listAll = _hargaTypeDal.ListData()?.ToList() ?? new List<HargaTypeModel>();
            TipeHargaCombo.DataSource = listAll;
            TipeHargaCombo.DisplayMember = "HargaTypeName";
            TipeHargaCombo.ValueMember = "HargaTypeId";
        }
        #endregion

        #region WILAYAH
        private void WilayahButton_Click(object sender, EventArgs e)
        {
            WilayahIdText.Text = _wilayahBrowser.Browse(WilayahIdText.Text);
            WilayahIdText_Validated(WilayahIdText, null);
        }

        private void WilayahIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var wilayah = _wilayahDal.GetData(new WilayahModel(textbox.Text))
                ?? new WilayahModel { WilayahName = string.Empty };
            WilayahNameText.Text = wilayah.WilayahName;
        }
        #endregion

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var customer = new CustomerModel(CustIdText.Text);
            if (CustIdText.Text.Length == 0)
                customer = _customerBuilder.CreateNew().Build();
            else
                customer = _customerBuilder.Load(customer).Build();

            customer = _customerBuilder
                .Attach(customer)
                .Name(CustNameText.Text)
                .Code(CustCodeText.Text)
                .Klasifikasi(new KlasifikasiModel(KlasifikasiCombo.SelectedValue.ToString()))
                .HargaType(new HargaTypeModel(TipeHargaCombo.SelectedValue.ToString()))
                .Plafond(PlafondText.Value)
                .CreditBalance(CreditBalanceText.Value)
                .Wilayah(new WilayahModel(WilayahIdText.Text))
                .Address(Alamat1Text.Text, Alamat2Text.Text, KotaText.Text)
                .KodePos(KodePosText.Text)
                .NoTelp(NoTelponText.Text)
                .NoFax(NoFaxText.Text)
                .IsKenaPajak(IsKenaPajakCheck.Checked)
                .Npwp(NpwpText.Text)
                .Nppkp(NppkpText.Text)
                .NamaWp(NamaWpText.Text)
                .AddressWp(Alamat1WpText.Text, Alamat2WpText.Text)
                .Email(EmailText.Text)
                .Nitku(NitkuText.Text)
                .JenisIdentitasPajak(JenisIdentitasCombo.Text)
                .Build();

            _customerWriter.Save(ref customer);
            ClearForm();
        }
        #endregion
    }

    public class CustomerFormGridDto
    {
        public CustomerFormGridDto(string id, string code, string name, string alamat, 
            string kota, decimal plafond, decimal creditBalance,
            string npwp, string namaWp)
        {
            Id = id;
            Code = code;
            Name = name;
            Alamat = alamat;
            Kota = kota;
            Plafond = plafond;
            CreditBalance = creditBalance;
            Npwp = npwp;
            NamaWp = namaWp;
        }
        public string Id { get; }
        public string Code{ get; }

        public string Name { get; }
        public string Alamat { get; }
        public string Kota { get; }
        public decimal Plafond { get; }
        public decimal CreditBalance { get; }
        public string Npwp { get; set; }
        public string NamaWp { get; set; }
    }

    public class CustomerFormExcelDto
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Kota { get; set; }
        public string NoTelp {get;set;  }
        public string NoFax {get;set;   }
        public string WilayahName {get;set; }
        public string KlasifikasiName {get;set; }
        public decimal Plafond {get;set; }
        public string Npwp {get;set;    }
        public string NamaWp {get;set;  }
        public string AddressWp {get;set;   }
        public bool IsKenaPajak {get;set; }
        public string HargaTypeName {get;set;   }
        public bool IsSuspend {get;set;   }
        
    }
}
