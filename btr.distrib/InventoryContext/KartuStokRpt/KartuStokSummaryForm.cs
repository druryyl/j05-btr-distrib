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
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.application.InventoryContext.StokPeriodikInfo;
using btr.application.InventoryContext.StokAgg;
using System.Drawing;

namespace btr.distrib.InventoryContext.KartuStokRpt
{
    public partial class KartuStokSummaryForm : Form
    {
        private readonly BindingList<KartuStokSummaryDto> _brgList;
        private readonly BindingSource _brgBindingSource;
        private readonly IKartuStokSummaryDal _kartuStokSummaryDal;
        private readonly IStokPeriodikDal _stokPeriodikDal;
        private readonly IWarehouseDal _warehouseDal;

        public KartuStokSummaryForm(IKartuStokSummaryDal kartuStokSummaryDal,
            IWarehouseDal warehouseDal,
            IStokPeriodikDal stokPeriodikDal)
        {
            InitializeComponent();

            _brgList = new BindingList<KartuStokSummaryDto>();
            _brgBindingSource = new BindingSource(_brgList, null);
            _kartuStokSummaryDal = kartuStokSummaryDal;
            _warehouseDal = warehouseDal;
            _stokPeriodikDal = stokPeriodikDal;

            RegisterEventHandler();
            InitGrid();
            InitComboWarehouse();
        }

        private void RegisterEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
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

            InfoGrid.TableDescriptor.Columns["Invoice"].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns["Faktur"].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns["Mutasi"].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;
            InfoGrid.TableDescriptor.Columns["Opname"].Appearance.AnyRecordFieldCell.BackColor = Color.PaleGreen;

            InfoGrid.TableDescriptor.Columns["StokAwal"].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;
            InfoGrid.TableDescriptor.Columns["MovingStok"].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;
            InfoGrid.TableDescriptor.Columns["StokAkhir"].Appearance.AnyRecordFieldCell.BackColor = Color.LightPink;

            InfoGrid.TableDescriptor.Columns["NilaiAwal"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["NilaiMoving"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;
            InfoGrid.TableDescriptor.Columns["NilaiAkhir"].Appearance.AnyRecordFieldCell.BackColor = Color.LightYellow;

            InfoGrid.TableDescriptor.Columns["Invoice"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Faktur"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Retur"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Mutasi"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["Opname"].Appearance.AnyRecordFieldCell.Format = "#,##";

            InfoGrid.TableDescriptor.Columns["StokAwal"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["MovingStok"].Appearance.AnyRecordFieldCell.Format = "#,##";
            InfoGrid.TableDescriptor.Columns["StokAkhir"].Appearance.AnyRecordFieldCell.Format = "#,##";

            InfoGrid.TableDescriptor.Columns["NilaiAwal"].Appearance.AnyRecordFieldCell.Format = "#,##.00";
            InfoGrid.TableDescriptor.Columns["NilaiMoving"].Appearance.AnyRecordFieldCell.Format = "#,##.00";
            InfoGrid.TableDescriptor.Columns["NilaiAkhir"].Appearance.AnyRecordFieldCell.Format = "#,##.00";

            InfoGrid.TableDescriptor.Columns["HppAvg"].Appearance.AnyRecordFieldCell.Format = "#,##.00";



        }

        private void InitComboWarehouse()
        {
            var listWarehouse = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
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

                var totalRow = listToExcel.Count + 2;
                ws.Cell($"P{totalRow}").FormulaA1 = $"SUM(P{2}:P{listToExcel.Count + 1})";
                ws.Cell($"Q{totalRow}").FormulaA1 = $"SUM(Q{2}:Q{listToExcel.Count + 1})";
                ws.Cell($"R{totalRow}").FormulaA1 = $"SUM(R{2}:R{listToExcel.Count + 1})";
                ws.Range(ws.Cell($"P{totalRow}"), ws.Cell($"R{totalRow}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    //.Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                    .NumberFormat.Format = "#,##";

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"S1")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  set format row data: font consolas 9, border medium, border inside hair
                ws.Range(ws.Cell("A2"), ws.Cell($"S{listToExcel.Count + 1}")).Style
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
                ws.Range(ws.Cell("H2"), ws.Cell($"O{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                ws.Range(ws.Cell("P2"), ws.Cell($"S{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##.00";

                //  set backcolor numeric column
                ws.Range(ws.Cell("H2"), ws.Cell($"L{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.PaleGreen);
                ws.Range(ws.Cell("M2"), ws.Cell($"O{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightPink);
                ws.Range(ws.Cell("P2"), ws.Cell($"R{listToExcel.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightYellow);

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            if (WarehouseCombo.SelectedValue == null)
            {
                MessageBox.Show("Pilih Gudang");
                return;
            }

            var warehouseKey = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var listStokAwal = _stokPeriodikDal.ListData(PeriodeCalender.SelectionStart.AddDays(-1), warehouseKey)
                ?.ToList() ?? new List<StokPeriodikDto>();
            var listStokAkhir = _stokPeriodikDal.ListData(PeriodeCalender.SelectionEnd, warehouseKey)
                ?.ToList() ?? new List<StokPeriodikDto>();

            var listBrg = _kartuStokSummaryDal
                .ListData(new Periode(PeriodeCalender.SelectionStart, PeriodeCalender.SelectionEnd),
                          warehouseKey)?.ToList() 
                ?? new List<KartuStokSummaryDto>();
            foreach (var x in listBrg)
            {
                x.MovingStok = x.Invoice + x.Faktur + x.Retur + x.Mutasi + x.Opname;
                var stokAwal = listStokAwal.FirstOrDefault(y => y.BrgId == x.BrgId);
                var stokAkhir = listStokAkhir.FirstOrDefault(y => y.BrgId == x.BrgId);
                x.StokAwal = stokAwal?.Qty ?? 0;
                x.StokAkhir = stokAkhir?.Qty ?? 0;
                x.NilaiAwal = (stokAwal?.Hpp ?? 0) * x.StokAwal;
                x.NilaiAkhir = (stokAkhir?.Hpp ?? 0) * x.StokAkhir;
                x.NilaiMoving = x.NilaiAkhir - x.NilaiAwal;
                if (x.MovingStok != 0)
                    x.HppAvg = Math.Abs(x.NilaiMoving / x.MovingStok);
                else
                    if (x.StokAkhir != 0)
                        x.HppAvg = Math.Abs(x.NilaiAkhir / x.StokAkhir);
                else
                    x.HppAvg = 0;

            };


            InfoGrid.DataSource = listBrg;
            InfoGrid.Refresh();
        }


    }
}
