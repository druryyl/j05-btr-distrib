using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.nuna.Domain;
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
using Syncfusion.Grouping;
using Syncfusion.Drawing;
using btr.distrib.InventoryContext.StokBalanceRpt;
using ClosedXML.Excel;

namespace btr.distrib.FinanceContext
{
    public partial class PelunasanInfoForm : Form
    {
        private List<PelunasanInfoDto> _listContent;
        private readonly IPelunasanInfoDal _contentDal;

        public PelunasanInfoForm(IPelunasanInfoDal contentDal)
        {
            InitializeComponent();

            _contentDal = contentDal;

            RegisterEventHandler();
            InitGrid();
        }
        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }
        private void Proses()
        {
            _listContent = _contentDal.ListData(new Periode(Periode1Date.Value, Periode2Date.Value))?.ToList() 
                ?? new List<PelunasanInfoDto>();
            InfoGrid.DataSource = _listContent;
            InfoGrid.Refresh();
            InfoGrid.Table.ExpandAllGroups();
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


            var sumColBayarTunai = new GridSummaryColumnDescriptor("Cash", SummaryType.DoubleAggregate, "Cash", "{Sum}");
            sumColBayarTunai.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColBayarTunai.Appearance.AnySummaryCell.Format = "N2";
            sumColBayarTunai.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColBayarGiro = new GridSummaryColumnDescriptor("BgTransfer", SummaryType.DoubleAggregate, "BgTransfer", "{Sum}");
            sumColBayarGiro.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColBayarGiro.Appearance.AnySummaryCell.Format = "N2";
            sumColBayarGiro.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { 
                sumColBayarTunai,
                sumColBayarGiro});
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["TotalFaktur"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["Potongan"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["Materai"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["BiayaAdmin"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["Cash"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["BgTransfer"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            InfoGrid.TableDescriptor.Columns["Sisa"].Appearance.AnyRecordFieldCell.Format = "#,###.00";
            
            InfoGrid.TableDescriptor.GroupedColumns.Add("SalesPersonName");
            
            InfoGrid.Refresh();
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
                saveFileDialog.FileName = $"pelunasan-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<PelunasanInfoDto>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as PelunasanInfoDto);
            }


            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("pelunasan-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"Q{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"Q{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  set format for  column  number 
                ws.Range(ws.Cell($"H{2}"), ws.Cell($"Q{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##.00";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"N{2}"), ws.Cell($"N{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "dd MMM yyyy";

                ws.Range(ws.Cell($"H{2}"), ws.Cell($"L{listToExcel.Count + 1}"))
                    .Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range(ws.Cell($"O{2}"), ws.Cell($"Q{listToExcel.Count + 1}"))
                    .Style.Fill.BackgroundColor = XLColor.LightPink;
                ws.Range(ws.Cell($"B{1}"), ws.Cell($"Q{1}"))
                    .Style.Fill.BackgroundColor = XLColor.LightBlue;


                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }
    }
}
