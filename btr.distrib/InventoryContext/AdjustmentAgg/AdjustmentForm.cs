using btr.application.BrgContext.BrgAgg;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.AdjustmentAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.nuna.Application;
using JetBrains.Annotations;

namespace btr.distrib.InventoryContext.AdjustmentAgg
{
    public partial class AdjustmentForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IKategoriDal _kategoriDal;
        private readonly IBrgDal _brgDal;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        private readonly IAdjustmentBuilder _adjustmentBuilder;
        private readonly IAdjustmentWriter _adjustmentWriter;
        private readonly IGenStokAdjustmentWorker _genStokAdjustmentWorker;

        private List<BrgModel> _listBrg;
        private readonly IBrgBuilder _brgBuilder;
        private BrgModel _brg;

        public AdjustmentForm(IWarehouseDal warehouseDal,
            IKategoriDal kategoriDal,
            IBrgDal brgDal,
            IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgBuilder brgBuilder, 
            IAdjustmentBuilder adjustmentBuilder, 
            IAdjustmentWriter adjustmentWriter, 
            IGenStokAdjustmentWorker genStokAdjustmentWorker)
        {
            InitializeComponent();
            _warehouseDal = warehouseDal;
            _kategoriDal = kategoriDal;
            _brgDal = brgDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgBuilder = brgBuilder;
            _adjustmentBuilder = adjustmentBuilder;
            _adjustmentWriter = adjustmentWriter;
            _genStokAdjustmentWorker = genStokAdjustmentWorker;

            ListStokButton.Click += ListStokButton_Click;
            StokGrid.CellDoubleClick += StokGrid_CellDoubleClick;
            SaveButton.Click += SaveButton_Click;
            QtyAdjustBesarText.ValueChanged += QtyAdjustText_ValueChanged;
            QtyAdjustKecilText.ValueChanged += QtyAdjustText_ValueChanged;
            
            InitWarehouseComboBox();
            InitKategoirComboBox();
            InitStokGrid();
            
            //  disable updown control in numeric text box
            QtyAwalBesarText.Controls[0].Enabled = false;
            QtyAwalKecilText.Controls[0].Enabled = false;
            QtyAwalInPcsText.Controls[0].Enabled = false;
            
            QtyAkhirBesarText.Controls[0].Enabled = false;
            QtyAkhirKecilText.Controls[0].Enabled = false;
            QtyAkhirInPcsText.Controls[0].Enabled = false;
        }

        private void QtyAdjustText_ValueChanged(object sender, EventArgs e)
        {
            if (_brg is null)
                return;
            var conversion = _brg.ListSatuan?.DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 }).Max(x => x.Conversion) ?? 1;
            var qtyBesar = conversion == 1 ? 0: QtyAdjustBesarText.Value;
            var qtyKecil = QtyAdjustKecilText.Value;
            
            var qtyInPcs = qtyBesar * conversion + qtyKecil;
            
            QtyAdjustBesarText.Value = qtyBesar;
            QtyAdjustInPcsText.Value = qtyInPcs;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (AlasanText.Text.Length == 0)
            {
                MessageBox.Show(@"Alasan harus diisi");
                return;
            }

            if (QtyAdjustInPcsText.Value == 0)
            {
                MessageBox.Show(@"Qty Adjustment harus diisi");
                return;
            }
            
            var adjustment = _adjustmentBuilder
                .Create()
                .AdjustmentDate(DateTime.Now)
                .Brg(new BrgModel(BrgIdText.Text))
                .Warehouse(new WarehouseModel(WarehouseIdText.Text))
                .Alasan(AlasanText.Text)
                .QtyAdjustInPcs((int)QtyAdjustInPcsText.Value)
                .Build();
            
            using (var trans = TransHelper.NewScope())
            {
                var result = _adjustmentWriter.Save(adjustment);
                _genStokAdjustmentWorker.Execute(new GenStokAdjustmentRequest(result.AdjustmentId));
                trans.Complete();
            }
            ClearForm();
        }

        private void ClearForm()
        {
            BrgIdText.Clear();
            BrgCodeText.Clear();
            BrgNameText.Clear();
            WarehouseIdText.Clear();
            WarehouseNameText.Clear();
            AlasanText.Clear();
            QtyAwalBesarText.Value = 0;
            QtyAwalKecilText.Value = 0;
            QtyAwalInPcsText.Value = 0;
            
            QtyAdjustBesarText.Value = 0;
            QtyAdjustKecilText.Value = 0;
            QtyAdjustInPcsText.Value = 0;
            
            QtyAkhirBesarText.Value = 0;
            QtyAkhirKecilText.Value = 0;
            QtyAkhirInPcsText.Value = 0;
            
            ListStok();
        }

        private void StokGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var brgKey = new BrgModel(StokGrid.Rows[e.RowIndex].Cells["BrgId"].Value.ToString());
            _brg = _brgBuilder.Load(brgKey).Build();
            var stok = _stokBalanceWarehouseDal.ListData(brgKey)?.ToList() ?? new List<StokBalanceWarehouseModel>();
            var stokInPcs = stok.FirstOrDefault(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())?.Qty ?? 0;
            var conversion = _brg.ListSatuan?.DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 }).Max(x => x.Conversion) ?? 1;
            var stokBesar = conversion == 1 ? 0 : stokInPcs / conversion;
            var stokKecil = conversion == 1 ? stokInPcs : stokInPcs % conversion;

            var warehouseId = WarehouseCombo.SelectedValue.ToString();
            var warehouse = _warehouseDal.GetData(new WarehouseModel(warehouseId))
                ?? throw new KeyNotFoundException("Warehouse not found");
            
            BrgIdText.Text = brgKey.BrgId;
            BrgCodeText.Text = _brg.BrgCode;
            BrgNameText.Text = _brg.BrgName;
            WarehouseIdText.Text = warehouse.WarehouseId;
            WarehouseNameText.Text = warehouse.WarehouseName;
            QtyAwalBesarText.Value = stokBesar;
            QtyAwalKecilText.Value = stokKecil;
            QtyAwalInPcsText.Value = stokInPcs;
        }

        private void ListStokButton_Click(object sender, EventArgs e)
        {
            ListStok();
        }

        private void ListStok()
        {
            var listStok = _stokBalanceWarehouseDal.ListData(new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))?.ToList()
                ?? new List<StokBalanceWarehouseModel>();

            List<BrgModel> listBrgResult;
            if (SearchText.Text.Length > 0)
            {
                var listNamaBrg = _listBrg.Where(x => x.BrgName.ToLower().ContainMultiWord(SearchText.Text.ToLower())).ToList();
                var listKodeBrg = _listBrg.Where(x => x.BrgCode.ToLower().StartsWith(SearchText.Text.ToLower())).ToList();
                listBrgResult = listNamaBrg.Union(listKodeBrg).ToList();
            }
            else
            {
                listBrgResult = _listBrg.Where(x => x.KategoriId == KategoriCombo.SelectedValue.ToString()).ToList();
            }
        
            var warehouseId = WarehouseCombo.SelectedValue.ToString();
            var warehouseName = WarehouseCombo.SelectedItem.ToString();
            var listDataSource = (
                from brg in listBrgResult
                let brgModel = _brgBuilder.Load(brg).Build()
                let stokInPcs = listStok.FirstOrDefault(x => x.BrgId == brg.BrgId)?.Qty ?? 0
                let conversion = brgModel.ListSatuan?.DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 }).Max(x => x.Conversion) ?? 1
                let stokBesar = conversion == 1 ? 0 : stokInPcs / conversion
                let stokKecil = conversion == 1 ? stokInPcs : stokInPcs % conversion
                let satBesar = brgModel.ListSatuan?.Where(x => x.Conversion > 1).OrderByDescending(x => x.Conversion).FirstOrDefault()?.Satuan ?? string.Empty
                let satKecil = brgModel.ListSatuan?.Where(x => x.Conversion == 1).OrderByDescending(x => x.Conversion).FirstOrDefault()?.Satuan ?? string.Empty
                select new AdjustmentDto
                {
                    BrgId = brg.BrgId,
                    BrgCode = brg.BrgCode,
                    BrgName = brg.BrgName,
                    KategoriId = brg.KategoriId,
                    KategoriName = brg.KategoriName,
                    WarehouseId = warehouseId,
                    WarehouseName = warehouseName,
                    StokBesar = stokBesar,
                    SatBesar = satBesar,
                    StokKecil = stokKecil,
                    SatKecil = satKecil,
                    StokInPcs = stokInPcs,
                }).ToList();

            StokGrid.DataSource = listDataSource;
            StokGrid.Refresh();
        }

        private void InitKategoirComboBox()
        {
            var listKategori = _kategoriDal.ListData()?? new List<KategoriModel>();
            KategoriCombo.DataSource = listKategori.OrderBy(x => x.KategoriName).ToList();
            KategoriCombo.DisplayMember = "KategoriName";
            KategoriCombo.ValueMember = "KategoriId";
            KategoriCombo.SelectedIndex = 0;
        }

        private void InitWarehouseComboBox()
        {
            var listWh = _warehouseDal.ListData();
            WarehouseCombo.DataSource = listWh.OrderBy(x => x.WarehouseName).ToList();
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }

        private void InitStokGrid()
        {
            var listStok = new List<AdjustmentDto>();
            StokGrid.DataSource = listStok;
            var grid = StokGrid.Columns;
            grid.GetCol("BrgId").Width = 50;
            grid.GetCol("BrgCode").Width = 80;
            grid.GetCol("BrgName").Width = 180;
            grid.GetCol("KategoriId").Width = 50;
            grid.GetCol("KategoriName").Width = 100;
            grid.GetCol("WarehouseId").Width = 50;
            grid.GetCol("WarehouseName").Width = 100;
            grid.GetCol("StokBesar").Width = 50;
            grid.GetCol("SatBesar").Width = 50;
            grid.GetCol("StokKecil").Width = 50;
            grid.GetCol("SatKecil").Width = 50;
            grid.GetCol("StokInPcs").Width = 50;

            grid.GetCol("BrgId").Visible = false;
            grid.GetCol("KategoriId").Visible = false;
            grid.GetCol("WarehouseId").Visible = false;
            grid.GetCol("WarehouseName").Visible = false;
            

            grid.GetCol("StokBesar").DefaultCellStyle.Format = "#,##";
            grid.GetCol("StokKecil").DefaultCellStyle.Format = "#,##";
            grid.GetCol("StokInPcs").DefaultCellStyle.Format = "#,##";

            grid.GetCol("StokBesar").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.GetCol("StokKecil").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.GetCol("StokInPcs").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grid.GetCol("StokBesar").DefaultCellStyle.BackColor = Color.LightGreen;
            grid.GetCol("StokKecil").DefaultCellStyle.BackColor = Color.LightGreen;
            grid.GetCol("StokInPcs").DefaultCellStyle.BackColor = Color.LightGreen;
            
            grid.GetCol("StokBesar").DefaultCellStyle.Font = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            grid.GetCol("StokKecil").DefaultCellStyle.Font = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            grid.GetCol("StokInPcs").DefaultCellStyle.Font = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            

            
            // set caption StokBesar to Qty 1
            grid.GetCol("StokBesar").HeaderText = @"Qty Besar";
            grid.GetCol("StokKecil").HeaderText = @"Qty Kecil";
            grid.GetCol("StokInPcs").HeaderText = @"In Pcs";
            
            _listBrg = _brgDal.ListData()?.ToList() ?? new List<BrgModel>();
        }
    }
    
    [PublicAPI]
    public class AdjustmentDto
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string KategoriId { get; set; }
        public string KategoriName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int StokBesar { get; set; }
        public string SatBesar { get; set; }
        public int StokKecil { get; set; }
        public string SatKecil { get; set; }
        public int StokInPcs { get; set; }
    }
}
