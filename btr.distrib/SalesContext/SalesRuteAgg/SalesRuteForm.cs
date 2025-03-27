using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.RuteAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.Helpers;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.SalesPersonAgg
{
    public partial class SalesRuteForm : Form
    {
        private readonly BindingList<CustomerViewDto> _listCustomerView;
        private readonly BindingSource _customerViewBindingSource;
        private string _hariRuteId;
        private readonly Dictionary<string, string> _hariDictionary;

        private readonly ISalesPersonDal _salesPersonDal;
        private readonly ICustomerDal _customerDal;
        private readonly IHariRuteDal _hariRuteDal;

        public SalesRuteForm(ISalesPersonDal salesPersonDal,
            ICustomerDal customerDal,
            IHariRuteDal hariRuteDal)
        {
            InitializeComponent();
            _salesPersonDal = salesPersonDal;
            _customerDal = customerDal;
            _hariRuteDal = hariRuteDal;

            _listCustomerView = new BindingList<CustomerViewDto>();
            _customerViewBindingSource = new BindingSource(_listCustomerView, null);
            _hariDictionary = new Dictionary<string, string>();

            RegisterEventHandler();
            InitComboBox();
            InitGrid();
            InitRadioButton();
        }

        private void RegisterEventHandler()
        {
            CustomerGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            H11Radio.CheckedChanged += HariRadio_CheckedChanged;
            H12Radio.CheckedChanged += HariRadio_CheckedChanged;
            H13Radio.CheckedChanged += HariRadio_CheckedChanged;
            H14Radio.CheckedChanged += HariRadio_CheckedChanged;
            H15Radio.CheckedChanged += HariRadio_CheckedChanged;
            H16Radio.CheckedChanged += HariRadio_CheckedChanged;
            H21Radio.CheckedChanged += HariRadio_CheckedChanged;
            H22Radio.CheckedChanged += HariRadio_CheckedChanged;
            H23Radio.CheckedChanged += HariRadio_CheckedChanged;
            H24Radio.CheckedChanged += HariRadio_CheckedChanged;
            H25Radio.CheckedChanged += HariRadio_CheckedChanged;
            H26Radio.CheckedChanged += HariRadio_CheckedChanged;
        }

        private void InitComboBox()
        {
            var listSales = _salesPersonDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            listSales = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesComboBox.DataSource = listSales;
            SalesComboBox.DisplayMember = "SalesPersonName";
            SalesComboBox.ValueMember = "SalesPersonId";
            SalesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void InitGrid()
        {
            InitGridCustomer();
        }
        private void InitGridCustomer()
        {
            CustomerGrid.DataSource = _customerViewBindingSource;
            PopulateCustomer();
            CustomerGrid.Columns["CustomerId"].Visible = false;
            CustomerGrid.Columns["CustomerCode"].HeaderText = "Kode";
            CustomerGrid.Columns["CustomerName"].HeaderText = "Nama";
            CustomerGrid.Columns["Address"].HeaderText = "Alamat";

            CustomerGrid.Columns["Wilayah"].Width = 70;
            CustomerGrid.Columns["CustomerCode"].Width = 70;
            CustomerGrid.Columns["CustomerName"].Width = 120;
            CustomerGrid.Columns["Address"].Width = 170;
            CustomerGrid.Columns["Hari"].Width = 70;
        }
        private void InitRadioButton()
        {
            H11Radio.Tag = "H11";
            H12Radio.Tag = "H12";
            H13Radio.Tag = "H13";
            H14Radio.Tag = "H14";
            H15Radio.Tag = "H15";
            H16Radio.Tag = "H16";
            H21Radio.Tag = "H21";
            H22Radio.Tag = "H22";
            H23Radio.Tag = "H23";
            H24Radio.Tag = "H24";
            H25Radio.Tag = "H25";
            H26Radio.Tag = "H26";
            _hariRuteId = "H11";
            H11Radio.Checked = true;

            var listHariRute = _hariRuteDal.ListData()?.ToList() ?? new List<HariRuteModel>();
            listHariRute.ForEach(x => _hariDictionary.Add(x.HariRuteId, x.HariRuteName));
        }


        private void HariRadio_CheckedChanged(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;
            if (!radio.Checked)
                return;
            _hariRuteId = radio.Tag.ToString();
            HariLabel.Text = _hariDictionary[_hariRuteId];
        }



        private void PopulateCustomer()
        {
            var listCustomer = _customerDal.ListData()?.ToList() ?? new List<CustomerModel>();
            _listCustomerView.Clear();

            listCustomer = listCustomer
                .OrderBy(x => x.WilayahName)
                .ThenBy(x => x.CustomerCode)
                .ToList();

            listCustomer
                .ForEach(x => _listCustomerView.Add(new CustomerViewDto(
                x.CustomerId, x.CustomerCode, x.CustomerName,
                x.Address1, x.WilayahName)));
            CustomerGrid.Refresh();
        }
    }

}
