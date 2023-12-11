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
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.ReturJualAgg.Workers;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using JetBrains.Annotations;
using Mapster;
using Polly;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    [UsedImplicitly]
    public partial class ReturJualForm : Form
    {
        private readonly BindingList<ReturJualItemDto> _listItem = new BindingList<ReturJualItemDto>();

        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IBrowser<SalesPersonBrowserView> _salesBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<DriverBrowserView> _driverBrowser;
        private readonly IBrowser<BrgBrowserView> _brgBrowser;
        private readonly IBrowser<ReturJualBrowserView> _returJualBrowser;

        private readonly ICustomerDal _customerDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDriverDal _driverDal;
        private readonly IReturJualBuilder _builder;
        private readonly IReturJualWriter _writer;

        private readonly ICreateReturJualItemWorker _createReturJualItemWorker;
        private readonly IGenStokReturJualWorker _genStokReturJualWorker;

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
            ICreateReturJualItemWorker createReturJualItemWorker, 
            IReturJualBuilder builder, 
            IReturJualWriter writer, 
            IBrowser<ReturJualBrowserView> returJualBrowser, 
            IGenStokReturJualWorker genStokReturJualWorker)
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
            
            _createReturJualItemWorker = createReturJualItemWorker;
            _builder = builder;
            _writer = writer;
            _returJualBrowser = returJualBrowser;
            _genStokReturJualWorker = genStokReturJualWorker;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            //  register event handler for button retur jual
            ReturJualButton.Click += ReturJualButton_Click;
            ReturJualIdText.Validating += ReturJualIdText_Validating;
            
            //  register event handler for sales
            SalesButton.Click += SalesPersonButton_Click;
            SalesIdText.Validated += SalesPersonIdText_Validated;
            SalesIdText.KeyDown += SalesPersonIdText_KeyDown;
            SalesIdText.KeyDown += SetFocusToNextControl;

            // register event handler for customer
            CustomerButton.Click += CustomerButton_Click;
            CustomerIdText.Validated += CustomerIdText_Validated;
            CustomerIdText.KeyDown += CustomerIdText_KeyDown;
            CustomerIdText.KeyDown += SetFocusToNextControl;

            // register event handler for warehouse
            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseIdText.KeyDown += WarehouseIdText_KeyDown;
            WarehouseIdText.KeyDown += SetFocusToNextControl;

            // register event handler for driver
            DriverButton.Click += DriverButton_Click;
            DriverIdText.Validated += DriverIdText_Validated;
            DriverIdText.KeyDown += DriverIdText_KeyDown;
            DriverIdText.KeyDown += SetFocusToNextControl;

            // register event handler for grid
            FakturItemGrid.CellContentClick += FakturItemGrid_CellContentClick;
            // FakturItemGrid.CellValueChanged += FakturItemGrid_CellValueChanged;
            FakturItemGrid.CellValidated += FakturItemGrid_CellValidated;
            // FakturItemGrid.KeyDown += FakturItemGrid_KeyDown;
            // FakturItemGrid.EditingControlShowing += FakturItemGrid_EditingControlShowing;
            
            SaveButton.Click += SaveButton_Click;
        }
        private void SetFocusToNextControl(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        #region RETUR-JUAL-ID
        private void ReturJualIdText_Validating(object sender, CancelEventArgs e)
        {
            var textbox = (TextBox)sender;
            var valid = true;
            if (textbox.Text.Length == 0)
                ClearDisplay();
            else
                valid = ValidateReturJual();

            if (!valid)
                e.Cancel = true;
        }

        private bool ValidateReturJual()
        {
            var textbox = ReturJualIdText;
            var policy = Policy<ReturJualModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as ReturJualModel, (r,c) => 
                {
                    MessageBox.Show(r.Exception.Message);
                });
            var returJual = policy.Execute(() => _builder
                .Load(new ReturJualModel(textbox.Text))
                .Build());
            if (returJual is null)
                return false;

            returJual.RemoveNull();
            CustomerNameText.Text = returJual.CustomerName;
            CustomerIdText.Text = returJual.CustomerId;

            ReturJualDateText.Value = returJual.ReturJualDate;
            SalesIdText.Text = returJual.SalesPersonId;
            SalesNameText.Text = returJual.SalesPersonName;
            CustomerIdText.Text = returJual.CustomerId;
            CustomerNameText.Text = returJual.CustomerName;

            WarehouseIdText.Text = returJual.WarehouseId;
            WarehouseNameText.Text = returJual.WarehouseName;

            DriverIdText.Text = returJual.DriverId;
            DriverNameText.Text = returJual.DriverName;

            TotalText.Value = returJual.Total;
            DiscountText.Value = returJual.DiscRp;
            TaxText.Value = returJual.PpnRp;
            GrandTotalText.Value = returJual.GrandTotal;

            _listItem.Clear();
            foreach (var item in returJual.ListItem)
            {
                var createReturJualItemRequest = new CreateReturJualItemRequest(item.BrgId, CustomerIdText.Text, item.HrgInputStr, item.QtyInputStr, item.QtyInputStrRusak, item.DiscInputStr, 11);
                var newItemModel = _createReturJualItemWorker.Execute(createReturJualItemRequest);
                var newItemDto = newItemModel.Adapt<ReturJualItemDto>();
                _listItem.Add(newItemDto);
            }

            if (returJual.IsVoid)
                ShowAsVoid(returJual);
            else
                ShowAsActive();

            CalcTotal();
            return true;
        }
        private void ShowAsVoid(ReturJualModel returJual)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            CancelLabel.Text = $@"Retur Jual sudah DIBATALKAN \noleh {returJual.UserIdVoid} \npada {returJual.VoidDate:ddd, dd MMM yyyy}";
            VoidPanel.Visible = true;
            SaveButton.Visible = false;
        }

        private void ShowAsActive()
        {
            this.BackColor = Color.Khaki;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.Cornsilk;

            VoidPanel.Visible = false;
            SaveButton.Visible = true;
        }
        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            DiscountText.Value = _listItem.Sum(x => x.DiscRp);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
        }

        private void ReturJualButton_Click(object sender, EventArgs e)
        {
            _returJualBrowser.Filter.Date = new Periode(DateTime.Now);

            ReturJualIdText.Text = _returJualBrowser.Browse(ReturJualIdText.Text);
            ValidateReturJual();
        }

        private void ClearDisplay()
        {
            ReturJualIdText.Clear();
            CustomerIdText.Clear();
            CustomerNameText.Clear();
            CustomerAddressText.Clear();
            WarehouseIdText.Clear();
            WarehouseNameText.Clear();
            SalesIdText.Clear();
            SalesNameText.Clear();
            DriverIdText.Clear();
            DriverNameText.Clear();
            _listItem.Clear();
            FakturItemGrid.Refresh();
        }
        #endregion
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var returJual = BuildReturJual();
            using (var trans = TransHelper.NewScope())
            {
                returJual = _writer.Save(returJual);
                
                _genStokReturJualWorker.Execute(new GenStokReturJualRequest(returJual.ReturJualId));
                
                trans.Complete();
            }
            LastIdText.Text = returJual.ReturJualId;
            ClearDisplay();
            return;

            //  LOCAL-FUNCTION
            ReturJualModel BuildReturJual()
            {
                ReturJualModel resultBuildReturJual;
                resultBuildReturJual = ReturJualIdText.Text.Length != 0 
                    ? _builder.Load(new ReturJualModel(ReturJualIdText.Text)).Build() 
                    : _builder.Create().Build();
                
                resultBuildReturJual = _builder
                    .Attach(resultBuildReturJual)
                    .Customer(new CustomerModel(CustomerIdText.Text))
                    .Warehouse(new WarehouseModel(WarehouseIdText.Text))
                    .SalesPerson(new SalesPersonModel(SalesIdText.Text))
                    .Driver(new DriverModel(DriverIdText.Text))
                    .ReturJualDate(ReturJualDateText.Value)
                    .Build();
                
                //  clearkan list item lebih dulu karna akan diisi ulang
                resultBuildReturJual.ListItem.Clear();
                
                resultBuildReturJual = _listItem
                    .Aggregate(resultBuildReturJual, (current, item) 
                        => _builder
                            .Attach(current)
                            .AddItem(item.Adapt<ReturJualItemModel>())
                            .Build());
                return resultBuildReturJual;
            }
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
            {
                CustomerNameText.Clear();
                CustomerAddressText.Clear();
                return;
            }

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
            {
                WarehouseNameText.Clear();                
                return;
            }

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
            {
                SalesNameText.Clear();            
                return;
            }

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
            {
                DriverNameText.Clear();                
                return;
            }

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

            cols.GetCol("QtyHrgDetilStr").Visible = true;
            cols.GetCol("QtyHrgDetilStr").Width = 110;
            cols.GetCol("QtyHrgDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("QtyHrgDetilStr").HeaderText = @"Qty-Harga";
            cols.GetCol("QtyHrgDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("DiscDetilStr").Visible = true;
            cols.GetCol("DiscDetilStr").Width = 110;
            cols.GetCol("DiscDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("DiscDetilStr").HeaderText = @"Discount";
            cols.GetCol("DiscDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("Qty").Visible = false;
            cols.GetCol("HrgSat").Visible = false;
            
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
        private void FakturItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <0 ) return;
            var grid = (DataGridView)sender;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "BrgId":
                case "QtyInputStr":
                case "HrgInputStr":
                case "DiscInputStr":
                    if (grid.CurrentCell.Value is null)
                        return;
                    ValidateRow(e.RowIndex);
                    break;
            }
        }
        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            
            if (e.ColumnIndex != grid.Columns["Find"]?.Index)
                return;
            
            BrowseBrg(e.RowIndex);
            FakturItemGrid.Refresh();
        }
        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId ?? string.Empty;
            _brgBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            brgId = _brgBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgId;
            ValidateRow(rowIndex);
        }
        private void ValidateRow(int rowIndex)
        {
            var item = _listItem[rowIndex];
            var req = new CreateReturJualItemRequest(item.BrgId, CustomerIdText.Text, 
                item.HrgInputStr, item.QtyInputStr, item.QtyInputStrRusak, item.DiscInputStr, 11);
            var newItem = _createReturJualItemWorker.Execute(req);
            
            _listItem[rowIndex] = newItem.Adapt<ReturJualItemDto>();
            FakturItemGrid.Refresh();
        }
        #endregion

    }
}
