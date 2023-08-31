using btr.application.InventoryContext.StokAgg;
using MediatR;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class TestPlayground : Form
    {
        public readonly IMediator _mediator;
        public TestPlayground(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            var cmd = new AddStokCommand("BR0001", "W19", 30, "PCS/40", 1900, "ADJ-001", "ADJUSTMENT");
            await _mediator.Send(cmd);
        }
    }
}