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
using btr.application.InventoryContext.StokBrgSupplierRpt;
using Syncfusion.Drawing;
using btr.nuna.Domain;
using Syncfusion.DataSource.Extensions;

namespace btr.distrib.InventoryContext.StokBrgSupplierRpt
{
    public partial class StokBrgSupplierForm : Form
    {
        private readonly IStokBrgSupplierDal _stokBrgSupplierDal;
        private List<StokBrgSupplierDto> _dataSource;

        public StokBrgSupplierForm(IStokBrgSupplierDal stokBrgSupplierDal)
        {
            InitializeComponent();
            _stokBrgSupplierDal = stokBrgSupplierDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
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

            var sumColNilaiStokBesar = new GridSummaryColumnDescriptor("NilaiStokBesar", SummaryType.DoubleAggregate, "NilaiStokBesar", "{Sum}");
            sumColNilaiStokBesar.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiStokBesar.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiStokBesar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColNilaiStokKecil = new GridSummaryColumnDescriptor("NilaiStokKecil", SummaryType.DoubleAggregate, "NilaiStokKecil", "{Sum}");
            sumColNilaiStokKecil.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiStokKecil.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiStokKecil.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColNilaiInPcs = new GridSummaryColumnDescriptor("NilaiInPcs", SummaryType.DoubleAggregate, "NilaiInPcs", "{Sum}");
            sumColNilaiInPcs.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiInPcs.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiInPcs.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;


            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColNilaiStokBesar, sumColNilaiStokKecil, sumColNilaiInPcs });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HppBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiStokBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HargaMTBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HargaGTBesar"].Appearance.AnyRecordFieldCell.Format = "N0";


            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HppKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiStokKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HargaMT"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["HargaGT"].Appearance.AnyRecordFieldCell.Format = "N0";

            InfoGrid.TableDescriptor.Columns["Qty"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiInPcs"].Appearance.AnyRecordFieldCell.Format = "N0";

            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["SatuanBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HppBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["NilaiStokBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HargaMTBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;
            InfoGrid.TableDescriptor.Columns["HargaGTBesar"].Appearance.AnyRecordFieldCell.BackColor = Color.LightGreen;

            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["SatuanKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["HppKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["NilaiStokKecil"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["HargaMT"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["HargaGT"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;

            InfoGrid.TableDescriptor.Columns["Qty"].Appearance.AnyRecordFieldCell.BackColor = Color.LightBlue;
            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.BackColor = Color.LightBlue;
            InfoGrid.TableDescriptor.Columns["NilaiInPcs"].Appearance.AnyRecordFieldCell.BackColor = Color.LightBlue;



            //  hide column Conversion
            InfoGrid.TableDescriptor.VisibleColumns.Remove("Conversion");

            InfoGrid.Refresh();
            Proses();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var listStok = _stokBrgSupplierDal.ListData()?.ToList() ?? new List<StokBrgSupplierView>();

            var filtered = Filter(listStok, SearchText.Text);
            _dataSource = (
                from c in filtered
                select new StokBrgSupplierDto(
                    c.BrgId,
                    c.Qty,
                    c.WarehouseName,
                    c.SupplierName,
                    c.BrgCode,
                    c.BrgName,
                    c.SatuanBesar,
                    c.SatuanKecil,
                    c.Conversion,
                    c.HargaMT,
                    c.HargaGT,
                    c.Hpp
                )
            ).ToList();
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
                saveFileDialog.FileName = $"stok-per-supplier-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<StokBrgSupplierDto>();
            foreach(var item in filtered)
            {
                listToExcel.Add(item.GetData() as StokBrgSupplierDto);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("stok-per-supplier-info")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"V{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"V{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Consolas")
                    .Font.SetFontSize(9);

                //  set format for  column  number 
                ws.Range(ws.Cell($"G{2}"), ws.Cell($"V{_dataSource.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  add rownumbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private List<StokBrgSupplierView> Filter(List<StokBrgSupplierView> source, string keyword)
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
    public class StokBrgSupplierDto
    {
        public StokBrgSupplierDto(string brgId, 
            int qty, 
            string warehousName, 
            string supplierName, 
            string brgCode, 
            string brgName, 
            string satuanBesar, 
            string satuanKecil, 
            int conversion, 
            decimal hargaMT, 
            decimal hargaGT, 
            decimal hpp)
        {
            BrgId = brgId;
            Qty = qty;
            WarehouseName = warehousName;
            SupplierName = supplierName;
            BrgCode = brgCode;
            BrgName = brgName;
            SatuanBesar = satuanBesar;
            SatuanKecil = satuanKecil;
            Conversion = conversion;
            HargaMT = hargaMT;
            HargaGT = hargaGT;
            Hpp = hpp;
        }
        public string BrgId { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierName { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }

        public int QtyBesar { get => Conversion > 0 ? Qty / Conversion : 0; }
        public string SatuanBesar { get; set; }
        public decimal HppBesar { get => Hpp * Conversion; }
        public decimal NilaiStokBesar { get => HppBesar * QtyBesar; }
        public decimal HargaMTBesar { get => HargaMT * Conversion; }
        public decimal HargaGTBesar { get => HargaGT * Conversion; }


        public int QtyKecil { get => Conversion > 0 ? Qty % Conversion : Qty; }
        public string SatuanKecil { get; set; }
        public decimal HppKecil { get => Hpp; }
        public decimal NilaiStokKecil { get => HppKecil * QtyKecil; }
        public decimal HargaMT { get; set; }
        public decimal HargaGT { get; set; }


        public int Conversion { get; set; }
        public int Qty { get; set; }
        public decimal Hpp { get; set; }
        public decimal NilaiInPcs { get => Qty * Hpp; }
    }
}
