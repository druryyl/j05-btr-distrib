using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.SalesContext.FakturBrgInfo;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturInfoRpt
{
    public partial class FakturBrgInfoForm : Form
    {
        private readonly IFakturBrgViewDal _fakturBrgViewDal;
        private List<FakturBrgView> _dataSource;

        public FakturBrgInfoForm(IFakturBrgViewDal fakturBrgViewDal)
        {
            InitializeComponent();
            _fakturBrgViewDal = fakturBrgViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;

            InitGrid();
            _dataSource = new List<FakturBrgView>();
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
                saveFileDialog.FileName = $"faktur-brg-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<FakturBrgView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as FakturBrgView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Faktu-Brg-Info")
                    .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set border and font
                ws.Range(ws.Cell("A1"), ws.Cell($"P{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Cell($"L{listToExcel.Count + 2}").Value= "Total";
                ws.Range(ws.Cell($"L{listToExcel.Count + 2}"), ws.Cell($"P{listToExcel.Count + 2}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(11)
                    .Font.SetBold();

                ws.Range(ws.Cell("A1"), ws.Cell($"P{listToExcel.Count + 2}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  add row total L,M,N,O
                ws.Cell($"M{listToExcel.Count + 2}").FormulaA1 = $"=SUM(M2:M{listToExcel.Count + 1})";
                ws.Cell($"N{listToExcel.Count + 2}").FormulaA1 = $"=SUM(N2:N{listToExcel.Count + 1})";
                ws.Cell($"O{listToExcel.Count + 2}").FormulaA1 = $"=SUM(O2:O{listToExcel.Count + 1})";
                ws.Cell($"P{listToExcel.Count + 2}").FormulaA1 = $"=SUM(P2:P{listToExcel.Count + 1})";

                //  set format number for column A, J, K, L, M, N, O to N0
                ws.Range(ws.Cell("L2"), ws.Cell($"Q{listToExcel.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("A2"), ws.Cell($"A{listToExcel.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("D2"), ws.Cell($"D{listToExcel.Count + 2}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";

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
            InfoGrid.DataSource = new List<FakturBrgView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColSubTotal = new GridSummaryColumnDescriptor("SubTotal", SummaryType.DoubleAggregate, "SubTotal", "{Sum}");
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

            var sumColTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new[] { sumColSubTotal, sumColDiskon, sumColTax, sumColTotal });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyJual"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HrgSat"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["DiscRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["FakturDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
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
            var listFaktur = _fakturBrgViewDal.ListData(periode)?.ToList() ?? new List<FakturBrgView>();
            listFaktur = listFaktur
                .OrderBy(x => x.FakturDate.Date)
                .ThenBy(x => x.FakturCode)
                .ToList();
            _dataSource = Filter(listFaktur, CustomerText.Text);
            _dataSource.ForEach(x => x.FakturDate= x.FakturDate.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private static List<FakturBrgView> Filter(List<FakturBrgView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredCustomer = source.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredAddress = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredNoFaktur = source.Where(x => x.FakturCode.ToLower() == keyword.ToLower()).ToList();


            var result = listFilteredCustomer
                .Union(listFilteredNoFaktur)
                .Union(listFilteredAddress);
            return result.ToList();
        }
    }
}
