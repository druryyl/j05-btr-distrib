using btr.application.BrgContext.BrgAgg;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.application.SalesContext.FakturBrgInfo;
using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.ReturJualRpt
{
    public partial class ReturJualReportForm : Form
    {
        private readonly IReturJualViewDal _returViewDal;
        private List<ReturJualView> _dataSource;

        public ReturJualReportForm(IReturJualViewDal returViewDal)
        {
            InitializeComponent();
            _returViewDal = returViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            _dataSource = new List<ReturJualView>();

            InitGrid();
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
                saveFileDialog.FileName = $"retur-jual-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }
            
            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<ReturJualView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as ReturJualView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Retur-Jual-Info")
                    .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set border and font
                ws.Range(ws.Cell("A1"), ws.Cell($"O{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Cell($"K{listToExcel.Count + 2}").Value = "Total";
                ws.Range(ws.Cell($"K{listToExcel.Count + 2}"), ws.Cell($"O{listToExcel.Count + 2}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(11)
                    .Font.SetBold();

                ws.Range(ws.Cell("A1"), ws.Cell($"Y{listToExcel.Count + 2}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  add row total V,W,X,Y
                ws.Cell($"L{listToExcel.Count + 2}").FormulaA1 = $"=SUM(L2:L{listToExcel.Count + 1})";
                ws.Cell($"M{listToExcel.Count + 2}").FormulaA1 = $"=SUM(M2:M{listToExcel.Count + 1})";
                ws.Cell($"N{listToExcel.Count + 2}").FormulaA1 = $"=SUM(N2:N{listToExcel.Count + 1})";
                ws.Cell($"O{listToExcel.Count + 2}").FormulaA1 = $"=SUM(O2:O{listToExcel.Count + 1})";

                //  set format number for column A, J, K, L, M, N, O to N0
                ws.Range(ws.Cell("L2"), ws.Cell($"O{listToExcel.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("A2"), ws.Cell($"A{listToExcel.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("D2"), ws.Cell($"D{listToExcel.Count + 2}")).Style.NumberFormat.Format = "dd-MMM-yyyy";


                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
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

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<ReturJualView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColSubTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColSubTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColSubTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColSubTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColDiskon = new GridSummaryColumnDescriptor("DiscRp", SummaryType.DoubleAggregate, "DiscRp", "{Sum}");
            sumColDiskon.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColDiskon.Appearance.AnySummaryCell.Format = "N0";
            sumColDiskon.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumColTax = new GridSummaryColumnDescriptor("PpnRp", SummaryType.DoubleAggregate, "PpnRp", "{Sum}");
            sumColTax.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTax.Appearance.AnySummaryCell.Format = "N0";
            sumColTax.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTotal = new GridSummaryColumnDescriptor("GrandTotal", SummaryType.DoubleAggregate, "GrandTotal", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new[] { sumColSubTotal, sumColDiskon, sumColTax, sumColTotal });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["DiscRp"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["ReturJualDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

            InfoGrid.Refresh();
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
                MessageBox.Show(@"Periode informasi maximal 3 bulan");
                return;
            }
            var listRetur = _returViewDal.ListData(periode)?.ToList() ?? new List<ReturJualView>();
            listRetur = listRetur
                .OrderBy(x => x.ReturJualDate.Date)
                .ToList();

            _dataSource = Filter(listRetur, SearchText.Text);
            _dataSource.ForEach(x => x.ReturJualDate = x.ReturJualDate.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private static List<ReturJualView> Filter(List<ReturJualView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            keyword = keyword.ToLower().Trim();
            var listFilteredCustomer = source.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredAddress = source.Where(x => x.CustomerCode.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredSales = source.Where(x => x.SalesName.ToLower().ContainMultiWord(keyword)).ToList();
            var listReturJualCode = source.Where(x => x.ReturJualCode.ToLower().StartsWith(keyword)).ToList();

            var result = listFilteredCustomer
                .Union(listFilteredAddress)
                .Union(listFilteredSales)
                .Union(listReturJualCode);
            return result.ToList();
        }
    }
}
