using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.domain.InventoryContext.WarehouseAgg;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.PrintDocs
{
    public partial class PrintManagerForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        public PrintManagerForm(IWarehouseDal warehouseDal)
        {
            InitializeComponent();
            _warehouseDal = warehouseDal;

            InitWarehouse();
        }

        private void InitWarehouse()
        {
            var listWarehouse = _warehouseDal.ListData()?.ToList()
                ?? new List<WarehouseModel>();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }

    }
}
