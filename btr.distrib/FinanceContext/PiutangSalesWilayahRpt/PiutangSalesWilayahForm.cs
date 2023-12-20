using btr.application.InventoryContext.StokBalanceInfo;
using btr.distrib.InventoryContext.StokBalanceRpt;
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
using btr.application.FinanceContext.PiutangAgg.Contracts;
using Syncfusion.Drawing;
using btr.nuna.Domain;

namespace btr.distrib.FinanceContext.PiutangSalesWilayahRpt
{
    public partial class PiutangSalesWilayahForm : Form
    {
        private readonly IPiutangSalesWilayahDal _piutangSalesWilayahDal;
        private List<PiutangSalesWilayahDto> _dataSource;

        public PiutangSalesWilayahForm(IPiutangSalesWilayahDal piutangSalesWilayahDal)
        {
            InitializeComponent();
            _piutangSalesWilayahDal = piutangSalesWilayahDal;
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

            var sumColTotalJual = new GridSummaryColumnDescriptor("TotalJual", SummaryType.DoubleAggregate, "TotalJual", "{Sum}");
            sumColTotalJual.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightPink);
            sumColTotalJual.Appearance.AnySummaryCell.Format = "N0";
            sumColTotalJual.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

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

            var sumColKurangBayar = new GridSummaryColumnDescriptor("KurangBayar", SummaryType.DoubleAggregate, "KurangBayar", "{Sum}");
            sumColKurangBayar.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightPink);
            sumColKurangBayar.Appearance.AnySummaryCell.Format = "N0";
            sumColKurangBayar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotalJual, sumColBayarTunai, 
                sumColBayarGiro, sumColRetur, sumColPotongan, sumColMateraiAdmin, sumColKurangBayar });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["TotalJual"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["BayarTunai"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["BayarGiro"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Potongan"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["MateraiAdmin"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["KurangBayar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.Refresh();

            //  auto group by sales and wilayah
            InfoGrid.TableDescriptor.GroupedColumns.Add("SalesName");
            InfoGrid.TableDescriptor.GroupedColumns.Add("WilayahName");
            Proses();

        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
            //  auto expand group
            InfoGrid.Table.ExpandAllGroups();
        }

        private void Proses()
        {
            var periode = new Periode(Faktur1Date.Value, Faktur2Date.Value);
            var listFaktur = _piutangSalesWilayahDal.ListData(periode)?.ToList() ?? new List<PiutangSalesWilayahDto>();
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
            _dataSource = listFaktur;
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
                saveFileDialog.FileName = $"piutang-sales-per-wilayah-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var excelContent = _dataSource
                    .OrderBy(x => x.SalesName)
                    .ThenBy(x => x.WilayahName)
                    .ThenBy(x => x.CustomerName)
                    .ToList();

                //  projection excel content to piutang structure using LINQ
                var piutangStructure = excelContent
                    .GroupBy(x => x.SalesName)
                    .Select(x => new PiutangStructureDto
                    {
                        SalesName = x.Key,
                        TotalPerSales = x.Sum(y => y.TotalJual),
                        ListWilayah = x.GroupBy(y => y.WilayahName)
                            .Select(y => new PiutangWilayahDto
                            {
                                WilayahName = y.Key,
                                TotalPerWilayah = y.Sum(z => z.TotalJual),
                                ListFaktur = y.Select(z => new PiutangFakturDto
                                {
                                    FakturCode = z.FakturCode,
                                    FakturDate = z.FakturDate,
                                    CustomerName = z.CustomerName,
                                    JatuhTempo = z.JatuhTempo,
                                    TotalJual = z.TotalJual,
                                    BayarTunai = z.BayarTunai,
                                    BayarGiro = z.BayarGiro,
                                    Retur = z.Retur,
                                    Potongan = z.Potongan,
                                    MateraiAdmin = z.MateraiAdmin,
                                    KurangBayar = z.KurangBayar
                                }).ToList()
                            }).ToList()
                    }).ToList();


                var ws = wb.AddWorksheet("piutang-sales-per-wilayah-info");
                var baris = 1;
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"L{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"L{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "LAPORAN PIUTANG PER-SALES PER-WILAYAH";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"L{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{Faktur1Date.Value:dd MMMM yyyy} - {Faktur2Date.Value:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"L{baris}")).Merge();
                baris++;

                foreach(var sales in piutangStructure)
                {
                    ws.Cell($"A{baris}").Value = sales.SalesName;
                    baris++;
                    foreach(var wilayah in sales.ListWilayah)
                    {
                        ws.Cell($"A{baris}").Value = wilayah.WilayahName;
                        baris++;
                        var noUrut = 1;
                        //  create header
                        ws.Cell($"A{baris}").Value = "No";
                        ws.Cell($"B{baris}").Value = "Faktur";
                        ws.Cell($"C{baris}").Value = "Tanggal";
                        ws.Cell($"D{baris}").Value = "Customer";
                        ws.Cell($"E{baris}").Value = "Jatuh Tempo";
                        ws.Cell($"F{baris}").Value = "Total Jual";
                        ws.Cell($"G{baris}").Value = "Bayar Tunai";
                        ws.Cell($"H{baris}").Value = "Bayar Giro";
                        ws.Cell($"I{baris}").Value = "Retur";
                        ws.Cell($"J{baris}").Value = "Potongan";
                        ws.Cell($"K{baris}").Value = "Materai Admin";
                        ws.Cell($"L{baris}").Value = "Kurang Bayar";
                        baris++;
                        //  set border and font for header
                        ws.Range(ws.Cell($"A{baris - 1}"), ws.Cell($"L{baris - 1}")).Style
                            .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                            .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                            .Font.SetBold(true);
                        //  set backcolor to light blue
                        ws.Range(ws.Cell($"A{baris - 1}"), ws.Cell($"L{baris - 1}")).Style
                            .Fill.SetBackgroundColor(XLColor.LightBlue);

                        var barisAwal = baris;
                        foreach (var faktur in wilayah.ListFaktur)
                        {
                            ws.Cell($"A{baris}").Value = noUrut;
                            ws.Cell($"B{baris}").Value = faktur.FakturCode;
                            ws.Cell($"C{baris}").Value = faktur.FakturDate;
                            ws.Cell($"D{baris}").Value = faktur.CustomerName;
                            ws.Cell($"E{baris}").Value = faktur.JatuhTempo;
                            ws.Cell($"F{baris}").Value = faktur.TotalJual;
                            ws.Cell($"G{baris}").Value = faktur.BayarTunai;
                            ws.Cell($"H{baris}").Value = faktur.BayarGiro;
                            ws.Cell($"I{baris}").Value = faktur.Retur;
                            ws.Cell($"J{baris}").Value = faktur.Potongan;
                            ws.Cell($"K{baris}").Value = faktur.MateraiAdmin;
                            ws.Cell($"L{baris}").Value = faktur.KurangBayar;
                            baris++;
                            noUrut++;
                        }
                        //  set border and font
                        ws.Range(ws.Cell($"A{barisAwal}"), ws.Cell($"L{baris - 1}")).Style
                            .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                            .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                        //  set format for  column  number
                        ws.Range(ws.Cell($"F{barisAwal}"), ws.Cell($"L{baris - 1}"))
                            .Style.NumberFormat.Format = "#,##";
                        //  set format date as dd-MM-yyyy
                        ws.Range(ws.Cell($"C{barisAwal}"), ws.Cell($"C{baris - 1}"))
                            .Style.NumberFormat.Format = "dd-MMM-yyyy";
                        ws.Range(ws.Cell($"E{barisAwal}"), ws.Cell($"E{baris - 1}"))
                            .Style.NumberFormat.Format = "dd-MMM-yyyy";
                        //  add footer sum for all number column
                        ws.Cell($"F{baris}").FormulaA1 = $"SUM(F{barisAwal}:F{baris-1})";
                        ws.Cell($"G{baris}").FormulaA1 = $"SUM(G{barisAwal}:G{baris - 1})";
                        ws.Cell($"H{baris}").FormulaA1 = $"SUM(H{barisAwal}:H{baris - 1})";
                        ws.Cell($"I{baris}").FormulaA1 = $"SUM(I{barisAwal}:I{baris - 1})";
                        ws.Cell($"J{baris}").FormulaA1 = $"SUM(J{barisAwal}:J{baris - 1})";
                        ws.Cell($"K{baris}").FormulaA1 = $"SUM(K{barisAwal}:K{baris - 1})";
                        ws.Cell($"L{baris}").FormulaA1 = $"SUM(L{barisAwal}:L{baris - 1})";
                        // set border footer
                        ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}")).Style
                            .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                            .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                        //  set backcolor to light yellow
                        ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}")).Style
                            .Fill.SetBackgroundColor(XLColor.LightYellow);
                        //  set format for  column  number
                        ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}"))
                            .Style.NumberFormat.Format = "#,##";
                        baris++;
                    }
                    //  add sum per-sales
                    ws.Cell($"F{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.TotalJual);
                    ws.Cell($"G{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.BayarTunai);
                    ws.Cell($"H{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.BayarGiro);
                    ws.Cell($"I{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.Retur);
                    ws.Cell($"J{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.Potongan);
                    ws.Cell($"K{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.MateraiAdmin);
                    ws.Cell($"L{baris}").Value = sales.ListWilayah.SelectMany(x => x.ListFaktur).Sum(x => x.KurangBayar);
                    //  format number with thousand separaot and 0 decimal place
                    ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}")).Style
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                    //  set backcolor to light pink
                    ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}")).Style
                        .Fill.SetBackgroundColor(XLColor.LightPink);
                    ws.Range(ws.Cell($"F{baris}"), ws.Cell($"L{baris}")).Style
                        .NumberFormat.Format = "#,##";
                    baris++;
                }
                ws.Columns().AdjustToContents();
                // set column A with to 2 characters
                ws.Column(1).Width = 2;

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

    public class PiutangStructureDto
    {
        public string SalesName { get; set; }
        public decimal TotalPerSales { get; set; }
        public List<PiutangWilayahDto> ListWilayah{ get; set; }
    }
    public class PiutangWilayahDto
    {
        public string WilayahName { get; set; }
        public decimal TotalPerWilayah { get; set; }
        public List<PiutangFakturDto> ListFaktur { get; set; }

    }

    public class PiutangFakturDto
    {
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerName { get; set; }
        public DateTime JatuhTempo { get; set; }
        public decimal TotalJual { get; set; }
        public decimal BayarTunai { get; set; }
        public decimal BayarGiro { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal MateraiAdmin { get; set; }
        public decimal KurangBayar { get; set; }
    }
}
