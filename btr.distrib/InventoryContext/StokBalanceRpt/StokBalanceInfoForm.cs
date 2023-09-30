using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.StokBalanceRpt;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.StokBalanceRpt;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;

namespace btr.distrib.InventoryContext.StokBalanceRpt
{
    public partial class StokBalanceInfoForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private List<StokBalanceView> _dataSource;
        private readonly IStokBalanceReportDal _stokBalanceReportDal;
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<KategoriBrowserView> _kategoriBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IKategoriDal _kategoriDal;

        public StokBalanceInfoForm(IWarehouseDal warehouseDal,
            IStokBalanceReportDal stokBalanceReportDal,
            IBrowser<SupplierBrowserView> supplierBrowser,
            ISupplierDal supplierDal,
            IBrowser<KategoriBrowserView> kategoriBrowser,
            IKategoriDal kategoriDal)
        {
            InitializeComponent();

            _warehouseDal = warehouseDal;
            _stokBalanceReportDal = stokBalanceReportDal;

            InitGridWarehouse();
            InitGridResult();
            RegisterEventHandler();
            _supplierBrowser = supplierBrowser;
            _supplierDal = supplierDal;
            _kategoriBrowser = kategoriBrowser;
            _kategoriDal = kategoriDal;
        }

        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            ResultGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;

            SupplierButton.Click += SupplierButton_Click;
            KategoriButton.Click += KategoriButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"stok-balance-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Stok Balance")
                    .FirstCell()
                    .InsertTable(_dataSource, false);
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void KategoriButton_Click(object sender, EventArgs e)
        {
            _kategoriBrowser.Filter.UserKeyword = KategoriText.Text;
            var currentText = SupplierText.Text;
            var katId = _kategoriBrowser.Browse(currentText);
            var kat = _kategoriDal.GetData(new KategoriModel(katId));
            KategoriText.Text = kat?.KategoriName ?? currentText;
        }

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            _supplierBrowser.Filter.UserKeyword = SupplierText.Text;
            var currentText = SupplierText.Text;
            var supId = _supplierBrowser.Browse(currentText);
            var supplier = _supplierDal.GetData(new SupplierModel(supId));
            SupplierText.Text = supplier?.SupplierName ?? currentText;
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            _dataSource = _stokBalanceReportDal.ListData()?.ToList();

            //      filter supplier
            if (SupplierText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.SupplierName.ContainMultiWord(SupplierText.Text)).ToList();

            //      filter kategori
            if (KategoriText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.KategoriName.ContainMultiWord(KategoriText.Text)).ToList();

            //      filter brg name
            if (BrgText.Text.Length > 0)
                _dataSource = _dataSource
                    .Where(x => x.BrgName.ContainMultiWord(BrgText.Text) 
                             || x.BrgCode.ToLower().StartsWith(BrgText.Text.ToLower()))
                    .ToList();

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
                    c.Conversion,
                    c.Hpp
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
                    Hpp = g.Key.Hpp,
                    NilaiSediaan = g.Sum(x => x.Hpp * x.Qty) 
                }).ToList();

            foreach(var item in _dataSource)
            {
                if (item.SatBesar.Length == 0) 
                {
                    item.QtyKecil = item.QtyBesar;
                    item.QtyBesar = 0;
                }
            }
            _dataSource 
                = _dataSource
                .OrderBy(x => string.IsNullOrEmpty(x.SupplierName))
                .ThenBy(x => x.SupplierName)
                .ThenBy(x => string.IsNullOrEmpty(x.KategoriName))
                .ThenBy(x => x.KategoriName)
                .ThenBy(x => string.IsNullOrEmpty(x.BrgCode))
                .ThenBy(x => x.BrgCode)
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
            g.GetCol("QtyBesar").DefaultCellStyle.Format = "0;0; ";
            g.GetCol("QtyBesar").DefaultCellStyle.BackColor = Color.PaleGoldenrod;

            g.GetCol("QtyKecil").Width = 50;
            g.GetCol("QtyKecil").DefaultCellStyle.Format = "0;0; ";
            g.GetCol("QtyKecil").DefaultCellStyle.BackColor = Color.PaleGoldenrod;

            g.GetCol("Qty").Width = 50;
            g.GetCol("Qty").HeaderText = "In-PCS";
            g.GetCol("Qty").DefaultCellStyle.BackColor = Color.PaleGoldenrod;

            g.GetCol("Hpp").Width = 80;
            g.GetCol("Hpp").HeaderText = "Hpp";
            g.GetCol("Hpp").DefaultCellStyle.BackColor = Color.Thistle;

            g.GetCol("NilaiSediaan").Width = 80;
            g.GetCol("NilaiSediaan").HeaderText = "Nilai Sediaan";
            g.GetCol("NilaiSediaan").DefaultCellStyle.BackColor = Color.Thistle;

            g.GetCol("WarehouseId").Visible = false;
            g.GetCol("WarehouseName").Visible = false;

    }

    private void InitGridWarehouse()
        {
            var list = _warehouseDal.ListData()?.ToList()
                ?? new List<WarehouseModel>();
            var datasource = list
                .Select(x => new WarehouseRptDto
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
}
