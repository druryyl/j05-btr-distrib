using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;

namespace btr.distrib.SharedForm
{
    public partial class StokBalancerForm : Form
    {
        private readonly IBrgDal _brgDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IGenStokBalanceWorker _genStokBalanceWorker;

        public StokBalancerForm(IBrgDal brgDal, IWarehouseDal warehouseDal, IGenStokBalanceWorker genStokBalanceWorker)
        {
            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            InitializeComponent();
            _genStokBalanceWorker = genStokBalanceWorker;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (textBox1.Text == string.Empty)
                GenAll();
            else
                GenSatuBrg(textBox1.Text);
        }

        private void GenSatuBrg(string brgId)
        {
            var listWareHouse = _warehouseDal.ListData();
            foreach (var warehouse in listWareHouse)
            {
                label1.Text = $"Processing {brgId} - {warehouse.WarehouseId}";
                Debug.WriteLine($"Processing {brgId} - {warehouse.WarehouseId}");
                this.Invalidate();
                _genStokBalanceWorker.Execute(new GenStokBalanceRequest(brgId, warehouse.WarehouseId));
            }
            label1.Text = "Done";
        }
        private void GenAll()
        {
            var listWareHouse = _warehouseDal.ListData();
            var listBrg = _brgDal.ListData();
            foreach (var warehouse in listWareHouse)
            {
                foreach (var brg in listBrg)
                {
                    label1.Text = $"Processing {brg.BrgId} - {warehouse.WarehouseId}";
                    Debug.WriteLine($"Processing {brg.BrgId} - {warehouse.WarehouseId}");
                    this.Invalidate();
                    _genStokBalanceWorker.Execute(new GenStokBalanceRequest(brg.BrgId, warehouse.WarehouseId));
                }
            }
            label1.Text = "Done";
        }

    }
}