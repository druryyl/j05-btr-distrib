using btr.application.SalesContext.FakturBrgInfo;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using btr.domain.BrgContext.BrgAgg;
using btr.application.FinanceContext.FpKeluaragAgg;

namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    public partial class FpKeluaranInfoForm : Form
    {
        private readonly IFpKeluaranViewDal _fpKeluaranViewDal;
        private List<FpKeluaranViewDto> _dataSource;

        public FpKeluaranInfoForm(IFpKeluaranViewDal fpKeluaranViewDal)
        {
            InitializeComponent();
            _fpKeluaranViewDal = fpKeluaranViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            _dataSource = new List<FpKeluaranViewDto>();

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
                saveFileDialog.FileName = $"faktur-pajak-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Faktu-Pajak-Info")
                    .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();

                //  set border and font
                ws.Range(ws.Cell("A1"), ws.Cell($"M{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Cell($"K{_dataSource.Count + 2}").Value = "Total";
                ws.Range(ws.Cell($"K{_dataSource.Count + 2}"), ws.Cell($"M{_dataSource.Count + 2}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(11)
                    .Font.SetBold();

                ws.Range(ws.Cell("A1"), ws.Cell($"M{_dataSource.Count + 2}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  sum total
                ws.Cell($"L{_dataSource.Count + 2}").FormulaA1 = $"=SUM(L2:L{_dataSource.Count + 1})";
                ws.Cell($"M{_dataSource.Count + 2}").FormulaA1 = $"=SUM(M2:M{_dataSource.Count + 1})";

                //  set number format
                ws.Range(ws.Cell("A2"), ws.Cell($"A{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("L2"), ws.Cell($"L{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("M2"), ws.Cell($"M{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("D2"), ws.Cell($"D{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";
                ws.Range(ws.Cell("G2"), ws.Cell($"F{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";


                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < _dataSource.Count; i++)
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
            InfoGrid.DataSource = new List<FpKeluaranViewDto>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColSubTotal = new GridSummaryColumnDescriptor("GrandTotal", SummaryType.DoubleAggregate, "GrandTotal", "{Sum}");
            sumColSubTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColSubTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColSubTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColDiskon = new GridSummaryColumnDescriptor("Ppn", SummaryType.DoubleAggregate, "Ppn", "{Sum}");
            sumColDiskon.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColDiskon.Appearance.AnySummaryCell.Format = "N0";
            sumColDiskon.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new[] { sumColSubTotal, sumColDiskon});
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Ppn"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["FakturDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.TableDescriptor.Columns["FpKeluaranDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

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
                MessageBox.Show(@"Periode informasi maximal 3 bulan");
                return;
            }
            var listFpKeluaran = _fpKeluaranViewDal.ListData(periode)?.ToList() ?? new List<FpKeluaranViewDto>();
            listFpKeluaran = listFpKeluaran
                .OrderBy(x => x.FakturDate.Date)
                .ToList();

            _dataSource = Filter(listFpKeluaran, CustomerText.Text);
            _dataSource.ForEach(x => x.FakturDate = x.FakturDate.Date);
            _dataSource.ForEach(x => x.FpKeluaranDate = x.FpKeluaranDate.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private static List<FpKeluaranViewDto> Filter(List<FpKeluaranViewDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredCustomerName = source.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredWpName = source.Where(x => x.NamaPembeli.ToLower().ContainMultiWord(keyword)).ToList();


            var result = listFilteredCustomerName
                .Union(listFilteredWpName);
            return result.ToList();
        }
    }

}
