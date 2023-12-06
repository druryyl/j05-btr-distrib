using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using JetBrains.Annotations;

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public partial class StokOpForm : Form
    {
        private BindingList<StokOpItem> _listItem;
        private readonly BindingSource _bindingSource;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        
        private readonly IKategoriDal _kategoriDal;
        private readonly IWarehouseDal _warehouseDal;
        
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly ISaveStokOpWorker _saveStokOpWorker;
        private readonly IStokOpDal _stokOpDal;
        private readonly IBrgBuilder _brgBuilder;
        
        public StokOpForm(IBrgDal brgDal, 
            IKategoriDal kategoriDal, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgSatuanDal brgSatuanDal, 
            IWarehouseDal warehouseDal, 
            ISaveStokOpWorker saveStokOpWorker, 
            IStokOpDal stokOpDal, 
            IBrgBuilder brgBuilder)
        {
            InitializeComponent();
            _brgDal = brgDal;
            _kategoriDal = kategoriDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgSatuanDal = brgSatuanDal;
            _warehouseDal = warehouseDal;
            _saveStokOpWorker = saveStokOpWorker;
            _stokOpDal = stokOpDal;
            _brgBuilder = brgBuilder;
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
        
        #region GRID
        private void BrgGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var grid = (DataGridView)sender;

            if (e.ColumnIndex == grid.Columns.GetCol("QtyOpnameInputStr").Index)
            {
                NormalizeInput(_listItem[e.RowIndex].QtyOpnameInputStr);
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

        private void NormalizeInput(string qtyOpnameInputStr)
        {
            var brg = _brgBuilder.Load(new BrgModel(_listItem[BrgGrid.CurrentCell.RowIndex].BrgId)).Build();
            var conversion = brg.ListSatuan
                .DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 })
                .Max(x => x.Conversion);

            var qtys = ParseStringMultiNumber(qtyOpnameInputStr,2);

            if (qtys[1] > conversion)
            {
                var addedQty = Convert.ToInt16(qtys[1] / conversion);
                qtys[0] += addedQty;
                qtys[1] -= addedQty * conversion;
            }
            
            
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyBesarOpname = qtys[0];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyKecilOpname = qtys[1];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyPcsOpname = (qtys[0] * conversion) + qtys[1];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyOpnameInputStr = $"{qtys[0]};{qtys[1]}";
        }

        private static List<int> ParseStringMultiNumber(string str, int size)
        {
            if (str is null)
                str = string.Empty;

            var result = new List<int>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr.TakeWhile(item => x < result.Count))
            {
                if (int.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }

        private void ListBrg()
        {
            var listBrgItem = GenListBrg();
            listBrgItem = SetStokAwalAndConversion(listBrgItem);
            listBrgItem = UpdateFromPreviousInput(listBrgItem);

            _listItem = new BindingList<StokOpItem>(listBrgItem.ToList());
            _bindingSource.DataSource = _listItem;
            BrgGrid.DataSource = _bindingSource;
            BrgGrid.Refresh();
        }

        private IEnumerable<StokOpItem> GenListBrg()
        {
            var kategori = new KategoriModel(KategoriCombo.SelectedValue.ToString());
            var listBrg = _brgDal.ListData(kategori)?.ToList()
                          ?? new List<BrgModel>();

            var listBrgItem = listBrg
                .OrderBy(x => x.BrgCode)
                .Select(x => new StokOpItem
                {
                    BrgId = x.BrgId,
                    BrgCode = x.BrgCode,
                    BrgName = x.BrgName,
                    QtyBesarAwal = 0,
                    QtyKecilAwal =  0,
                    QtyPcsAwal =  0, 
                        
                    QtyBesarAdjust =  0,
                    QtyKecilAdjust =  0,
                    QtyPcsAdjust =  0,
                        
                    QtyBesarOpname =  0,
                    QtyKecilOpname =  0,
                    QtyPcsOpname =  0,
                        
                    QtyOpnameInputStr = string.Empty,
                    StokOpId = string.Empty,
                    Conversion = 1,
                }).ToList();
            return listBrgItem;
        }
        
        private IEnumerable<StokOpItem> SetStokAwalAndConversion(IEnumerable<StokOpItem> listBrgItem)
        {
            var warehouse = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var listQty = _stokBalanceWarehouseDal.ListData(warehouse)?.ToList()
                          ?? new List<StokBalanceWarehouseModel>();
            var kategori = new KategoriModel(KategoriCombo.SelectedValue.ToString());
            var listConversion = _brgSatuanDal.ListData(kategori)?.ToList()
                                 ?? new List<BrgSatuanModel>();
            var fetchedListBrg = listBrgItem.ToList();
            foreach (var item in fetchedListBrg)
            {
                item.Conversion = listConversion
                    .Where(y => y.BrgId == item.BrgId)
                    .DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 })
                    .Max(y => y.Conversion);
                
                var qty = listQty.FirstOrDefault(x => x.BrgId == item.BrgId);
                if (qty == null) continue;
                
                item.QtyBesarAwal = (int)Math.Floor((decimal)qty.Qty / item.Conversion);
                item.QtyKecilAwal = qty.Qty - (item.QtyBesarAwal * item.Conversion);
                item.QtyPcsAwal = qty.Qty;

                item.QtyBesarOpname = (int)Math.Floor((decimal)qty.Qty / item.Conversion);
                item.QtyKecilOpname = qty.Qty - (item.QtyBesarAwal * item.Conversion);
                item.QtyPcsOpname = qty.Qty;
                

                if (item.Conversion != 1) continue;
                item.QtyKecilAwal = item.QtyBesarAwal;
                item.QtyBesarAwal = 0;
            }

            return fetchedListBrg;
        }

        private IEnumerable<StokOpItem> UpdateFromPreviousInput(IEnumerable<StokOpItem> listBrgItem2)
        {
            var listStokOp = _stokOpDal.ListData(new Periode(PeriodeOpText.Value.Date),
                    new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))
                ?.ToList() ?? new List<StokOpModel>();
            var fetchedListBrg = listBrgItem2.ToList();
            foreach (var item in fetchedListBrg)
            {
                var stokOp = listStokOp.FirstOrDefault(y => y.BrgId == item.BrgId);
                if (stokOp == null) continue;
                
                item.QtyBesarAwal = stokOp.QtyBesarAwal;
                item.QtyKecilAwal = stokOp.QtyKecilAwal;
                item.QtyPcsAwal = stokOp.QtyPcsAwal;
                
                item.QtyBesarAdjust = stokOp.QtyBesarAdjust;
                item.QtyKecilAdjust = stokOp.QtyKecilAdjust;
                item.QtyPcsAdjust = stokOp.QtyPcsAdjust;
                
                item.QtyBesarOpname = stokOp.QtyBesarOpname;
                item.QtyKecilOpname = stokOp.QtyKecilOpname;
                item.QtyPcsOpname = stokOp.QtyPcsOpname;
                item.QtyOpnameInputStr = stokOp.QtyOpnameInputStr;
                item.StokOpId = stokOp.StokOpId;
            }

            return fetchedListBrg;
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
            col.GetCol("QtyOpnameInputStr").ReadOnly = false;
            
            col.GetCol("QtyBesarAwal").DefaultCellStyle.BackColor = Color.LemonChiffon; 
            col.GetCol("QtyKecilAwal").DefaultCellStyle.BackColor = Color.LemonChiffon;
            col.GetCol("QtyBesarAdjust").DefaultCellStyle.BackColor = Color.Pink;
            col.GetCol("QtyKecilAdjust").DefaultCellStyle.BackColor = Color.Pink;
            col.GetCol("QtyBesarOpname").DefaultCellStyle.BackColor = Color.LemonChiffon;
            col.GetCol("QtyKecilOpname").DefaultCellStyle.BackColor = Color.LemonChiffon;
            col.GetCol("QtyOpnameInputStr").DefaultCellStyle.BackColor = Color.White;
            
            col.GetCol("BrgId").HeaderText = @"Id";
            col.GetCol("BrgCode").HeaderText = @"Brg Code";
            col.GetCol("BrgName").HeaderText = @"Brg Name";
            col.GetCol("QtyBesarAwal").HeaderText = @"Qty-B Awal"; 
            col.GetCol("QtyKecilAwal").HeaderText = @"Qty-K Awal"; 
            col.GetCol("QtyBesarAdjust").HeaderText = @"Qty-B Adjust"; 
            col.GetCol("QtyKecilAdjust").HeaderText = @"Qty-K Adjust"; 
            col.GetCol("QtyBesarOpname").HeaderText = @"Qty-B Akhir"; 
            col.GetCol("QtyKecilOpname").HeaderText = @"Qty-K Akhir";
            col.GetCol("StokOpId").HeaderText = @"Stok-Op ID";
            col.GetCol("QtyOpnameInputStr").HeaderText = @"Qty Opname";

            col.GetCol("BrgId").Width = 80;
            col.GetCol("BrgCode").Width = 80;
            col.GetCol("BrgName").Width = 200;
            col.GetCol("QtyBesarAwal").Width = 50;
            col.GetCol("QtyKecilAwal").Width = 50;
            col.GetCol("QtyBesarAdjust").Width = 50;
            col.GetCol("QtyKecilAdjust").Width = 50;
            col.GetCol("QtyBesarOpname").Width = 50;
            col.GetCol("QtyKecilOpname").Width = 50;
            col.GetCol("QtyOpnameInputStr").Width = 70;
        }
        #endregion
    }

    [PublicAPI]
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
        public string QtyOpnameInputStr { get; set; }
        
        public string StokOpId { get; set; }
        public int Conversion { get; set; }
    }

}
