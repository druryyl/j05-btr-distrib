﻿using btr.application.SalesContext.FakturPerSupplierRpt;
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
using btr.application.PurchaseContext.InvoiceHarianDetilRpt;
using btr.infrastructure.PurchaseContext.InvoiceHarianDetilRpt;
using Syncfusion.Drawing;

namespace btr.distrib.PurchaseContext.InvoiceHarianDetilRpt
{
    public partial class InvoiceHarianDetilForm : Form
    {
        private readonly IInvoiceHarianDetilDal _invoiceHarianDetilDal;
        private List<InvoiceHarianDetilView> _dataSource;

        public InvoiceHarianDetilForm(IInvoiceHarianDetilDal invoiceHarianDetilDal)
        {
            InitializeComponent();
            _invoiceHarianDetilDal = invoiceHarianDetilDal;
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
            InfoGrid.TableDescriptor.Columns["HppSatBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HppSatKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyBonus"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["DiscProsen1"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen2"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen3"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["DiscProsen4"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["TotalDisc"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["TotalSebelumTax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";

            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SatBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HppSatBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SatKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HppSatKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
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
            var listStok = _invoiceHarianDetilDal.ListData(periode)?.ToList() ?? new List<InvoiceHarianDetilView>();

            var filtered = Filter(listStok, SearchText.Text);
            _dataSource = filtered.ToList();
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
                saveFileDialog.FileName = $"invoice-harian-detil-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<InvoiceHarianDetilView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as InvoiceHarianDetilView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("invoice-harian-detil-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"W1")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  set format row data: font consolas 9, border medium, border inside hair
                ws.Range(ws.Cell("A2"), ws.Cell($"W{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Range(ws.Cell("A2"), ws.Cell($"W{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column  
                ws.Range(ws.Cell("H2"), ws.Cell($"W{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column DiscTotal with 2 decimal places but hide zero
                ws.Range(ws.Cell("P2"), ws.Cell($"S{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.00_);(#,##0.00);-";

                //  set backcolor numeric column
                ws.Range(ws.Cell("H2"), ws.Cell($"O{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                ws.Range(ws.Cell("P2"), ws.Cell($"U{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightYellow);
                ws.Range(ws.Cell("V2"), ws.Cell($"W{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightMauve);

                //  add row footer: sum column NilaiStokBesar, NilaiStokKecil, NilaiInPcs
                ws.Cell($"O{listToExcel.Count + 2}").FormulaA1 = $"=SUM(O2:O{listToExcel.Count + 1})";
                ws.Cell($"T{listToExcel.Count + 2}").FormulaA1 = $"=SUM(T2:T{listToExcel.Count + 1})";
                ws.Cell($"U{listToExcel.Count + 2}").FormulaA1 = $"=SUM(U2:U{listToExcel.Count + 1})";
                ws.Cell($"V{listToExcel.Count + 2}").FormulaA1 = $"=SUM(V2:V{listToExcel.Count + 1})";
                ws.Cell($"W{listToExcel.Count + 2}").FormulaA1 = $"=SUM(W2:W{listToExcel.Count + 1})";

                //  format row footer font bold, background yellow, border medium
                ws.Range(ws.Cell($"O{listToExcel.Count + 2}"), ws.Cell($"W{listToExcel.Count + 2}")).Style
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

        private static IEnumerable<InvoiceHarianDetilView> Filter(IReadOnlyCollection<InvoiceHarianDetilView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listFilteredSupplier = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredBrgName
                .Union(listFilteredBrgCode)
                .Union(listFilteredSupplier);
            return result.ToList();
        }

        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}