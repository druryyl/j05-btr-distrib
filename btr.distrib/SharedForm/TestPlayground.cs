using System;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.UseCases;
using System.Windows.Forms;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.application.SupportContext.PlaygroundAgg;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;

namespace btr.distrib.SharedForm
{
    public partial class TestPlayground : Form
    {
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        
        private readonly IImportOpnameDal _importOpnameDal;
        private readonly IBrgDal _brgDal;
        private readonly IOpnameBuilder _opnameBuilder;
        private readonly IOpnameWriter _opnameWriter;

        public TestPlayground(
            IRemoveFifoStokWorker removeFifoStokWorker,
            IRollBackStokWorker rollBackStokWorker,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IBrgDal brgDal,
            IOpnameBuilder opnameBulilder,
            IOpnameWriter opnameWriter,
            IAddStokWorker addStokWorker,
            IImportOpnameDal importOpnameDal)
        {
            InitializeComponent();
            AddStokButton.Click += AddStokButton_Click;
            RemoveFifoButton.Click += RemoveFifo_Click;
            RollBackButton.Click += RollBackButton_Click;
            RemovePriorityButton.Click += RemovePriorityButtonOnClick;
            ImportOpnameButton.Click += ImportOpnameButton_Click;

            _removeFifoStokWorker = removeFifoStokWorker;
            _rollBackStokWorker = rollBackStokWorker;
            _removePriorityStokWorker = removePriorityStokWorker;
            _brgDal = brgDal;
            _opnameBuilder = opnameBulilder;
            _opnameWriter = opnameWriter;
            _addStokWorker = addStokWorker;
            _importOpnameDal = importOpnameDal;
        }

        private void ImportOpnameButton_Click(object sender, EventArgs e)
        {
            var listImport = _importOpnameDal.ListData()?.ToList();
            PrgBar.Maximum = listImport.Count;
            PrgBar.Value = 0;
            using (var trans = TransHelper.NewScope())
            {
                foreach (var item in listImport)
                {
                    PrgBar.Value++;
                    if (item.Qty <= 0)
                        continue;

                    var brg = _brgDal.GetData(item.BrgCode);
                    if (brg is null)
                        continue;

                    var opname = _opnameBuilder
                        .Create()
                        .Brg(brg)
                        .Warehouse(new WarehouseModel(item.Gudang))
                        .User(new UserModel("jude7"))
                        .QtyAwal(0, 0)
                        .QtyOpname(0, item.Qty)
                        .Nilai(item.Nilai)
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
                var req = new AddStokRequest(opname.BrgId, opname.WarehouseId, qtyAdjust, opname.Satuan1, opname.Nilai, opname.OpnameId, "OPNAME");
                _addStokWorker.Execute(req);
            }
        }
        private void AddStokButton_Click(object sender, System.EventArgs e)
        {
            var req3 = new AddStokRequest("BR0002", "G00", 50, "pcs", 35000, "TR-003", "ADJUST-PLUS");
            _addStokWorker.Execute(req3);
            MessageBox.Show("Done");
        }

        private void RemoveFifo_Click(object sender, System.EventArgs e)
        {
            var req = new RemoveFifoStokRequest("BR0001", "G00", 55, "pcs/40", 45000, "TR-003", "ADJUST-MIN");
            _removeFifoStokWorker.Execute(req);

            // var req2 = new RemoveFifoStokRequest("BR0001", "G00", 80, "pcs/40", 45000, "TR-004", "ADJUST-MIN");
            // _removeFifoStokWorker.Execute(req2);
            MessageBox.Show("Done");
        }
        private void RollBackButton_Click(object sender, System.EventArgs e)
        {
            var req = new RollBackStokRequest("TR-003");
            _rollBackStokWorker.Execute(req);
            MessageBox.Show("Done");
        }

        private void RemovePriorityButtonOnClick(object sender, EventArgs e)
        {
            var req = new RemovePriorityStokRequest("BR0001", "G00", 55, "pcs/40", 45000, "TR-003", "ADJUST-MIN", "TR-002");
            _removePriorityStokWorker.Execute(req);
            MessageBox.Show("Done");
        }
    }
}