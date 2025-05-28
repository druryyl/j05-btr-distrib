using btr.application.SalesContext.FakturPerCustomerRpt;
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
using Syncfusion.Drawing;

namespace btr.distrib.SalesContext.FakturPerCustomerRpt
{
    public partial class FakturPerCustomerForm : Form
    {
        private readonly IFakturPerCustomerDal _fakturPerCustomerDal;
        private List<FakturPerCustomerView> _dataSource;

        public FakturPerCustomerForm(IFakturPerCustomerDal fakturPerCustomerDal)
        {
            InitializeComponent();
            _fakturPerCustomerDal = fakturPerCustomerDal;
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
            Proses();

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

            var sumColSubTotal = new GridSummaryColumnDescriptor("SubTotal", SummaryType.DoubleAggregate, "SubTotal", "{Sum}");
            sumColSubTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColSubTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColSubTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTotalDisc = new GridSummaryColumnDescriptor("TotalDisc", SummaryType.DoubleAggregate, "TotalDisc", "{Sum}");
            sumColTotalDisc.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColTotalDisc.Appearance.AnySummaryCell.Format = "N0";
            sumColTotalDisc.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTotalSebelumTax = new GridSummaryColumnDescriptor("TotalSebelumTax", SummaryType.DoubleAggregate, "TotalSebelumTax", "{Sum}");
            sumColTotalSebelumTax.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColTotalSebelumTax.Appearance.AnySummaryCell.Format = "N0";
            sumColTotalSebelumTax.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColPpnRp = new GridSummaryColumnDescriptor("PpnRp", SummaryType.DoubleAggregate, "PpnRp", "{Sum}");
            sumColPpnRp.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColPpnRp.Appearance.AnySummaryCell.Format = "N0";
            sumColPpnRp.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColSubTotal, sumColTotalDisc, sumColTotalSebelumTax, sumColPpnRp, sumColTotal });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HrgSatBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HrgSatKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyPotStok"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["DiscProsen1"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen2"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen3"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen4"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["TotalDisc"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["TotalSebelumTax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            
            //  format column FaturDate and DueDate to dd MMM yyyy
            InfoGrid.TableDescriptor.Columns["FakturDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.TableDescriptor.Columns["DueDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SatBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HrgSatBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SatKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HrgSatKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["QtyPotStok"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["QtyBonus"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;

            InfoGrid.TableDescriptor.Columns["DiscProsen1"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["DiscProsen2"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["DiscProsen3"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["DiscProsen4"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["TotalDisc"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["TotalSebelumTax"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;

            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.BackColor = Color.LightBlue;
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.BackColor = Color.LightBlue;

            InfoGrid.Refresh();
            Proses();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var periode = new Periode(Tgl1Date.Value, Tgl2Date.Value);
            var listStok = _fakturPerCustomerDal.ListData(periode)?.ToList() ?? new List<FakturPerCustomerView>();

            var filtered = Filter(listStok, SearchText.Text).ToList();
            filtered.ForEach(x => x.FakturDate = x.FakturDate.Date);
            _dataSource = filtered;
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
                saveFileDialog.FileName = $"faktur-per-customer-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<FakturPerCustomerView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as FakturPerCustomerView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("faktur-per-customer-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"AF1")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  set format row data: font Lucida Console 9, border medium, border inside hair
                ws.Range(ws.Cell("A2"), ws.Cell($"AF{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Range(ws.Cell("A2"), ws.Cell($"AF{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column  
                ws.Range(ws.Cell("P2"), ws.Cell($"AF{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format date column to dd MMM yyyy
                ws.Range(ws.Cell("C2"), ws.Cell($"C{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";
                ws.Range(ws.Cell("D2"), ws.Cell($"D{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";
                
                //  format numeric column DiscTotal with 2 decimal places but hide zero
                ws.Range(ws.Cell("Y2"), ws.Cell($"AB{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.00_);(#,##0.00);-";

                //  set backcolor numeric column
                ws.Range(ws.Cell("P2"), ws.Cell($"X{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                ws.Range(ws.Cell("Y2"), ws.Cell($"AD{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightYellow);
                ws.Range(ws.Cell("AE2"), ws.Cell($"AF{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightMauve);

                //  add row footer: sum column NilaiStokBesar, NilaiStokKecil, NilaiInPcs
                ws.Cell($"X{listToExcel.Count + 2}").FormulaA1 = $"=SUM(X2:X{listToExcel.Count + 1})";
                ws.Cell($"AC{listToExcel.Count + 2}").FormulaA1 = $"=SUM(AC2:AC{listToExcel.Count + 1})";
                ws.Cell($"AD{listToExcel.Count + 2}").FormulaA1 = $"=SUM(AD2:AD{listToExcel.Count + 1})";
                ws.Cell($"AE{listToExcel.Count + 2}").FormulaA1 = $"=SUM(AE2:AE{listToExcel.Count + 1})";
                ws.Cell($"AF{listToExcel.Count + 2}").FormulaA1 = $"=SUM(AF2:AF{listToExcel.Count + 1})";

                //  format row footer font bold, background yellow, border medium
                ws.Range(ws.Cell($"S{listToExcel.Count + 2}"), ws.Cell($"AF{listToExcel.Count + 2}")).Style
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

        private static IEnumerable<FakturPerCustomerView> Filter(IReadOnlyCollection<FakturPerCustomerView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listFilteredSupplier = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredCustomer = source.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredFakturCode = source.Where(x => x.FakturCode.ToLower().StartsWith(keyword.ToLower())).ToList();

            var result = listFilteredBrgName
                .Union(listFilteredBrgCode)
                .Union(listFilteredSupplier)
                .Union(listFilteredCustomer)
                .Union(listFilteredFakturCode);
            return result.ToList();
        }
    }
}
