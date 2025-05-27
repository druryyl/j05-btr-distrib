using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.PurchaseContext.InvoiceBrgInfo;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;

namespace btr.distrib.PurchaseContext.ReturBeliInfo
{
    public partial class ReturBeliBrgInfoForm : Form
    {
        private readonly IReturBeliBrgViewDal _returBeliBrgViewDal;
        private List<ReturBeliBrgViewDto> _dataSource;

        public ReturBeliBrgInfoForm(IReturBeliBrgViewDal fakturViewDal)
        {
            InitializeComponent();
            _returBeliBrgViewDal = fakturViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<ReturBeliBrgViewDto>();
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
                saveFileDialog.FileName = $"retur-beli-detil-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("returBeli-brg-info")
                    .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"Q{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"Q{_dataSource.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  set format number for columnto N0
                ws.Range(ws.Cell($"I{2}"), ws.Cell($"Q{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##.00";
                ws.Range(ws.Cell($"J{2}"), ws.Cell($"L{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##.00";
                ws.Range(ws.Cell($"D{2}"), ws.Cell($"D{_dataSource.Count + 1}"))
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
            InfoGrid.DataSource = new List<ReturBeliBrgViewDto>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColTotal = new GridSummaryColumnDescriptor("SubTotal", SummaryType.DoubleAggregate, "SubTotal", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColDiskon = new GridSummaryColumnDescriptor("Disc", SummaryType.DoubleAggregate, "Disc", "{Sum}");
            sumColDiskon.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColDiskon.Appearance.AnySummaryCell.Format = "N0";
            sumColDiskon.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumColTax = new GridSummaryColumnDescriptor("Tax", SummaryType.DoubleAggregate, "Tax", "{Sum}");
            sumColTax.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColTax.Appearance.AnySummaryCell.Format = "N0";
            sumColTax.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColGrandTot = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColGrandTot.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColGrandTot.Appearance.AnySummaryCell.Format = "N0";
            sumColGrandTot.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumColDiskon, sumColTax, sumColGrandTot });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Qty"].Appearance.AnyRecordFieldCell.Format = "N0";


            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Disc"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
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
            var listReturBeli = _returBeliBrgViewDal.ListData(periode)?.ToList() ?? new List<ReturBeliBrgViewDto>();
            _dataSource = Filter(listReturBeli, CustomerText.Text);
            _dataSource.ForEach(x => x.Tgl = x.Tgl.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private List<ReturBeliBrgViewDto> Filter(List<ReturBeliBrgViewDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredCustomer = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredNoReturBeli = source.Where(x => x.ReturBeliCode.ToLower() == keyword.ToLower()).ToList();


            var result = listFilteredCustomer
                .Union(listFilteredNoReturBeli);
            return result.ToList();
        }
    }
}
