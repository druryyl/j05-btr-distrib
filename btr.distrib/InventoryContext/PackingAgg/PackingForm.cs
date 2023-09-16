using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.PackingAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btr.domain.BrgContext.BrgAgg;

namespace btr.distrib.InventoryContext.PackingAgg
{
    public partial class PackingForm : Form
    {
        private readonly IFakturDal _fakturDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IPackingBuilder _packingBuilder;
        private readonly IDriverDal _driverDal;
        private readonly IPackingWriter _packingWriter;
        private readonly ITglJamDal _dateTime;
        private readonly IFakturBuilder _fakturBuilder;
        
        private ContextMenu _fakturKiriContextMenu;

        public PackingForm(IFakturDal fakturDal,
            IWarehouseDal warehouseDal,
            IPackingBuilder packingBuilder,
            IDriverDal driverDal,
            IPackingWriter packingWriter,
            ITglJamDal dateTime,
            IFakturBuilder fakturBuilder)
        {
            InitializeComponent();
            _fakturDal = fakturDal;
            _warehouseDal = warehouseDal;
            _packingBuilder = packingBuilder;
            _driverDal = driverDal;
            _packingWriter = packingWriter;
            _dateTime = dateTime;
            _fakturBuilder = fakturBuilder;

            InitWarehouse();
            InitDriver();
            InitFakturKiriGrid();
            InitFakturKiriKanan();
            InitFakturBrgKanan();
            InitPeriode();
            InitContextMenu();

            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            WarehouseCombo.SelectedValueChanged += WarehouseCombo_SelectedValueChanged;
            DriverCombo.SelectedValueChanged += DriverCombo_SelectedValueChanged;

            FakturKiriGrid.CellDoubleClick += FakturKiriGrid_CellDoubleClick;
            FakturKiriGrid.MouseClick += FakturKiriGrid_MouseClick;
            
            FakturKananGrid.DoubleClick += FakturKananGrid_DoubleClick;
            
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;
        }

        private void FakturKananGrid_DoubleClick(object sender, EventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentRow is null)
                return;

            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            LoadBrgFaktur(fakturKey);
        }

        #region WAREHOUSE
        private void WarehouseCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            RefreshFakturKiri();
            LoadPacking();
        }
        private void InitWarehouse()
        {
            var listWarehouse = _warehouseDal.ListData() ?? new List<WarehouseModel>();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }
        #endregion

        #region SEARCH FAKTUR
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                RefreshFakturKiri();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshFakturKiri();
        }
        private void InitPeriode()
        {
            Faktur1Date.Value = _dateTime.Now().AddDays(-2);
            Faktur2Date.Value = _dateTime.Now();
        }
        #endregion

        #region GRID-FAKTUR-KIRI
        private void FakturKiriGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _fakturKiriContextMenu.Show(FakturKiriGrid, e.Location);
            }
        }
        
        private void FakturKiriGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var grid = (DataGridView)sender;

            var packingKey = new PackingModel(grid.Rows[e.RowIndex].Cells["PackingId"].Value.ToString());
            var fakturKey = new FakturModel(grid.Rows[e.RowIndex].Cells["FakturId"].Value.ToString());
            if (packingKey.PackingId.Length > 0)
            {
                CancelPacking(packingKey, fakturKey);
            }

            var driverKey = new DriverModel(DriverCombo.SelectedValue.ToString());
            var warehouseKey = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var deliveryDate = DeliveryDate.Value;
            var packing = _packingBuilder
                .LoadOrCreate(driverKey, deliveryDate)
                .Warehouse(warehouseKey)
                .AddFaktur(fakturKey)
                .Build();
            _ = _packingWriter.Save(packing);

            RefreshFakturKiri();
            LoadPacking();
        }
        
        private void UnpackingFaktur_OnClick(object sender, EventArgs e)
        {
            var grid = FakturKiriGrid;
            if (grid.CurrentRow is null)
                return;
            
            var packingKey = new PackingModel(grid.CurrentRow.Cells["PackingId"].Value.ToString());
            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            CancelPacking(packingKey, fakturKey);
            LoadPacking();
        }
        
        private void InitContextMenu()
        {
            _fakturKiriContextMenu = new ContextMenu();
            _fakturKiriContextMenu.MenuItems.Add(new MenuItem("Unpacking Faktur", UnpackingFaktur_OnClick));
            FakturKiriGrid.ContextMenu = _fakturKiriContextMenu;
        }
        
        private void InitFakturKiriGrid()
        {
            RefreshFakturKiri();
            FakturKiriGrid.Columns.SetDefaultCellStyle(Color.PowderBlue);
            var g = FakturKiriGrid.Columns;
            g.GetCol("FakturId").Visible = false;
            g.GetCol("PackingId").Visible = false;

            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = "Code";
            g.GetCol("CustomerName").Width = 80;
            g.GetCol("CustomerName").HeaderText = "Customer";

            g.GetCol("Address").Width = 120;
            g.GetCol("Kota").Width = 70;
            g.GetCol("DriverName").Width = 60;
            g.GetCol("DriverName").HeaderText = "Driver";
            g.GetCol("GrandTotal").Width = 70;
        }
        
        private void RefreshFakturKiri()
        {
            var periode = new Periode(Faktur1Date.Value, Faktur2Date.Value);
            var listFaktur = _fakturDal.ListDataPacking(periode) ?? new List<FakturPackingView>();

            var dataSource = (
                from c in listFaktur
                where c.WarehouseId == WarehouseCombo.SelectedValue.ToString()
                select c.Adapt<PackingFakturDto>()
                ).ToList();

            var keyword = SearchText.Text;
            if (keyword.Length > 0)
            {
                var filterName = dataSource.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword.ToLower()));
                var filterAddress = dataSource.Where(x => x.Address.ToLower().ContainMultiWord(keyword.ToLower()));
                var filterKota = dataSource.Where(x => x.Kota.ToLower().StartsWith(keyword.ToLower()));
                var filterDriver = dataSource.Where(x => x.DriverName.ToLower() == keyword.ToLower());
                var filterFakturCode = dataSource.Where(x => x.FakturCode.ToLower().StartsWith(keyword.ToLower()));

                dataSource = filterName
                    .Concat(filterAddress)
                    .Concat(filterKota)
                    .Concat(filterDriver)
                    .Concat(filterFakturCode)
                    .ToList();
            }
            FakturKiriGrid.DataSource = dataSource;
            FakturKiriGrid.Refresh();
        }

        private void CancelPacking(IPackingKey packingKey, IFakturKey fakturKey)
        {
            if (packingKey.PackingId.Length == 0)
                return;

            var packing = _packingBuilder
                .Load(packingKey)
                .RemoveFaktur(fakturKey)
                .Build();
            _packingWriter.Save(packing);
            RefreshFakturKiri();
        }
        #endregion

        #region DRIVER
        private void DriverCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadPacking();
        }
        private void InitDriver()
        {
            var listDriver = _driverDal.ListData() ?? new List<DriverModel>();
            DriverCombo.DataSource = listDriver;
            DriverCombo.DisplayMember = "DriverName";
            DriverCombo.ValueMember = "DriverId";
        }
        #endregion

        #region PACKING
        #endregion

        #region GRID-FAKTUR-KANAN
        private void InitFakturKiriKanan()
        {
            var listFaktur = new List<PackingFakturDto>();
            FakturKananGrid.DataSource = listFaktur;
            FakturKananGrid.Refresh();
            FakturKananGrid.Columns.SetDefaultCellStyle(Color.Cornsilk);

            var g = FakturKananGrid.Columns;
            g.GetCol("FakturId").Visible = true;
            g.GetCol("PackingId").Visible = false;
            g.GetCol("DriverName").Visible = false;

            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = @"Code";
            g.GetCol("CustomerName").Width = 80;
            g.GetCol("CustomerName").HeaderText = @"Customer";

            g.GetCol("Address").Width = 120;
            g.GetCol("Kota").Width = 70;

            g.GetCol("GrandTotal").Width = 70;
        }

        private void LoadPacking()
        {
            var driverKey = new DriverModel(DriverCombo.SelectedValue.ToString());
            var deliveryDate = DeliveryDate.Value;

            var packing = _packingBuilder
                .Load(driverKey, deliveryDate)
                .Build();

            var listFaktur = (
                from c in packing.ListFaktur
                select c.Adapt<PackingFakturDto>()
                ).ToList();
            FakturKananGrid.DataSource = listFaktur;
            FakturKananGrid.Refresh();
        }
        #endregion

        #region GRID-BRG-PER-FAKTUR
        
        private void LoadBrgFaktur(IFakturKey fakturKey)
        {
            var faktur = _fakturBuilder.Load(fakturKey).Build();
            var listBrg = new List<PackingBrgFakturDto>();
            foreach (var item in faktur.ListItem)
            {
                //  conversion belum benar2 update
                var newItem = new PackingBrgFakturDto(item.BrgId, item.BrgCode, item.BrgName, 
                    item.QtyPotStok, item.Conversion, item.Total);
                listBrg.Add(newItem);
            }

            FakturBrgGrid.DataSource = listBrg;
            FakturBrgGrid.Refresh();
        }
        private void InitFakturBrgKanan()
        {
            var listBrg = new List<PackingBrgFakturDto>();
            FakturBrgGrid.DataSource = listBrg;
            FakturBrgGrid.Refresh();
            FakturBrgGrid.Columns.SetDefaultCellStyle(Color.Cornsilk);

            var g = FakturBrgGrid.Columns;
            g.GetCol("BrgId").Visible = false;

            g.GetCol("BrgCode").Width = 80;
            g.GetCol("BrgName").Width = 250;
            g.GetCol("Qty1").Width = 50;
            g.GetCol("Qty2").Width = 50;
            g.GetCol("HargaJual").Width = 100;
        }
        #endregion
    }

    public class PackingBrgFakturDto
    {
        public PackingBrgFakturDto(string id, string code, string name, int qty, int conversion, decimal harga)
        {
            BrgId = id;
            BrgName = name;
            BrgCode = code;
            HargaJual = harga;

            if (conversion == 1)
                Qty2 = qty;
            else
            {
                decimal division = qty / conversion;
                Qty1 = (int)division;
                Qty2 = Qty1  % conversion;
            }
        }

        public string BrgId { get; private set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int Qty1 { get; private set; }
        public int Qty2 { get; private set; }   
        public decimal HargaJual { get; private set; }
    }


    
}
