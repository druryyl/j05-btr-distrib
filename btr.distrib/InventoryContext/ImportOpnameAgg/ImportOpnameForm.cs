using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.ImportOpnameAgg.Contracts;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.ImportOpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
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
        private readonly IImportOpnameDal _importOpnameDal;
        private readonly IBrgDal _brgDal;
        private readonly IOpnameBuilder _opnameBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IOpnameWriter _opnameWriter;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;

        public ImportOpnameForm(IImportOpnameDal importOpnameDal,
            IBrgDal brgDal,
            IOpnameBuilder opnameBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IOpnameWriter opnameWriter,
            IAddStokWorker addStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker)
        {
            InitializeComponent();
            _importOpnameDal = importOpnameDal;
            _brgDal = brgDal;
            _opnameBuilder = opnameBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _opnameWriter = opnameWriter;
            _addStokWorker = addStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadExcel();
            AdjustStok();
            MessageBox.Show("Done");
        }

        private void LoadExcel()
        {
            var list = new List<ImportOpnameModel>();
            using (XLWorkbook workbook = new XLWorkbook(textBox1.Text))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                foreach (IXLRow row in worksheet.Rows())
                {
                    if (row.RowNumber() == 1)
                        continue;
                    var newItem = new ImportOpnameModel
                    {
                        WarehouseId = row.Cell(1).Value.ToString(),
                        BrgCode = row.Cell(2).Value.ToString()
                    };
                    _ = int.TryParse(row.Cell(3).Value.ToString(), out int qty);
                    newItem.Qty = qty;

                    list.Add(newItem);
                }
            }
            _importOpnameDal.Delete();
            _importOpnameDal.Insert(list);
        }

        private void AdjustStok()
        {
            var listImport = _importOpnameDal.ListData()?.ToList() ?? new List<ImportOpnameModel> ();
            using (var trans = TransHelper.NewScope())
            {
                PrgBar.Value = 0;
                PrgBar.Maximum = listImport.Count();

                foreach (var item in listImport)
                {
                    PrgBar.Value++;
                    if (item.Qty <= 0)
                        continue;

                    var brg = _brgDal.GetData(item.BrgCode);
                    if (brg is null)
                        continue;

                    var stokBalance = _stokBalanceBuilder.Load(brg).Build();
                    var stokAwal = stokBalance.ListWarehouse.First(x => x.WarehouseId == item.WarehouseId).Qty;
                    var opname = _opnameBuilder
                        .Create()
                        .Brg(brg)
                        .Warehouse(new WarehouseModel(item.WarehouseId))
                        .User(new UserModel("jude7"))
                        .QtyAwal(0, stokAwal)
                        .QtyOpname(0, item.Qty)
                        .Nilai(brg.Hpp)
                        .Build();
                    _opnameWriter.Save(ref opname);
                    GenStok(opname);
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
}
