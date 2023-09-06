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
        private readonly IMediator _mediator;
        private readonly IOpnameBuilder _opnameBuilder;
        private readonly IOpnameWriter _opnameWriter;
        private readonly IImportOpnameDal _importOpnameDal;
        private readonly IBrgDal _brgDal;

        public TestPlayground(IMediator mediator,
            IOpnameBuilder opnameBuilder,
            IOpnameWriter opnameWriter,
            IImportOpnameDal importOpnameDal,
            IBrgDal brgDal)
        {
            InitializeComponent();
            _mediator = mediator;
            _opnameBuilder = opnameBuilder;
            _opnameWriter = opnameWriter;
            _importOpnameDal = importOpnameDal;
            _brgDal = brgDal;
            button1.Click += button1_Click;
        }


        private async void button1_Click(object sender, System.EventArgs e)
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

                    var opname = _opnameBuilder
                        .Create()
                        .Brg(brg)
                        .Warehouse(new WarehouseModel("G00"))
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

            //var cmd = new AddStokCommand("BR0001", "W19", 10, "PCS/40", 1900, "ADJ-001", "ADJUSTMENT");
            //await _mediator.Send(cmd);

            //var cmd2 = new AddStokCommand("BR0001", "W19", 4, "PCS/40", 1900, "ADJ-001", "ADJUSTMENT");
            //await _mediator.Send(cmd2);


            //var cmd3 = new RemoveStokCommand("BR0001", "W19", 13, "PCS/40", 2100, "FAKTUR-001", "FAKTUR");
            //await _mediator.Send(cmd3);
        }
        private async void GenStok(OpnameModel opname)
        {
            var qtyAdjust = opname.Qty2Adjust * opname.Conversion2;
            qtyAdjust += opname.Qty1Adjust;

            if (qtyAdjust > 0)
            {
                var cmd = new AddStokCommand(opname.BrgId, opname.WarehouseId,
                    qtyAdjust, opname.Satuan1, opname.Nilai, opname.OpnameId, "OPNAME");
                await _mediator.Send(cmd);
            }
            else
            {
                var cmd = new RemoveStokCommand(opname.BrgId, opname.WarehouseId,
                    -qtyAdjust, opname.Satuan1, 0, opname.OpnameId, "OPNAME");
                await _mediator.Send(cmd);
            }
        }
    }
}