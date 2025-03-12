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
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;

namespace btr.distrib.InventoryContext.ReturJualRpt
{
    public partial class ReturJualBrgReportForm : Form
    {
        private readonly IReturJualBrgViewDal _returBrgViewDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private List<ReturJualBrgView> _dataSource;

        public ReturJualBrgReportForm(IReturJualBrgViewDal returBrgViewDal, 
            IBrgSatuanDal brgSatuanDal)
        {
            InitializeComponent();
            _returBrgViewDal = returBrgViewDal;
            _brgSatuanDal = brgSatuanDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            _dataSource = new List<ReturJualBrgView>();

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
                saveFileDialog.FileName = $"retur-jual-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Retur-Jual-Info")
                    .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();

                //  set border and font
                ws.Range(ws.Cell("A1"), ws.Cell($"Y{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Cell($"U{_dataSource.Count + 2}").Value = "Total";
                ws.Range(ws.Cell($"U{_dataSource.Count + 2}"), ws.Cell($"Y{_dataSource.Count + 2}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(11)
                    .Font.SetBold();

                ws.Range(ws.Cell("A1"), ws.Cell($"Y{_dataSource.Count + 2}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  add row total V,W,X,Y
                ws.Cell($"V{_dataSource.Count + 2}").FormulaA1 = $"=SUM(W2:V{_dataSource.Count + 1})";
                ws.Cell($"W{_dataSource.Count + 2}").FormulaA1 = $"=SUM(W2:W{_dataSource.Count + 1})";
                ws.Cell($"X{_dataSource.Count + 2}").FormulaA1 = $"=SUM(X2:X{_dataSource.Count + 1})";
                ws.Cell($"Y{_dataSource.Count + 2}").FormulaA1 = $"=SUM(Y2:Y{_dataSource.Count + 1})";

                //  set format number for column A, J, K, L, M, N, O to N0
                ws.Range(ws.Cell("J2"), ws.Cell($"Y{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("A2"), ws.Cell($"A{_dataSource.Count + 2}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("D2"), ws.Cell($"D{_dataSource.Count + 2}"))
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
            InfoGrid.DataSource = new List<ReturJualBrgView>();

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

            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["InPcs"].Appearance.AnyRecordFieldCell.Format = "N0";//
            InfoGrid.TableDescriptor.Columns["HrgSat"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            
            InfoGrid.TableDescriptor.Columns["Disc1"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["Disc2"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["Disc3"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["Disc4"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["DiscRp"].Appearance.AnyRecordFieldCell.Format = "###.##";
            InfoGrid.TableDescriptor.Columns["PpnRp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["ReturJualDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

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
            var listAllSatuan = _brgSatuanDal.ListData()?.ToList() ?? new List<BrgSatuanModel>();
            var listFaktur = _returBrgViewDal.ListData(periode)?.ToList() ?? new List<ReturJualBrgView>();
            listFaktur = listFaktur
                .OrderBy(x => x.ReturJualDate.Date)
                .ToList();

            foreach(var item in listFaktur)
            {
                var listThisSatuan = listAllSatuan.Where(x => x.BrgId == item.BrgId);
                if (!listThisSatuan.Any())
                    continue;

                var satBesar = listThisSatuan
                    .OrderBy(x => x.Conversion)
                    .Last();
                var satKecil = listThisSatuan
                    .OrderBy(x => x.Conversion)
                    .First();
                item.QtyBesar = item.InPcs / satBesar.Conversion;
                item.QtyKecil = item.InPcs % satBesar.Conversion;
                item.SatBesar = satBesar.Satuan;
                item.SatKecil = satKecil.Satuan;
                item.ReturJualDate = item.ReturJualDate.Date;

                if (satBesar.Satuan == satKecil.Satuan)
                {
                    item.QtyKecil = item.QtyBesar;
                    item.QtyBesar = 0;
                    item.SatBesar = string.Empty;
                }
            }

            _dataSource = Filter(listFaktur, CustomerText.Text);
            _dataSource.ForEach(x => x.ReturJualDate = x.ReturJualDate.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private static List<ReturJualBrgView> Filter(List<ReturJualBrgView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredCustomer = source.Where(x => x.CustomerName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredAddress = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredCustomer
                .Union(listFilteredAddress);
            return result.ToList();
        }
    }
}
