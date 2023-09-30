using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.MutasiAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SalesContext.MutasiAgg.UseCases;
using btr.application.SalesContext.MutasiAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using Mapster;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.PurchaseContext.MutasiAgg
{
    public partial class MutasiForm : Form
    {
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<MutasiBrowserView> _invoiceBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;
        private readonly ITglJamDal _dateTime;

        private readonly ICreateMutasiItemWorker _createItemWorker;
        private readonly ISaveMutasiWorker _saveMutasiWorker;

        private readonly IBrgBuilder _brgBuilder;
        private readonly IMutasiBuilder _invoiceBuilder;


        private readonly BindingList<MutasiItemDto> _listItem = new BindingList<MutasiItemDto>();


        public MutasiForm(IBrowser<SupplierBrowserView> supplierBrowser,
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser,
            ISupplierDal supplierDal,
            IWarehouseDal warehouseDal,
            ICreateMutasiItemWorker createItemWorker,
            IBrgBuilder brgBuilder,
            IBrgDal brgDal,
            ISaveMutasiWorker saveMutasiWorker,
            IMutasiBuilder invoiceBuilder,
            ITglJamDal dateTime,
            IBrowser<MutasiBrowserView> invoiceBrowser)
        {
            InitializeComponent();

            _supplierBrowser = supplierBrowser;
            _warehouseBrowser = warehouseBrowser;

            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;

            _brgBuilder = brgBuilder;
            _createItemWorker = createItemWorker;

            RegisterEventHandler();
            InitGrid();
            _brgStokBrowser = brgStokBrowser;
            _saveMutasiWorker = saveMutasiWorker;
            _invoiceBuilder = invoiceBuilder;
            _dateTime = dateTime;
            _invoiceBrowser = invoiceBrowser;
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
            _invoiceBrowser.Filter.Date = new Periode(_dateTime.Now);

            MutasiIdText.Text = _invoiceBrowser.Browse(MutasiIdText.Text);
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

            var invoice = policy.Execute(() => _invoiceBuilder
                .Load(new MutasiModel(textbox.Text))
                .Build());
            if (invoice is null)
                return false;

            invoice.RemoveNull();
            MutasiDateText.Value = invoice.MutasiDate;
            WarehouseIdText.Text = invoice.WarehouseId;
            WarehouseNameText.Text = invoice.WarehouseName;
            TotalText.Value = invoice.Total;
            //LastIdLabel.Text = $"{invoice.MutasiCode}";

            _listItem.Clear();
            foreach (var item in invoice.ListItem)
            {
                var newItem = item.Adapt<MutasiItemDto>();
                _listItem.Add(newItem);
            }

            if (invoice.IsVoid)
                ShowAsVoid(invoice);
            else
                ShowAsActive();
            CalcTotal();
            return true;
        }

        #region SUPPLIER
        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var supplier = _supplierDal.GetData(new SupplierModel(textbox.Text));
            SupplierNameText.Text = supplier?.SupplierName ?? string.Empty;
        }

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            _supplierBrowser.Filter.UserKeyword = SupplierIdText.Text;
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }
        #endregion

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
            if (e.ColumnIndex != grid.Columns["Find"].Index)
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
            if (e.ColumnIndex == grid.Columns.GetCol("DiscInputStr").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("PpnProsen").Index)
                CalcTotal();
        }

        private void MutasiItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
            {
                if (grid.CurrentCell.Value is null)
                {
                    CleanRow(e.RowIndex);
                    return;
                }
                ValidateRow(e.RowIndex);
            }

            if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("QtyInputStr").Index)
            {
                if (grid.CurrentCell.Value is null)
                    return;
                ValidateRow(e.RowIndex);
            }

            if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("HrgInputStr").Index)
            {
                if (grid.CurrentCell.Value is null)
                    return;
                ValidateRow(e.RowIndex);
            }

            if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("DiscInputStr").Index)
            {
                if (grid.CurrentCell.Value is null)
                    return;
                ValidateRow(e.RowIndex);
            }
        }

        private void MutasiItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.KeyCode == Keys.F1)
            {
                if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                    BrowseBrg(grid.CurrentCell.RowIndex);
            }

            if (e.KeyCode == Keys.Delete)
            {
                _listItem.RemoveAt(grid.CurrentCell.RowIndex);
                grid.Refresh();
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
            var grid = MutasiItemGrid;
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
            {
                CleanRow(rowIndex);
                return;
            }

            var req = new CreateMutasiItemRequest(
                _listItem[rowIndex].BrgId,
                _listItem[rowIndex].HrgInputStr,
                _listItem[rowIndex].QtyInputStr,
                _listItem[rowIndex].DiscInputStr,
                _listItem[rowIndex].PpnProsen);
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

        private void CleanRow(int rowIndex)
        {
            //_listItem[rowIndex].SetBrgName(string.Empty);
            //_listItem[rowIndex].SetCode(string.Empty);
            //MutasiItemGrid.Refresh();
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

            cols.GetCol("HrgInputStr").Visible = true;
            cols.GetCol("HrgInputStr").Width = 110;
            cols.GetCol("HrgInputStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HrgInputStr").HeaderText = "Hrg Beli";
            cols.GetCol("HrgInputStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("HrgDetilStr").Visible = true;
            cols.GetCol("HrgDetilStr").Width = 110;
            cols.GetCol("HrgDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HrgDetilStr").HeaderText = "Detil Harga";
            cols.GetCol("HrgDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;


            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyInputStr").Width = 50;
            cols.GetCol("QtyInputStr").HeaderText = "Qty";

            cols.GetCol("QtyDetilStr").Visible = true;
            cols.GetCol("QtyDetilStr").Width = 80;
            cols.GetCol("QtyDetilStr").HeaderText = "Qty Desc";
            cols.GetCol("QtyDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("QtyDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyBesar").Visible = false;
            cols.GetCol("SatBesar").Visible = false;
            cols.GetCol("Conversion").Visible = false;
            cols.GetCol("HppSatBesar").Visible = false;

            cols.GetCol("QtyKecil").Visible = false;
            cols.GetCol("SatKecil").Visible = false;
            cols.GetCol("HppSatKecil").Visible = false;

            cols.GetCol("QtyBeli").Visible = false;
            cols.GetCol("HppSat").Visible = false;

            cols.GetCol("SubTotal").Visible = true;
            cols.GetCol("SubTotal").Width = 70;

            cols.GetCol("QtyBonus").Visible = false;
            cols.GetCol("QtyPotStok").Visible = false;

            cols.GetCol("DiscInputStr").Visible = true;
            cols.GetCol("DiscInputStr").Width = 65;
            cols.GetCol("DiscInputStr").HeaderText = "Disc";

            cols.GetCol("DiscDetilStr").Visible = true;
            cols.GetCol("DiscDetilStr").Width = 90;
            cols.GetCol("DiscDetilStr").HeaderText = "Disc Rp";
            cols.GetCol("DiscDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("DiscDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("DiscRp").Visible = false;

            cols.GetCol("PpnProsen").Visible = true;
            cols.GetCol("PpnProsen").Width = 50;
            cols.GetCol("PpnProsen").HeaderText = "Ppn";

            cols.GetCol("PpnRp").Visible = false;

            cols.GetCol("Total").Visible = true;
            cols.GetCol("Total").Width = 80;

            //  auto-resize-rows
            MutasiItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MutasiItemGrid.AutoResizeRows();
        }

        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            DiscountText.Value = _listItem.Sum(x => x.DiscRp);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
            SisaText.Value = GrandTotalText.Value - UangMukaText.Value;
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
                MutasiCode = MutasiCodeText.Text,
                SupplierId = SupplierIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                TermOfPayment = TermOfPaymentCombo.SelectedIndex,
                DueDate = DueDateText.Value.ToString("yyyy-MM-dd"),
                UserId = mainform.UserId.UserId,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName?.Length > 0
                select new SaveMutasiRequestItem
                {
                    BrgId = c.BrgId,
                    HrgInputStr = c.HrgInputStr,
                    QtyString = c.QtyInputStr,
                    DiscountString = c.DiscInputStr,
                    PpnProsen = c.PpnProsen,
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
            MutasiCodeText.Text = string.Empty;

            SupplierIdText.Text = string.Empty;
            SupplierNameText.Text = string.Empty;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;
            TermOfPaymentCombo.SelectedIndex = 0;

            TotalText.Value = 0;
            DiscountText.Value = 0;
            TaxText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;

            _listItem.Clear();
            _listItem.Add(new MutasiItemDto());
            ShowAsActive();
        }
        private void ShowAsVoid(MutasiModel invoice)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            CancelLabel.Text = $"Mutasi sudah DIBATALKAN \noleh {invoice.UserIdVoid} \npada {invoice.VoidDate:ddd, dd MMM yyyy}";
            VoidPanel.Visible = true;
            SaveButton.Visible = false;
        }

        private void ShowAsActive()
        {
            this.BackColor = Color.DarkSeaGreen;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.Honeydew;

            VoidPanel.Visible = false;
            SaveButton.Visible = true;
        }
        #endregion

    }

    public class MutasiItemDto
    {
        public MutasiItemDto()
        {
            PpnProsen = 11;
        }
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string HrgInputStr { get; set; }
        public string HrgDetilStr { get; private set; }

        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; private set; }

        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int Conversion { get; private set; }
        public decimal HppSatBesar { get; private set; }

        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HppSatKecil { get; private set; }

        //      harga
        public int QtyBeli { get; private set; }
        public decimal HppSat { get; private set; }
        public decimal SubTotal { get; private set; }

        public int QtyBonus { get; private set; }
        public int QtyPotStok { get; private set; }

        //      diskon
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; private set; }
        public decimal DiscRp { get; private set; }

        //      ppn
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; private set; }

        public decimal Total { get; private set; }

    }
}
