using btr.application.InventoryContext.StokBalanceReport;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.StokBalanceReport;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.InventoryInfo
{
    public partial class StokBalanceInfoForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private List<StokBalanceView> _dataSource;
        private readonly IStokBalanceReportDal _stokBalanceReportDal;

        public StokBalanceInfoForm(IWarehouseDal warehouseDal, 
            IStokBalanceReportDal stokBalanceReportDal)
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _stokBalanceReportDal = stokBalanceReportDal;

            InitGridWarehouse();
            InitGridResult();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ResultGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            _dataSource = _stokBalanceReportDal.ListData()?.ToList();

            if (SupplierText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.SupplierName.ContainMultiWord(SupplierText.Text)).ToList();

            if (KategoriText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.KategoriName.ContainMultiWord(KategoriText.Text)).ToList();

            if (BrgText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.BrgName.ContainMultiWord(BrgText.Text)).ToList();

            _dataSource = (
                from c in _dataSource
                group c by new
                {
                    c.BrgId,
                    c.BrgCode,
                    c.BrgName,
                    c.SupplierId,
                    c.SupplierName,
                    c.KategoriId,
                    c.KategoriName,
                    c.SatBesar,
                    c.SatKecil,
                    c.Conversion
                } into g
                select new StokBalanceView
                {
                    BrgId = g.Key.BrgId,
                    BrgCode = g.Key.BrgCode,
                    BrgName = g.Key.BrgName,
                    SupplierId = g.Key.SupplierId,
                    SupplierName = g.Key.SupplierName,
                    KategoriId = g.Key.KategoriId,
                    KategoriName = g.Key.KategoriName,
                    SatBesar = g.Key.SatBesar,
                    SatKecil = g.Key.SatKecil,
                    Conversion = g.Key.Conversion,
                    Qty = g.Sum(x => x.Qty),
                    QtyBesar = (int)(g.Sum(x => x.Qty)/g.Key.Conversion),
                    QtyKecil = g.Sum(x => x.Qty % g.Key.Conversion),
                }).ToList();
            foreach(var item in _dataSource)
            {
                if (item.SatBesar.Length == 0) 
                {
                    item.QtyKecil = item.QtyBesar;
                    item.QtyBesar = 0;
                }
            }
            //_dataSource.RemoveAll(x => x.BrgCode.Length == 0);
            _dataSource = _dataSource
                .OrderBy(x => x.BrgCode)
                .ToList();


            ResultGrid.DataSource = _dataSource;
            ResultGrid.Refresh();

        }

        private void InitGridResult()
        {
            if (_dataSource is null)
                _dataSource = new List<StokBalanceView>();

            ResultGrid.DataSource = _dataSource;
            ResultGrid.Refresh();
            ResultGrid.Columns.SetDefaultCellStyle(Color.AliceBlue);
            var g = ResultGrid.Columns;
            g.GetCol("BrgId").Width = 50;
            g.GetCol("BrgCode").Width = 60;
            g.GetCol("BrgName").Width = 180;
            
            g.GetCol("SupplierId").Visible = false;
            g.GetCol("SupplierName").Width = 150;
            g.GetCol("SupplierName").HeaderText = "Supplier";

            g.GetCol("KategoriId").Visible= false;
            g.GetCol("KategoriName").Width = 100;
            g.GetCol("KategoriName").HeaderText= "Kategori";

            g.GetCol("SatBesar").Width = 50;
            g.GetCol("SatKecil").Width = 50;
            g.GetCol("Conversion").Visible= false;
            
            g.GetCol("QtyBesar").Width = 50;
            g.GetCol("QtyKecil").Width = 50;

            g.GetCol("Qty").Width = 50;
            g.GetCol("Qty").HeaderText = "In-PCS";


            g.GetCol("WarehouseId").Visible = false;
            g.GetCol("WarehouseName").Visible = false;

    }

    private void InitGridWarehouse()
        {
            var list = _warehouseDal.ListData()?.ToList()
                ?? new List<WarehouseModel>();
            var datasource = list
                .Select(x => new WarehouseDto
                {
                    Id = x.WarehouseId,
                    IsPilih = true,
                    Name = x.WarehouseName
                }).ToList();
            WarehouseGrid.DataSource = datasource;
            WarehouseGrid.Columns.SetDefaultCellStyle(Color.Beige);
            var g = WarehouseGrid.Columns;
            g.GetCol("Id").Visible = false;
            g.GetCol("Id").Width = 25;
            g.GetCol("IsPilih").HeaderText = "Pilih";
            g.GetCol("IsPilih").Width = 25;
            g.GetCol("Name").Width = 140;
            WarehouseGrid.Refresh();

        }
    }

    internal class WarehouseDto
    {
        public string Id { get; set; }
        public bool IsPilih { get; set; }
        public string Name { get; set; }
    }
}
