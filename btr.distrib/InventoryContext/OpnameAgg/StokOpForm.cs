using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public partial class StokOpForm : Form
    {
        private BindingList<StokOpItem> _listItem;
        private BindingSource _bindingSource;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        
        private readonly IKategoriDal _kategoriDal;
        private readonly IWarehouseDal _warehouseDal;
        
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly ISaveStokOpWorker _saveStokOpWorker;
        private readonly IStokOpDal _stokOpDal;
        
        public StokOpForm(IBrgDal brgDal, 
            IKategoriDal kategoriDal, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgSatuanDal brgSatuanDal, 
            IWarehouseDal warehouseDal, 
            ISaveStokOpWorker saveStokOpWorker, 
            IStokOpDal stokOpDal)
        {
            InitializeComponent();
            _brgDal = brgDal;
            _kategoriDal = kategoriDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgSatuanDal = brgSatuanDal;
            _warehouseDal = warehouseDal;
            _saveStokOpWorker = saveStokOpWorker;
            _stokOpDal = stokOpDal;
            _listItem = new BindingList<StokOpItem>();
            _bindingSource = new BindingSource
            {
                DataSource = _listItem
            };
            
            RegisterEventHandler(); 
            InitKetegoriCombo();
            InitWarehouseCombo();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            ListBrgButton.Click += ListBrgButton_Click;
            BrgGrid.CellValueChanged += BrgGrid_CellValueChanged;
        }

        private void BrgGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var grid = (DataGridView)sender;
            var qtyBesarColIndex = grid.Columns.GetCol("QtyBesarOpname").Index;
            var qtyKecilColIndex = grid.Columns.GetCol("QtyKecilOpname").Index;
            if (e.ColumnIndex == qtyBesarColIndex || e.ColumnIndex == qtyKecilColIndex)
            {
                var reqStokOp = new SaveStokOpRequest
                {
                    BrgId = _listItem[e.RowIndex].BrgId,
                    StokOpId = _listItem[e.RowIndex].StokOpId,
                    WarehouseId = WarehouseCombo.SelectedValue.ToString(),
                    PeriodeOp = PeriodeOpText.Value.Date,
                    QtyBesar = _listItem[e.RowIndex].QtyBesarOpname,
                    QtyKecil = _listItem[e.RowIndex].QtyKecilOpname,
                };
                var stokOp = _saveStokOpWorker.Execute(reqStokOp);

                _listItem[e.RowIndex].StokOpId = stokOp.StokOpId;
                _listItem[e.RowIndex].QtyBesarAwal = stokOp.QtyBesarAwal;
                _listItem[e.RowIndex].QtyKecilAwal = stokOp.QtyKecilAwal;
                _listItem[e.RowIndex].QtyPcsAwal = stokOp.QtyPcsAwal;
                
                _listItem[e.RowIndex].QtyBesarAdjust = stokOp.QtyBesarAdjust;
                _listItem[e.RowIndex].QtyKecilAdjust = stokOp.QtyKecilAdjust;
                _listItem[e.RowIndex].QtyPcsAdjust = stokOp.QtyPcsAdjust;

                BrgGrid.EndEdit();
                BrgGrid.Refresh();
            }
        }

        private void ListBrgButton_Click(object sender, EventArgs e)
        {
            ListBrg();
        }

        private void InitKetegoriCombo()
        {
            var listKategori = _kategoriDal.ListData()?.ToList()
                ?? new List<KategoriModel>();
            var ds = listKategori.OrderBy(x => x.KategoriName).ToList();
            KategoriCombo.DataSource = ds;
            KategoriCombo.DisplayMember = "KategoriName";
            KategoriCombo.ValueMember = "KategoriId";
        }
        
        private void InitWarehouseCombo()
        {
            var listWarehouse = _warehouseDal.ListData();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }
        
        private void ListBrg()
        {
            var kategori = new KategoriModel(KategoriCombo.SelectedValue.ToString());
            var listBrg = _brgDal.ListData(kategori)?.ToList()
                ?? new List<BrgModel>();
            var listConversion = _brgSatuanDal.ListData(kategori)?.ToList()
                ?? new List<BrgSatuanModel>();
            var listStokOp = _stokOpDal.ListData(new Periode(PeriodeOpText.Value.Date),
                    new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))
                ?.ToList() ?? new List<StokOpModel>();
            
            var listBrgItem = listBrg
                .OrderBy(x => x.BrgCode)
                .Select(x =>
                {
                    var stokOp = listStokOp.FirstOrDefault(y => y.BrgId == x.BrgId);
                    return new StokOpItem
                    {
                        BrgId = x.BrgId,
                        BrgCode = x.BrgCode,
                        BrgName = x.BrgName,
                        QtyBesarAwal = stokOp?.QtyBesarAwal ?? 0,
                        QtyKecilAwal = stokOp?.QtyKecilAwal ?? 0,
                        QtyPcsAwal = stokOp?.QtyPcsAwal ?? 0,
                        
                        QtyBesarAdjust = stokOp?.QtyBesarAdjust ?? 0,
                        QtyKecilAdjust = stokOp?.QtyKecilAdjust ?? 0,
                        QtyPcsAdjust = stokOp?.QtyPcsAdjust ?? 0,
                        
                        QtyBesarOpname = stokOp?.QtyBesarOpname ?? 0,
                        QtyKecilOpname = stokOp?.QtyKecilOpname ?? 0,
                        QtyPcsOpname = stokOp?.QtyPcsOpname ?? 0,
                        
                        StokOpId = stokOp?.StokOpId ?? "",
                        Conversion = listConversion
                            .Where(y => y.BrgId == x.BrgId)
                            .DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 })
                            .Max(y => y.Conversion),
                    };
                }).ToList();
            _listItem = new BindingList<StokOpItem>(listBrgItem);
            var warehouse = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var listQty = _stokBalanceWarehouseDal.ListData(warehouse)?.ToList()
                ?? new List<StokBalanceWarehouseModel>();

            foreach(var item in _listItem)
            {
                var qty = listQty.FirstOrDefault(x => x.BrgId == item.BrgId);
                if (qty == null) continue;
                item.QtyBesarAwal = (int)Math.Floor((decimal)qty.Qty / item.Conversion);
                item.QtyKecilAwal = qty.Qty - (item.QtyBesarAwal * item.Conversion);
                item.QtyPcsAwal = qty.Qty;
                
                if (item.Conversion != 1) continue;
                item.QtyKecilAwal = item.QtyBesarAwal;
                item.QtyBesarAwal = 0;
            }
            _bindingSource.DataSource = _listItem;
            BrgGrid.DataSource = _bindingSource;
            BrgGrid.Refresh();
        }    
        
        private void InitGrid()
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _listItem;
            BrgGrid.DataSource = bindingSource;
            BrgGrid.Refresh();

            var col = BrgGrid.Columns;
            col.SetDefaultCellStyle(Color.Beige);

            col.GetCol("QtyPcsAwal").Visible = false;
            col.GetCol("QtyPcsAdjust").Visible = false;
            col.GetCol("QtyPcsOpname").Visible = false;
            col.GetCol("Conversion").Visible = false;

            foreach (DataGridViewColumn c in BrgGrid.Columns)
            {
                c.ReadOnly = true;
                c.DefaultCellStyle.BackColor = Color.PowderBlue;
            }
            col.GetCol("QtyBesarOpname").ReadOnly = false;
            col.GetCol("QtyKecilOpname").ReadOnly = false;
            col.GetCol("QtyBesarOpname").DefaultCellStyle.BackColor = Color.White;
            col.GetCol("QtyKecilOpname").DefaultCellStyle.BackColor = Color.White;

            col.GetCol("BrgId").HeaderText = @"Id";
            col.GetCol("BrgCode").HeaderText = @"Brg Code";
            col.GetCol("BrgName").HeaderText = @"Brg Name";
            col.GetCol("QtyBesarAwal").HeaderText = @"Qty-B Awal"; 
            col.GetCol("QtyKecilAwal").HeaderText = @"Qty-K Awal"; 
            col.GetCol("QtyBesarAdjust").HeaderText = @"Qty-B Adjust"; 
            col.GetCol("QtyKecilAdjust").HeaderText = @"Qty-K Adjust"; 
            col.GetCol("QtyBesarOpname").HeaderText = @"Qty-B Opname"; 
            col.GetCol("QtyKecilOpname").HeaderText = @"Qty-K Opname";
            col.GetCol("StokOpId").HeaderText = @"Stok-Op ID";

            col.GetCol("BrgId").Width = 80;
            col.GetCol("BrgCode").Width = 80;
            col.GetCol("BrgName").Width = 200;
            col.GetCol("QtyBesarAwal").Width = 70;
            col.GetCol("QtyKecilAwal").Width = 70;
            col.GetCol("QtyBesarAdjust").Width = 70;
            col.GetCol("QtyKecilAdjust").Width = 70;
            col.GetCol("QtyBesarOpname").Width = 70;
            col.GetCol("QtyKecilOpname").Width = 70;
        }
    }

    public class StokOpItem
    {
        public string BrgId { get;  set; }
        public string BrgCode { get;  set; }
        public string BrgName { get;  set; }
        
        public int QtyBesarAwal { get;  set; }
        public int QtyKecilAwal { get;  set; }
        public int QtyPcsAwal { get;  set; }
        
        public int QtyBesarAdjust { get;  set; }
        public int QtyKecilAdjust { get;  set; }
        public int QtyPcsAdjust { get; set; }
        
        public int QtyBesarOpname { get; set; }
        public int QtyKecilOpname { get; set; }
        public int QtyPcsOpname { get; set; }
        
        public string StokOpId { get; set; }
        public int Conversion { get; set; }
    }

}
