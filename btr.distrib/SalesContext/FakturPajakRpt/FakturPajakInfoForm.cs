using btr.application.SalesContext.FakturInfo;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Drawing;
using btr.application.SalesContext.FakturPajakInfo;

namespace btr.distrib.SalesContext.FakturPajakRpt
{
    public partial class FakturPajakInfoForm : Form
    {
        private readonly IFakturPajakViewDal _fakturPajakViewDal;
        private List<FakturPajakView> _dataSource;

        public FakturPajakInfoForm(IFakturPajakViewDal fakturPajakViewDal)
        {
            InitializeComponent();
            _fakturPajakViewDal = fakturPajakViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;
            InitGrid();
            _dataSource = new List<FakturPajakView>();
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
                saveFileDialog.FileName = $"faktur-pajak-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<FakturPajakView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as FakturPajakView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("faktur-pajak-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"L1")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                ws.Range(ws.Cell("A2"), ws.Cell($"L{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Range(ws.Cell("A2"), ws.Cell($"L{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column  
                ws.Range(ws.Cell("G2"), ws.Cell($"J{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                
                //  format date columns  
                ws.Range(ws.Cell("D2"), ws.Cell($"D{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";

                //  set backcolor npwp dan no faktur pajak
                ws.Range(ws.Cell("K2"), ws.Cell($"L{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                //  set backcolor numeric column
                ws.Range(ws.Cell("G2"), ws.Cell($"J{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightBlue);

                //  add row footer: sum column NilaiStokBesar, NilaiStokKecil, NilaiInPcs
                ws.Cell($"G{listToExcel.Count + 2}").FormulaA1 = $"=SUM(G2:G{listToExcel.Count + 1})";
                ws.Cell($"H{listToExcel.Count + 2}").FormulaA1 = $"=SUM(H2:H{listToExcel.Count + 1})";
                ws.Cell($"I{listToExcel.Count + 2}").FormulaA1 = $"=SUM(I2:I{listToExcel.Count + 1})";
                ws.Cell($"J{listToExcel.Count + 2}").FormulaA1 = $"=SUM(J2:J{listToExcel.Count + 1})";

                //  format row footer font bold, background yellow, border medium
                ws.Range(ws.Cell($"G{listToExcel.Count + 2}"), ws.Cell($"J{listToExcel.Count + 2}")).Style
                    .Font.SetFontName("Concolas")
                    .Font.SetBold()
                    .NumberFormat.SetFormat("#,##")
                    .Fill.SetBackgroundColor(XLColor.Yellow)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

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
            InfoGrid.DataSource = new List<FakturPajakView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
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
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumColDiskon, sumColTax, sumColGrandTot });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Discount"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tgl"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
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
            var listFaktur = _fakturPajakViewDal.ListData(periode)?.ToList() ?? new List<FakturPajakView>();
            _dataSource = Filter(listFaktur, CustomerText.Text);
            _dataSource.ForEach(x => x.Tgl = x.Tgl.Date);
            InfoGrid.DataSource = _dataSource;
            InfoGrid.Refresh();
        }

        private List<FakturPajakView> Filter(List<FakturPajakView> source, string keyword)
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
