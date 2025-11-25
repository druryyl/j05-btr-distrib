using btr.application.SalesContext.OrderFeature;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Syncfusion.Drawing;
using Syncfusion.Grouping;

namespace btr.distrib.SalesContext.SalesPersonAgg
{
    public partial class SalesOmzetInfoForm : Form
    {
        private readonly ISalesOmzetDal _salesOmzetDal;
        private List<SalesOmzetView> _dataSource;

        public SalesOmzetInfoForm(ISalesOmzetDal salesOmzetDal)
        {
            InitializeComponent();
            _salesOmzetDal = salesOmzetDal;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<SalesOmzetView>();
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            // Export _dataSource to excel
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"sales-omzet-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<SalesOmzetView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as SalesOmzetView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("sales-omzet");

                // Create header row
                ws.Cell("A1").Value = "No";
                ws.Cell("B1").Value = "Sales Name";
                ws.Cell("C1").Value = "Order Date";
                ws.Cell("D1").Value = "Order Total";
                ws.Cell("E1").Value = "Invoice Code";
                ws.Cell("F1").Value = "Invoice Date";
                ws.Cell("G1").Value = "Invoice Total";
                ws.Cell("H1").Value = "Omzet Date";
                ws.Cell("I1").Value = "Order ID";

                // Fill data rows
                for (var i = 0; i < listToExcel.Count; i++)
                {
                    var omzet = listToExcel[i];
                    var row = i + 2;

                    ws.Cell($"A{row}").Value = i + 1;
                    ws.Cell($"B{row}").Value = omzet.SalesPersonName;
                    ws.Cell($"C{row}").Value = omzet.OrderDate;
                    ws.Cell($"D{row}").Value = omzet.OrderTotal;
                    ws.Cell($"E{row}").Value = omzet.FakturCode;
                    ws.Cell($"F{row}").Value = omzet.FakturDate;
                    ws.Cell($"G{row}").Value = omzet.FakturTotal;
                    ws.Cell($"H{row}").Value = omzet.OmzetDate;
                    ws.Cell($"I{row}").Value = omzet.OrderId;
                }

                // Apply styling to the entire data range
                var dataRange = ws.Range(ws.Cell($"A1"), ws.Cell($"I{listToExcel.Count + 1}"));
                dataRange.Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                // Format header row
                var headerRange = ws.Range(ws.Cell($"A1"), ws.Cell($"I1"));
                headerRange.Style
                    .Font.Bold = true;

                // Format numeric columns
                var orderTotalRange = ws.Range(ws.Cell($"D2"), ws.Cell($"D{listToExcel.Count + 1}"));
                var fakturTotalRange = ws.Range(ws.Cell($"G2"), ws.Cell($"G{listToExcel.Count + 1}"));
                orderTotalRange.Style.NumberFormat.Format = "#,##0";
                fakturTotalRange.Style.NumberFormat.Format = "#,##0";

                // Format date columns
                var dateRange = ws.Range(ws.Cell($"C2"), ws.Cell($"C{listToExcel.Count + 1}"));
                var fakturDateRange = ws.Range(ws.Cell($"F2"), ws.Cell($"F{listToExcel.Count + 1}"));
                var omzetDateRange = ws.Range(ws.Cell($"H2"), ws.Cell($"H{listToExcel.Count + 1}"));
                dateRange.Style.NumberFormat.Format = "dd-MMM-yyyy";
                fakturDateRange.Style.NumberFormat.Format = "dd-MMM-yyyy";
                omzetDateRange.Style.NumberFormat.Format = "dd-MMM-yyyy";

                // Auto-fit all columns
                ws.Columns().AdjustToContents();

                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(filePath);
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<SalesOmzetView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            // Configure column appearances
            InfoGrid.TableDescriptor.Columns["OrderTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["OrderTotal"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            InfoGrid.TableDescriptor.Columns["FakturTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["FakturTotal"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            InfoGrid.TableDescriptor.Columns["OrderDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.TableDescriptor.Columns["FakturDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.TableDescriptor.Columns["OmzetDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

            // Set column widths for better readability
            InfoGrid.TableDescriptor.Columns["SalesPersonName"].Width = 150;
            InfoGrid.TableDescriptor.Columns["OrderTotal"].Width = 100;
            InfoGrid.TableDescriptor.Columns["FakturCode"].Width = 120;
            InfoGrid.TableDescriptor.Columns["FakturTotal"].Width = 100;
            InfoGrid.TableDescriptor.Columns["OrderId"].Width = 120;

            // Summary rows for totals
            var sumColOrderTotal = new GridSummaryColumnDescriptor("OrderTotal", SummaryType.DoubleAggregate, "OrderTotal", "{Sum}");
            sumColOrderTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColOrderTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColOrderTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColFakturTotal = new GridSummaryColumnDescriptor("FakturTotal", SummaryType.DoubleAggregate, "FakturTotal", "{Sum}");
            sumColFakturTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColFakturTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColFakturTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColOrderTotal, sumColFakturTotal });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            //// Optional: Add grouping by SalesName
            //InfoGrid.TableDescriptor.Columns["SalesName"].AllowGroup = true;

            // For all records field cells
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.AutoSize = true;
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.WrapText = false;

            InfoGrid.Refresh();
            Proses();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var tgl1 = Tgl1Date.Value;
            var tgl2 = Tgl2Date.Value;
            var periode = new Periode(tgl1, tgl2);
            var timeSpan = tgl2 - tgl1;
            var dayCount = timeSpan.Days;

            if (dayCount > 122)
            {
                MessageBox.Show("Periode informasi maximal 3 bulan");
                return;
            }

            var listOmzet = _salesOmzetDal.ListData(periode)?.ToList() ?? new List<SalesOmzetView>();
            _dataSource = Filter(listOmzet, SearchText.Text);
            InfoGrid.DataSource = _dataSource;
        }

        private List<SalesOmzetView> Filter(List<SalesOmzetView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var keywordLower = keyword.ToLower();
            var listFilteredSales = source.Where(x =>
                x.SalesPersonName.ToLower().ContainMultiWord(keywordLower)).ToList();

            var listFilteredFaktur = source.Where(x =>
                x.FakturCode.ToLower().Contains(keywordLower)).ToList();

            var listFilteredOrder = source.Where(x =>
                x.OrderId.ToLower().Contains(keywordLower)).ToList();

            var result = listFilteredSales
                .Union(listFilteredFaktur)
                .Union(listFilteredOrder);

            return result.ToList();
        }

        // Optional: Style grouping headers
        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = Color.PowderBlue;
            }
        }
    }
}
