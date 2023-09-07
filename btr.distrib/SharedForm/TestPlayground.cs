using System;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.UseCases;
using System.Windows.Forms;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;

namespace btr.distrib.SharedForm
{
    public partial class TestPlayground : Form
    {
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        public TestPlayground(IAddStokWorker addStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker,
            IRollBackStokWorker rollBackStokWorker, 
            IRemovePriorityStokWorker removePriorityStokWorker)
        {
            InitializeComponent();
            AddStokButton.Click += AddStokButton_Click;
            RemoveFifoButton.Click += RemoveFifo_Click;
            RollBackButton.Click += RollBackButton_Click;
            RemovePriorityButton.Click += RemovePriorityButtonOnClick;
            _addStokWorker = addStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
            _rollBackStokWorker = rollBackStokWorker;
            _removePriorityStokWorker = removePriorityStokWorker;
        }


        private void AddStokButton_Click(object sender, System.EventArgs e)
        {
            var req = new AddStokRequest("BR0001", "G00", 100, "pcs/40", 40000, "TR-001", "ADJUST-PLUS");
            _addStokWorker.Execute(req);

            var req2 = new AddStokRequest("BR0001", "G00", 70, "pcs/40", 40000, "TR-002", "ADJUST-PLUS");
            _addStokWorker.Execute(req2);
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
            var req = new RollBackStokRequest("BR0001", "G00", "TR-003");
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