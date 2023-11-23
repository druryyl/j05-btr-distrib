using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.Browsers;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.ReturJualAgg.Workers;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;
using Polly;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    public partial class ReturJualForm : Form
    {
        private readonly BindingList<ReturJualItemDto> _listItem = new BindingList<ReturJualItemDto>();

        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IBrowser<SalesPersonBrowserView> _salesBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<DriverBrowserView> _driverBrowser;
        private readonly IBrowser<BrgBrowserView> _brgBrowser;

        private readonly ICustomerDal _customerDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDriverDal _driverDal;
        private readonly IBrgDal _brgDal;

        private readonly IBrgBuilder _brgBuilder;

        private readonly ICreateReturJualItemWorker _createReturJualItemWorker;

        public ReturJualForm(
            IBrowser<CustomerBrowserView> customerBrowser,
            IBrowser<SalesPersonBrowserView> salesBrowser,
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<DriverBrowserView> driverBrowser,
            IBrowser<BrgBrowserView> brgBrowser,
            ICustomerDal customerDal,
            ISalesPersonDal salesPersonDal,
            IWarehouseDal warehouseDal, 
            IDriverDal driverDal, 
            IBrgDal brgDal, 
            IBrgBuilder brgBuilder, 
            ICreateReturJualItemWorker createReturJualItemWorker)
        {
            InitializeComponent();

            _customerBrowser = customerBrowser;
            _salesBrowser = salesBrowser;
            _warehouseBrowser = warehouseBrowser;
            _driverBrowser = driverBrowser;
            _brgBrowser = brgBrowser;

            _salesPersonDal = salesPersonDal;
            _customerDal = customerDal;
            _warehouseDal = warehouseDal;
            _driverDal = driverDal;
            _brgDal = brgDal;
            
            _brgBuilder = brgBuilder;
            _createReturJualItemWorker = createReturJualItemWorker;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SalesButton.Click += SalesPersonButton_Click;
            SalesIdText.Validated += SalesPersonIdText_Validated;
            SalesIdText.KeyDown += SalesPersonIdText_KeyDown;

            // register event handler for customer
            CustomerButton.Click += CustomerButton_Click;
            CustomerIdText.Validated += CustomerIdText_Validated;
            CustomerIdText.KeyDown += CustomerIdText_KeyDown;

            // register event handler for warehouse
            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseIdText.KeyDown += WarehouseIdText_KeyDown;

            // register event handler for driver
            DriverButton.Click += DriverButton_Click;
            DriverIdText.Validated += DriverIdText_Validated;
            DriverIdText.KeyDown += DriverIdText_KeyDown;

            // register event handler for grid
            FakturItemGrid.CellContentClick += FakturItemGrid_CellContentClick;
            // FakturItemGrid.CellValueChanged += FakturItemGrid_CellValueChanged;
            // FakturItemGrid.CellValidated += FakturItemGrid_CellValidated;
            // FakturItemGrid.KeyDown += FakturItemGrid_KeyDown;
            // FakturItemGrid.EditingControlShowing += FakturItemGrid_EditingControlShowing;

        }

        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex != grid.Columns["Find"].Index)
                return;

            BrowseBrg(e.RowIndex);
            FakturItemGrid.Refresh();
        }

        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId;
            _brgBrowser.Filter.StaticFilter1 = WarehouseIdText.Text;
            _brgBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            brgId = _brgBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgId;
            ValidateRow(rowIndex);
        }

        private void ValidateRow(int rowIndex)
        {
            var item = _listItem[rowIndex];
            var req = new CreateReturJualItemRequest(item.BrgId, CustomerIdText.Text, item.HrgInputStr, item.QtyInputStr, item.DiscInputStr, 11);
            var newItem = _createReturJualItemWorker.Execute(req);
            
            // TODO: Sampai di sinilah untuk validasi item di grid (ReturJualForm.cs) 
            var brg = BuildBrg(rowIndex);
            _listItem[rowIndex].BrgId = brg?.BrgId ?? string.Empty;
            _listItem[rowIndex].BrgCode = brg?.BrgCode ?? string.Empty;
            _listItem[rowIndex].BrgName = brg?.BrgName ?? string.Empty;

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

            if (!(brg is null)) return brg;
            
            brg = GetBrgByCode(id);
            return brg;
        }
        private BrgModel GetBrgByCode(string id)
        {
            var result = _brgDal.GetData(id);
            if (result is null) return null;

            result = _brgBuilder.Load(result).Build();
            return result;
        }
        
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
            CustomerNameText.Text = customer?.CustomerName ?? string.Empty;
            CustomerAddressText.Text = customer?.Address1 ?? string.Empty;
        }
        private void CustomerIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _customerBrowser.Filter.UserKeyword = CustomerIdText.Text;
                CustomerIdText.Text = _customerBrowser.Browse(CustomerIdText.Text);
                CustomerIdText_Validated(sender, null);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        #endregion

        
        #region WAREHOUSE
        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            _warehouseBrowser.Filter.UserKeyword = WarehouseIdText.Text;
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
                WarehouseIdText_Validated(sender, null);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        #endregion

        
        #region SALES
        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
            SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
            SalesPersonIdText_Validated(SalesIdText, null);
        }
        private void SalesPersonIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var sales = _salesPersonDal.GetData(new SalesPersonModel(textbox.Text));
            SalesNameText.Text = sales?.SalesPersonName ?? string.Empty;
        }
        private void SalesPersonIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
                SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
                SalesPersonIdText_Validated(sender, null);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        #endregion

        
        #region DRIVER
        private void DriverButton_Click(object sender, EventArgs e)
        {
            _driverBrowser.Filter.UserKeyword = DriverIdText.Text;
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
        private void DriverIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _driverBrowser.Filter.UserKeyword = DriverIdText.Text;
                DriverIdText.Text = _driverBrowser.Browse(DriverIdText.Text);
                DriverIdText_Validated(sender, null);
            }

            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        #endregion

        #region GRID

        private void InitGrid()
        {
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturItemGrid.DataSource = binding;
            FakturItemGrid.Refresh();
            FakturItemGrid.Columns.SetDefaultCellStyle(Color.Beige);
            
            var buttonCol = new DataGridViewButtonColumn
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

            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyInputStr").Width = 50;
            cols.GetCol("QtyInputStr").HeaderText = @"Qty";

            cols.GetCol("HrgInputStr").Visible = true;
            cols.GetCol("HrgInputStr").Width = 110;
            cols.GetCol("HrgInputStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HrgInputStr").HeaderText = @"Hrg Retur";
            cols.GetCol("HrgInputStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;


            cols.GetCol("SubTotal").Visible = true;
            cols.GetCol("SubTotal").Width = 70;

            cols.GetCol("DiscRp").Visible = false;

            cols.GetCol("PpnRp").Visible = false;

            cols.GetCol("Total").Visible = true;
            cols.GetCol("Total").Width = 80;

            //  auto-resize-rows
            FakturItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FakturItemGrid.AutoResizeRows();
            
        }
        #endregion

    }
}
