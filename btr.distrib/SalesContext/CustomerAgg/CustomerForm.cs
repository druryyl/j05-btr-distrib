using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.distrib.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using btr.nuna.Domain;
using btr.domain.SalesContext.CustomerAgg;
using btr.distrib.Browsers;
using btr.distrib.SharedForm;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.application.BrgContext.HargaTypeAgg;
using btr.domain.BrgContext.HargaTypeAgg;

namespace btr.distrib.SalesContext.CustomerAgg
{
    public partial class CustomerForm : Form
    {
        private readonly ICustomerDal _customerDal;
        private readonly ICustomerBuilder _customerBuilder;
        private IEnumerable<CustomerFormGridDto> _listCust;
        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IKlasifikasiDal _klasifikasiDal;
        private readonly IHargaTypeDal _hargaTypeDal;
        private readonly ICustomerWriter _customerWriter;

        public CustomerForm(ICustomerDal customerDal,
            ICustomerBuilder customerBuilder,
            IBrowser<CustomerBrowserView> customerBrowser,
            IKlasifikasiDal klasifikasiDal,
            IHargaTypeDal hargaTypeDal,
            ICustomerWriter customerWriter)
        {
            InitializeComponent();
            _customerDal = customerDal;
            _customerBuilder = customerBuilder;
            _customerBrowser = customerBrowser;
            _klasifikasiDal = klasifikasiDal;
            _hargaTypeDal = hargaTypeDal;

            RegisterEventHandler();
            InitGrid();
            InitKlasifikasi();
            InitTipeHarga();
            _customerWriter = customerWriter;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            CustIdText.Validated += CustIdText_Validated;
            CustButton.Click += CustButton_Click;

            SaveButton.Click += SaveButton_Click;
        }

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
                .AddressWp(Alamat1WpText.Text, Alamat2WpText.Text)
                .Build();

            _customerWriter.Save(ref customer);
            ClearForm();
        }

        private void ClearForm()
        {
            CustIdText.Clear();;
            CustNameText.Clear();;
            CustCodeText.Clear();;
            //KlasifikasiCombo.SelectedValue = string.Empty;
            //TipeHargaCombo.SelectedValue = null;
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
            Alamat1WpText.Clear();
            Alamat2WpText.Clear();
        }
        private void InitKlasifikasi()
        {
            var listAll = _klasifikasiDal.ListData()?.ToList() ?? new List<KlasifikasiModel>();
            KlasifikasiCombo.DataSource = listAll;
            KlasifikasiCombo.DisplayMember = "KlasifikasiName";
            KlasifikasiCombo.ValueMember = "KlasifikasiId";
        }

        private void InitTipeHarga()
        {
            var listAll = _hargaTypeDal.ListData()?.ToList() ?? new List<HargaTypeModel>();
            TipeHargaCombo.DataSource = listAll;
            TipeHargaCombo.DisplayMember = "HargaTypeName";
            TipeHargaCombo.ValueMember = "HargaTypeId";
        }

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
            Alamat1WpText.Text = customer.AddressWp;
            Alamat2WpText.Text = customer.AddressWp2;
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

        private void InitGrid()
        {
            var listCust = _customerDal.ListData()?.ToList()
                ?? new List<domain.SalesContext.CustomerAgg.CustomerModel>();

            _listCust = listCust
                .Select(x => new CustomerFormGridDto(x.CustomerId,
                    x.CustomerName,
                    $"{x.Address1} {x.Kota}",
                    x.Plafond,
                    x.CreditBalance)).ToList();
            CustGrid.DataSource = _listCust;
            
            CustGrid.Columns.SetDefaultCellStyle();
            CustGrid.Columns.GetCol("Id").Width = 50;
            CustGrid.Columns.GetCol("Name").Width = 150;
            CustGrid.Columns.GetCol("Alamat").Width = 220;
            CustGrid.Columns.GetCol("Plafond").Width = 100;
            CustGrid.Columns.GetCol("CreditBalance").Width = 100;
        }

        private void FilterCustGrid(string keyword)
        {
            if (keyword.Length == 0)
                CustGrid.DataSource = _listCust;
            var listFilter = _listCust.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            var listByAlamat = _listCust.Where(x => x.Alamat.ContainMultiWord(keyword)).ToList();
            listFilter.AddRange(listByAlamat);
            CustGrid.DataSource = listFilter;
        }
        #endregion
    }

    public class CustomerFormGridDto
    {
        public CustomerFormGridDto(string id, string name, string alamat, decimal plafond, decimal creditBalance)
        {
            Id = id;
            Name = name;
            Alamat = alamat;
            Plafond = plafond;
            CreditBalance = creditBalance;
        }
        public string Id { get; }
        public string Name { get; }
        public string Alamat { get; }
        public decimal Plafond { get; }
        public decimal CreditBalance { get; }
    }
}
