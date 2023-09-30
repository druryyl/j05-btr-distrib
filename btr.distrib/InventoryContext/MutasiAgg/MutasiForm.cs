using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.MutasiAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using Mapster;
using Polly;

namespace btr.distrib.InventoryContext.MutasiAgg
{
    public partial class MutasiForm : Form
    {
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<MutasiBrowserView> _mutasiBrowser;

        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;
        private readonly ITglJamDal _dateTime;

        private readonly ICreateMutasiItemWorker _createItemWorker;
        private readonly ISaveMutasiWorker _saveMutasiWorker;

        private readonly IBrgBuilder _brgBuilder;
        private readonly IMutasiBuilder _mutasiBuilder;


        private readonly BindingList<MutasiItemDto> _listItem = new BindingList<MutasiItemDto>();


        public MutasiForm(
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser,
            IWarehouseDal warehouseDal,
            ICreateMutasiItemWorker createItemWorker,
            IBrgBuilder brgBuilder,
            IBrgDal brgDal,
            ISaveMutasiWorker saveMutasiWorker,
            IMutasiBuilder mutasiBuilder,
            ITglJamDal dateTime,
            IBrowser<MutasiBrowserView> mutasiBrowser)
        {
            InitializeComponent();

            _warehouseBrowser = warehouseBrowser;
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;

            _brgBuilder = brgBuilder;
            _createItemWorker = createItemWorker;
            _brgStokBrowser = brgStokBrowser;
            _saveMutasiWorker = saveMutasiWorker;
            _mutasiBuilder = mutasiBuilder;
            _dateTime = dateTime;
            _mutasiBrowser = mutasiBrowser;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            MutasiButton.Click += MutasiButton_Click;

            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.Validated += WarehouseIdText_Validated;

            MutasiItemGrid.CellContentClick += MutasiItemGrid_CellContentClick;
            MutasiItemGrid.CellValueChanged += MutasiItemGrid_CellValueChanged;
            MutasiItemGrid.CellValidated += MutasiItemGrid_CellValidated;
            MutasiItemGrid.KeyDown += MutasiItemGrid_KeyDown;
            MutasiItemGrid.EditingControlShowing += MutasiItemGrid_EditingControlShowing;

            SaveButton.Click += SaveButton_Click;
            NewButton.Click += NewButton_Click;
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void MutasiButton_Click(object sender, EventArgs e)
        {
            _mutasiBrowser.Filter.Date = new Periode(_dateTime.Now);

            MutasiIdText.Text = _mutasiBrowser.Browse(MutasiIdText.Text);
            LoadMutasi();
        }
        private bool LoadMutasi()
        {
            var textbox = MutasiIdText;
            var policy = Policy<MutasiModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as MutasiModel, (r, c) =>
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var mutasi = policy.Execute(() => _mutasiBuilder
                .Load(new MutasiModel(textbox.Text))
                .Build());
            if (mutasi is null)
                return false;

            mutasi.RemoveNull();
            MutasiDateText.Value = mutasi.MutasiDate;
            WarehouseIdText.Text = mutasi.WarehouseId;
            WarehouseNameText.Text = mutasi.WarehouseName;
            TotalText.Value = mutasi.NilaiSediaan;
            LastIdLabel.Text = $@"{mutasi.MutasiId}";

            _listItem.Clear();
            foreach (var item in mutasi.ListItem)
            {
                var newItem = item.Adapt<MutasiItemDto>();
                _listItem.Add(newItem);
            }

            if (mutasi.IsVoid)
                ShowAsVoid(mutasi);
            else
                ShowAsActive();
            CalcTotal();
            return true;
        }

        #region WAREHOUSE
        private void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var warehouse = _warehouseDal.GetData(new WarehouseModel(textbox.Text));
            WarehouseNameText.Text = warehouse?.WarehouseName ?? string.Empty;
        }

        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            _warehouseBrowser.Filter.UserKeyword = WarehouseIdText.Text;
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        #endregion

        #region GRID
        private void MutasiItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            var colFindIndex = grid.Columns["Find"]?.Index ?? 99;
            if (e.ColumnIndex != colFindIndex)
                return;

            BrowseBrg(e.RowIndex);
        }

        private void MutasiItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("QtyInputStr").Index)
                CalcTotal();
        }

        private void MutasiItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            var colHeaderText = grid.Columns[e.ColumnIndex].Name;
            switch (colHeaderText)
            {
                case "BrgId":
                case "QtyInputStr":
                    if (grid.CurrentCell.Value is null)
                        return;
                    ValidateRow(e.RowIndex);
                    break;                    
            }
        }

        private void MutasiItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;

            switch (e.KeyCode)
            {
                case Keys.F1:
                    if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                        BrowseBrg(grid.CurrentCell.RowIndex);
                    break;
                case Keys.Delete:
                    _listItem.RemoveAt(grid.CurrentCell.RowIndex);
                    grid.Refresh();
                    break;
            }
        }

        #region browse-brg-saat-cell-aktif
        private void MutasiItemGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex != 1)
                return;

            if (e.Control is TextBox textBox)
            {
                textBox.KeyDown -= TextBox_KeyDown;
                textBox.KeyDown += TextBox_KeyDown;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MutasiItemGrid.EndEdit();
                BrowseBrg(MutasiItemGrid.CurrentCell.RowIndex);
            }
        }
        #endregion

        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId;
            _brgStokBrowser.Filter.StaticFilter1 = WarehouseIdText.Text;
            _brgStokBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            brgId = _brgStokBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgId;
            ValidateRow(rowIndex);
        }

        private void ValidateRow(int rowIndex)
        {
            var brg = BuildBrg(rowIndex);
            if (brg == null)
                return;

            var req = new CreateMutasiItemRequest(
                _listItem[rowIndex].BrgId,
                WarehouseIdText.Text,
                _listItem[rowIndex].QtyInputStr);
            var item = _createItemWorker.Execute(req);
            _listItem[rowIndex] = item.Adapt<MutasiItemDto>();
            MutasiItemGrid.Refresh();
            CalcTotal();
        }

        private BrgModel BuildBrg(int rowIndex)
        {
            var id = _listItem[rowIndex].BrgId ?? string.Empty;
            if (id.Length == 0)
                return null;

            var brgKey = new BrgModel(id);
            var fbk = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as BrgModel);
            var brg = fbk.Execute(() => _brgBuilder.Load(brgKey).Build());

            if (brg is null)
            {
                brg = GetBrgByCode(id);
                _listItem[rowIndex].BrgId = brg?.BrgId ?? string.Empty;
                MutasiItemGrid.Refresh();
            }

            return brg;
        }

        private BrgModel GetBrgByCode(string id)
        {
            var result = _brgDal.GetData(id);
            if (result is null) return null;

            result = _brgBuilder.Load(result).Build();
            return result;
        }

        private void InitGrid()
        {
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            MutasiItemGrid.DataSource = binding;
            MutasiItemGrid.Refresh();
            MutasiItemGrid.Columns.SetDefaultCellStyle(Color.Beige);

            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            MutasiItemGrid.Columns.Insert(1, buttonCol);

            var cols = MutasiItemGrid.Columns;
            cols.GetCol("BrgId").Visible = true;
            cols.GetCol("BrgId").Width = 50;

            cols.GetCol("Find").Width = 20;

            cols.GetCol("BrgCode").Visible = true;
            cols.GetCol("BrgCode").Width = 80;

            cols.GetCol("BrgName").Visible = true;
            cols.GetCol("BrgName").Width = 160;
            cols.GetCol("BrgName").DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyInputStr").Width = 50;
            cols.GetCol("QtyInputStr").HeaderText = @"Qty";

            cols.GetCol("QtyDetilStr").Visible = true;
            cols.GetCol("QtyDetilStr").Width = 80;
            cols.GetCol("QtyDetilStr").HeaderText = @"Qty Desc";
            cols.GetCol("QtyDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("QtyDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyBesar").Visible = false;
            cols.GetCol("SatBesar").Visible = false;
            cols.GetCol("Conversion").Visible = false;
            cols.GetCol("HppBesar").Visible = false;

            cols.GetCol("QtyKecil").Visible = false;
            cols.GetCol("SatKecil").Visible = false;
            cols.GetCol("HppKecil").Visible = false;
            
            cols.GetCol("Qty").Visible = false;
            cols.GetCol("Sat").Visible = false;
            cols.GetCol("Hpp").Visible = false;

            cols.GetCol("StokDetilStr").Visible = true;
            cols.GetCol("StokDetilStr").Width = 80;
            cols.GetCol("StokDetilStr").HeaderText = @"Stok";
            cols.GetCol("StokDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("StokDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("HppDetilStr").Visible = true;
            cols.GetCol("HppDetilStr").Width = 80;
            cols.GetCol("HppDetilStr").HeaderText = @"HPP";
            cols.GetCol("HppDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HppDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            
            cols.GetCol("NilaiSediaan").Visible = true;

            //  auto-resize-rows
            MutasiItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MutasiItemGrid.AutoResizeRows();
        }

        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.NilaiSediaan);
        }

        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var mainform = (MainForm)this.Parent.Parent;
            var cmd = new SaveMutasiRequest
            {
                MutasiId = MutasiIdText.Text,
                MutasiDate = MutasiDateText.Value.ToString("yyyy-MM-dd"),
                WarehouseId = WarehouseIdText.Text,
                UserId = mainform.UserId.UserId,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName?.Length > 0
                select new SaveMutasiRequestItem
                {
                    BrgId = c.BrgId,
                    QtyString = c.QtyInputStr,
                }).ToList();
            cmd.ListBrg = listItem;
            var result = _saveMutasiWorker.Execute(cmd);
            LastIdLabel.Text = result.MutasiId;
            ClearForm();
        }
        private void ClearForm()
        {
            MutasiIdText.Text = string.Empty;
            MutasiDateText.Value = _dateTime.Now;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;

            TotalText.Value= 0;

            _listItem.Clear();
            _listItem.Add(new MutasiItemDto());
            ShowAsActive();
        }
        private void ShowAsVoid(MutasiModel mutasi)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            SaveButton.Visible = false;
        }

        private void ShowAsActive()
        {
            this.BackColor = Color.DarkSeaGreen;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.Honeydew;

            SaveButton.Visible = true;
        }
        #endregion

    }

    public class MutasiItemDto
    {
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; private set; }

        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public decimal HppBesar { get; private set; }
        public int Conversion { get; private set; }

        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HppKecil { get; private set; }

        public int Qty { get; private set; }
        public string Sat { get; private set; }
        public decimal Hpp { get; private set; }

        public string StokDetilStr { get; private  set; }
        public string HppDetilStr { get; private set; }
        
        public decimal NilaiSediaan { get; set; } 

    }
}
