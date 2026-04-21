using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.SalesContext.FakturInfo;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Mapster;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace btr.distrib.SalesContext.FakturInfoRpt
{
    public partial class FakturInfoForm : Form
    {
        private readonly IFakturViewDal _fakturViewDal;
        private List<FakturView> _dataSource;

        public FakturInfoForm(IFakturViewDal fakturViewDal)
        {
            InitializeComponent();
            _fakturViewDal = fakturViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<FakturView>();
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            //  export listToExcel to excel
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"faktur-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<FakturView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as FakturView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var excelDs = new List<FakturView>();
                GridTable table = InfoGrid.Table;
                foreach (var record in table.FilteredRecords)
                {
                    var item = record.GetData();
                    var strData = record.ToString().Substring(28);
                    var listProp = strData.Split(',');
                    var fakturId = listProp[0].Substring(11,13);
                    var faktur = listToExcel.FirstOrDefault(x => x.FakturId == fakturId);
                    excelDs.Add(faktur);
                }
                // insert table starting at row 3 so we can add two title rows above
                wb.AddWorksheet("Faktu-Info")
                    .Cell($"B3")
                    .InsertTable(excelDs, false);

                var ws = wb.Worksheets.First();

                // add titles in first and second row
                ws.Cell($"A1").Value = "INFO FAKTUR JUAL";
                ws.Cell($"A2").Value = $"Periode : {Tgl1Date.Value:dd-MM-yyyy} s/d {Tgl2Date.Value:dd-MM-yyyy}";

                // merge first 5 columns for the two title rows
                ws.Range("A1:E1").Merge();
                ws.Range("A2:E2").Merge();

                // style titles: first title bold and larger font, center align both rows
                ws.Range("A1:E1").Style.Font.SetBold();
                ws.Range("A1:E1").Style.Font.SetFontSize(14);
                ws.Range("A1:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("A1:E2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // get the full table range including the 'No' column 'A'
                var lastCol = ws.LastColumnUsed().ColumnNumber();
                var tableRange = ws.Range(3, 1, excelDs.Count + 3, lastCol);
                var headerRange = ws.Range(3, 1, 3, lastCol);
                var dataRange = ws.Range(4, 1, excelDs.Count + 3, lastCol);

                //  set border and font (adjusted for two title rows)
                tableRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                tableRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                tableRange.Style.Border.SetOutsideBorderColor(XLColor.Gray);
                tableRange.Style.Border.SetInsideBorderColor(XLColor.LightGray);

                tableRange.Style.Font.SetFontName("Calibri");
                tableRange.Style.Font.SetFontSize(10);
                ws.Range($"C1:F{excelDs.Count+3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range($"S1:S{excelDs.Count + 3}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // style header row
                headerRange.Style.Font.SetBold();
                headerRange.Style.Fill.SetBackgroundColor(XLColor.FromArgb(178,178,178));
                headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                headerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //  hide columns O (P in closedxml 1-based index)
                ws.Column("R").Hide();

                //  set format number
                ws.Range(ws.Cell($"N{4}"), ws.Cell($"Q{excelDs.Count + 3}"))
                    .Style.NumberFormat.Format = "#,##0";
                ws.Range(ws.Cell($"A{4}"), ws.Cell($"A{excelDs.Count + 3}"))
                    .Style.NumberFormat.Format = "#,##0";
                ws.Range(ws.Cell($"D{4}"), ws.Cell($"D{excelDs.Count + 3}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";
                ws.Range(ws.Cell($"E{4}"), ws.Cell($"E{excelDs.Count + 3}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";

                //  add rownumbering (header is now at row 3)
                ws.Cell($"A3").Value = "No";
                for (var i = 0; i < excelDs.Count; i++)
                    ws.Cell($"A{i + 4}").Value = i + 1;

                //  replace status TRUE dengan string "YA""
                for (var i = 0; i < excelDs.Count; i++)
                {
                    var cellValue = ws.Cell($"S{i + 4}").Value.ToString();
                    if (cellValue == "TRUE")
                        ws.Cell($"S{i + 4}").Value = "YA";
                    else
                        ws.Cell($"S{i + 4}").Value = "";
                }
                ws.Columns().AdjustToContents();
                tableRange.Style.Font.SetFontSize(10);
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
            InfoGrid.DataSource = new List<FakturView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }
            InfoGrid.TableDescriptor.VisibleColumns.Remove("StatusFaktur");

            var sumColTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTotal.Appearance.AnySummaryCell.Format= "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColDiskon = new GridSummaryColumnDescriptor("Discount", SummaryType.DoubleAggregate, "Discount", "{Sum}");
            sumColDiskon.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColDiskon.Appearance.AnySummaryCell.Format = "N0";
            sumColDiskon.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;
            

            var sumColTax = new GridSummaryColumnDescriptor("Tax", SummaryType.DoubleAggregate, "Tax", "{Sum}");
            sumColTax.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTax.Appearance.AnySummaryCell.Format = "N0";
            sumColTax.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColGrandTot = new GridSummaryColumnDescriptor("GrandTotal", SummaryType.DoubleAggregate, "GrandTotal", "{Sum}");
            sumColGrandTot.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColGrandTot.Appearance.AnySummaryCell.Format = "N0";
            sumColGrandTot.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumColDiskon, sumColTax, sumColGrandTot});
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Discount"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tgl"].Appearance.AnyRecordFieldCell.Format= "dd-MMM-yyyy";
            InfoGrid.TableDescriptor.Columns["JatuhTempo"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
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
            List<FakturView> listFaktur;
            if (FakturTerhapusCheck.Checked == false)
                listFaktur = _fakturViewDal.ListData(periode)?.ToList() ?? new List<FakturView>();
            else
                listFaktur = _fakturViewDal.ListTerhapus(periode)?.ToList() ?? new List<FakturView>();

            _dataSource = Filter(listFaktur, CustomerText.Text);
            _dataSource.ForEach(x => x.Tgl = x.Tgl.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private List<FakturView> Filter(List<FakturView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredCustomer = source.Where(x => x.Customer.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredAddress = source.Where(x => x.Address.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredNoFaktur = source.Where(x => x.FakturCode.ToLower() == keyword.ToLower()).ToList();


            var result = listFilteredCustomer
                .Union(listFilteredNoFaktur)
                .Union(listFilteredAddress);
            return result.ToList();
        }
    }
}
