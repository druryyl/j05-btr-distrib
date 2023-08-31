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
            //var cmd = new AddStokCommand("BR0001", "W19", 10, "PCS/40", 1900, "ADJ-001", "ADJUSTMENT");
            //await _mediator.Send(cmd);

            //var cmd2 = new AddStokCommand("BR0001", "W19", 4, "PCS/40", 1900, "ADJ-001", "ADJUSTMENT");
            //await _mediator.Send(cmd2);


            //var cmd3 = new RemoveStokCommand("BR0001", "W19",13, "PCS/40", 2100, "FAKTUR-001", "FAKTUR");
            //await _mediator.Send(cmd3);


        }
    }
}