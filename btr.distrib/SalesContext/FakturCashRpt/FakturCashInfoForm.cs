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
using btr.application.SalesContext.FakturCashRpt;

namespace btr.distrib.SalesContext.FakturCashRpt
{
    public partial class FakturCashInfoForm : Form
    {
        private readonly IFakturCashViewDal _fakturCashViewDal;
        private List<FakturCashView> _dataSource;

        public FakturCashInfoForm(IFakturCashViewDal fakturCashViewDal)
        {
            InitializeComponent();
            _fakturCashViewDal = fakturCashViewDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;
            InitGrid();
            _dataSource = new List<FakturCashView>();
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
                saveFileDialog.FileName = $"faktur-cash-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Faktu-Cash-Info")
                    .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"P{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"P{_dataSource.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  set format number for column K, L, M, N to N0
                ws.Range(ws.Cell($"K{2}"), ws.Cell($"P{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  set backcolor column K, L, M, N to lightpink
                ws.Range(ws.Cell($"K{2}"), ws.Cell($"N{_dataSource.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightPink);
                ws.Range(ws.Cell($"O{2}"), ws.Cell($"P{_dataSource.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                //  set backcolor header to light blue
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"P{1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightBlue);


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
            InfoGrid.DataSource = new List<FakturCashView>();

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

            var sumColCash = new GridSummaryColumnDescriptor("Cash", SummaryType.DoubleAggregate, "Cash", "{Sum}");
            sumColCash.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColCash.Appearance.AnySummaryCell.Format = "N0";
            sumColCash.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColKurangBayar = new GridSummaryColumnDescriptor("KurangBayar", SummaryType.DoubleAggregate, "KurangBayar", "{Sum}");
            sumColKurangBayar.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColKurangBayar.Appearance.AnySummaryCell.Format = "N0";
            sumColKurangBayar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumColDiskon, sumColTax, sumColGrandTot, sumColCash, sumColKurangBayar });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Discount"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Cash"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["KurangBayar"].Appearance.AnyRecordFieldCell.Format = "N0";

            InfoGrid.TableDescriptor.Columns["Tgl"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.Refresh();
            //Proses();
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
            var listFaktur = _fakturCashViewDal.ListData(periode)?.ToList() ?? new List<FakturCashView>();
            _dataSource = Filter(listFaktur, CustomerText.Text);
            _dataSource.ForEach(x => x.Tgl = x.Tgl.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private List<FakturCashView> Filter(List<FakturCashView> source, string keyword)
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
