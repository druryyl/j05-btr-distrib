using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokAgg;
using btr.application.SupportContext.PlaygroundAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using MediatR;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace btr.distrib.SharedForm
{
    public partial class TestPlayground : Form
    {
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;

        public TestPlayground(IAddStokWorker addStokWorker, 
            IRemoveFifoStokWorker removeFifoStokWorker)
        {
            InitializeComponent();
            AddStokButton.Click += AddStokButton_Click;
            RemoveFifoButton.Click += RemoveFifo_Click;
            _addStokWorker = addStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
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

            var req2 = new RemoveFifoStokRequest("BR0001", "G00", 80, "pcs/40", 45000, "TR-004", "ADJUST-MIN");
            _removeFifoStokWorker.Execute(req2);
            MessageBox.Show("Done");
        }

    }
}