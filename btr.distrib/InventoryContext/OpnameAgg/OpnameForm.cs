using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly IMediator _mediator;
        private readonly IOpnameDal _opnameDal;
        private readonly ITglJamDal _tglJamDal;

        public OpnameForm(IWarehouseDal warehouseDal,
            IBrgDal brgDal,
            IBrgBuilder brgBuilder,
            IStokBuilder stokBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IOpnameBuilder opnameBuilder,
            IOpnameWriter opnameWriter,
            IMediator mediator,
            IOpnameDal opnameDal,
            ITglJamDal tglJamDal)
        {
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _opnameBuilder = opnameBuilder;
            _opnameWriter = opnameWriter;
            _mediator = mediator;

            InitializeComponent();
            InitWarehouseCombo();
            InitGridBrg();
            InitPeriodeReport();
            RegisterEventHandler();
            _opnameDal = opnameDal;
            _tglJamDal = tglJamDal;
        }

        private void InitPeriodeReport()
        {
            Tgl1Date.CustomFormat = "ddd, dd MMM yyyy";
            Tgl2Date.CustomFormat = "ddd, dd MMM yyyy";
            Tgl1Date.Format = DateTimePickerFormat.Custom;
            Tgl2Date.Format = DateTimePickerFormat.Custom;
        }
        private void RegisterEventHandler()
        {
            SearchText.KeyDown += SearchText_KeyDown;
            BrgGrid.KeyDown += BrgGrid_KeyDown;
            BrgGrid.CellDoubleClick += BrgGrid_CellDoubleClick;
            SaveButton.Click += SaveButton_Click;
            ReportButton.Click += ReportButton_Click;
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            RefreshGridReport();
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

            GenStok(opname);

            ClearForm();
        }

        private async void GenStok(OpnameModel opname)
        {
            //var qtyAdjust = opname.Qty2Adjust * opname.Conversion2;
            //qtyAdjust += opname.Qty1Adjust;

            //if (qtyAdjust > 0)
            //{
            //    var cmd = new AddStokCommand(opname.BrgId, opname.WarehouseId, 
            //        qtyAdjust, opname.Satuan1, opname.Nilai, opname.OpnameId, "OPNAME");
            //    await _mediator.Send(cmd);
            //}
            //else
            //{
            //    var cmd = new RemoveStokCommand(opname.BrgId, opname.WarehouseId,
            //        -qtyAdjust, opname.Satuan1, 0, opname.OpnameId, "OPNAME");
            //    await _mediator.Send(cmd);
            //}
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

            WarehouseReportCombo.DataSource = listWh;
            WarehouseReportCombo.DisplayMember = "WarehouseName";
            WarehouseReportCombo.ValueMember = "WarehouseId";
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

        private void RefreshGridReport()
        {
            var tgl1 = Tgl1Date.Value;
            var tgl2 = Tgl2Date.Value;
            var periode = new Periode(tgl1, tgl2);
            var listOpname = _opnameDal.ListData(periode)?.ToList()
                ?? new List<OpnameModel>();

            var result = listOpname
                .Where(x => x.WarehouseId == WarehouseReportCombo.SelectedValue.ToString())
                .OrderBy(x => x.OpnameDate)
                .Select(x => new ReportOpnameFormDto(x)).ToList();
            ReportGrid.DataSource = result;
            ReportGrid.Columns.SetDefaultCellStyle(Color.LemonChiffon);
            ReportGrid.AutoResizeColumns();
            ReportGrid.Columns.GetCol("A").Width = 10;
            ReportGrid.Columns.GetCol("B").Width = 10;

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
        public ReportOpnameFormDto(OpnameModel opname)
        {
            if (opname is null)
                return;
            TglJam = opname.OpnameDate;
            BrgId = opname.BrgId;
            Code = opname.BrgCode;
            BrgName = opname.BrgName;
            Qty_Awal = $"{opname.Qty2Awal:N0} {opname.Satuan2}";
            Qty_Awal_ = $"{opname.Qty1Awal:N0} {opname.Satuan1}";
            Qty_Op = $"{opname.Qty2Opname:N0} {opname.Satuan2}";
            Qty_Op_ = $"{opname.Qty1Opname:N0} {opname.Satuan1}";

        }
        public DateTime TglJam { get; }
        public string BrgId { get; }
        public string Code { get; }
        public string BrgName { get; }
        public string A {get;set;}
        public string Qty_Awal { get; }
        public string Qty_Awal_ { get; }
        public string B { get; set; }
        public string Qty_Op { get; }
        public string Qty_Op_ { get; }
    }
}
