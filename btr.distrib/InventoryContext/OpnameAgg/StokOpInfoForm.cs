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
using btr.application.InventoryContext.OpnameAgg;
using Syncfusion.WinForms.DataGrid;

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public partial class StokOpInfoForm : Form
    {
        private readonly IStokOpInfoDal _stokOpViewDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private List<StokOpInfoView> _dataSource;

        public StokOpInfoForm(IStokOpInfoDal stokOpViewDal,
            IBrgSatuanDal brgSatuanDal)
        {
            InitializeComponent();
            _stokOpViewDal = stokOpViewDal;
            _brgSatuanDal = brgSatuanDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            _dataSource = new List<StokOpInfoView>();
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
                saveFileDialog.FileName = $"stok-opname-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Stok-Opname-Info")
                    .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();

                //  set border and font
                ws.Range(ws.Cell("A1"), ws.Cell($"W{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Cell($"H{_dataSource.Count + 2}").Value = "Total";
                ws.Range(ws.Cell($"H{_dataSource.Count + 2}"), ws.Cell($"W{_dataSource.Count + 2}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(11)
                    .Font.SetBold();

                ws.Range(ws.Cell("A1"), ws.Cell($"W{_dataSource.Count + 2}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  add row total V,W,X,Y
                ws.Cell($"I{_dataSource.Count + 2}").FormulaA1 = $"=SUM(I2:I{_dataSource.Count + 1})";
                ws.Cell($"J{_dataSource.Count + 2}").FormulaA1 = $"=SUM(J2:J{_dataSource.Count + 1})";
                ws.Cell($"K{_dataSource.Count + 2}").FormulaA1 = $"=SUM(K2:K{_dataSource.Count + 1})";
                ws.Cell($"L{_dataSource.Count + 2}").FormulaA1 = $"=SUM(L2:L{_dataSource.Count + 1})";
                ws.Cell($"M{_dataSource.Count + 2}").FormulaA1 = $"=SUM(M2:M{_dataSource.Count + 1})";
                ws.Cell($"N{_dataSource.Count + 2}").FormulaA1 = $"=SUM(N2:N{_dataSource.Count + 1})";
                ws.Cell($"U{_dataSource.Count + 2}").FormulaA1 = $"=SUM(U2:U{_dataSource.Count + 1})";
                ws.Cell($"V{_dataSource.Count + 2}").FormulaA1 = $"=SUM(V2:V{_dataSource.Count + 1})";
                ws.Cell($"W{_dataSource.Count + 2}").FormulaA1 = $"=SUM(W2:W{_dataSource.Count + 1})";

                //  set format number for column H,I,J,K,L,M, O P Q R
                ws.Range(ws.Cell("A2"), ws.Cell($"A{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("C2"), ws.Cell($"C{_dataSource.Count + 2}")).Style.NumberFormat.Format = "dd-MM-yyyy";
                ws.Range(ws.Cell("I2"), ws.Cell($"I{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("J2"), ws.Cell($"J{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##"; 
                ws.Range(ws.Cell("K2"), ws.Cell($"K{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("L2"), ws.Cell($"L{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("M2"), ws.Cell($"M{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("N2"), ws.Cell($"H{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("T2"), ws.Cell($"W{_dataSource.Count + 2}")).Style.NumberFormat.Format = "#,##.00";


                ws.Range(ws.Cell("I1"), ws.Cell($"K{_dataSource.Count + 2}")).Style.Fill.BackgroundColor = XLColor.LemonChiffon;
                ws.Range(ws.Cell("L1"), ws.Cell($"N{_dataSource.Count + 2}")).Style.Fill.BackgroundColor = XLColor.PaleGreen;
                ws.Range(ws.Cell("O1"), ws.Cell($"Q{_dataSource.Count + 2}")).Style.Fill.BackgroundColor = XLColor.LemonChiffon;
                ws.Range(ws.Cell("U1"), ws.Cell($"W{_dataSource.Count + 2}")).Style.Fill.BackgroundColor = XLColor.LightPink;

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
            InfoGrid.DataSource = new List<StokOpInfoView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            //  format PeriodeOp to dd-MM-yyyy
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.PeriodeOp)].Appearance.AnyRecordFieldCell.Format = "dd-MM-yyyy";
            //  format all numeric columns to #,##.##
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAwal)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAwal)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAwal)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAdjust)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAdjust)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAdjust)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarOpname)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilOpname)].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsOpname)].Appearance.AnyRecordFieldCell.Format = "#,##";

            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.HppSatuan)].Appearance.AnyRecordFieldCell.Format = "#,##.00";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiAwal)].Appearance.AnyRecordFieldCell.Format = "#,##.00";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiAdjust)].Appearance.AnyRecordFieldCell.Format = "#,##.00";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiOpname)].Appearance.AnyRecordFieldCell.Format = "#,##.00";

            //  set all numeric columns width to 50
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAwal)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAwal)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAwal)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAdjust)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAdjust)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAdjust)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarOpname)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilOpname)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsOpname)].Width = 50;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.PeriodeOp)].Width = 100;

            //  Set Column "QtyBesarAwal" Caption to "Qty-B Awal", "QtyKecilAwal" Caption to "Qty-K Awal
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAwal)].HeaderText = "Qty-B Awal";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAwal)].HeaderText = "Qty-K Awal";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAwal)].HeaderText = "Qty-PCS Awal";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAwal)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAwal)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAwal)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;

            //  Set Column "QtyBesarAdjust" Caption to "Qty-B Adjust", "QtyKecilAdjust" Caption to "Qty-K Adjust"
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAdjust)].HeaderText = "Qty-B Adjust";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAdjust)].HeaderText = "Qty-K Adjust";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAdjust)].HeaderText = "Qty-PCS ADjust";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarAdjust)].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilAdjust)].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsAdjust)].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;

            //  Set Column "QtyBesarOpname" Caption to "Qty-B Opname", "QtyKecilOpname" Caption to "Qty-K Opname"
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarOpname)].HeaderText = "Qty-B Opname";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilOpname)].HeaderText = "Qty-K Opname";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsOpname)].HeaderText = "Qty-PCS Opname";
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyBesarOpname)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyKecilOpname)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.QtyPcsOpname)].Appearance.AnyRecordFieldCell.BackColor = Color.LemonChiffon;

            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiAwal)].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiAdjust)].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;
            InfoGrid.TableDescriptor.Columns[nameof(StokOpInfoView.NilaiOpname)].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;

            //  show row number
            InfoGrid.TableOptions.ShowRowHeader = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

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
            List<StokOpInfoView> listStokOp = _stokOpViewDal.ListData(periode)?.ToList() ?? new List<StokOpInfoView>();
            listStokOp = listStokOp
                .OrderBy(x => x.PeriodeOp)
                .ToList();

            _dataSource = Filter2(listStokOp, CustomerText.Text);
            _dataSource.ForEach(x => x.PeriodeOp = x.PeriodeOp.Date);
            InfoGrid.DataSource = _dataSource;
        }

        private List<StokOpInfoView> Filter2(List<StokOpInfoView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredBrgCode
                .Union(listFilteredBrgName);
            return result.ToList();
        }
    }
}
