using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.ImportOpnameAgg.Contracts;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.ImportOpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.InventoryContext.OpnameAgg;
using btr.nuna.Application;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace btr.distrib.InventoryContext.ImportOpnameAgg
{
    public partial class ImportOpnameForm : Form
    {
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly IStokDal _stokDal;
        private readonly IOpnameBuilder _opnameBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IOpnameWriter _opnameWriter;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;

        private readonly BindingList<OpnameItemDto> _listOpnameItem;
        private readonly BindingSource _bindingSource;

        public ImportOpnameForm(IImportOpnameDal importOpnameDal,
            IBrgDal brgDal,
            IOpnameBuilder opnameBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IOpnameWriter opnameWriter,
            IAddStokWorker addStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker,
            IBrgSatuanDal brgSatuanDal,
            IStokDal stokDal)
        {
            InitializeComponent();

            _brgDal = brgDal;
            _opnameBuilder = opnameBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _opnameWriter = opnameWriter;
            _addStokWorker = addStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
            _brgSatuanDal = brgSatuanDal;
            _stokDal = stokDal;

            _listOpnameItem = new BindingList<OpnameItemDto>();
            _bindingSource = new BindingSource
            {
                DataSource = _listOpnameItem
            };

            RegisterControlHandler();
            InitGrid();
        }

        private void InitGrid()
        {
            OpnameItemGrid.DataSource = _bindingSource;
            OpnameItemGrid.Columns["QtyBesar"].DefaultCellStyle.Format = "N0";
            OpnameItemGrid.Columns["QtyBesar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            OpnameItemGrid.Columns["QtyKecil"].DefaultCellStyle.Format = "N0";
            OpnameItemGrid.Columns["QtyKecil"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            OpnameItemGrid.Columns["BrgCode"].Width = 80;
            OpnameItemGrid.Columns["BrgName"].Width = 240;
            OpnameItemGrid.Columns["QtyBesar"].Width = 50;
            OpnameItemGrid.Columns["QtyKecil"].Width = 50;
            OpnameItemGrid.Columns["WarehouseId"].Width = 50;
        }

        private void OpnameItemGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var isProses = (bool)grid.Rows[e.RowIndex].Cells["IsProses"].Value;

            if (isProses)
            {
                grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gray;
            }
            else
            {
                grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void RegisterControlHandler()
        {
            ExcelButton.Click += ExcelButton_Click;
            ProsesButton.Click += ProsesButton_Click;

            ExportGagalButton.Click += ExportGagalButton_Click;

            OpnameItemGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            OpnameItemGrid.RowPrePaint += OpnameItemGrid_RowPrePaint;
        }

        private void ExportGagalButton_Click(object sender, EventArgs e)
        {
            ExportFailedRowsToExcel();
        }

        private void ExportFailedRowsToExcel()
        {
            var failedItems = _listOpnameItem.Where(item => !item.IsProses).ToList();
            if (failedItems.Count == 0)
            {
                MessageBox.Show("No failed items to export.");
                return;
            }

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"failed-{DateTime.Now:yyyy-MM-dd-HHmm}.xlsx";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Failed Items");
                worksheet.Cell(1, 1).Value = "BrgCode";
                worksheet.Cell(1, 2).Value = "BrgName";
                worksheet.Cell(1, 3).Value = "QtyBesar";
                worksheet.Cell(1, 4).Value = "QtyKecil";
                worksheet.Cell(1, 5).Value = "WarehouseId";

                for (int i = 0; i < failedItems.Count; i++)
                {
                    var item = failedItems[i];
                    worksheet.Cell(i + 2, 1).Value = item.BrgCode;
                    worksheet.Cell(i + 2, 2).Value = item.BrgName;
                    worksheet.Cell(i + 2, 3).Value = item.QtyBesar;
                    worksheet.Cell(i + 2, 4).Value = item.QtyKecil;
                    worksheet.Cell(i + 2, 5).Value = item.WarehouseId;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }

            MessageBox.Show($"Failed items exported to {filePath}");
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
                LoadGrid(textBox1.Text);
            }
        }

        private void LoadGrid(string excelFileName)
        {
            _listOpnameItem.Clear();
            using (XLWorkbook workbook = new XLWorkbook(excelFileName))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                foreach (IXLRow row in worksheet.Rows())
                {
                    if (row.RowNumber() == 1)
                        continue;

                    var brgCode = row.Cell(1).Value.ToString();
                    var brgName = row.Cell(2).Value.ToString();
                    var qBesar = int.TryParse(row.Cell(3).Value.ToString(), out int qtyBesar) ? qtyBesar : 0;
                    var qKecil = int.TryParse(row.Cell(4).Value.ToString(), out int qtyKecil) ? qtyKecil : 0;
                    var warehouseId = row.Cell(5).Value.ToString();

                    var newItem = new OpnameItemDto(brgCode, brgName, qBesar, qKecil, warehouseId);

                    _listOpnameItem.Add(newItem);
                }
            }
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Proses Adjustment Stok Opname?", "Opname", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            AdjustStok();
            MessageBox.Show("Done");
        }

        private void AdjustStok()
        {
            var listSatuan = _brgSatuanDal?.ListData()?? new List<BrgSatuanModel>();
            var listBrg = _brgDal.ListData();
            var mainForm = (MainForm)this.Parent.Parent;
            using (var trans = TransHelper.NewScope())
            {
                PrgBar.Value = 0;
                PrgBar.Maximum = _listOpnameItem.Count();

                foreach (var item in _listOpnameItem)
                {
                    PrgBar.Value++;
                    //  cari by code. jika ga ketemu coba cari by name
                    var brg = listBrg.FirstOrDefault(x => x.BrgCode == item.BrgCode);
                    if (brg is null)
                    {
                        brg = listBrg.FirstOrDefault(x => x.BrgName == item.BrgName);
                        if (brg is null)
                            continue;
                    }


                    //  cari qty inPcs hasil opname
                    int inPcsOpname = 0;
                    if (item.QtyBesar > 0)
                    {
                        var conversion = listSatuan
                            .Where(x => x.BrgId == brg.BrgId)
                            .FirstOrDefault(x => x.Conversion > 1)
                            ?.Conversion
                            ?? throw new KeyNotFoundException($"BrgCode {item.BrgCode} tidak punya satuan besar");
                        inPcsOpname = item.QtyBesar * conversion;
                    }
                    inPcsOpname += item.QtyKecil;

                    //  cari qtyStok saat ini
                    var listStok = _stokDal.ListData(brg, new WarehouseModel(item.WarehouseId))
                        ?.ToList() ?? new List<StokModel>();
                    var inPcsStok = listStok.Sum(x => x.Qty);

                    //  cari qty adjust
                    var qtyAdjust = inPcsOpname - inPcsStok;

                    //  tidak ada adjustment, skip next item
                    if (qtyAdjust == 0)
                    {
                        item.Proses();
                        continue;
                    }

                    var opname = _opnameBuilder
                        .Create()
                        .Brg(brg)
                        .Warehouse(new WarehouseModel(item.WarehouseId))
                        .User(mainForm.UserId)
                        .QtyAwal(0, inPcsStok)
                        .QtyOpname(0, inPcsOpname)
                        .Nilai(brg.Hpp)
                        .Build();
                    _opnameWriter.Save(ref opname);
                    GenStok(opname);

                    item.Proses();
                }

                trans.Complete();
            }
        }
        private void GenStok(OpnameModel opname)
        {
            var qtyAdjust = opname.Qty2Adjust * opname.Conversion2;
            qtyAdjust += opname.Qty1Adjust;

            if (qtyAdjust > 0)
            {
                var req = new AddStokRequest(opname.BrgId, opname.WarehouseId, qtyAdjust, opname.Satuan1, opname.Nilai, opname.OpnameId, "OPNAME", "Import Opname", opname.OpnameDate);
                _addStokWorker.Execute(req);
            }

            if (qtyAdjust < 0)
            {
                var req = new RemoveFifoStokRequest(opname.BrgId, opname.WarehouseId, qtyAdjust * (-1), opname.Satuan1, opname.Nilai, opname.OpnameId, "OPNAME", "Import Opname", opname.OpnameDate);
                _removeFifoStokWorker.Execute(req);
            }
        }
    }

    public class OpnameItemDto
    {
        public OpnameItemDto(string code, string name, int qbesar, int qkecil, string wh)
        {
            BrgCode = code;
            BrgName = name;
            QtyBesar = qbesar;
            QtyKecil = qkecil;
            WarehouseId = wh;
        }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public int QtyKecil { get; private set; }
        public string WarehouseId { get; private set; }
        public bool IsProses { get;  private set; }
        public void Proses() => IsProses = true;
    }
}
