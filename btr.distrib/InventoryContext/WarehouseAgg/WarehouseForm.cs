using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.WarehouseAgg
{
    public partial class WarehouseForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;

        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;

        private readonly IWarehouseBuilder _warehouseBuilder;
        private readonly IWarehouseWriter _warehouseWriter;

        private IEnumerable<WarehouseFormGridDto> _listWarehouse;

        public WarehouseForm(IWarehouseDal warehouseDal,
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IWarehouseBuilder warehouseBuilder,
            IWarehouseWriter warehouseWriter
            )
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _warehouseBrowser = warehouseBrowser;

            _warehouseBuilder = warehouseBuilder;
            _warehouseWriter = warehouseWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseButton.Click += WarehouseButton_Click;


            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
        }

        #region GRID-CUSTOMER
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FilterListGrid(SearchText.Text);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FilterListGrid(SearchText.Text);
        }

        private void ListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var grid = (DataGridView)sender;
            var warehouseId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var warehouse = _warehouseBuilder.Load(new WarehouseModel(warehouseId)).Build();
            ShowData(warehouse);
        }

        private void InitGrid()
        {
            var listWarehouse = _warehouseDal.ListData()?.ToList()
                ?? new List<WarehouseModel>();

            _listWarehouse = listWarehouse
                .Select(x => new WarehouseFormGridDto(x.WarehouseId,
                    x.WarehouseName)).ToList();
            ListGrid.DataSource = _listWarehouse;

            ListGrid.Columns.SetDefaultCellStyle(Color.Azure);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 150;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listWarehouse;
                return;
            }
            var listFilter = _listWarehouse.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region KATEGORI
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

            var warehouse = _warehouseBuilder
                .Load(new WarehouseModel(textbox.Text))
                .Build();

            ShowData(warehouse);
        }

        private void ShowData(WarehouseModel warehouse)
        {
            WarehouseIdText.Text = warehouse.WarehouseId;
            WarehouseNameText.Text = warehouse.WarehouseName;
        }

        private void ClearForm()
        {
            WarehouseIdText.Clear();
            WarehouseNameText.Clear();
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
            var warehouse = new WarehouseModel(WarehouseIdText.Text);
            if (WarehouseIdText.Text.Length == 0)
                warehouse = _warehouseBuilder.Create().Build();
            else
                warehouse = _warehouseBuilder.Load(warehouse).Build();

            warehouse = _warehouseBuilder
                .Attach(warehouse)
                .Name(WarehouseNameText.Text)
                .Build();

            _warehouseWriter.Save(ref warehouse);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class WarehouseFormGridDto
    {
        public WarehouseFormGridDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }
        public string Name { get; }
    }
}
