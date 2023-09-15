using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.PackingAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.DriverAgg;
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

namespace btr.distrib.InventoryContext.PackingAgg
{
    public partial class PackingForm : Form
    {
        private readonly IFakturDal _fakturDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IPackingBuilder _packingBuilder;
        private readonly IDriverDal _driverDal;
        private readonly IPackingWriter _packingWriter;

        public PackingForm(IFakturDal fakturDal,
            IWarehouseDal warehouseDal,
            IPackingBuilder packingBuilder,
            IDriverDal driverDal,
            IPackingWriter packingWriter)
        {
            InitializeComponent();
            _fakturDal = fakturDal;
            _warehouseDal = warehouseDal;
            _packingBuilder = packingBuilder;
            _driverDal = driverDal;


            InitWarehouse();
            InitDriver();
            InitFakturKiriGrid();
            RegisterEventHandler();
            _packingWriter = packingWriter;
        }

        private void RegisterEventHandler()
        {
            WarehouseCombo.SelectedValueChanged += WarehouseCombo_SelectedValueChanged;
            FakturKiriGrid.CellDoubleClick += FakturKiriGrid_CellDoubleClick;
        }

        private void FakturKiriGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            var driverKey = new DriverModel(DriverCombo.SelectedValue.ToString());
            var warehouseKey = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var deliveryDate = DeliveryDate.Value;
            var fakturKey = new FakturModel(grid.Rows[e.RowIndex].Cells["FakturId"].Value.ToString());
            var packing = _packingBuilder
                .LoadOrCreate(driverKey, deliveryDate)
                .Warehouse(warehouseKey)
                .AddFaktur(fakturKey)
                .GenSupplier()
                .Build();
            _ = _packingWriter.Save(packing);
        }

        private void WarehouseCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            RefreshFakturKiri();
        }

        private void InitWarehouse()
        {
            var listWarehouse = _warehouseDal.ListData() ?? new List<WarehouseModel>();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }
        private void InitDriver()
        {
            var listDriver = _driverDal.ListData() ?? new List<DriverModel>();
            DriverCombo.DataSource = listDriver;
            DriverCombo.DisplayMember = "DriverName";
            DriverCombo.ValueMember = "DriverId";
        }
        private void InitFakturKiriGrid()
        {
            RefreshFakturKiri();
            FakturKiriGrid.Columns.SetDefaultCellStyle(Color.PowderBlue);
            var g = FakturKiriGrid.Columns;
            g.GetCol("FakturId").Visible = false;
            g.GetCol("FakturCode").Width = 60;
            g.GetCol("FakturCode").HeaderText = "Code";
            g.GetCol("CustomerName").Width = 80;
            g.GetCol("CustomerName").HeaderText = "Customer";

            g.GetCol("Address").Width = 120;
            g.GetCol("Kota").Width = 70;
            g.GetCol("Driver").Width = 60;
            g.GetCol("GrandTotal").Width = 70;
        }

        private void RefreshFakturKiri()
        {
            var periode = new Periode(Faktur1Date.Value, Faktur2Date.Value);
            var listFaktur = _fakturDal.ListData(periode) ?? new List<FakturModel>();

            var dataSource = (
                from c in listFaktur
                where c.WarehouseId == WarehouseCombo.SelectedValue.ToString()
                select c.Adapt<PackingFakturDto>()
                ).ToList();
            FakturKiriGrid.DataSource = dataSource;
            FakturKiriGrid.Refresh();
        }
    }

    public class PackingFakturDto
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Address { get; private set; }
        public string Kota { get; private set; }
        public string Driver { get; private set; }
        public decimal GrandTotal { get; private set; }
    }
}
