using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.nuna.Domain;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using Mapster;
using Syncfusion.DataSource;

namespace btr.distrib.InventoryContext.PackingAgg
{
    public partial class Packing2Form : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDriverDal _driverDal;
        private readonly IFakturDal _fakturDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly ObservableCollection<Packing2FakturDto> _listFaktur = new ObservableCollection<Packing2FakturDto>();
        private readonly ObservableCollection<Packing2FakturDto> _listFakturSelected = new ObservableCollection<Packing2FakturDto>();
        private readonly ObservableCollection<Packing2FakturBrgDto> _listFakturSelectedBrg =
            new ObservableCollection<Packing2FakturBrgDto>();
        
        public Packing2Form(IWarehouseDal warehouseDal,
            IDriverDal driverDal,
            IFakturDal fakturDal, 
            IFakturBuilder fakturBuilder)
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _driverDal = driverDal;
            _fakturDal = fakturDal;
            _fakturBuilder = fakturBuilder;

            InitComboBox();
            InitGrid();            
            InitEventHandler();
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
            ListFakturGrid.DataSource = _listFaktur;
            ListFakturGrid.Style.CellStyle.Font.Facename = "Consolas";
            ListFakturGrid.Columns["FakturId"].Visible = false;
            ListFakturGrid.Columns["FakturCode"].Width = 90;
            ListFakturGrid.Columns["FakturDate"].Width = 90;
            ListFakturGrid.Columns["CustomerName"].Width = 225;
            ListFakturGrid.Columns["Address"].Width = 225;
            ListFakturGrid.Columns["Kota"].Width = 100;
            ListFakturGrid.Columns["GrandTotal"].Width = 100;
            ListFakturGrid.Columns["Pilih"].Width = 50;
            ListFakturGrid.CellCheckBoxClick += ListFakturGrid_CellCheckBoxClick;
            
            //  Grid PerBrg-Faktur
            PerFakturAtasGrid.DataSource = _listFakturSelected;
            PerFakturAtasGrid.Style.CellStyle.Font.Facename = "Consolas";
            PerFakturAtasGrid.Columns["FakturId"].Visible = false;
            PerFakturAtasGrid.Columns["FakturCode"].Width = 90;
            PerFakturAtasGrid.Columns["FakturDate"].Width = 90;
            PerFakturAtasGrid.Columns["CustomerName"].Width = 225;
            PerFakturAtasGrid.Columns["Address"].Width = 225;
            PerFakturAtasGrid.Columns["Kota"].Width = 100;
            PerFakturAtasGrid.Columns["GrandTotal"].Width = 100;
            PerFakturAtasGrid.Columns["Pilih"].Width = 50;
            PerFakturAtasGrid.CellCheckBoxClick += PerFakturAtasGrid_CellCheckBoxClick;
            PerFakturAtasGrid.CurrentCellActivated += PerFakturAtasGrid_CurrentCellActivated;

            PerFakturBawahGrid.DataSource = _listFakturSelectedBrg;
            PerFakturBawahGrid.Style.CellStyle.Font.Facename = "Consolas";
            PerFakturBawahGrid.Columns["BrgId"].Width = 90;
            PerFakturBawahGrid.Columns["BrgCode"].Width = 120;
            PerFakturBawahGrid.Columns["BrgName"].Width = 250;
            PerFakturBawahGrid.Columns["QtyBesar"].Width = 80;
            PerFakturBawahGrid.Columns["SatBesar"].Width = 50;
            PerFakturBawahGrid.Columns["QtyKecil"].Width = 80;
            PerFakturBawahGrid.Columns["SatKecil"].Width = 50;
            PerFakturBawahGrid.Columns["HargaJual"].Width = 120;            
            
        }

        private void PerFakturAtasGrid_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            var row = e.DataRow.RowIndex;
            var fakturId = _listFakturSelected[row - 1].FakturId;
            var faktur = _fakturBuilder.Load(new FakturModel(fakturId)).Build();
            _listFakturSelectedBrg.Clear();
            foreach (var item in faktur.ListItem)
            {
                _listFakturSelectedBrg.Add(new Packing2FakturBrgDto(item.BrgId, item.BrgName, item.BrgCode, 
                    item.QtyBesar, item.SatBesar, item.QtyKecil, item.SatKecil, item.HrgSat));
            }

        }


        private void PerFakturAtasGrid_CellCheckBoxClick(object sender, CellCheckBoxClickEventArgs e)
        {
            var row = e.RowIndex;
            var fakturSelected = _listFakturSelected[row -1];
            var fakturId = fakturSelected.FakturId;
            foreach (var item in _listFakturSelected)
            {
                if (item.FakturId != fakturId) continue;
                _listFakturSelected.Remove(item);
                break;
            }
            var faktur = _listFaktur.FirstOrDefault(x => x.FakturId == fakturId) ?? new Packing2FakturDto();
            faktur.Pilih = false;

        }

        private void InitEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
        }

        private void ListFakturGrid_CellCheckBoxClick(object sender, CellCheckBoxClickEventArgs e)
        {
            var row = e.RowIndex;
            var faktur = _listFaktur[row -1];
            if (e.NewValue == CheckState.Checked)
            {
                var newFaktur = faktur.Adapt<Packing2FakturDto>();
                newFaktur.Pilih = true;
                _listFakturSelected.Add(newFaktur);
            }
            else
            {
                var fakturSelected = _listFakturSelected.FirstOrDefault(x => x.FakturId == faktur.FakturId);
                _listFakturSelected.Remove(fakturSelected);
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
                Pilih = false,
            }));
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

}
