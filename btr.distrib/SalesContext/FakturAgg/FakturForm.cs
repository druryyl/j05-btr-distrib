using btr.application.BrgContext.BrgAgg;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.FakturAgg.UseCases;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.OrderFeature;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SupportContext.ParamSistemAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.application.SupportContext.UserAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.OrderAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using Mapster;
using Microsoft.Reporting.WinForms;
using Polly;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturForm : Form
    {
        private BindingList<FakturItemDto> _listItem = new BindingList<FakturItemDto>();
        private readonly BindingList<FakturItemDto> _listItemJual = new BindingList<FakturItemDto>();
        private readonly BindingList<FakturItemDto> _listItemKlaim = new BindingList<FakturItemDto>();
        private readonly BindingSource _bindingSource = new BindingSource();

        private readonly IBrowser<SalesPersonBrowserView> _salesBrowser;
        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<Faktur2BrowserView> _fakturBrowser;
        private readonly IBrowser<DriverBrowserView> _driverBrowser;
        private readonly IBrowser<FakturCodeOpenBrowserView> _fakturCodeOpenBrowser;

        private readonly ISalesPersonDal _salesPersonDal;
        private readonly ICustomerDal _customerDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly IBrgDal _brgDal;
        private readonly IDriverDal _driverDal;
        private readonly IUserDal _userDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly ISaveFakturWorker _saveFakturWorker;
        private readonly ICreateFakturItemWorker _createItemWorker;
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
        private readonly IParamSistemDal _paramSistemDal;
        private readonly IPiutangDal _piutangDal;


        private string _tipeHarga = string.Empty;
        private decimal _ppnProsen = 0;
        private decimal _dppProsen = 0;


        public FakturForm(
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<SalesPersonBrowserView> salesBrowser,
            IBrowser<CustomerBrowserView> customerBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser,
            IBrowser<Faktur2BrowserView> fakturBrowser,
            ISalesPersonDal salesPersonDal,
            ICustomerDal customerDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime,
            IBrgDal brgDal,
            IFakturBuilder fakturBuilder,
            ISaveFakturWorker saveFakturWorker,
            ICreateFakturItemWorker createItemWorker,
            IPiutangBuilder piutangBuilder,
            IPiutangWriter piutangWriter,
            IBrowser<DriverBrowserView> driverBrowser,
            IDriverDal driverDal,
            IParamSistemDal paramSIstemDal,
            IBrowser<FakturCodeOpenBrowserView> fakturCodeOpenBrowser,
            IUserDal userDal,
            IOrderBuilder orderBuilder,
            IPiutangDal piutangDal)
        {
            InitializeComponent();
            _warehouseBrowser = warehouseBrowser;
            _salesBrowser = salesBrowser;
            _customerBrowser = customerBrowser;
            _brgStokBrowser = brgStokBrowser;
            _fakturBrowser = fakturBrowser;

            _salesPersonDal = salesPersonDal;
            _customerDal = customerDal;
            _warehouseDal = warehouseDal;

            _brgBuilder = brgBuilder;

            _dateTime = dateTime;
            _brgDal = brgDal;
            _fakturBuilder = fakturBuilder;
            _saveFakturWorker = saveFakturWorker;
            _createItemWorker = createItemWorker;
            _piutangBuilder = piutangBuilder;
            _piutangWriter = piutangWriter;
            _driverBrowser = driverBrowser;
            _driverDal = driverDal;
            _paramSistemDal = paramSIstemDal;
            _fakturCodeOpenBrowser = fakturCodeOpenBrowser;
            _userDal = userDal;
            _bindingSource = new BindingSource();
            _orderBuilder = orderBuilder;

            InitParamSistem();
            RegisterEventHandler();
            InitGrid();
            InitTextBox();
            ClearForm();
            _piutangDal = piutangDal;
        }

        private void InitParamSistem()
        {
            var paramKey = new ParamSistemModel("SISTEM_PPN_PROSEN");
            var paramPpn = _paramSistemDal.GetData(paramKey).ParamValue ?? "0";
            _ppnProsen = Convert.ToDecimal(paramPpn);

            paramKey = new ParamSistemModel("SISTEM_DPP_PROSEN");
            var paramDpp = _paramSistemDal.GetData(paramKey).ParamValue ?? "0";
            _dppProsen = Convert.ToDecimal(paramDpp);
        }

        private void RegisterEventHandler()
        {
            FakturIdText.Validating += FakturIdText_Validating;
            FakturButton.Click += FakturButton_Click;

            FakturCodeButton.Click += FakturCodeButton_Click;

            OrderIdText.Validated += OrderIdText_Validated;

            SalesPersonButton.Click += SalesPersonButton_Click;
            SalesIdText.Validated += SalesIdText_Validated;
            SalesIdText.KeyDown += SalesIdText_KeyDown;

            CustomerButton.Click += CustomerButton_Click;
            CustomerIdText.Validated += CustomerIdText_Validated;
            CustomerIdText.KeyDown += CustomerIdText_KeyDown;

            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.KeyDown += WarehouseIdText_KeyDown;

            DriverIdText.Validated += DriverIdText_Validated;
            DriverButton.Click += DriverButton_Click;
            DriverIdText.KeyDown += DriverIdTextBox_KeyDown;

            FakturItemGrid.CellContentClick += FakturItemGrid_CellContentClick;
            FakturItemGrid.CellValueChanged += FakturItemGrid_CellValueChanged;
            FakturItemGrid.CellValidated += FakturItemGrid_CellValidated;
            FakturItemGrid.KeyDown += FakturItemGrid_KeyDown;
            FakturItemGrid.EditingControlShowing += FakturItemGrid_EditingControlShowing;
            FakturItemGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;

            NewButton.Click += NewButton_Click;
            UangMukaText.KeyDown += UangMukaText_KeyDown;
            CopyItemJualButton.Click += CopyItemJualButton_Click;

            FakturJualRadio.CheckedChanged += JenisFakturRadio_CheckedChanged;
            FakturKlaimRadio.CheckedChanged += JenisFakturRadio_CheckedChanged;
        }

        private void InitTextBox()
        {
            FakturIdText.SetPlaceholder("Faktur Id");
            FakturCodeText.SetPlaceholder("Faktur Code");
            SalesIdText.SetPlaceholder("Sales Id...");
            OrderIdText.SetPlaceholder("Order Id...");
        }

        private void CopyItemJualButton_Click(object sender, EventArgs e)
        {
            _listItemKlaim.Clear();
            foreach (var item in _listItemJual)
            {
                var newItem = item.Adapt<FakturItemDto>();
                _listItemKlaim.Add(newItem);
            }
            FakturItemGrid.Refresh();
        }

        private void JenisFakturRadio_CheckedChanged(object sender, EventArgs e)
        {
            RefreshItemView();
        }

        private void RefreshItemView()
        {
            if (FakturJualRadio.Checked)
            {
                _listItem = _listItemJual;
                foreach (var item in this.Controls)
                    if (item is Panel panel)
                        panel.BackColor = Color.Cornsilk;

            }
            else
            {
                _listItem = _listItemKlaim;
                foreach (var item in this.Controls)
                    if (item is Panel panel)
                        panel.BackColor = Color.Thistle;
            }

            _bindingSource.DataSource = _listItem;
            FakturItemGrid.DataSource = _bindingSource;
            FakturItemGrid.Refresh();
            CalcTotal();
        }

        private void FakturCodeButton_Click(object sender, EventArgs e)
        {
            FakturCodeText.Text = _fakturCodeOpenBrowser.Browse(FakturCodeText.Text);
        }

        private void DriverIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _driverBrowser.Filter.UserKeyword = DriverIdText.Text;
                DriverIdText.Text = _driverBrowser.Browse(DriverIdText.Text);
                DriverIdText_Validated(DriverIdText, null);
                if (DriverNameText.Text.Length > 0)
                    TermOfPaymentCombo.Focus();
            }
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            DriverIdText.Text = _driverBrowser.Browse(DriverIdText.Text);
            DriverIdText_Validated(DriverIdText, null);
        }

        private void DriverIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var driver = _driverDal.GetData(new DriverModel(textbox.Text));
            DriverNameText.Text = driver?.DriverName ?? string.Empty;
        }

        private void UangMukaText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                UangMukaText.Value = GrandTotalText.Value;
            }
        }

        public void ShowFaktur(string fakturId)
        {
            FakturIdText.Text = fakturId;
            ValidateFaktur();
        }

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            var fakturDate = _dateTime.Now.AddDays(1);
            if (fakturDate.DayOfWeek == DayOfWeek.Sunday)   
                fakturDate = fakturDate.AddDays(1);

            FakturIdText.Text = string.Empty;
            FakturCodeText.Clear();
            FakturDateText.Value = fakturDate;
            SalesIdText.Text = string.Empty;
            SalesPersonNameTextBox.Text = string.Empty;
            CustomerIdText.Text = string.Empty;
            CustomerNameTextBox.Text = string.Empty;
            CustomerAddressText.Text = string.Empty;
            PlafondTextBox.Value = 0;
            CreditBalanceTextBox.Value = 0;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;
            TglRencanaKirimTextBox.Value= fakturDate.AddDays(1);
            TermOfPaymentCombo.SelectedIndex = 0;
            DueDateText.Value= DateTime.Now.AddDays(15);
            NoteTextBox.Clear();
            DriverIdText.Clear();
            DriverNameText.Clear();

            TotalText.Value = 0;
            DiscountText.Value = 0;
            DppText.Value = 0;
            TaxText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;
            _tipeHarga = string.Empty;

            var newItem = new FakturItemDto();
            newItem.SetPPnProsen(_ppnProsen);

            _listItemJual.Clear();
            _listItemJual.Add(newItem);
            _listItemKlaim.Clear();
            _listItemKlaim.Add(newItem);

            RefreshItemView();

            ShowAsActive();
        }
        #endregion

        #region FAKTUR
        private void FakturButton_Click(object sender, EventArgs e)
        {
            _fakturBrowser.Filter.Date = new Periode(DateTime.Now);

            FakturIdText.Text = _fakturBrowser.Browse(FakturIdText.Text);
            FakturIdText_Validating(FakturIdText, null);
        }

        private void FakturIdText_Validating(object sender, CancelEventArgs e)
        {
            var textbox = (TextBox)sender;
            var valid = true;
            if (textbox.Text.Length == 0)
                ClearForm();
            else
                valid = ValidateFaktur();

            if (!valid)
                e.Cancel = true;
        }

        private bool ValidateFaktur()
        {
            var textbox = FakturIdText;
            var policy = Policy<FakturModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as FakturModel, (r,c) => 
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var faktur = policy.Execute(() => _fakturBuilder
                .Load(new FakturModel(textbox.Text))
                .Build());
            if (faktur is null)
                return false;

            faktur.RemoveNull();

            FakturDateText.Value = faktur.FakturDate;
            FakturCodeText.Text = faktur.FakturCode;
            OrderIdText.Text = faktur.OrderId;
            SalesIdText.Text = faktur.SalesPersonId;
            SalesPersonNameTextBox.Text = faktur.SalesPersonName;
            CustomerIdText.Text = faktur.CustomerId;
            CustomerNameTextBox.Text = faktur.CustomerName;
            CustomerAddressText.Text = faktur.Address;

            _tipeHarga = faktur.HargaTypeId;
            PlafondTextBox.Value = faktur.Plafond;
            CreditBalanceTextBox.Value = faktur.CreditBalance;
            WarehouseIdText.Text = faktur.WarehouseId;
            WarehouseNameText.Text = faktur.WarehouseName;
            TglRencanaKirimTextBox.Value = faktur.TglRencanaKirim;
            DriverIdText.Text = faktur.DriverId;
            DriverNameText.Text = faktur.DriverName;

            TermOfPaymentCombo.SelectedIndex = (int)faktur.TermOfPayment;
            DueDateText.Value = faktur.DueDate;
            TotalText.Value = faktur.Total;
            DiscountText.Value = faktur.Discount;
            DppText.Value = faktur.Dpp;
            TaxText.Value = faktur.Tax;
            GrandTotalText.Value = faktur.GrandTotal;
            UangMukaText.Value = faktur.UangMuka;
            SisaText.Value = faktur.KurangBayar;
            LastIdLabel.Text = $@"{faktur.FakturCode}".Trim();
            NoteTextBox.Text = faktur.Note;

            _listItemJual.Clear();
            foreach (var item in faktur.ListItem)
            {
                var newItem = item.Adapt<FakturItemDto>();
                _listItemJual.Add(newItem);
            }

            _listItemKlaim.Clear();
            foreach (var item in faktur.ListItemKlaim)
            {
                var newItem = item.Adapt<FakturItemDto>();
                _listItemKlaim.Add(newItem);
            }

            RefreshItemView();

            if (faktur.IsVoid)
                ShowAsVoid(faktur);
            else
                ShowAsActive();

            CalcTotal();
            return true;
        }
        
        private void ShowAsVoid(FakturModel faktur)
        {
            this.BackColor = Color.RosyBrown;

            CancelLabel.Text = $@"Faktur sudah DIBATALKAN \noleh {faktur.UserIdVoid} \npada {faktur.VoidDate:ddd, dd MMM yyyy}";
            VoidPanel.Visible = true;
            SaveButton.Visible = false;
        }
        
        private void ShowAsActive()
        {
            this.BackColor = Color.Khaki;

            VoidPanel.Visible = false;
            SaveButton.Visible = true;
        }
        #endregion


        #region SALES-PERSON
        public void LoadOrder(string orderId)
        {
            ClearForm();
            OrderIdText.Text = orderId;
            OrderIdText_Validated(OrderIdText, null);
        }
        private void OrderIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;
            
            var order = _orderBuilder.Load(OrderModel.Key(textbox.Text)).Build();
            SalesIdText.Text = order.SalesId;
            var sales = _salesPersonDal.GetData(new SalesPersonModel(order.SalesId));
            SalesPersonNameTextBox.Text = sales?.SalesPersonName ?? string.Empty;
            var customer = _customerDal.GetData(new CustomerModel(order.CustomerId));
            CustomerIdText.Text = order.CustomerId;
            CustomerNameTextBox.Text = customer?.CustomerName ?? string.Empty;
            CustomerAddressText.Text = customer?.Address1 ?? string.Empty;
            _tipeHarga = customer?.HargaTypeId ?? string.Empty;
            LoadBrg(order);
        }
        private void LoadBrg(OrderModel order)
        {
            _listItemJual.Clear();
            _listItemKlaim.Clear();
            foreach (var item in order.ListItems)   
            {
                var newItem = item.Adapt<FakturItemDto>();
                newItem.DiscInputStr = $"{item.Disc1.ToSmartString()};{item.Disc2.ToSmartString()};{item.Disc3.ToSmartString()};{item.Disc4.ToSmartString()}";
                newItem.SetPPnProsen(_ppnProsen);
                newItem.QtyInputStr = $"{item.QtyBesar};{item.QtyKecil};{item.QtyBonus}";
                _listItemJual.Add(newItem);
            }
            RefreshItemView();
            foreach(DataGridViewRow item in FakturItemGrid.Rows)
            {
                if (item.Index > _listItemJual.Count - 1)
                    continue;
                ValidateRow(item.Index);
            }
        }
        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
            SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
            SalesIdText_Validated(SalesIdText, null);
        }
        private void SalesIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var sales = _salesPersonDal.GetData(new SalesPersonModel(textbox.Text));
            SalesPersonNameTextBox.Text = sales?.SalesPersonName ?? string.Empty;
        }
        private void SalesIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
                SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
                SalesIdText_Validated(SalesIdText, null);
                if (SalesPersonNameTextBox.Text.Length > 0)
                    CustomerIdText.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        #endregion

        #region CUSTOMER
        private void CustomerButton_Click(object sender, EventArgs e)
        {
            _customerBrowser.Filter.UserKeyword = CustomerIdText.Text;
            CustomerIdText.Text = _customerBrowser.Browse(CustomerIdText.Text);
            CustomerIdText_Validated(CustomerIdText, null);
        }
        private void CustomerIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var customer = _customerDal.GetData(new CustomerModel(textbox.Text));
            CustomerNameTextBox.Text = customer?.CustomerName ?? string.Empty;
            CustomerAddressText.Text = customer?.Address1 ?? string.Empty;
            _tipeHarga = customer?.HargaTypeId??string.Empty;

            //  cek piutang
            var duaBulanLalu = DateTime.Now.AddMonths(-2);
            var startDate = new DateTime(2000, 1, 1);
            var periode = new Periode(startDate, duaBulanLalu);
            var listPiutang = _piutangDal.ListData(customer, periode);
            if (listPiutang is null)
                return;
            var sumPiutang = listPiutang.Sum(x => x.Sisa);
            if (sumPiutang > 10)
                MessageBox.Show($"Customer masih mempunya piutang lebih dari 2 bulan senilai {sumPiutang:N0}");
        }
        private void CustomerIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _customerBrowser.Filter.UserKeyword = CustomerIdText.Text;
                CustomerIdText.Text = _customerBrowser.Browse(CustomerIdText.Text);
                CustomerIdText_Validated(CustomerIdText, null);
                if (CustomerNameTextBox.Text.Length > 0)
                    WarehouseIdText.Focus();
            }
        }
        #endregion

        #region WAREHOUSE
        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        private void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var warehouse = _warehouseDal.GetData(new WarehouseModel(textbox.Text));
            WarehouseNameText.Text = warehouse?.WarehouseName ?? string.Empty;
        }
        private void WarehouseIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _warehouseBrowser.Filter.UserKeyword = WarehouseIdText.Text;
                WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
                WarehouseIdText_Validated(WarehouseIdText, null);
                if (WarehouseNameText.Text.Length > 0)
                    TglRencanaKirimTextBox.Focus();
            }
        }
        #endregion

        #region GRID
        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex != grid.Columns.GetCol("Find").Index)
                return;

            BrowseBrg(e.RowIndex);
        }

        private void FakturItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
            {
                ValidateRow(e.RowIndex);
            }
                //CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("QtyInputStr").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("DppProsen").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("DiscInputStr").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("PpnProsen").Index)
                CalcTotal();
        }

        private void FakturItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <0 ) return;
            var grid = (DataGridView)sender;
            switch (grid.CurrentCell.OwningColumn.Name)
            {
                case "BrgId":
                case "QtyInputStr":
                case "DiscInputStr":
                case "HrgInputStr":
                case "PpnProsen":
                    if (grid.CurrentCell.Value is null)
                    {
                        return;
                    }
                    ValidateRow(e.RowIndex);
                    break;
            }
        }

        private void FakturItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
            switch (e.KeyCode)
            {
                case Keys.F1:
                    if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                        BrowseBrg(grid.CurrentCell.RowIndex);
                    if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("DiscInputStr").Index)
                        if (MessageBox.Show("Set semua item dengan diskon ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            SetAllDiscount(grid.CurrentCell.RowIndex);

                    break;

                case Keys.Delete:
                    _listItem.RemoveAt(grid.CurrentCell.RowIndex);
                    grid.Refresh();
                    break;
            }
        }

        private void SetAllDiscount(int rowIndex)
        {
            var disc = _listItem[rowIndex].DiscInputStr;
            foreach (var item in _listItem)
                item.DiscInputStr = disc;
            FakturItemGrid.Refresh();

            foreach(DataGridViewRow item in FakturItemGrid.Rows)
            {
                if (item.Index > _listItem.Count - 1)
                    continue;
                ValidateRow(item.Index);
            }
            CalcTotal();
        }

        #region browse-brg-saat-cell-aktif
        private void FakturItemGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex != 1)
                return;

            if (e.Control is TextBox textBox)
            {
                textBox.KeyDown -= TextBox_KeyDown;
                textBox.KeyDown += TextBox_KeyDown;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                FakturItemGrid.EndEdit();
                BrowseBrg(FakturItemGrid.CurrentCell.RowIndex);
            }
        }
        #endregion

        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId;
            _brgStokBrowser.Filter.StaticFilter1 = WarehouseIdText.Text;
            _brgStokBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            brgId = _brgStokBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgId;
            ValidateRow(rowIndex);
        }

        private void ValidateRow(int rowIndex)
        {
            var brg = BuildBrg(rowIndex);
            if (brg == null)
            {
                return;
            }

            var req = new CreateFakturItemRequest(
                _listItem[rowIndex].BrgId,
                _listItem[rowIndex].QtyInputStr,
                _listItem[rowIndex].DiscInputStr,
                _listItem[rowIndex].HrgInputStr,
                _listItem[rowIndex].DppProsen == 0 ? _dppProsen : _listItem[rowIndex].DppProsen,
                _listItem[rowIndex].PpnProsen == 0 ? _ppnProsen : _listItem[rowIndex].PpnProsen,
                _tipeHarga,
                WarehouseIdText.Text);
            var item = _createItemWorker.Execute(req);
            _listItem[rowIndex] = item.Adapt<FakturItemDto>();

            FakturItemGrid.Refresh();
            CalcTotal();
        }

        private BrgModel BuildBrg(int rowIndex)
        {
            var id = _listItem[rowIndex].BrgId ?? string.Empty;
            if (id.Length == 0)
                return null;

            var brgKey = new BrgModel(id);
            var fbk = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as BrgModel);
            var brg = fbk.Execute(() => _brgBuilder.Load(brgKey).Build());

            if (brg is null)
            {
                brg = GetBrgByCode(id);
                _listItem[rowIndex].BrgId = brg?.BrgId??string.Empty;
                FakturItemGrid.Refresh();
            }

            return brg;
        }
        
        private BrgModel GetBrgByCode(string id)
        {
            var result = _brgDal.GetData(id);
            if (result is null) return null;

            result = _brgBuilder.Load(result).Build();
            return result;
        }

        private void InitGrid()
        {
            _bindingSource.DataSource = _listItem;
            FakturItemGrid.DataSource = _bindingSource;
            FakturItemGrid.Refresh();
            FakturItemGrid.Columns.SetDefaultCellStyle(Color.Beige);

            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            FakturItemGrid.Columns.Insert(1, buttonCol);

            var cols = FakturItemGrid.Columns;
            cols.GetCol("BrgId").Visible = true;
            cols.GetCol("BrgId").Width = 50;
            
            cols.GetCol("Find").Width = 20;

            cols.GetCol("BrgCode").Visible = true;
            cols.GetCol("BrgCode").Width = 80;

            cols.GetCol("BrgName").Visible = true;
            cols.GetCol("BrgName").Width = 160;
            cols.GetCol("BrgName").DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            cols.GetCol("StokHargaStr").Visible = true;
            cols.GetCol("StokHargaStr").Width = 110;
            cols.GetCol("StokHargaStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("StokHargaStr").HeaderText= @"StokHarga";
            cols.GetCol("StokHargaStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyInputStr").Width = 50;
            cols.GetCol("QtyInputStr").HeaderText = @"Qty";

            cols.GetCol("HrgInputStr").Visible = true;
            cols.GetCol("HrgInputStr").Width = 70;
            cols.GetCol("HrgInputStr").HeaderText = @"Harga";
            cols.GetCol("QtyDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyDetilStr").Visible = true;
            cols.GetCol("QtyDetilStr").Width = 80;
            cols.GetCol("QtyDetilStr").HeaderText = @"Qty Desc";
            cols.GetCol("QtyDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("QtyDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyBesar").Visible = false;
            cols.GetCol("SatBesar").Visible = false;
            cols.GetCol("Conversion").Visible = false;
            cols.GetCol("HrgSatBesar").Visible = false;
            
            cols.GetCol("QtyKecil").Visible = false;
            cols.GetCol("SatKecil").Visible = false;
            cols.GetCol("HrgSatKecil").Visible = false;
            cols.GetCol("QtyJual").Visible = false;
            cols.GetCol("HrgSat").Visible = false;

            cols.GetCol("SubTotal").Visible = true;
            cols.GetCol("SubTotal").Width = 70;

            cols.GetCol("QtyBonus").Visible = false;
            cols.GetCol("QtyPotStok").Visible = false;
            
            cols.GetCol("DiscInputStr").Visible = true;
            cols.GetCol("DiscInputStr").Width = 65;
            cols.GetCol("DiscInputStr").HeaderText = @"Disc";

            cols.GetCol("DiscDetilStr").Visible = true;
            cols.GetCol("DiscDetilStr").Width = 90;
            cols.GetCol("DiscDetilStr").HeaderText= @"Disc Rp";
            cols.GetCol("DiscDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("DiscDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("DiscRp").Visible = false;

            cols.GetCol("DppProsen").Visible = false;
            cols.GetCol("DppRp").Visible = false;

            cols.GetCol("PpnProsen").Visible = true;
            cols.GetCol("PpnProsen").Width = 50;
            cols.GetCol("PpnProsen").HeaderText = @"Ppn";

            cols.GetCol("PpnRp").Visible = false;

            cols.GetCol("Total").Visible = true;
            cols.GetCol("Total").Width = 80;

            //  auto-resize-rows
            FakturItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FakturItemGrid.AutoResizeRows();

        }
        #endregion

        #region TOTAL
        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            DiscountText.Value = _listItem.Sum(x => x.DiscRp);
            DppText.Value = _listItem.Sum(x => x.DppRp);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
            SisaText.Value = GrandTotalText.Value - UangMukaText.Value;
        }
        private void DiscountLainText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        private void BiayaLainText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        private void UangMukaText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var faktur = SaveFaktur();
                SavePiutang(faktur);

                ClearForm();

                var fakturDb = _fakturBuilder
                    .Load(faktur)
                    .Build();
                LastIdLabel.Text = $@"{fakturDb.FakturId} - {fakturDb.FakturCode}".Trim();
                var customer = _customerDal.GetData(fakturDb);
                var user = _userDal.GetData(fakturDb) ?? new UserModel
                {
                    UserId = fakturDb.UserId,
                    UserName = fakturDb.UserId
                };
                var isKlaim = FakturKlaimRadio.Checked;
                var fakturPrintout = new FakturPrintOutDto(fakturDb, customer, user, isKlaim);
                PrintFakturRdlc(fakturPrintout);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private FakturModel SaveFaktur()
        {
            var mainform = (MainForm)this.Parent.Parent;
            var cmd = new SaveFakturRequest
            {
                FakturId = FakturIdText.Text,
                FakturCode = FakturCodeText.Text,
                FakturDate = FakturDateText.Value.ToString("yyyy-MM-dd"),
                OrderId = OrderIdText.Text,
                CustomerId = CustomerIdText.Text,
                SalesPersonId = SalesIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                RencanaKirimDate = TglRencanaKirimTextBox.Value.ToString("yyyy-MM-dd"),
                DriverId = DriverIdText.Text,
                TermOfPayment = TermOfPaymentCombo.SelectedIndex,
                DueDate = DueDateText.Value.ToString("yyyy-MM-dd"),
                UserId = mainform.UserId.UserId,
                Cash = UangMukaText.Value,
                Note = NoteTextBox.Text,

            };
            var listItem = (
                from c in _listItemJual
                where c.BrgName?.Length > 0
                select new SaveFakturRequestItem
                {
                    BrgId = c.BrgId,
                    StokHarga = c.StokHargaStr,
                    QtyString = c.QtyInputStr,
                    HrgString = c.HrgInputStr,
                    DiscountString = c.DiscInputStr,
                    DppProsen = c.DppProsen,
                    PpnProsen = c.PpnProsen,
                }).ToList();
            cmd.ListBrg = listItem;

            var listItemKlaim = (
                from c in _listItemKlaim
                where c.BrgName?.Length > 0
                select new SaveFakturRequestItem
                {
                    BrgId = c.BrgId,
                    StokHarga = c.StokHargaStr,
                    QtyString = c.QtyInputStr,
                    HrgString = c.HrgInputStr,
                    DiscountString = c.DiscInputStr,
                    DppProsen = c.DppProsen,
                    PpnProsen = c.PpnProsen,
                }).ToList();
            cmd.ListBrgKlaim = listItemKlaim;

            var result = _saveFakturWorker.Execute(cmd);
            return result;
        }

        private void SavePiutang(FakturModel faktur)
        {
            if (faktur is null)
                return;

            PiutangModel piutang;
            try
            {
                piutang = _piutangBuilder
                    .Load(new PiutangModel(faktur.FakturId))
                    .Customer(faktur)
                    .PiutangDate(faktur.FakturDate)
                    .DueDate(faktur.DueDate)
                    .NilaiPiutang(faktur.GrandTotal)
                    .Build();
            }
            catch (KeyNotFoundException)
            {
                piutang = _piutangBuilder
                    .Create(faktur)
                    .Customer(faktur)
                    .PiutangDate(faktur.FakturDate)
                    .DueDate(faktur.DueDate)
                    .NilaiPiutang(faktur.GrandTotal)
                    .StatusPiutang(StatusPiutangEnum.Tercatat)
                    .Build();
            }
            piutang = _piutangBuilder
                .Attach(piutang)
                .SetUangMuka(faktur.UangMuka)
                .Build();
            _piutangWriter.Save(ref piutang);
        }

        private void PrintFakturRdlc(FakturPrintOutDto faktur)
        {
            var fakturJualDataset = new ReportDataSource("FakturJualDataset", new List<FakturPrintOutDto> { faktur });
            var fakturJualItemDataset = new ReportDataSource("FakturJualItemDataset", faktur.ListItem);
            var clientId = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ID"))?.ParamValue ?? string.Empty;
            
            var printOutTemplate = string.Empty;
            switch (clientId)
            {
                case "BTR-YK":
                    printOutTemplate = "FakturPrintOut-Yk";
                    break;
                case "BTR-MGL":
                    printOutTemplate = "FakturPrintOut-Mgl";
                    break;
                default:
                    break;
            }

            var listDataset = new List<ReportDataSource>
            {
                fakturJualDataset,
                fakturJualItemDataset
            };
            var rdlcViewerForm = new RdlcViewerForm();
            rdlcViewerForm.SetReportData(printOutTemplate, listDataset);
            rdlcViewerForm.ShowDialog();
        }

        internal void ShowKlaim()
        {
            FakturJualRadio.Checked = false;
            FakturKlaimRadio.Checked = false;
            RefreshItemView();
        }
        #endregion

    }

public static class DecimalExtensions
    {
        public static string ToSmartString(this decimal value, int maxDecimalPlaces = 8)
        {
            // Check if the number is an integer (no fractional part)
            if (value == Math.Truncate(value))
            {
                return value.ToString("0", CultureInfo.InvariantCulture);
            }

            // For numbers with fractional part, remove trailing zeros
            string formatString = $"0.{new string('#', maxDecimalPlaces)}";
            return value.ToString(formatString, CultureInfo.InvariantCulture);
        }
    }
}