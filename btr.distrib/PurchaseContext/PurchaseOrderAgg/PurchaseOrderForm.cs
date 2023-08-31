using MediatR;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.distrib.SharedForm;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.distrib.Browsers;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.InventoryContext.StokBalanceAgg;
using Polly;
using btr.domain.InventoryContext.StokBalanceAgg;
using System.Collections.Generic;
using btr.domain.BrgContext.BrgAgg;
using btr.application.PurchaseContext.PurchaseOrderAgg.Workers;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using Mapster;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;

namespace btr.distrib.PurchaseContext.PurchaseOrderAgg
{
    public partial class PurchaseOrderForm : Form
    {
        private readonly IMediator _mediator;
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStok2Browser;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IBrgBuilder _brgBuilder;
        
        private readonly IPurchaseOrderBuilder _purchaseOrderBuilder;
        private readonly IPurchaseOrderWriter _purchaseOrderWriter;

        private readonly BindingList<PurchaseOrderItemDto> _listItem = new BindingList<PurchaseOrderItemDto>();

        public PurchaseOrderForm(IMediator mediator,
            IBrowser<SupplierBrowserView> supplierBrowser,
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<BrgStokBrowserView> brgSto2Browser,
            ISupplierDal supplierDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IPurchaseOrderBuilder purchaseOrderBuilder,
            IPurchaseOrderWriter purchaseOrderWriter)
        {
            InitializeComponent();
            RegisterEventHandler();
            InitGrid();

            _mediator = mediator;
            _supplierBrowser = supplierBrowser;
            _warehouseBrowser = warehouseBrowser;
            _brgStok2Browser = brgSto2Browser;
            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _purchaseOrderBuilder = purchaseOrderBuilder;
            _purchaseOrderWriter = purchaseOrderWriter;
        }

        private void RegisterEventHandler()
        {
            SupplierIdText.Validated += SupplierIdText_Validated;
            SupplierButton.Click += SupplierButton_Click;

            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseButton.Click += WarehouseButton_Click;

            Grid.CellContentClick += PurchaseOrderItemGrid_CellContentClick;
            Grid.CellValidated += Grid_CellValidated;

            DiscountLainText.Validated += DiscountLainText_Validated;
            BiayaLainText.Validated += BiayaLainText_Validated;

            SaveButton.Click += SaveButton_Click;
        }

        #region SUPPLIER
        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;
            
            var supplier = _supplierDal.GetData(new SupplierModel(textbox.Text));
            SupplierNameTextBox.Text = supplier?.SupplierName??string.Empty;
        }

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }
        #endregion

        #region WAREHOUSE
        private void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var warehouse = _warehouseDal.GetData(new WarehouseModel(textbox.Text));
            WarehouseNameText.Text = warehouse?.WarehouseName??string.Empty;
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
            if (e.ColumnIndex != grid.Columns["Find"].Index)
                return;

            var brgId = _listItem[grid.CurrentRow.Index].BrgId;
            _brgStok2Browser.Filter.StaticFilter1 = WarehouseIdText.Text;
            brgId = _brgStok2Browser.Browse(brgId);
            _listItem[grid.CurrentRow.Index].BrgId = brgId;
            ValidateRow(e.RowIndex);
        }

        private void Grid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            CalculateTotal();
            if (e.ColumnIndex == grid.Columns["BrgId"].Index)
                return;

            ValidateRow(e.RowIndex);
        }

        private void InitGrid()
        {
            if (!_listItem.Any())
                _listItem.Add(new PurchaseOrderItemDto());

            var binding = new BindingSource
            {
                DataSource = _listItem
            };
            Grid.DataSource = binding;

            foreach (DataGridViewColumn col in Grid.Columns)
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
                Name = "Find" // Set the button name
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            Grid.Columns.Insert(1, buttonCol);

            //  width
            Grid.Columns.GetCol("BrgId").Width = 50;
            Grid.Columns.GetCol("Find").Width = 20;
            Grid.Columns.GetCol("BrgName").Width = 150;
            Grid.Columns.GetCol("Qty").Width = 50;
            Grid.Columns.GetCol("Satuan").Width = 50;
            Grid.Columns.GetCol("Harga").Width = 80;
            Grid.Columns.GetCol("SubTotal").Width = 80;
            Grid.Columns.GetCol("Disc").Width = 35;
            Grid.Columns.GetCol("DiscRp").Width = 100;
            Grid.Columns.GetCol("Tax").Width = 30;
            Grid.Columns.GetCol("TaxRp").Width = 80;
            Grid.Columns.GetCol("Total").Width = 80;
            //  right align
            Grid.Columns.GetCol("Qty").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("Harga").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("SubTotal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("Disc").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("DiscRp").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("Tax").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("TaxRp").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            Grid.Columns.GetCol("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            //  number-format
            Grid.Columns.GetCol("Harga").DefaultCellStyle.Format = "#,##0.00";
            Grid.Columns.GetCol("DiscRp").DefaultCellStyle.Format = "#,##0.00";
            Grid.Columns.GetCol("TaxRp").DefaultCellStyle.Format = "#,##0.00";
            Grid.Columns.GetCol("SubTotal").DefaultCellStyle.Format = "#,##0.00";
            Grid.Columns.GetCol("Total").DefaultCellStyle.Format = "#,##0.00";
            //  auto-resize-rows
            Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Grid.AutoResizeRows();
        }

        private void ValidateRow(int row)
        {
            var key = new BrgModel(_listItem[row].BrgId ?? string.Empty);
            var fallbackStok = Policy<StokBalanceModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(new StokBalanceModel());
            var fallbackBrg = Policy<BrgModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(new BrgModel());

            var stok = fallbackStok.Execute(() => _stokBalanceBuilder.Load(key).Build());
            var brg = fallbackBrg.Execute(() => _brgBuilder.Load(key).Build());

            _listItem[row].SetBrgName(brg.BrgName ?? string.Empty);
            _listItem[row].SetSatuan(brg.ListSatuan?
                .FirstOrDefault(x => x.Conversion == 1)?
                .Satuan ?? string.Empty);
            CalculateTotal();
            Grid.Refresh();
        }

        private void CalculateTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.Total);
            GrandTotalText.Value = TotalText.Value - DiscountLainText.Value + BiayaLainText.Value;
        }
        #endregion

        #region TOTAL SUMMARY
        private void BiayaLainText_Validated(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void DiscountLainText_Validated(object sender, EventArgs e)
        {
            CalculateTotal();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var agg = _purchaseOrderBuilder
                .Create()
                .Warehouse(new WarehouseModel(WarehouseIdText.Text))
                .Supplier(new SupplierModel(SupplierIdText.Text))
                .DiscountLain(DiscountLainText.Value)
                .BiayaLain(BiayaLainText.Value)
                .Build();
            foreach(var item in _listItem)
            {
                var newItem = item.Adapt<PurchaseOrderItemModel>();
                agg = _purchaseOrderBuilder
                    .Attach(agg)
                    .AddItem(item, item.Qty, item.Satuan, item.Harga, item.Disc, item.Tax)
                    .Build();
            }

            _purchaseOrderWriter.Save(ref agg);
        }
        #endregion

    }
}
