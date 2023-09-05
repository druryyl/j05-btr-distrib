using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
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

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public partial class OpnameForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IOpnameBuilder _opnameBuilder;
        private readonly IOpnameWriter _opnameWriter;

        public OpnameForm(IWarehouseDal warehouseDal,
            IBrgDal brgDal,
            IBrgBuilder brgBuilder,
            IStokBuilder stokBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IOpnameBuilder opnameBuilder,
            IOpnameWriter opnameWriter)
        {
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;

            InitializeComponent();
            InitWarehouseCombo();
            InitGridBrg();
            RegisterEventHandler();
            _opnameBuilder = opnameBuilder;
            _opnameWriter = opnameWriter;
        }

        private void RegisterEventHandler()
        {
            SearchText.KeyDown += SearchText_KeyDown;
            BrgGrid.KeyDown += BrgGrid_KeyDown;
            BrgGrid.CellDoubleClick += BrgGrid_CellDoubleClick;
            SaveButton.Click += SaveButton_Click;
        }
        private void ClearForm()
        {
            BrgIdText.Clear();
            BrgCodeText.Clear();
            BrgNameText.Clear();
            Qty2AwalText.Value = 0;
            Qty2OpnameText.Value = 0;
            Qty2AdjustText.Value = 0;

            Qty1AwalText.Value = 0;
            Qty1OpnameText.Value = 0;
            Qty1AdjustText.Value = 0;
        }

        private void InitReport()
        {
            Tgl1Date.CustomFormat = "ddd, dd-MM-yyyy";
            Tgl2Date.CustomFormat = "ddd, dd-MM-yyyy";

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var mainMenu = (MainForm)this.Parent.Parent;
            var opname = _opnameBuilder
                .Create()
                .Brg(new BrgModel(BrgIdText.Text))
                .Warehouse(new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))
                .User(new UserModel(mainMenu.UserId.UserId))
                .QtyAwal((int)Qty2AwalText.Value, (int)Qty1AwalText.Value)
                .QtyOpname((int)Qty2OpnameText.Value, (int)Qty1OpnameText.Value)
                .Build();

            _opnameWriter.Save(ref opname);
            ClearForm();
        }

        private void BrgGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowBrg(BrgGrid.CurrentCell.RowIndex);
        }

        private void BrgGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            ShowBrg(BrgGrid.CurrentCell.RowIndex);
        }

        private void ShowBrg(int rowIndex)
        {
            var brgId = BrgGrid.Rows[rowIndex].Cells[0].Value.ToString();
            var brg = _brgBuilder.Load(new BrgModel(brgId)).Build();

            BrgIdText.Text = brg.BrgId;
            BrgNameText.Text = brg.BrgName;
            BrgCodeText.Text = brg.BrgCode;
            var defSatuan = new BrgSatuanModel
            {
                Conversion = 1,
                Satuan = string.Empty
            };
            var satBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1) ?? defSatuan;
            var satKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1) ?? defSatuan;
            SatuanBesarLabel.Text = satBesar?.Satuan ?? string.Empty;
            SatuanKecilLabel.Text = satKecil?.Satuan ?? string.Empty;

            var stokBalance = _stokBalanceBuilder.Load(brg).Build();
            var stokAwal = stokBalance.ListWarehouse
                .FirstOrDefault(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())?
                .Qty ?? 0;

            var qtyBesar = decimal.Floor(stokAwal / satBesar.Conversion);
            var qtyKecil = stokAwal - (qtyBesar * satBesar.Conversion);
            Qty2AwalText.Value = qtyBesar;
            Qty1AwalText.Value = qtyKecil;
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            RefreshGrid();
        }

        private void InitWarehouseCombo()
        {
            var listWh = _warehouseDal.ListData();
            WarehouseCombo.DataSource = listWh;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }

        private void InitGridBrg()
        {
            RefreshGrid();
            BrgGrid.Columns.SetDefaultCellStyle(Color.LemonChiffon);
            BrgGrid.Columns.GetCol("BrgId").Width = 50;
            BrgGrid.Columns.GetCol("Code").Width = 80;
            BrgGrid.Columns.GetCol("Name").Width = 200;
        }

        private void RefreshGrid()
        {
            var listAll = _brgDal.ListData()?.ToList() ?? new List<BrgModel>();
            var listBrg = listAll.Select(
                x => new BrgOpnameFormDto(x.BrgId, x.BrgCode, x.BrgName))?.ToList();
            var keyword = SearchText.Text;
            if (keyword.Length > 0)
            {
                var filterName = listBrg.Where(x => x.Name.ContainMultiWord(keyword))?.ToList();
                var filterCode = listBrg.Where(x => x.Code.ToLower().StartsWith(keyword.ToLower()));
                listBrg = filterName;
                listBrg.AddRange(filterCode);
            }
            BrgGrid.DataSource = listBrg;

        }
    }

    public class BrgOpnameFormDto
    {
        public BrgOpnameFormDto(string id, string code, string name)
        {
            BrgId = id;
            Code = code;
            Name = name;
        }
        public string BrgId { get; }
        public string Code { get; }
        public string Name { get; }
    }

    public class ReportOpnameFormDto
    {
        public DateTime TglJam { get; }
        public string BrgId { get; }
        public string Code { get; }
        public string BrgName { get; }

        public int QtyAwalBesar { get; }
        public int QtyOpnameBesar { get; }
        public int SatuanBesar { get; set; }

        public int QtyAwalKecil { get; }
        public int QtyOpnameKecil { get; }
        public int SatuanKecil { get; set; }


    }
}
