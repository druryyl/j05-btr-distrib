using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceInfo;
using btr.application.InventoryContext.StokPeriodikInfo;
using btr.distrib.InventoryContext.StokBrgSupplierRpt;
using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.DataSource;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;

namespace btr.distrib.InventoryContext.StokPeriodikRpt
{
    public partial class StokPeriodikForm : Form
    {
        private readonly IStokPeriodikDal _stokPeriodikDal;
        private readonly BindingList<StokPeriodikViewDto> _dataSource;
        private readonly BindingSource _bindingSource;
        private readonly IBrgDal _brgDal;
        private readonly IBrgBuilder _brgBuilder;

        public StokPeriodikForm(IStokPeriodikDal stokPeriodikDal, 
            IBrgDal brgDal, 
            IBrgBuilder brgBuilder)
        {
            InitializeComponent();
            _stokPeriodikDal = stokPeriodikDal;
            _brgDal = brgDal;
            _brgBuilder = brgBuilder;

            _dataSource = new BindingList<StokPeriodikViewDto>();
            _bindingSource = new BindingSource(_dataSource, null);

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;
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
            InfoGrid.DataSource = _dataSource;

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColNilaiSediaan = new GridSummaryColumnDescriptor("NilaiSediaan", SummaryType.DoubleAggregate, "NilaiSediaan", "{Sum}");
            sumColNilaiSediaan.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiSediaan.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiSediaan.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColNilaiSediaan });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["InPcs"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiSediaan"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.Refresh();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var tgl = PeriodikCalender.SelectionEnd;
            var listStok = _stokPeriodikDal.ListData(tgl)?.ToList() ?? new List<StokPeriodikDto>();
            var listBrg = _brgDal.ListData();
            ProsesBar.Maximum = listStok.Count;
            ProsesBar.Value = 0;
            ProsesBar.Visible = true;

            _dataSource.Clear();
            foreach (var item in listStok)
            {
                ProsesBar.Value++;
                var newView = new StokPeriodikViewDto
                {
                    Supplier = item.SupplierName,
                    Kategori = item.KategoriName,
                    BrgId = item.BrgId,
                    BrgCode = item.BrgCode,
                    BrgName = item.BrgName,
                    Warehouse = item.WarehouseName,
                    InPcs = item.Qty,
                    Hpp = item.Hpp,
                    NilaiSediaan = item.Qty * item.Hpp,
                };
                var brg = _brgBuilder.Load(item).Build();
                var satBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion != 1)
                    ?? new BrgSatuanModel("", "", 1, "");
                newView.SatBesar = satBesar.Satuan;
                newView.SatKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
                newView.Conversion = satBesar.Conversion;
                _dataSource.Add(newView);
            }
            ProsesBar.Visible = false;
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
                saveFileDialog.FileName = $"stok-periodik-{PeriodikCalender.SelectionEnd:yyyy-MM-dd}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<StokPeriodikViewDto>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as StokPeriodikViewDto);
            }


            using (IXLWorkbook wb = new XLWorkbook())
            {
                var periode = PeriodikCalender.SelectionEnd.ToString("yyyy-MM-dd");
                wb.AddWorksheet($"stok-periodik-{periode}")
                .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();
                //  set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"O{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                //  set format for  column  number 
                ws.Range(ws.Cell($"H{2}"), ws.Cell($"O{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0";
                ws.Range(ws.Cell($"A{2}"), ws.Cell($"A{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0";
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
