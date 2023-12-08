using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.InventoryContext.StokBalanceInfo;
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

namespace btr.distrib.FinanceContext.PenerimaanPelunasanSalesRpt
{
    public partial class PenerimaanPelunasanSalesForm : Form
    {
        private readonly IPenerimaanPelunasanSalesDal _penerimaanPelunasanSalesDal;
        private List<PiutangSalesWilayahDto> _dataSource;

        public PenerimaanPelunasanSalesForm(IPenerimaanPelunasanSalesDal piutangSalesWilayahDal)
        {
            InitializeComponent();
            _penerimaanPelunasanSalesDal = piutangSalesWilayahDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
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

            var sumColBayarTunai = new GridSummaryColumnDescriptor("BayarTunai", SummaryType.DoubleAggregate, "BayarTunai", "{Sum}");
            sumColBayarTunai.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColBayarTunai.Appearance.AnySummaryCell.Format = "N0";
            sumColBayarTunai.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColBayarGiro = new GridSummaryColumnDescriptor("BayarGiro", SummaryType.DoubleAggregate, "BayarGiro", "{Sum}");
            sumColBayarGiro.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColBayarGiro.Appearance.AnySummaryCell.Format = "N0";
            sumColBayarGiro.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColRetur = new GridSummaryColumnDescriptor("Retur", SummaryType.DoubleAggregate, "Retur", "{Sum}");
            sumColRetur.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColRetur.Appearance.AnySummaryCell.Format = "N0";
            sumColRetur.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColPotongan = new GridSummaryColumnDescriptor("Potongan", SummaryType.DoubleAggregate, "Potongan", "{Sum}");
            sumColPotongan.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColPotongan.Appearance.AnySummaryCell.Format = "N0";
            sumColPotongan.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColMateraiAdmin = new GridSummaryColumnDescriptor("MateraiAdmin", SummaryType.DoubleAggregate, "MateraiAdmin", "{Sum}");
            sumColMateraiAdmin.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColMateraiAdmin.Appearance.AnySummaryCell.Format = "N0";
            sumColMateraiAdmin.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTotalBayar = new GridSummaryColumnDescriptor("TotalBayar", SummaryType.DoubleAggregate, "TotalBayar", "{Sum}");
            sumColTotalBayar.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightPink);
            sumColTotalBayar.Appearance.AnySummaryCell.Format = "N0";
            sumColTotalBayar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColBayarTunai,
                sumColBayarGiro, sumColRetur, sumColPotongan, sumColMateraiAdmin, sumColTotalBayar });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["BayarTunai"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["BayarGiro"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Potongan"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["MateraiAdmin"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["TotalBayar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.Refresh();

            Proses();

        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var periode = new Periode(Faktur1Date.Value, Faktur2Date.Value);
            var listFaktur = _penerimaanPelunasanSalesDal.ListData(periode)?.ToList() ?? new List<PenerimaanPelunasanSalesDto>();
            //var filtered = Filter(listFaktur, SearchText.Text);
            //_dataSource = (
            //    from c in filtered
            //    select new StokBalanceInfoDto
            //    {
            //        Supplier = c.SupplierName,
            //        Kategori = c.KategoriName,
            //        BrgId = c.BrgId,
            //        BrgCode = c.BrgCode,
            //        BrgName = c.BrgName,
            //        Warehouse = c.WarehouseName,
            //        //QtyBesar = c.QtyBesar,
            //        SatBesar = c.SatBesar,
            //        Conversion = c.Conversion,
            //        //QtyKecil = c.QtyKecil,
            //        SatKecil = c.SatKecil,
            //        InPcs = c.Qty,
            //        Hpp = c.Hpp,
            //        NilaiSediaan = c.NilaiSediaan,
            //    }).ToList();
            //InfoGrid.DataSource = _dataSource;
            InfoGrid.DataSource = listFaktur;
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
                saveFileDialog.FileName = $"stok-balance-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("stok-balance-info")
                .Cell($"B1")
                    .InsertTable(_dataSource, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{_dataSource.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{_dataSource.Count + 1}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  set format for  column  number 
                ws.Range(ws.Cell($"H{2}"), ws.Cell($"O{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < _dataSource.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private List<StokBalanceView> Filter(List<StokBalanceView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listFilteredKategori = source.Where(x => x.KategoriName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredSupplier = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredBrgName
                .Union(listFilteredBrgCode)
                .Union(listFilteredKategori)
                .Union(listFilteredSupplier);
            return result.ToList();
        }
    }
}
