using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.StokBalanceInfo;
using btr.distrib.InventoryContext.StokBrgSupplierRpt;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.DataSource;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;


namespace btr.distrib.InventoryContext.StokBalanceRpt
{
    public partial class StokBalanceInfo2Form : Form
    {
        private readonly IStokBalanceViewDal _stokBalanceViewDal;
        private List<StokBalanceInfoDto> _dataSource;

        public StokBalanceInfo2Form(IStokBalanceViewDal stokBalanceViewDal)
        {
            InitializeComponent();
            _stokBalanceViewDal = stokBalanceViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;
            InitGrid();
        }

        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = Color.PowderBlue;
            }
        }

        private void InitGrid()
        {
            var datasource = new List<StokBalanceInfoDto>();
            InfoGrid.DataSource = datasource;

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColNilaiSediaan = new GridSummaryColumnDescriptor("NilaiSediaan", SummaryType.DoubleAggregate, "NilaiSediaan", "{Sum}");
            sumColNilaiSediaan.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiSediaan.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiSediaan.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColNilaiSediaan });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["InPcs"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiSediaan"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.Refresh();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }
        
        private void Proses()
        {
            var listFaktur = _stokBalanceViewDal.ListData()?.ToList() ?? new List<StokBalanceView>();
            listFaktur.ForEach(x => x.NilaiSediaan = x.Hpp * x.Qty);
            var filtered = Filter(listFaktur, SearchText.Text);
            _dataSource = (
                from c in filtered
                select new StokBalanceInfoDto
                {
                    Supplier = c.SupplierName,
                    Kategori = c.KategoriName,
                    BrgId = c.BrgId,
                    BrgCode = c.BrgCode,
                    BrgName = c.BrgName,
                    Warehouse = c.WarehouseName,
                    SatBesar = c.SatBesar,
                    Conversion = c.Conversion,
                    SatKecil = c.SatKecil,
                    InPcs = c.Qty,
                    Hpp = c.Hpp,
                    NilaiSediaan = c.NilaiSediaan,
                }).ToList();
            InfoGrid.DataSource = _dataSource;
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
                saveFileDialog.FileName = $"stok-balance-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<StokBalanceInfoDto>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as StokBalanceInfoDto);
            }


            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("stok-balance-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  set format for  column  number 
                ws.Range(ws.Cell($"H{2}"), ws.Cell($"O{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0";
                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private List<StokBalanceView> Filter(List<StokBalanceView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listFilteredKategori = source.Where(x => x.KategoriName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredSupplier = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredBrgName
                .Union(listFilteredBrgCode)
                .Union(listFilteredKategori)
                .Union(listFilteredSupplier);
            return result.ToList();
        }
    }
}
