using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.nuna.Domain;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using Mapster;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;

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

        private readonly ObservableCollection<Packing2FakturDto> _listFaktur;
        private readonly ObservableCollection<Packing2FakturDto> _listFakturSelected;
        private readonly ObservableCollection<Packing2FakturBrgDto> _listFakturSelectedBrg;
        private readonly ObservableCollection<Packing2SupplierDto> _listSupplier;
        private readonly ObservableCollection<Packing2SupplierBrgDto> _listSupplierBrg;


        private readonly List<Packing2AllBrgSupplierDto> _listAllBrg;

        public Packing2Form(IWarehouseDal warehouseDal,
            IDriverDal driverDal,
            IFakturDal fakturDal,
            IFakturBuilder fakturBuilder,
            IBrgDal brgDal,
            ISupplierDal supplierDal)
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _driverDal = driverDal;
            _fakturDal = fakturDal;
            _fakturBuilder = fakturBuilder;

            _listFaktur = new ObservableCollection<Packing2FakturDto>();
            _listFakturSelected = new ObservableCollection<Packing2FakturDto>();
            _listFakturSelectedBrg = new ObservableCollection<Packing2FakturBrgDto>();
            _listSupplier = new ObservableCollection<Packing2SupplierDto>();
            _listSupplierBrg = new ObservableCollection<Packing2SupplierBrgDto>();
            _listAllBrg = new List<Packing2AllBrgSupplierDto>();

            InitComboBox();
            InitGrid();
            InitEventHandler();
            _brgDal = brgDal;
            _supplierDal = supplierDal;
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

        private void SupplierGrid_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            var row = e.DataRow.RowIndex;
            var supplierId = _listSupplier[row - 1].SupplierId;

            var listBrg = (
                from c in _listAllBrg
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

        private void InitEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
        }

        private void FakturSelectedGrid_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            var row = e.DataRow.RowIndex;
            var fakturId = _listFakturSelected[row - 1].FakturId;
            var listBrg =
                from c in _listAllBrg
                where c.FakturId == fakturId
                select new Packing2FakturBrgDto(c.BrgId, c.BrgName, c.BrgCode,
                    c.QtyBesar, c.SatBesar, c.QtyKecil, c.SatKecil, c.HargaJual);


            var faktur = _fakturBuilder.Load(new FakturModel(fakturId)).Build();
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
                RemoveFakturFromAllBrg(item.FakturId);
                break;
            }
            var faktur = _listFaktur.FirstOrDefault(x => x.FakturId == fakturId) ?? new Packing2FakturDto();
            faktur.Pilih = false;

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
                AddFakturToAllBrg(faktur.FakturId);
            }
            else
            {
                var fakturSelected = _listFakturSelected.FirstOrDefault(x => x.FakturId == faktur.FakturId);
                _listFakturSelected.Remove(fakturSelected);
                RemoveFakturFromAllBrg(fakturSelected.FakturId);
            }
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
                Pilih = _listFakturSelected.Any(y => y.FakturId == x.FakturId) ? true : false,
            }));
        }

        private void AddFakturToAllBrg(string fakturId)
        {
            var faktur = _fakturBuilder.Load(new FakturModel(fakturId)).Build();
            foreach(var item in faktur.ListItem)
            {
                var brg = _brgDal.GetData(new BrgModel(item.BrgId)) ?? new BrgModel { SupplierId = string.Empty};
                _listAllBrg.Add(new Packing2AllBrgSupplierDto(item.FakturId, brg.SupplierId, 
                    item.BrgId, item.BrgName, item.BrgCode, 
                    item.QtyBesar, item.SatBesar, 
                    item.QtyKecil, item.SatKecil, item.HrgSat));
            }
            UpdateSupplierGrid();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            var jumFaktur = _listAllBrg.Select(x => x.FakturId).Distinct().Count();
            var jumItemBrg = _listAllBrg.Select(x => x.BrgId).Distinct().Count();
            var jumSupplier = _listAllBrg.Select(x => x.SupplierId).Distinct().Count();

            JumlahFakturStatusLabel.Text = $"Jumlah Faktur: {jumFaktur}";
            JumlahItemStatusLabel.Text = $"Jumlah Item Brg: {jumItemBrg}";
            JumlahSupplierStatusLabel.Text = $"Jumlah Supplier: {jumSupplier}";
        }

        private void RemoveFakturFromAllBrg(string fakturId)
        {
            _listAllBrg.RemoveAll(x => x.FakturId == fakturId);
            UpdateSupplierGrid();
            UpdateStatusBar();
        }

        private void UpdateSupplierGrid()
        {
            var listAllSupplier = _supplierDal.ListData() ?? new List<SupplierModel>();
            var listSupplierGroupByBrg =
                from c in _listAllBrg
                join d in listAllSupplier on c.SupplierId equals d.SupplierId
                group new { c.SupplierId, d.SupplierName, c.BrgId, c.BrgName } 
                by new { c.SupplierId, d.SupplierName } into g
                select new Packing2SupplierDto(g.Key.SupplierId,g.Key.SupplierName, g.Select(s => s.BrgId).Distinct().Count());

            _listSupplier.Clear();
            foreach(var item in listSupplierGroupByBrg)
                _listSupplier.Add(item);
        }
    }

    internal class Packing2FakturDto
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public string FakturDate { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Kota { get; set; }
        public decimal GrandTotal { get; set; }
        public bool Pilih { get; set; }
    }

    internal class Packing2FakturBrgDto
    {
        public Packing2FakturBrgDto(string id, string name, string code,
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

    internal class Packing2SupplierDto
    {
        public Packing2SupplierDto(string id, string name, int jumItem)
        {
            SupplierId = id;
            SupplierName = name;
            JumItem = jumItem;
        }
        public string SupplierId { get; private set; }  
        public string SupplierName { get; private set; }
        public int JumItem { get; private set; }
    }

    internal class Packing2AllBrgSupplierDto
    {
        public Packing2AllBrgSupplierDto(string fakturId, 
            string supplierId,  string brgId, string brgName, string code,
            int qtyBesar, string satBesar, int qtyKecil, string satKecil,
            decimal hargaJual)
        {
            FakturId = fakturId;
            SupplierId = supplierId;
            BrgId = brgId;
            BrgName = brgName;
            BrgCode = code;
            QtyBesar = qtyBesar;
            SatBesar = satBesar;
            QtyKecil = qtyKecil;
            SatKecil = satKecil;
            HargaJual = hargaJual;
        }

        public string FakturId { get; set; }
        public string SupplierId { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HargaJual { get; private set; }
    }

    internal class Packing2SupplierBrgDto
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public int QtyBesar { get;  set; }
        public string SatBesar { get;  set; }
        public int QtyKecil { get;  set; }
        public string SatKecil { get;  set; }
        public int Faktur { get;  set; }

    }

}
