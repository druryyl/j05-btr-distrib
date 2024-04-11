using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.nuna.Domain;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using Mapster;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.PackingAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.distrib.Browsers;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SupportContext.UserAgg;
using ClosedXML.Excel;

namespace btr.distrib.InventoryContext.PackingAgg
{
    public partial class Packing2Form : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDriverDal _driverDal;
        private readonly IFakturDal _fakturDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IBrgDal _brgDal;
        private readonly ISupplierDal _supplierDal;

        private readonly IPackingBuilder _builder;
        private readonly IPackingWriter _writer;
        private readonly IBrowser<PackingBrowserView> _fakturBrowser;

        private readonly ObservableCollection<Packing2FakturDto> _listFaktur;
        private readonly ObservableCollection<Packing2FakturDto> _listFakturSelected;
        private readonly ObservableCollection<Packing2FakturBrgDto> _listFakturSelectedBrg;
        private readonly ObservableCollection<Packing2SupplierDto> _listSupplier;
        private readonly ObservableCollection<Packing2SupplierBrgDto> _listSupplierBrg;


        private readonly List<Packing2BrgDto> _listBrg;

        public Packing2Form(IWarehouseDal warehouseDal,
            IDriverDal driverDal,
            IFakturDal fakturDal,
            IFakturBuilder fakturBuilder,
            IBrgDal brgDal,
            ISupplierDal supplierDal, 
            IPackingBuilder builder, 
            IPackingWriter writer, 
            IBrowser<PackingBrowserView> fakturBrowser)
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _driverDal = driverDal;
            _fakturDal = fakturDal;
            _fakturBuilder = fakturBuilder;
            _brgDal = brgDal;
            _supplierDal = supplierDal;
            _builder = builder;
            _writer = writer;
            _fakturBrowser = fakturBrowser;

            _listFaktur = new ObservableCollection<Packing2FakturDto>();
            _listFakturSelected = new ObservableCollection<Packing2FakturDto>();
            _listFakturSelectedBrg = new ObservableCollection<Packing2FakturBrgDto>();
            _listSupplier = new ObservableCollection<Packing2SupplierDto>();
            _listSupplierBrg = new ObservableCollection<Packing2SupplierBrgDto>();
            _listBrg = new List<Packing2BrgDto>();

            InitComboBox();
            InitGrid();
            InitButton();
            InitTextBox();
        }

        private void InitComboBox()
        {
            WarehouseCombo.DataSource = _warehouseDal.ListData();
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";

            DriverCombo.DataSource = _driverDal.ListData();
            DriverCombo.DisplayMember = "DriverName";
            DriverCombo.ValueMember = "DriverId";
        }

        private void InitGrid()
        {
            //  Grid Faktur
            FakturGrid.DataSource = _listFaktur;
            FakturGrid.Style.CellStyle.Font.Facename = "Consolas";
            FakturGrid.Columns["FakturId"].Visible = false;
            FakturGrid.Columns["FakturCode"].Width = 90;
            FakturGrid.Columns["FakturDate"].Width = 90;
            FakturGrid.Columns["CustomerName"].Width = 225;
            FakturGrid.Columns["Address"].Width = 225;
            FakturGrid.Columns["Kota"].Width = 100;
            FakturGrid.Columns["GrandTotal"].Width = 100;
            FakturGrid.Columns["Pilih"].Width = 50;
            FakturGrid.Style.HeaderStyle.BackColor = Color.PowderBlue;
            FakturGrid.CellCheckBoxClick += FakturGrid_CellCheckBoxClick;
            
            //  Grid PerBrg-Faktur
            FakturSelectedGrid.DataSource = _listFakturSelected;
            FakturSelectedGrid.Style.CellStyle.Font.Facename = "Consolas";
            FakturSelectedGrid.Columns["FakturId"].Visible = false;
            FakturSelectedGrid.Columns["FakturCode"].Width = 90;
            FakturSelectedGrid.Columns["FakturDate"].Width = 90;
            FakturSelectedGrid.Columns["CustomerName"].Width = 225;
            FakturSelectedGrid.Columns["Address"].Width = 225;
            FakturSelectedGrid.Columns["Kota"].Width = 100;
            FakturSelectedGrid.Columns["GrandTotal"].Width = 100;
            FakturSelectedGrid.Columns["Pilih"].Width = 50;
            FakturSelectedGrid.Style.HeaderStyle.BackColor = Color.PowderBlue;
            FakturSelectedGrid.CellCheckBoxClick += FakturSelectedGrid_CellCheckBoxClick;
            FakturSelectedGrid.CurrentCellActivated += FakturSelectedGrid_CurrentCellActivated;

            FakturSelectedBrgGrid.DataSource = _listFakturSelectedBrg;
            FakturSelectedBrgGrid.Style.CellStyle.Font.Facename = "Consolas";
            FakturSelectedBrgGrid.Columns["BrgId"].Width = 90;
            FakturSelectedBrgGrid.Columns["BrgCode"].Width = 120;
            FakturSelectedBrgGrid.Columns["BrgName"].Width = 250;
            FakturSelectedBrgGrid.Columns["QtyBesar"].Width = 80;
            FakturSelectedBrgGrid.Columns["SatBesar"].Width = 50;
            FakturSelectedBrgGrid.Columns["QtyKecil"].Width = 80;
            FakturSelectedBrgGrid.Columns["SatKecil"].Width = 50;
            FakturSelectedBrgGrid.Columns["HargaJual"].Width = 120;            
            FakturSelectedBrgGrid.Style.HeaderStyle.BackColor = Color.PowderBlue;

            SupplierGrid.DataSource = _listSupplier;
            SupplierGrid.Style.CellStyle.Font.Facename = "Consolas";
            SupplierGrid.Columns["SupplierId"].Visible = false;
            SupplierGrid.Columns["SupplierName"].Width = 220;
            SupplierGrid.Columns["JumItem"].Width = 80;
            SupplierGrid.Style.HeaderStyle.BackColor = Color.PowderBlue;
            SupplierGrid.CurrentCellActivated += SupplierGrid_CurrentCellActivated;

            SupplierBrgGrid.DataSource = _listSupplierBrg;
            SupplierBrgGrid.Style.CellStyle.Font.Facename = "Consolas";
            SupplierBrgGrid.Columns["BrgId"].Visible = false;
            SupplierBrgGrid.Columns["BrgCode"].Width = 75;
            SupplierBrgGrid.Columns["BrgName"].Width = 200;
            SupplierBrgGrid.Columns["QtyBesar"].Width = 65;
            SupplierBrgGrid.Columns["SatBesar"].Width = 60;
            SupplierBrgGrid.Columns["QtyKecil"].Width = 65;
            SupplierBrgGrid.Columns["SatKecil"].Width = 60;
            SupplierBrgGrid.Columns["Faktur"].Width = 65;
            SupplierBrgGrid.Style.HeaderStyle.BackColor = Color.PowderBlue;
        }

        private void InitButton()
        {
            SearchButton.Click += SearchButton_Click;
            SaveButton.Click += SaveButton_Click;
            PackingIdButton.Click += PackingIdButton_Click;
            PreviewPerFakturButton.Click += PreviewFakturButton_Click;
            PreviewPerSupplier.Click += PreviewPerSupplierButton_Click;
        }

        private void InitTextBox()
        {
            PackingIdText.Validated += PackingIdText_Validated;
        }

        private void PackingIdText_Validated(object sender, EventArgs e)
        {
            PackingModel packing;
            if (PackingIdText.Text.Trim() == string.Empty)
                packing = _builder.Create().Build();
            else
                packing = _builder
                    .Load(new PackingModel(PackingIdText.Text))
                    .Build();

            PackingDateText.Value = packing.PackingDate;
            UserText.Text = packing.UserId;
            DeliveryDateText.Value = packing.DeliveryDate;
            WarehouseCombo.SelectedValue = packing.WarehouseId;
            DriverCombo.SelectedValue = packing.DriverId;
            Faktur1Date.Value = packing.TglAwalFaktur;
            Faktur2Date.Value = packing.TglAkhirFaktur;
            SearchText.Text = packing.KeywordSearch;
            
            _listFakturSelected.Clear();
            packing.ListFaktur.ForEach(x => _listFakturSelected.Add(new Packing2FakturDto
            {
                FakturId = x.FakturId,
                FakturCode = x.FakturCode,
                FakturDate = $"{x.FakturDate:dd-MMM HH:mm}",
                Address = x.Address,
                Kota = x.Kota,
                CustomerName = x.CustomerName,
                GrandTotal = x.GrandTotal,
                Pilih = true
            }));
            
            _listFaktur.Clear();
            packing.ListFaktur.ForEach(x => _listFaktur.Add(new Packing2FakturDto
            {
                FakturId = x.FakturId,
                FakturCode = x.FakturCode,
                FakturDate = $"{x.FakturDate:dd-MMM HH:mm}",
                Address = x.Address,
                Kota = x.Kota,
                CustomerName = x.CustomerName,
                GrandTotal = x.GrandTotal,
                Pilih = true
            }));
            
            _listBrg.Clear();
            packing.ListBrg.ForEach(x => _listBrg.Add(new Packing2BrgDto(
                x.FakturId, x.SupplierId, x.BrgId, x.BrgName,
                x.BrgCode, x.QtyBesar, x.SatBesar, x.QtyKecil, x.SatKecil,
                x.HargaJual)));
            
            UpdateSupplierGrid();
            UpdateStatusBar();
        }

        private void PackingIdButton_Click(object sender, EventArgs e)
        {
            _fakturBrowser.Filter.Date = new Periode(DateTime.Now);

            PackingIdText.Text = _fakturBrowser.Browse(PackingIdText.Text);
            PackingIdText_Validated(PackingIdText, null);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var packing = PackingIdText.Text.Trim() == string.Empty 
                ? _builder.Create().Build() 
                : _builder.Load(new PackingModel(PackingIdText.Text)).Build();
            var mainform = (MainForm)this.Parent.Parent;
            
            packing = _builder
                .Attach(packing)
                .User(new UserModel(mainform.UserId.UserId))
                .Warehouse(new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))
                .Driver(new DriverModel(DriverCombo.SelectedValue.ToString()))
                .DeliveryDate(DeliveryDateText.Value)
                .FilterFakturDate(new Periode(Faktur1Date.Value, Faktur2Date.Value))
                .Build();
            
            packing.ListFaktur.Clear();
            packing = _listFakturSelected
                .Aggregate(packing, (current, faktur) => _builder
                    .Attach(current)
                    .AddFaktur(new FakturModel(faktur.FakturId))
                    .Build());

            packing.ListBrg.Clear();
            packing = _listBrg
                .Aggregate(packing, (current, brg) => _builder
                    .Attach(current)
                    .AddBrg(brg, brg.QtyBesar, brg.SatBesar, brg.QtyKecil, brg.SatBesar, brg.HargaJual)
                    .Build());

            packing = _writer.Save(packing);
            LastIdStatusLabel.Text = packing.PackingId;
            ClearForm();
        }

        private void PreviewFakturButton_Click(object sender, EventArgs e)
        {
            var path = Path.GetTempPath();
            var fileName = $"{path}\\PackingListFaktur_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("PackingPerFaktur");
                var baris = 1;
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18"; 
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "LAPORAN PACKING LIST";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{DeliveryDateText.Value:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;
                baris++;

                ws.Cell($"A{baris}").Value = $"Driver";
                ws.Cell($"B{baris}").Value = $"{DriverCombo.Text}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);

                baris++;
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);
                baris++;
                var barisStart = baris;
                ws.Cell($"A{baris}").Value = "No";
                ws.Cell($"B{baris}").Value = "FakturNo";
                ws.Cell($"C{baris}").Value = "Nama Customer";
                ws.Cell($"D{baris}").Value = "Alamat";
                ws.Cell($"E{baris}").Value = "Total";
                ws.Cell($"F{baris}").Value = "Keterangan";
                baris++;
                var i = 1;
                var startTableRow = baris;
                foreach(var item in _listFakturSelected)
                {
                    ws.Cell($"A{baris}").Value = i;
                    ws.Cell($"B{baris}").Value = item.FakturCode;
                    ws.Cell($"C{baris}").Value = item.CustomerName;
                    ws.Cell($"D{baris}").Value = item.Address;
                    ws.Cell($"E{baris}").Value = item.GrandTotal;
                    ws.Cell($"F{baris}").Value = "";
                    baris++;
                    i++;
                }
                ws.Cell($"B{baris}").Value = $"Grand Total";
                ws.Cell($"E{baris}").Value = _listFakturSelected.Sum(x => x.GrandTotal);
                ws.Range(ws.Cell($"E{startTableRow}"), ws.Cell($"E{baris}"))
                    .Style.NumberFormat.Format = "#,##";
                
                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"F{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"F{barisStart}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetBold(true);

                ws.Column("A").Width = 8;
                ws.Column("B").Width = 12;
                ws.Column("C").Width = 30;
                ws.Column("D").Width = 35;
                ws.Column("F").Width = 25;
                workbook.SaveAs(fileName);
            }
            System.Diagnostics.Process.Start(fileName);
        }
        private void PreviewPerSupplierButton_Click(object sender, EventArgs e)
        {
            var path = Path.GetTempPath();
            var fileName = $"{path}\\PackingListSupplier_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("PackingPerSupplier");
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

                ws.Cell($"A{baris}").Value = $"{DeliveryDateText.Value:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Merge();
                baris++;
                baris++;

                ws.Cell($"A{baris}").Value = $"Driver";
                ws.Cell($"B{baris}").Value = $"{DriverCombo.Text}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"E{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);

                baris++;
                baris++;

                foreach(var item in _listSupplier)
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
                    var startTableRow = baris;
                    var listBrgThisSupplier = _listBrg
                        .Where(x => x.SupplierId == item.SupplierId)
                        .OrderBy(x => x.BrgCode)
                        .ToList(); 
                    foreach (var brg in listBrgThisSupplier)
                    {
                        ws.Cell($"A{baris}").Value = $"{brg.BrgCode}";
                        ws.Cell($"B{baris}").Value = $"{brg.BrgName}";
                        ws.Cell($"C{baris}").Value = $"{brg.QtyBesar:N0} {brg.SatBesar}";
                        ws.Cell($"D{baris}").Value = $"{brg.QtyKecil:N0} {brg.SatKecil}";
                        ws.Cell($"E{baris}").Value = brg.HargaJual;
                        baris++;
                    }
                    ws.Cell($"B{baris}").Value = $"Sub Total";
                    ws.Cell($"C{baris}").Value = listBrgThisSupplier.Sum(x => x.QtyBesar);
                    ws.Cell($"D{baris}").Value = $"";
                    ws.Cell($"E{baris}").Value = listBrgThisSupplier.Sum(x => x.HargaJual);
                    ws.Range(ws.Cell($"E{startTableRow}"), ws.Cell($"E{baris}"))
                        .Style.NumberFormat.Format = "#,##";
                    
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
                workbook.SaveAs(fileName);
            }
            System.Diagnostics.Process.Start(fileName);
        }
        
        
        private void ClearForm()
        {
            PackingIdText.Clear();
            PackingDateText.Value = DateTime.Now;
            UserText.Clear();
            DeliveryDateText.Value = DateTime.Now.AddDays(1);
            DriverCombo.SelectedIndex = 0;
            _listFaktur.Clear();
            _listFakturSelected.Clear();
            _listFakturSelectedBrg.Clear();
            _listSupplier.Clear();
            _listSupplierBrg.Clear();
            _listBrg.Clear();

            UpdateStatusBar();
            DriverCombo.Focus();
        }

        private void FakturGrid_CellCheckBoxClick(object sender, CellCheckBoxClickEventArgs e)
        {
            var row = e.RowIndex;
            var faktur = _listFaktur[row -1];
            if (e.NewValue == CheckState.Checked)
            {
                var newFaktur = faktur.Adapt<Packing2FakturDto>();
                newFaktur.Pilih = true;
                _listFakturSelected.Add(newFaktur);
                AddFakturToListBrg(faktur.FakturId);
            }
            else
            {
                var fakturSelected = _listFakturSelected.FirstOrDefault(x => x.FakturId == faktur.FakturId) ?? new Packing2FakturDto();
                _listFakturSelected.Remove(fakturSelected);
                RemoveFakturFromListBrg(fakturSelected.FakturId);
            }
        }
        
        private void FakturSelectedGrid_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            var row = e.DataRow.RowIndex;
            var fakturId = _listFakturSelected[row - 1].FakturId;
            var listBrg =
                from c in _listBrg
                where c.FakturId == fakturId
                select new Packing2FakturBrgDto(c.BrgId, c.BrgName, c.BrgCode,
                    c.QtyBesar, c.SatBesar, c.QtyKecil, c.SatKecil, c.HargaJual);

            _listFakturSelectedBrg.Clear();
            foreach (var item in listBrg)
            {
                _listFakturSelectedBrg.Add(item);
            }
        }

        private void FakturSelectedGrid_CellCheckBoxClick(object sender, CellCheckBoxClickEventArgs e)
        {
            var row = e.RowIndex;
            var fakturSelected = _listFakturSelected[row -1];
            var fakturId = fakturSelected.FakturId;
            foreach (var item in _listFakturSelected)
            {
                if (item.FakturId != fakturId) continue;
                _listFakturSelected.Remove(item);
                RemoveFakturFromListBrg(item.FakturId);
                break;
            }
            var faktur = _listFaktur.FirstOrDefault(x => x.FakturId == fakturId) ?? new Packing2FakturDto();
            faktur.Pilih = false;

        }

        private void AddFakturToListBrg(string fakturId)
        {
            var faktur = _fakturBuilder.Load(new FakturModel(fakturId)).Build();
            foreach(var item in faktur.ListItem)
            {
                var brg = _brgDal.GetData(new BrgModel(item.BrgId)) ?? new BrgModel { SupplierId = string.Empty};
                _listBrg.Add(new Packing2BrgDto(item.FakturId, brg.SupplierId, 
                    item.BrgId, item.BrgName, item.BrgCode, 
                    item.QtyBesar, item.SatBesar, 
                    item.QtyKecil, item.SatKecil, item.HrgSat));
            }
            UpdateSupplierGrid();
            UpdateStatusBar();
        }

        private void RemoveFakturFromListBrg(string fakturId)
        {
            _listBrg.RemoveAll(x => x.FakturId == fakturId);
            UpdateSupplierGrid();
            UpdateStatusBar();
        }

        private void SupplierGrid_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            var row = e.DataRow.RowIndex;
            var supplierId = _listSupplier[row - 1].SupplierId;

            var listBrg = (
                from c in _listBrg
                where c.SupplierId == supplierId
                group c by new { c.BrgId, c.BrgCode, c.BrgName, c.SatBesar, c.SatKecil } into g
                select new Packing2SupplierBrgDto
                {
                    BrgId = g.Key.BrgId,
                    BrgCode = g.Key.BrgCode,
                    BrgName = g.Key.BrgName,
                    SatBesar = g.Key.SatBesar,
                    SatKecil = g.Key.SatKecil,
                    QtyBesar = g.Sum(x => x.QtyBesar),
                    QtyKecil = g.Sum(x => x.QtyKecil),
                    Faktur = g.Select(x => x.FakturId).Distinct().Count()
                }).ToList();

            _listSupplierBrg.Clear();
            listBrg.ForEach(x => _listSupplierBrg.Add(x));
        }

        private void UpdateSupplierGrid()
        {
            var listAllSupplier = _supplierDal.ListData() ?? new List<SupplierModel>();
            var listSupplierGroupByBrg =
                from c in _listBrg
                join d in listAllSupplier on c.SupplierId equals d.SupplierId
                group new { c.SupplierId, d.SupplierName, c.BrgId, c.BrgName } 
                    by new { c.SupplierId, d.SupplierName } into g
                select new Packing2SupplierDto(g.Key.SupplierId,g.Key.SupplierName, g.Select(s => s.BrgId).Distinct().Count());

            _listSupplier.Clear();
            foreach(var item in listSupplierGroupByBrg)
                _listSupplier.Add(item);
        }

        private void UpdateStatusBar()
        {
            var jumFaktur = _listBrg.Select(x => x.FakturId).Distinct().Count();
            var jumItemBrg = _listBrg.Select(x => x.BrgId).Distinct().Count();
            var jumSupplier = _listBrg.Select(x => x.SupplierId).Distinct().Count();

            JumlahFakturStatusLabel.Text = $@"[Jumlah Faktur] {jumFaktur}";
            JumlahItemStatusLabel.Text = $@"Jumlah Item Brg: {jumItemBrg}";
            JumlahSupplierStatusLabel.Text = $@"Jumlah Supplier: {jumSupplier}";
        }
        
        private void SearchButton_Click(object sender, EventArgs e)
        {
            var periode = new Periode(Faktur1Date.Value, Faktur2Date.Value);
            var listFakturA = _fakturDal.ListData(periode)?.ToList() ?? new List<FakturModel>();
            _listFaktur.Clear();
            listFakturA.ForEach(x => _listFaktur.Add(new Packing2FakturDto
            {
                FakturId = x.FakturId,
                FakturDate = $"{x.FakturDate:dd-MMM HH:mm}",
                FakturCode = x.FakturCode,
                CustomerName = x.CustomerName,
                Address = x.Address,
                Kota = x.Kota,
                GrandTotal = x.GrandTotal,
                Pilih = _listFakturSelected.Any(y => y.FakturId == x.FakturId),
            }));
        }
    }
}
