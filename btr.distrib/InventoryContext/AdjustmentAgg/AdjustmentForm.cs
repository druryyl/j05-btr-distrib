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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.AdjustmentAgg
{
    public partial class AdjustmentForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IKategoriDal _kategoriDal;
        private readonly IBrgDal _brgDal;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;

        private List<BrgModel> _listBrg;

        public AdjustmentForm(IWarehouseDal warehouseDal,
            IKategoriDal kategoriDal,
            IBrgDal brgDal,
            IStokBalanceWarehouseDal stokBalanceWarehouseDal)
        {
            InitializeComponent();
            _warehouseDal = warehouseDal;
            _kategoriDal = kategoriDal;
            _brgDal = brgDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;

            ListStokButton.Click += ListStokButton_Click;

            InitWarehouseComboBox();
            InitKategoirComboBox();
            InitStokGrid();
        }

        private void ListStokButton_Click(object sender, EventArgs e)
        {
            ListStok();
        }

        private void ListStok()
        {
            var listStok = _stokBalanceWarehouseDal.ListData(new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))?.ToList()
                ?? new List<StokBalanceWarehouseModel>();

            var listDataSource = new List<AdjustmentDto>();
            var listBrgResult = new List<BrgModel>();
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

            foreach (var brg in listBrgResult)
            {
                var stokInPcs = listStok.FirstOrDefault(x => x.BrgId == brg.BrgId)?.Qty ?? 0;
                var conversion = brg.ListSatuan?.DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 }).Max(x => x.Conversion) ?? 1;
                var stokBesar = conversion == 1 ? 0 : stokInPcs / conversion;
                var stokKecil = conversion == 1 ? stokInPcs : stokInPcs % conversion;
                var stok = new AdjustmentDto
                {
                    BrgId = brg.BrgId,
                    BrgCode = brg.BrgCode,
                    BrgName = brg.BrgName,
                    KategoriId = brg.KategoriId,
                    KategoriName = brg.KategoriName,
                    WarehouseId = WarehouseCombo.SelectedValue.ToString(),
                    WarehouseName = WarehouseCombo.SelectedText.ToString(),
                    StokBesar = stokBesar,
                    StokKecil = stokKecil,
                    StokInPcs = stokInPcs,
                };
                listDataSource.Add(stok);
            }

            StokGrid.DataSource = listDataSource;
            StokGrid.Refresh();
        }

        private void InitKategoirComboBox()
        {
            var listKategori = _kategoriDal.ListData()?? new List<KategoriModel>();
            KategoriCombo.DataSource = listKategori;
            KategoriCombo.DisplayMember = "KategoriName";
            KategoriCombo.ValueMember = "KategoriId";
            KategoriCombo.SelectedIndex = 0;

        }

        private void InitWarehouseComboBox()
        {
            var listWh = _warehouseDal.ListData();
            WarehouseCombo.DataSource = listWh;
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
            grid.GetCol("BrgName").Width = 200;
            grid.GetCol("KategoriId").Width = 50;
            grid.GetCol("KategoriName").Width = 100;
            grid.GetCol("WarehouseId").Width = 50;
            grid.GetCol("WarehouseName").Width = 100;
            grid.GetCol("StokBesar").Width = 50;
            grid.GetCol("StokKecil").Width = 50;
            grid.GetCol("StokInPcs").Width = 50;

            grid.GetCol("BrgId").Visible = false;
            grid.GetCol("KategoriId").Visible = false;
            grid.GetCol("WarehouseId").Visible = false;

            grid.GetCol("StokBesar").DefaultCellStyle.Format = "#,##";
            grid.GetCol("StokKecil").DefaultCellStyle.Format = "#,##";
            grid.GetCol("StokInPcs").DefaultCellStyle.Format = "#,##";

            grid.GetCol("StokBesar").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.GetCol("StokKecil").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.GetCol("StokInPcs").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            _listBrg = _brgDal.ListData()?.ToList() ?? new List<BrgModel>();
        }
    }

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
        public int StokKecil { get; set; }
        public int StokInPcs { get; set; }
    }
}
