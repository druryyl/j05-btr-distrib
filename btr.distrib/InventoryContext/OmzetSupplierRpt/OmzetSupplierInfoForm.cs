using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.OmzetSupplierInfo;
using Syncfusion.Drawing;
using Syncfusion.Windows.Forms.Diagram;

namespace btr.distrib.InventoryContext.OmzetSupplierRpt
{
    public partial class OmzetSupplierInfoForm : Form
    {
        private readonly IOmzetSupplierViewDal _omzetSupplierViewDal;
        private List<OmzetSupplierHarianDto> _dataSourceHarian;
        private List<OmzetSupplierBulananDto> _dataSourceBulanan;

        public OmzetSupplierInfoForm(IOmzetSupplierViewDal omzetSupplierViewDal)
        {
            InitializeComponent();
            _omzetSupplierViewDal = omzetSupplierViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;

            _dataSourceHarian = new List<OmzetSupplierHarianDto>();
            _dataSourceBulanan = new List<OmzetSupplierBulananDto>();

            HarianText.Mask = "00-0000";
            HarianText.ValidatingType = typeof(System.DateTime);
            HarianText.Text = $"{DateTime.Now.Month:D2}-{DateTime.Now.Year}";

            BulananText.Mask = "0000";
            BulananText.ValidatingType = typeof(System.DateTime);
            BulananText.Text = $"{DateTime.Now.Year}";

            InitGridHarian();
            InitGridBulanan();
            HarianRadio.Checked = true;

        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            //  export _dataSource to excel
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"omzet-per-supplier-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }
            if (HarianRadio.Checked)
                ExcelHarian(filePath);
            else
                ExcelBulanan(filePath);

        }

        private void ExcelBulanan(string filePath)
        {
            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Omzet-Per-Supplier")
                    .Cell($"B1")
                    .InsertTable(_dataSourceBulanan, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{_dataSourceBulanan.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{_dataSourceBulanan.Count + 1}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  set format number for column K, L, M, N to N0
                ws.Range(ws.Cell($"C{2}"), ws.Cell($"O{_dataSourceBulanan.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{_dataSourceBulanan.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < _dataSourceBulanan.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void ExcelHarian(string filePath)
        {
            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Omzet-Per-Supplier")
                    .Cell($"B1")
                    .InsertTable(_dataSourceHarian, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"AH{_dataSourceHarian.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"AH{_dataSourceHarian.Count + 1}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  set format number for column K, L, M, N to N0
                ws.Range(ws.Cell($"C{2}"), ws.Cell($"AH{_dataSourceHarian.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{_dataSourceHarian.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < _dataSourceHarian.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = Color.PowderBlue;
            }
        }

        private void InitGridHarian()
        {
            InfoGrid.DataSource = new List<OmzetSupplierHarianDto>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;

            var listSumCol = new List<GridSummaryColumnDescriptor>();
            for (var i = 1; i <= 31; i++ )
            {
                var colName = $"T{i:D2}";
                var sumColT = new GridSummaryColumnDescriptor(colName, SummaryType.DoubleAggregate, colName, "{Sum}");
                sumColT.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
                sumColT.Appearance.AnySummaryCell.Format = "N0";
                sumColT.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;
                InfoGrid.TableDescriptor.Columns[colName].Appearance.AnyRecordFieldCell.Format = "N0";
                listSumCol.Add(sumColT);
            }
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            foreach (var item in listSumCol)
            {
                sumRowDescriptor.SummaryColumns.Add(item);
            }
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            InfoGrid.Refresh();
            ProsesHarian();
        }

        private void InitGridBulanan()
        {
            InfoGridBulanan.DataSource = new List<OmzetSupplierBulananDto>();

            InfoGridBulanan.TableDescriptor.AllowEdit = false;
            InfoGridBulanan.TableDescriptor.AllowNew = false;
            InfoGridBulanan.TableDescriptor.AllowRemove = false;

            var listSumCol = new List<GridSummaryColumnDescriptor>();
            for (var i = 1; i <= 12; i++)
            {
                var colName = $"B{i:D2}";
                var sumColT = new GridSummaryColumnDescriptor(colName, SummaryType.DoubleAggregate, colName, "{Sum}");
                sumColT.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
                sumColT.Appearance.AnySummaryCell.Format = "N0";
                sumColT.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;
                InfoGridBulanan.TableDescriptor.Columns[colName].Appearance.AnyRecordFieldCell.Format = "N0";
                listSumCol.Add(sumColT);
            }
            InfoGridBulanan.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            foreach (var item in listSumCol)
            {
                sumRowDescriptor.SummaryColumns.Add(item);
            }
            InfoGridBulanan.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            InfoGridBulanan.Refresh();
            ProsesBulanan();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            if (HarianRadio.Checked)
                ProsesHarian();
            else
                ProsesBulanan();

        }

        private void ProsesBulanan()
        {
            var tgl1 = $"{BulananText.Text}-01-01".ToDate();
            var tgl2 = $"{BulananText.Text}-12-31".ToDate();
            var periode = new Periode(tgl1, tgl2);
            var listOmzet = _omzetSupplierViewDal.ListData(periode)?.ToList() ?? new List<OmzetSupplierView>();

            //  distinct list SupplierName in listOmzet
            var listSupplierName = listOmzet
                .Select(x => x.SupplierName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = listOmzet
                .GroupBy(x => x.SupplierName)
                .Select(x => new OmzetSupplierBulananDto
                {
                    SupplierName = x.Key,
                    B01 = x.Where(y => y.FakturDate.Month == 1).Sum(y => y.Total),
                    B02 = x.Where(y => y.FakturDate.Month == 2).Sum(y => y.Total),
                    B03 = x.Where(y => y.FakturDate.Month == 3).Sum(y => y.Total),
                    B04 = x.Where(y => y.FakturDate.Month == 4).Sum(y => y.Total),
                    B05 = x.Where(y => y.FakturDate.Month == 5).Sum(y => y.Total),
                    B06 = x.Where(y => y.FakturDate.Month == 6).Sum(y => y.Total),
                    B07 = x.Where(y => y.FakturDate.Month == 7).Sum(y => y.Total),
                    B08 = x.Where(y => y.FakturDate.Month == 8).Sum(y => y.Total),
                    B09 = x.Where(y => y.FakturDate.Month == 9).Sum(y => y.Total),
                    B10 = x.Where(y => y.FakturDate.Month == 10).Sum(y => y.Total),
                    B11 = x.Where(y => y.FakturDate.Month == 11).Sum(y => y.Total),
                    B12 = x.Where(y => y.FakturDate.Month == 12).Sum(y => y.Total),
                    Total = x.Sum(y => y.Total)
                })
                .ToList();

            _dataSourceBulanan = result;
            InfoGridBulanan.DataSource = _dataSourceBulanan;
        }

        private void ProsesHarian()
        {
            var bln = HarianText.Text.Substring(0, 2);
            var thn = HarianText.Text.Substring(3, 4);
            var tgl1 = $"{thn}-{bln}-01".ToDate();
            var maxDay = DateTime.DaysInMonth(tgl1.Year, tgl1.Month);
            var tgl2 = $"{thn}-{bln}-{maxDay}".ToDate();

            var periode = new Periode(tgl1, tgl2);
            var listOmzet = _omzetSupplierViewDal.ListData(periode)?.ToList() ?? new List<OmzetSupplierView>();
            //  distinct list SupplierName in listOmzet
            var listSupplierName = listOmzet
                .Select(x => x.SupplierName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = listOmzet
                .GroupBy(x => x.SupplierName)
                .Select(x => new OmzetSupplierHarianDto
                {
                    SupplierName = x.Key,
                    T01 = x.Where(y => y.FakturDate.Day == 1).Sum(y => y.Total),
                    T02 = x.Where(y => y.FakturDate.Day == 2).Sum(y => y.Total),
                    T03 = x.Where(y => y.FakturDate.Day == 3).Sum(y => y.Total),
                    T04 = x.Where(y => y.FakturDate.Day == 4).Sum(y => y.Total),
                    T05 = x.Where(y => y.FakturDate.Day == 5).Sum(y => y.Total),
                    T06 = x.Where(y => y.FakturDate.Day == 6).Sum(y => y.Total),
                    T07 = x.Where(y => y.FakturDate.Day == 7).Sum(y => y.Total),
                    T08 = x.Where(y => y.FakturDate.Day == 8).Sum(y => y.Total),
                    T09 = x.Where(y => y.FakturDate.Day == 9).Sum(y => y.Total),
                    T10 = x.Where(y => y.FakturDate.Day == 10).Sum(y => y.Total),
                    T11 = x.Where(y => y.FakturDate.Day == 11).Sum(y => y.Total),
                    T12 = x.Where(y => y.FakturDate.Day == 12).Sum(y => y.Total),
                    T13 = x.Where(y => y.FakturDate.Day == 13).Sum(y => y.Total),
                    T14 = x.Where(y => y.FakturDate.Day == 14).Sum(y => y.Total),
                    T15 = x.Where(y => y.FakturDate.Day == 15).Sum(y => y.Total),
                    T16 = x.Where(y => y.FakturDate.Day == 16).Sum(y => y.Total),
                    T17 = x.Where(y => y.FakturDate.Day == 17).Sum(y => y.Total),
                    T18 = x.Where(y => y.FakturDate.Day == 18).Sum(y => y.Total),
                    T19 = x.Where(y => y.FakturDate.Day == 19).Sum(y => y.Total),
                    T20 = x.Where(y => y.FakturDate.Day == 20).Sum(y => y.Total),
                    T21 = x.Where(y => y.FakturDate.Day == 21).Sum(y => y.Total),
                    T22 = x.Where(y => y.FakturDate.Day == 22).Sum(y => y.Total),
                    T23 = x.Where(y => y.FakturDate.Day == 23).Sum(y => y.Total),
                    T24 = x.Where(y => y.FakturDate.Day == 24).Sum(y => y.Total),
                    T25 = x.Where(y => y.FakturDate.Day == 25).Sum(y => y.Total),
                    T26 = x.Where(y => y.FakturDate.Day == 26).Sum(y => y.Total),
                    T27 = x.Where(y => y.FakturDate.Day == 27).Sum(y => y.Total),
                    T28 = x.Where(y => y.FakturDate.Day == 28).Sum(y => y.Total),
                    T29 = x.Where(y => y.FakturDate.Day == 29).Sum(y => y.Total),
                    T30 = x.Where(y => y.FakturDate.Day == 30).Sum(y => y.Total),
                    T31 = x.Where(y => y.FakturDate.Day == 31).Sum(y => y.Total),
                    Total = x.Sum(y => y.Total)
                })
                .ToList();
            
            _dataSourceHarian = result;
            InfoGrid.DataSource = _dataSourceHarian;
        }

        private void BulananRadio_CheckedChanged(object sender, EventArgs e)
        {
            InfoGrid.Visible = false;
            InfoGridBulanan.Visible = false;

            if (BulananRadio.Checked)
                InfoGridBulanan.Visible = true;
            else
                InfoGrid.Visible = true;
        }
    }
}
