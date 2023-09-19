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
using ClosedXML.Excel;
using Mapster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        private PackingModel _aggregate;

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
            _aggregate = null;

            InitWarehouse();
            InitDriver();
            InitFakturKiriGrid();
            InitFakturKanan();
            InitFakturBrgKanan();
            InitPeriode();
            InitContextMenu();

            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            WarehouseCombo.SelectedValueChanged += WarehouseCombo_SelectedValueChanged;
            DriverCombo.SelectedValueChanged += DriverCombo_SelectedValueChanged;

            FakturKananGrid.DoubleClick += FakturKananGrid_DoubleClick;
            FakturKiriGrid.CellDoubleClick += FakturKiriGrid_CellDoubleClick;
            FakturKiriGrid.MouseClick += FakturKiriGrid_MouseClick;
              
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            PackingTab.SelectedIndexChanged += PackingTab_SelectedIndexChanged;
            SupplierGrid.CellDoubleClick += SupplierGrid_CellDoubleClick;

            PreviewFakturButton.Click += PreviewFakturButton_Click;

        }

        private void PreviewFakturButton_Click(object sender, EventArgs e)
        {
            var path = Path.GetTempPath();
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("PackingPerBarang");
                var baris = 1;
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18"; 
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "LAPORAN PACKING LIST";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{_aggregate.DeliveryDate:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Merge();
                baris++;
                baris++;

                ws.Cell($"A{baris}").Value = $"Driver";
                ws.Cell($"B{baris}").Value = $"{_aggregate.DriverName}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);

                baris++;
                baris++;

                foreach(var item in _aggregate.ListSupplier)
                {
                    ws.Cell($"A{baris}").Value = $"Supplier: {item.SupplierName}";
                    ws.Cell($"A{baris}").Style
                        .Font.SetFontSize(11)
                        .Font.SetBold(true);
                    baris++;
                    var barisStart = baris;
                    ws.Cell($"A{baris}").Value = "Kode";
                    ws.Cell($"B{baris}").Value = "Nama Barang";
                    ws.Cell($"C{baris}").Value = "Satuan 1";
                    ws.Cell($"D{baris}").Value = "Satuan 2";
                    ws.Cell($"E{baris}").Value = "Harga Jual";
                    baris++;

                    foreach (var brg in item.ListBrg.OrderBy(x => x.BrgCode))
                    {
                        ws.Cell($"A{baris}").Value = $"{brg.BrgCode}";
                        ws.Cell($"B{baris}").Value = $"{brg.BrgName}";
                        ws.Cell($"C{baris}").Value = $"{brg.QtyBesar:N0} {brg.SatuanBesar}";
                        ws.Cell($"D{baris}").Value = $"{brg.QtyKecil:N0} {brg.SatuanKecil}";
                        ws.Cell($"E{baris}").Value = $"{brg.HargaJual:N0}";
                        baris++;
                    }
                    ws.Cell($"B{baris}").Value = $"Sub Total";
                    ws.Cell($"C{baris}").Value = $"{item.ListBrg.Sum(x => x.QtyBesar)}";
                    ws.Cell($"D{baris}").Value = $"";
                    ws.Cell($"E{baris}").Value = $"{item.ListBrg.Sum(x => x.HargaJual)}";
                    ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"E{baris}")).Style
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                    ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"E{barisStart}")).Style
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Font.SetBold(true);

                    baris++;
                    baris++;
                }
                ws.Column("A").Width = 12;
                ws.Column("B").Width = 45;
                ws.Column("E").Width = 15;
                workbook.SaveAs($"{path}\\PackingList.xlsx");
            }
            System.Diagnostics.Process.Start($"{path}\\PackingList.xlsx");
        }

        private void SupplierGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentRow is null)
                return;

            var supplierId = grid.CurrentRow.Cells["SupplierId"].Value.ToString();
            var listBrg = _aggregate.ListSupplier
                .FirstOrDefault(x => x.SupplierId == supplierId)?
                .ListBrg ?? new List<PackingBrgModel>();

            var dataSource = (
                from c in listBrg
                select new PackingSupplierBrgDto(c.BrgId, c.BrgName, c.BrgCode,
                    c.QtyBesar, c.SatuanBesar, c.QtyKecil, c.SatuanKecil, c.HargaJual)
                ).ToList();

            SupplierBrgGrid.DataSource = dataSource;
            var col = SupplierBrgGrid.Columns;
            col.SetDefaultCellStyle(Color.Beige);
            col.GetCol("BrgId").Visible = false;
            col.GetCol("BrgCode").Width = 80;
            col.GetCol("BrgName").Width = 200;
            col.GetCol("QtyBesar").Width = 40;
            col.GetCol("SatBesar").Width = 40;
            col.GetCol("QtyKecil").Width = 40;
            col.GetCol("SatKecil").Width = 40;
            col.GetCol("HargaJual").Width = 60;
        }

        private void PackingTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = (TabControl)sender;
            if (tab.SelectedIndex == 1)
            {
                if (_aggregate is null)
                    return;

                var listSupplier = (
                    from c in _aggregate.ListSupplier
                    select new PackingSupplierDto(c.SupplierId, c.SupplierName, c.ListBrg.Count()))
                    .ToList();
                SupplierGrid.DataSource = listSupplier;
                SupplierGrid.Columns.SetDefaultCellStyle(Color.AliceBlue);
                SupplierGrid.Refresh();
                SupplierGrid.Columns.GetCol("SupplierName").Width = 200;
            }
        }

        #region AGGREGATE-USE-CASE
        private void LoadOrCreate()
        {
            var driverKey = new DriverModel(DriverCombo.SelectedValue.ToString());
            var warehouseKey = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var deliveryDate = DeliveryDate.Value;

            _aggregate = _packingBuilder
                .LoadOrCreate(driverKey, deliveryDate)
                .Warehouse(warehouseKey)
                .Build();
            _aggregate = _packingWriter.Save(_aggregate);
            ShowAggregate();
        }

        private void ShowAggregate()
        {
            PackingIdLabel.Text = _aggregate?.PackingId ?? "[Packing ID]";
            var listFaktur = (
                from c in _aggregate.ListFaktur
                select c.Adapt<PackingFakturDto>()
                ).ToList();
            FakturKananGrid.DataSource = listFaktur;
            FakturKananGrid.Refresh();
        }

        private void AddFaktur(IFakturKey fakturKey)
        {
            _aggregate = _packingBuilder
                .Attach(_aggregate)
                .AddFaktur(fakturKey)
                .Build();
            _aggregate = _packingWriter.Save(_aggregate);
        }

        private void RemoveFaktur(IFakturKey fakturKey)
        {
            _aggregate = _packingBuilder
                .Attach(_aggregate)
                .RemoveFaktur(fakturKey)
                .Build();
            _aggregate = _packingWriter.Save(_aggregate);
        }
        #endregion


        #region WAREHOUSE
        private void WarehouseCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadOrCreate();
            ShowAggregate();
            RefreshFakturKiri();
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
            var grid = (DataGridView)sender;
            if (e.Button == MouseButtons.Right)
            {
                _fakturKiriContextMenu.Show(grid, e.Location);
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


            if (_aggregate is null)
                LoadOrCreate();

            AddFaktur(fakturKey);
            ShowAggregate();
            RefreshFakturKiri();
        }
        
        private void UnpackingFaktur_OnClick(object sender, EventArgs e)
        {
            var grid = FakturKiriGrid;
            if (grid.CurrentRow is null)
                return;
            
            var packingKey = new PackingModel(grid.CurrentRow.Cells["PackingId"].Value.ToString());
            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            CancelPacking(packingKey, fakturKey);
            LoadOrCreate();
        }
        
        private void InitContextMenu()
        {
            _fakturKiriContextMenu = new ContextMenu();
            _fakturKiriContextMenu.MenuItems.Add(new MenuItem("Remove Faktur", UnpackingFaktur_OnClick));
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
            LoadOrCreate();
            ShowAggregate();
        }
        private void InitDriver()
        {
            var listDriver = _driverDal.ListData() ?? new List<DriverModel>();
            DriverCombo.DataSource = listDriver;
            DriverCombo.DisplayMember = "DriverName";
            DriverCombo.ValueMember = "DriverId";
        }
        #endregion

        #region GRID-FAKTUR-KANAN
        private void FakturKananGrid_DoubleClick(object sender, EventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentRow is null)
                return;

            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            LoadBrgFaktur(fakturKey);
            ShowAggregate();
        }
        private void InitFakturKanan()
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
                    item.QtyBesar, item.QtyKecil, item.Total);
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
            g.GetCol("QtyBesar").Width = 50;
            g.GetCol("QtyKecil").Width = 50;
            g.GetCol("HargaJual").Width = 100;
        }
        #endregion
    }

    public class PackingBrgFakturDto
    {
        public PackingBrgFakturDto(string brgId, 
            string brgCode, string brgName, 
            int qtyBesar, int qtyKecil, decimal hargaJual)
        {
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
            QtyBesar = qtyBesar;
            QtyKecil = qtyKecil;
            HargaJual = hargaJual;
        }
        public string BrgId { get; private set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public int QtyKecil { get; private set; }   
        public decimal HargaJual { get; private set; }
    }

    public class PackingSupplierDto
    {
        public PackingSupplierDto(string id, string name, int jumItem)
        {
            SupplierId = id;
            SupplierName = name;
            JumlahItem = jumItem;
        }
        public string SupplierId { get; private set; }  
        public string SupplierName { get; private set; }
        public int JumlahItem { get; private set; }
    }

    public class PackingSupplierBrgDto
    {
        public PackingSupplierBrgDto(string id, string name, string code,
            int qtyBesar, string satBesar, int qtyKecil, string satKecil,
            decimal hargaJual)
        {
            BrgId = id;
            BrgName = name;
            BrgCode = code;
            QtyBesar = qtyBesar;
            SatBesar = satBesar;
            QtyKecil = qtyKecil;
            SatKecil = satKecil;
            HargaJual = hargaJual;
        }

        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HargaJual { get; private set; }
    }
}
