using btr.application.InventoryContext.KartuStokRpt;
using btr.distrib.InventoryContext.StokBalanceRpt;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms.Grid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using btr.nuna.Domain;
using btr.application.PurchaseContext.InvoiceHarianDetilRpt;
using ClosedXML.Excel;
using System.Linq;

namespace btr.distrib.InventoryContext.KartuStokRpt
{
    public partial class KartuStokSummaryForm : Form
    {
        private readonly BindingList<KartuStokSummaryDto> _brgList;
        private readonly BindingSource _brgBindingSource;
        private readonly IKartuStokSummaryDal _kartuStokSummaryDal;

        public KartuStokSummaryForm(IKartuStokSummaryDal kartuStokSummaryDal)
        {
            InitializeComponent();

            _brgList = new BindingList<KartuStokSummaryDto>();
            _brgBindingSource = new BindingSource(_brgList, null);
            _kartuStokSummaryDal = kartuStokSummaryDal;

            InitGrid();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
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
                saveFileDialog.FileName = $"kartu-stok-summary-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<KartuStokSummaryDto>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as KartuStokSummaryDto);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("kartu-stok-summary")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"M1")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  set format row data: font consolas 9, border medium, border inside hair
                ws.Range(ws.Cell("A2"), ws.Cell($"M{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Range(ws.Cell("A2"), ws.Cell($"M{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column  
                ws.Range(ws.Cell("H2"), ws.Cell($"M{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  format date column
                ws.Range(ws.Cell("C2"), ws.Cell($"C{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";

                //  format numeric column DiscTotal with 2 decimal places but hide zero
                ws.Range(ws.Cell("P2"), ws.Cell($"S{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.00_);(#,##0.00);-";

                //  set backcolor numeric column
                ws.Range(ws.Cell("H2"), ws.Cell($"L{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                ws.Range(ws.Cell("M2"), ws.Cell($"M{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightBlue);

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            var listBrg = _kartuStokSummaryDal
                .ListData(new Periode(PeriodeCalender.SelectionStart, PeriodeCalender.SelectionEnd))?.ToList() ?? new List<KartuStokSummaryDto>();
            listBrg.ForEach(x => x.QtyInPcs = x.Invoice + x.Faktur - x.Retur + x.Mutasi - x.Opname);
            //listBrg.ForEach(x => x.QtyBesar = x.Conversion != 1 ? x.QtyInPcs / x.Conversion : 0);
            //listBrg.ForEach(x => x.QtyKecil = x.QtyInPcs % x.Conversion);
            //listBrg.ForEach(x => x.SatBesar = x.Conversion != 1 ? x.Satuan: string.Empty);
            //listBrg.ForEach(x => x.Satuan = "PCS");


            InfoGrid.DataSource = listBrg;
            InfoGrid.Refresh();
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = _brgBindingSource;
            InfoGrid.Refresh();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }


            InfoGrid.TableDescriptor.Columns["Invoice"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Faktur"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Mutasi"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Opname"].Appearance.AnyRecordFieldCell.Format = "#,##";
        }

    }
}
