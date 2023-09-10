using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.JenisBrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.BrgContext.HargaTypeAgg;
using btr.application.BrgContext.JenisBrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using Polly;
using btr.nuna.Domain;
using System.Drawing;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.WarehouseAgg;

namespace btr.distrib.InventoryContext.BrgAgg
{
    public partial class BrgForm : Form
    {
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<KategoriBrowserView> _kategoriBrowser;
        private readonly IBrowser<BrgBrowserView> _brgBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IJenisBrgDal _jenisBrgDal;
        private readonly IKategoriDal _kategoriDal;
        private readonly IHargaTypeDal _hargaTypeDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;

        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;

        private readonly IBrgWriter _writer;

        private readonly BindingList<BrgFormSatuanDto> _listSatuan = new BindingList<BrgFormSatuanDto>();
        private readonly BindingList<BrgFormHargaDto> _listHarga = new BindingList<BrgFormHargaDto>();
        private readonly BindingList<BrgFormStokDto> _listStok = new BindingList<BrgFormStokDto>();
        private readonly BindingList<BrgFormBrgDto> _listBrg = new BindingList<BrgFormBrgDto>();

        public BrgForm(
            IBrowser<SupplierBrowserView> supplierBrowser,
            IBrowser<KategoriBrowserView> kategoriBrowser,
            IBrowser<BrgBrowserView> brgBrowser,
            ISupplierDal supplierDal,
            IJenisBrgDal jenisBrgDal, 
            IKategoriDal kategoriDal, 
            IHargaTypeDal hargaTypeDal, 
            IWarehouseDal warehouseDal, 
            IBrgDal brgDal, 
            IBrgBuilder brgBuilder, 
            IStokBalanceBuilder stokBalanceBuilder, 
            IBrgWriter writer)
        {
            InitializeComponent();
            RegisterEventHandler();

            _supplierBrowser = supplierBrowser;
            _kategoriBrowser = kategoriBrowser;

            _supplierDal = supplierDal;
            _jenisBrgDal = jenisBrgDal;
            _kategoriDal = kategoriDal;
            _hargaTypeDal = hargaTypeDal;
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _writer = writer;
            _brgBrowser = brgBrowser;

            InitJenisBrg();
            InitGridSatuan();
            InitGridHarga();
            InitGridStok();
            InitGridBrg();
        }

        private void RegisterEventHandler()
        {
            SupplierButton.Click += SupplierButton_Click;
            SupplierIdText.Validated += SupplierIdText_Validated;

            KategoriButton.Click += KategoriButton_Click;
            KategoriIdText.Validated += KategoriIdText_Validated;

            BrgButton.Click += BrgButton_Click;
            BrgIdText.Validated += BrgIdText_Validated;
            
            SaveButton.Click += SaveButton_Click;
            NewButton.Click += NewButton_Click;
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;
            BrgGrid.CellDoubleClick += BrgGrid_CellDoubleClick;

        }

        private void ClearForm()
        {
            BrgIdText.Clear();
            BrgNameText.Clear();
            BrgCodeText.Clear();
            SupplierIdText.Clear();
            SupplierNameText.Clear();
            JenisBrgCombo.SelectedIndex = 0;
            KategoriIdText.Clear();
            KategoriNameText.Clear();
            HppText.Value = 0;
            HppTimestampText.Value = new DateTime(3000, 1, 1);
            
            _listSatuan.Clear();
            ResetGridHarga();
            ResetGridStok();
        }

        private void ShowData(string brgId)
        {
            var fallback = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new BrgModel());
            var brg = fallback.Execute(()
                => _brgBuilder.Load(new BrgModel(brgId)).Build());

            BrgIdText.Text = brgId;
            BrgNameText.Text = brg.BrgName;
            BrgCodeText.Text = brg.BrgCode;
            HppText.Value = brg.Hpp;
            HppTimestampText.Value = brg.HppTimestamp;
            SupplierIdText.Text = brg.SupplierId;
            SupplierNameText.Text = brg.SupplierName;
            JenisBrgCombo.SelectedItem = brg.JenisBrgId;
            KategoriIdText.Text = brg.KategoriId;
            KategoriNameText.Text = brg.KategoriName;

            //  satuan
            _listSatuan.Clear();
            brg.ListSatuan
                .OrderBy(x => x.Conversion).ToList()
                .ForEach(x => _listSatuan
                    .Add(new BrgFormSatuanDto(x.Satuan, x.Conversion, x.SatuanPrint)));
            SatuanGrid.Refresh();

            //  harga
            foreach (var item in _listHarga)
            {
                item.Clear();
                var harga = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == item.TypeId);
                if (harga is null)
                    continue;

                item.SetHpp(brg.Hpp);
                var marginRp = harga.Harga - harga.Hpp;
                item.Harga = harga.Harga;
            }
            HargaGrid.Refresh();

            //  stok
            var fallbackStok = Policy<StokBalanceModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new StokBalanceModel
                {
                    ListWarehouse = new List<StokBalanceWarehouseModel>()
                });
            var stokBalance = fallbackStok.Execute(()
                => _stokBalanceBuilder.Load(new BrgModel(brgId)).Build());
            foreach (var item in _listStok)
            {
                item.SetQty(0);
                var stok = stokBalance.ListWarehouse
                    .FirstOrDefault(x => x.WarehouseId == item.Id);
                if (stok is null)
                    continue;
                item.SetQty(stok.Qty);
            }
            StokGrid.Refresh();
        }

        #region SEARCH-TEXT
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBrg();
        }
        private void SearchBrg()
        {
            var keyword = SearchText.Text;
            if (keyword.Length == 0)
            {
                InitGridBrg();
                return;
            }
            
            var resultName = _listBrg.Where(x => x.BrgName.ContainMultiWord(keyword)).ToList();
            var resultId = _listBrg.Where(x => x.Id.ToLower().StartsWith(keyword.ToLower())).ToList();
            var resultCode = _listBrg.Where(x => x.Code.ToLower().StartsWith(keyword.ToLower())).ToList();
            var result = resultName.Concat(resultId).Concat(resultCode).ToList();

            BrgGrid.DataSource = result;
            BrgGrid.Refresh();
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchBrg();
        }
        #endregion

        #region BRG
        private void BrgButton_Click(object sender, EventArgs e)
        {
            BrgIdText.Text = _brgBrowser.Browse(SupplierIdText.Text);
            BrgIdText_Validated(BrgIdText, null);
        }

        private void BrgIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
            {
                ClearForm();
                return;
            }
            ShowData(textbox.Text);
        }

        #endregion
        
        #region SUPPLIER
        private void SupplierButton_Click(object sender, EventArgs e)
        {
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }

        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var supplier = _supplierDal.GetData(new SupplierModel(textbox.Text));
            SupplierNameText.Text = supplier?.SupplierName ?? string.Empty;
        }

        #endregion

        #region JENIS-BRG
        private void InitJenisBrg()
        {
            var listJenisBrg = _jenisBrgDal.ListData()?.ToList() ?? new List<JenisBrgModel>();
            JenisBrgCombo.DataSource = listJenisBrg;
            JenisBrgCombo.DisplayMember = "JenisBrgName";
            JenisBrgCombo.ValueMember = "JenisBrgId";
        }
        #endregion

        #region KATEGORI-BRG
        private void KategoriButton_Click(object sender, EventArgs e)
        {
            KategoriIdText.Text = _kategoriBrowser.Browse(KategoriIdText.Text);
            KategoriIdText_Validated(KategoriIdText, null);
        }

        private void KategoriIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var kategori = _kategoriDal.GetData(new KategoriModel(textbox.Text));
            KategoriNameText.Text = kategori?.KategoriName ?? string.Empty;
        }
        #endregion

        #region GRID-SATUAN
        private void InitGridSatuan()
        {
            var binding = new BindingSource();
            binding.DataSource = _listSatuan;
            SatuanGrid.DataSource = binding;
            SatuanGrid.Refresh();
            SatuanGrid.Columns.SetDefaultCellStyle(Color.Beige);
            SatuanGrid.Columns.GetCol("Satuan").Width = 80;
            SatuanGrid.Columns.GetCol("Conversion").Width = 80;
        }
        #endregion

        #region GRID-HARGA
        private void InitGridHarga()
        {
            ResetGridHarga();
            HargaGrid.AllowUserToAddRows = false;
            HargaGrid.AllowUserToDeleteRows = false;
            
            var binding = new BindingSource();
            binding.DataSource = _listHarga;
            HargaGrid.DataSource = binding;
            HargaGrid.Refresh();
            
            HargaGrid.Columns.SetDefaultCellStyle(Color.Beige);
            HargaGrid.Columns.GetCol("TypeId").Width = 50;
            HargaGrid.Columns.GetCol("Name").Width = 100;
            HargaGrid.Columns.GetCol("Margin").Width = 60;
            HargaGrid.Columns.GetCol("Harga").Width = 60;
            HargaGrid.Columns.GetCol("Keterangan").Width = 120;
            HargaGrid.Columns.GetCol("Hpp").Visible= false;
        }

        private void ResetGridHarga()
        {
            _listHarga.Clear();
            var listHargaType = _hargaTypeDal.ListData()?.ToList() ?? new List<HargaTypeModel>();
            var prio = listHargaType.Where(x => x.HargaTypeName.EndsWith("Trade")).ToList();
            var non = listHargaType.Where(x => !x.HargaTypeName.EndsWith("Trade")).ToList();
            foreach (var item in prio.Concat(non))
            {
                var harga = new BrgFormHargaDto();
                harga.TypeId = item.HargaTypeId;
                harga.SetName(item.HargaTypeName);
                _listHarga.Add(harga);
            }
            HargaGrid.Refresh();
        }
        #endregion
        
        #region GRID-STOK
        private void InitGridStok()
        {
            ResetGridStok();
            
            StokGrid.AllowUserToAddRows = false;
            StokGrid.AllowUserToDeleteRows = false;
            
            var binding = new BindingSource();
            binding.DataSource = _listStok;
            StokGrid.DataSource = binding;
            StokGrid.Refresh();
            
            StokGrid.Columns.SetDefaultCellStyle(Color.Beige);
            StokGrid.Columns.GetCol("Id").Width = 50;
            StokGrid.Columns.GetCol("WarehouseName").Width = 200;
            StokGrid.Columns.GetCol("Qty").Width = 60;
        }

        private void ResetGridStok()
        {
            _listStok.Clear();
            var listWarehouse = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();
            foreach (var item in listWarehouse)
            {
                var brgStok = new BrgFormStokDto(
                    item.WarehouseId, item.WarehouseName, 0);
                _listStok.Add(brgStok);
            }
            StokGrid.Refresh();
        }
        #endregion

        #region GRID-BRG
        private void BrgGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.RowIndex < 0)
                ClearForm();
            else
            {
                var brgId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                ShowData(brgId);
            }
        }

        private void InitGridBrg()
        {
            _listBrg.Clear();
            var listBrg = _brgDal.ListData()?.ToList() ?? new List<BrgModel>();
            foreach (var item in listBrg)
            {
                var brg = new BrgFormBrgDto(
                    item.BrgId, item.BrgCode, item.BrgName, item.KategoriName);
                _listBrg.Add(brg);
            }

            BrgGrid.AllowUserToAddRows = false;
            BrgGrid.AllowUserToDeleteRows = false;
            
            var binding = new BindingSource();
            binding.DataSource = _listBrg;
            BrgGrid.DataSource = binding;
            BrgGrid.Refresh();
            
            BrgGrid.Columns.SetDefaultCellStyle(Color.Beige);
            BrgGrid.Columns.GetCol("Id").Width = 50;
            BrgGrid.Columns.GetCol("Code").Width = 80;
            BrgGrid.Columns.GetCol("BrgName").Width = 150;
            BrgGrid.Columns.GetCol("Kategori").Width = 100;
            if (BrgGrid.Rows.Count == 0)
                return;
            BrgGrid.FirstDisplayedScrollingRowIndex = BrgGrid.Rows.Count - 1;
        }
        #endregion
        
        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            BrgModel brg;
            if (BrgIdText.Text.IsNullOrEmpty())
                brg = _brgBuilder.Create().Build();
            else
                brg = _brgBuilder.Load(new BrgModel(BrgIdText.Text)).Build();

            brg = _brgBuilder
                .Attach(brg)
                .BrgId(BrgIdText.Text)
                .Name(BrgNameText.Text)
                .Code(BrgCodeText.Text)
                .Supplier(new SupplierModel(SupplierIdText.Text))
                .JenisBrg(new JenisBrgModel(JenisBrgCombo.SelectedValue.ToString()))
                .Kategori(new KategoriModel(KategoriIdText.Text))
                .Build();

            brg.ListSatuan.Clear();
            foreach (var item in _listSatuan)
                brg = _brgBuilder
                    .Attach(brg)
                    .AddSatuan(item.Satuan, item.Conversion, item.SatuanPrint)
                    .Build();

            brg.ListHarga.Clear();
            foreach (var item in _listHarga)
                brg = _brgBuilder
                    .Attach(brg)
                    .AddHarga(new HargaTypeModel(item.TypeId), item.Harga)
                    .Build();

            _writer.Save(ref brg);
            ClearForm();
            InitGridBrg();
        }
        #endregion

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion
    }
}
    