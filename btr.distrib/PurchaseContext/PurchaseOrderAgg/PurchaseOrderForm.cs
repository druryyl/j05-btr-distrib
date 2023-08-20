using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btr.application.PurchaseContext.SupplierAgg.UseCases;
using btr.distrib.SharedForm;
using btr.domain.PurchaseContext.SupplierAgg;
using Polly;
using btr.distrib.Browsers;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.application.InventoryContext.WarehouseAgg.UseCases;
using btr.distrib.Helpers;
using btr.application.InventoryContext.StokAgg.UseCases;
using btr.distrib.SalesContext.FakturAgg;

namespace btr.distrib.PurchaseContext.PurchaseOrderAgg
{
    public partial class PurchaseOrderForm : Form
    {
        private readonly IMediator _mediator;
        private readonly ISupplierBrowser _supplierBrowser;
        private readonly IWarehouseBrowser _warehouseBrowser;
        private readonly IBrgStok2Browser _brgStok2Browser;

        private readonly BindingList<PurchaseOrderItemDto> _listItem = new BindingList<PurchaseOrderItemDto>();

        public PurchaseOrderForm(IMediator mediator,
            ISupplierBrowser supplierBrowser,
            IWarehouseBrowser warehouseBrowser,
            IBrgStok2Browser brgSto2Browser)
        {
            InitializeComponent();
            RegisterEventHandler();
            InitGrid();

            _mediator = mediator;
            _supplierBrowser = supplierBrowser;
            _warehouseBrowser = warehouseBrowser;
            _brgStok2Browser = brgSto2Browser;
        }

        private void RegisterEventHandler()
        {
            SupplierIdText.Validated += SupplierIdText_Validated;
            SupplierButton.Click += SupplierButtonOnClick;

            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseButton.Click += WarehouseButton_Click;

            PurchaseOrderItemGrid.CellContentClick += PurchaseOrderItemGrid_CellContentClick;
        }

        #region SUPPLIER
        private async void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;
            
            var fallback = Policy<SupplierModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new SupplierModel());
            var query = new GetSupplierQuery(textbox.Text);
            var supplier = await fallback.ExecuteAsync(() => _mediator.Send(query));
            SupplierNameTextBox.Text = supplier.SupplierName;
        }
        private void SupplierButtonOnClick(object sender, EventArgs e)
        {
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }
        #endregion

        #region WAREHOUSE
        private async void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var fallback = Policy<WarehouseModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new WarehouseModel());
            var query = new GetWarehouseQuery(textbox.Text);
            var warehouse = await fallback.ExecuteAsync(() => _mediator.Send(query));
            WarehouseNameText.Text = warehouse.WarehouseName;
        }
        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        #endregion

        #region GRID
        private void PurchaseOrderItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            var brgId = _listItem[grid.CurrentRow.Index].BrgId;
            var warehouseId = WarehouseIdText.Text;
            brgId =  _brgStok2Browser
                .Warehouse(warehouseId)
                .Browse(brgId);
            _listItem[grid.CurrentRow.Index].BrgId = brgId;

            //if (e.ColumnIndex == grid.Columns["Find"]?.Index && e.RowIndex >= 0)
            //{
            //    if (WarehouseIdText.Text.Length == 0)
            //        return;

            //    var defaultBrgId = grid.CurrentCell.Value?.ToString() ?? string.Empty;
            //    var warehouse = WarehouseIdText.Text;
            //    _brgStokBrowser.BrowserQueryArgs = new[] { warehouse };
            //    var form = new BrowserForm<ListBrgStokResponse, string>(_brgStokBrowser, defaultBrgId, x => x.BrgName);
            //    var resultDialog = form.ShowDialog();
            //    if (resultDialog == DialogResult.OK)
            //        if (grid.CurrentRow != null)
            //            grid.CurrentRow.Cells["BrgId"].Value = form.ReturnedValue;
            //}
            //if (_listItem.Last().BrgName.Length != 0)
            //    _listItem.Add(new FakturItemDto(_mediator));
        }
        private void InitGrid()
        {
            RefreshGrid();
            foreach (DataGridViewColumn col in PurchaseOrderItemGrid.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Consolas", 8.25f);
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                if (col.ReadOnly)
                    col.DefaultCellStyle.BackColor = Color.Beige;
            }
            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            PurchaseOrderItemGrid.Columns.Insert(1, buttonCol);

            //  width
            PurchaseOrderItemGrid.Columns.GetCol("BrgId").Width = 50;
            PurchaseOrderItemGrid.Columns.GetCol("Find").Width = 20;
            PurchaseOrderItemGrid.Columns.GetCol("BrgName").Width = 150;
            PurchaseOrderItemGrid.Columns.GetCol("Qty").Width = 50;
            PurchaseOrderItemGrid.Columns.GetCol("Satuan").Width = 50;
            PurchaseOrderItemGrid.Columns.GetCol("Harga").Width = 80;
            PurchaseOrderItemGrid.Columns.GetCol("SubTotal").Width = 80;
            PurchaseOrderItemGrid.Columns.GetCol("Disc").Width = 35;
            PurchaseOrderItemGrid.Columns.GetCol("DiscRp").Width = 100;
            PurchaseOrderItemGrid.Columns.GetCol("Tax").Width = 30;
            PurchaseOrderItemGrid.Columns.GetCol("TaxRp").Width = 80;
            PurchaseOrderItemGrid.Columns.GetCol("Total").Width = 80;
            //  right align
            PurchaseOrderItemGrid.Columns.GetCol("Qty").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("Harga").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("SubTotal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("Disc").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("DiscRp").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("Tax").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("TaxRp").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            PurchaseOrderItemGrid.Columns.GetCol("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            //  number-format
            PurchaseOrderItemGrid.Columns.GetCol("Harga").DefaultCellStyle.Format = "#,##0.00";
            PurchaseOrderItemGrid.Columns.GetCol("DiscRp").DefaultCellStyle.Format = "#,##0.00";
            PurchaseOrderItemGrid.Columns.GetCol("TaxRp").DefaultCellStyle.Format = "#,##0.00";
            PurchaseOrderItemGrid.Columns.GetCol("SubTotal").DefaultCellStyle.Format = "#,##0.00";
            PurchaseOrderItemGrid.Columns.GetCol("Total").DefaultCellStyle.Format = "#,##0.00";
            //  auto-resize-rows
            PurchaseOrderItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            PurchaseOrderItemGrid.AutoResizeRows();
        }
        private void RefreshGrid()
        {
            if (!_listItem.Any())
                _listItem.Add(new PurchaseOrderItemDto());

            var binding = new BindingSource
            {
                DataSource = _listItem
            };
            PurchaseOrderItemGrid.DataSource = binding;
        }
        #endregion
    }
}
