using btr.application.SalesContext.FakturAgg.UseCases;
using btr.distrib.Browsers;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.distrib.Helpers;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using Mapster;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturForm : Form
    {
        private readonly BindingList<FakturItem2Dto> _listItem = new BindingList<FakturItem2Dto>();

        private readonly IBrowser<SalesPersonBrowserView> _salesBrowser;
        private readonly IBrowser<CustomerBrowserView> _customerBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<Faktur2BrowserView> _fakturBrowser;

        private readonly ISalesPersonDal _salesPersonDal;
        private readonly ICustomerDal _customerDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly IBrgDal _brgDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly ISaveFakturWorker _saveFakturWorker;
        private readonly ICreateFakturItemWorker _createItemWorker;

        private string _tipeHarga = string.Empty;


        public FakturForm(
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<SalesPersonBrowserView> salesBrowser,
            IBrowser<CustomerBrowserView> customerBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser,
            IBrowser<Faktur2BrowserView> fakturBrowser,
            ISalesPersonDal salesPersonDal,
            ICustomerDal customerDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            ITglJamDal dateTime,
            IBrgDal brgDal,
            IFakturBuilder fakturBuilder,
            ISaveFakturWorker saveFakturWorker,
            ICreateFakturItemWorker createItemWorker)
        {
            _warehouseBrowser = warehouseBrowser;
            _salesBrowser = salesBrowser;
            _customerBrowser = customerBrowser;
            _brgStokBrowser = brgStokBrowser;
            _fakturBrowser = fakturBrowser;

            _salesPersonDal = salesPersonDal;
            _customerDal = customerDal;
            _warehouseDal = warehouseDal;

            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;

            _dateTime = dateTime;

            InitializeComponent();
            InitGrid();
            ClearForm();
            RegisterEventHandler();
            _brgDal = brgDal;
            _fakturBuilder = fakturBuilder;
            _saveFakturWorker = saveFakturWorker;
            _createItemWorker = createItemWorker;
        }

        private void RegisterEventHandler()
        {
            FakturIdText.Validating += FakturIdText_Validating; ;

            SalesPersonButton.Click += SalesPersonButton_Click;
            SalesIdText.Validated += SalesIdText_Validated;
            SalesIdText.KeyDown += SalesIdText_KeyDown;

            CustomerButton.Click += CustomerButton_Click;
            CustomerIdText.Validated += CustomerIdText_Validated;
            CustomerIdText.KeyDown += CustomerIdText_KeyDown;

            WarehouseIdText.Validated += WarehouseIdText_Validated;
            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.KeyDown += WarehouseIdText_KeyDown;

            FakturItemGrid.CellContentClick += FakturItemGrid_CellContentClick;
            FakturItemGrid.CellValueChanged += FakturItemGrid_CellValueChanged;
            FakturItemGrid.CellValidated += FakturItemGrid_CellValidated;
            FakturItemGrid.KeyDown += FakturItemGrid_KeyDown;
            FakturItemGrid.EditingControlShowing += FakturItemGrid_EditingControlShowing;

            NewButton.Click += NewButton_Click;
        }

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            FakturIdText.Text = string.Empty;
            FakturDateText.Value = _dateTime.Now();
            SalesIdText.Text = string.Empty;
            SalesPersonNameTextBox.Text = string.Empty;
            CustomerIdText.Text = string.Empty;
            CustomerNameTextBox.Text = string.Empty;
            PlafondTextBox.Value = 0;
            CreditBalanceTextBox.Value = 0;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;
            TglRencanaKirimTextBox.Value= DateTime.Now.AddDays(1);
            TermOfPaymentCombo.SelectedIndex = 0;

            TotalText.Value = 0;
            DiscountText.Value = 0;
            TaxText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;
            _tipeHarga = string.Empty;

            _listItem.Clear();
            _listItem.Add(new FakturItem2Dto());
            ShowAsActive();
        }
        #endregion

        #region FAKTUR
        private void FakturButton_Click(object sender, EventArgs e)
        {
            _fakturBrowser.Filter.Date = new Periode(DateTime.Now);

            FakturIdText.Text = _fakturBrowser.Browse(FakturIdText.Text);
            FakturIdText_Validating(FakturIdText, null);
        }

        private void FakturIdText_Validating(object sender, CancelEventArgs e)
        {
            var textbox = (TextBox)sender;
            var valid = true;
            if (textbox.Text.Length == 0)
                ClearForm();
            else
                valid = ValidateFaktur();

            if (!valid)
                e.Cancel = true;
        }

        private bool ValidateFaktur()
        {
            var textbox = FakturIdText;
            var policy = Policy<FakturModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as FakturModel, (r,c) => 
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var faktur = policy.Execute(() => _fakturBuilder
                .Load(new FakturModel(textbox.Text))
                .Build());
            if (faktur is null)
                return false;

            faktur.RemoveNull();
            CustomerNameTextBox.Text = faktur.CustomerName;

            FakturDateText.Value = faktur.FakturDate;
            SalesIdText.Text = faktur.SalesPersonId;
            SalesPersonNameTextBox.Text = faktur.SalesPersonName;
            CustomerIdText.Text = faktur.CustomerId;
            CustomerNameTextBox.Text = faktur.CustomerName;
            _tipeHarga = faktur.HargaTypeId;
            PlafondTextBox.Value = faktur.Plafond;
            CreditBalanceTextBox.Value = faktur.CreditBalance;
            WarehouseIdText.Text = faktur.WarehouseId;
            WarehouseNameText.Text = faktur.WarehouseName;
            TglRencanaKirimTextBox.Value = faktur.TglRencanaKirim;
            TermOfPaymentCombo.SelectedIndex = (int)faktur.TermOfPayment;
            DueDateText.Value = faktur.DueDate;
            TotalText.Value = faktur.Total;
            DiscountText.Value = faktur.Discount;
            TaxText.Value = faktur.Tax;
            GrandTotalText.Value = faktur.GrandTotal;
            UangMukaText.Value = faktur.UangMuka;
            SisaText.Value = faktur.KurangBayar;

            _listItem.Clear();

            foreach (var item in faktur.ListItem)
            {
                //var qtyString = item.QtyInputStr;
                //var discString = item.DiscInputStr;
                //var listQtyHarga = item.ListQtyHarga
                //    .Where(x => x.SubTotal != 0)
                //    .Select(x => new FakturItemDtoStokHargaSatuan(x.Qty, x.HargaSatuan, x.Satuan));
                //var newItem = new FakturItemDto()
                //{
                //    BrgId = item.BrgId,
                    
                //    Qty = qtyString,
                //    Disc = discString,
                //    Ppn = item.PpnProsen,
                //    ListStokHargaSatuan = listQtyHarga.ToList(),
                //};
                //newItem.SetBrgName(item.BrgName);
                //newItem.SetCode(item.BrgCode);
                //newItem.SetStokHarga(item.StokHargaStr);
                //newItem.ReCalc();
                //_listItem.Add(newItem);
            }
            //_listItem.Add(new FakturItem2Dto());

            if (faktur.IsVoid)
                ShowAsVoid(faktur);
            else
                ShowAsActive();

            return true;
        }
        
        private void ShowAsVoid(FakturModel faktur)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            CancelLabel.Text = $"Faktur sudah DIBATALKAN \noleh {faktur.UserIdVoid} \npada {faktur.VoidDate:ddd, dd MMM yyyy}";
            VoidPanel.Visible = true;
            SaveButton.Visible = false;
        }
        
        private void ShowAsActive()
        {
            this.BackColor = Color.Khaki;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.Cornsilk;

            VoidPanel.Visible = false;
            SaveButton.Visible = true;
        }
        #endregion

        #region SALES-PERSON
        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
            SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
            SalesIdText_Validated(SalesIdText, null);
        }
        private void SalesIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var sales = _salesPersonDal.GetData(new SalesPersonModel(textbox.Text));
            SalesPersonNameTextBox.Text = sales?.SalesPersonName ?? string.Empty;
        }
        private void SalesIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _salesBrowser.Filter.UserKeyword = SalesIdText.Text;
                SalesIdText.Text = _salesBrowser.Browse(SalesIdText.Text);
                SalesIdText_Validated(SalesIdText, null);
                if (SalesPersonNameTextBox.Text.Length > 0)
                    CustomerIdText.Focus();
            }
        }
        #endregion

        #region CUSTOMER
        private void CustomerButton_Click(object sender, EventArgs e)
        {
            _customerBrowser.Filter.UserKeyword = CustomerIdText.Text;
            CustomerIdText.Text = _customerBrowser.Browse(CustomerIdText.Text);
            CustomerIdText_Validated(CustomerIdText, null);
        }
        private void CustomerIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var customer = _customerDal.GetData(new CustomerModel(textbox.Text));
            CustomerNameTextBox.Text = customer?.CustomerName ?? string.Empty;
            _tipeHarga = customer?.HargaTypeId??string.Empty;
        }
        private void CustomerIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _customerBrowser.Filter.UserKeyword = CustomerIdText.Text;
                CustomerIdText.Text = _customerBrowser.Browse(CustomerIdText.Text);
                CustomerIdText_Validated(CustomerIdText, null);
                if (CustomerNameTextBox.Text.Length > 0)
                    WarehouseIdText.Focus();
            }
        }
        #endregion

        #region WAREHOUSE
        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        private void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var warehouse = _warehouseDal.GetData(new WarehouseModel(textbox.Text));
            WarehouseNameText.Text = warehouse?.WarehouseName ?? string.Empty;
        }
        private void WarehouseIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _warehouseBrowser.Filter.UserKeyword = WarehouseIdText.Text;
                WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
                WarehouseIdText_Validated(WarehouseIdText, null);
                if (WarehouseNameText.Text.Length > 0)
                    TglRencanaKirimTextBox.Focus();
            }
        }
        #endregion

        #region GRID
        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex != grid.Columns["Find"].Index)
                return;

            BrowseBrg(e.RowIndex);
        }
        private void FakturItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("Qty").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("Disc").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("Ppn").Index)
                CalcTotal();
        }
        private void FakturItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <0 ) return;
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
        }
        private void FakturItemGrid_KeyDown(object sender, KeyEventArgs e)
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
        private void FakturItemGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                FakturItemGrid.EndEdit();
                BrowseBrg(FakturItemGrid.CurrentCell.RowIndex);
            }
        }
        #endregion

        private void BrowseBrg(int rowIndex)
        {
            var grid = FakturItemGrid;
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
            var stok = BuildStok(rowIndex);


            var req = new CreateFakturItemRequest(
                _listItem[rowIndex].BrgId,
                _listItem[rowIndex].QtyInputStr,
                _listItem[rowIndex].DiscInputStr,
                _listItem[rowIndex].PpnProsen,
                _tipeHarga);
            var item = _createItemWorker.Execute(req);
            _listItem[rowIndex] = item.Adapt<FakturItem2Dto>();
            //decimal hrg = 0M;
            //if (_tipeHarga.Length == 0)
            //    hrg = brg.ListHarga.FirstOrDefault()?.Harga ?? 0;
            //else
            //    hrg = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == _tipeHarga)?.Harga ?? 0;

            //_listItem[rowIndex].SetBrgName(brg.BrgName);
            //_listItem[rowIndex].SetCode(brg.BrgCode);
            //_listItem[rowIndex].Ppn = 11;

            //_listItem[rowIndex].ListStokHargaSatuan = BuildStokHrgSatuan(stok.Qty, hrg, brg.ListSatuan).ToList();
            FakturItemGrid.Refresh();
            CalcTotal();
        }

        private static IEnumerable<FakturItemDtoStokHargaSatuan> BuildStokHrgSatuan(
            int qty, decimal harga, IEnumerable<BrgSatuanModel> listSatuan)
        {
            var result = new List<FakturItemDtoStokHargaSatuan>();
            var sisa = qty;
            foreach(var item in listSatuan.OrderByDescending(x => x.Conversion))
            {
                var thisQty = (int)(sisa / item.Conversion);
                var thisHrg = harga * item.Conversion;
                var newItem = new FakturItemDtoStokHargaSatuan(thisQty, thisHrg, item.Satuan);
                result.Add(newItem);
                sisa -= (thisQty * item.Conversion);
            }
            return result;
        }
        private BrgModel BuildBrg(int rowIndex)
        {
            var id = _listItem[rowIndex].BrgId ?? string.Empty;
            var brgKey = new BrgModel(id);
            var fbk = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as BrgModel);
            var brg = fbk.Execute(() => _brgBuilder.Load(brgKey).Build());

            if (brg is null)
            {
                brg = GetBrgByCode(id);
                _listItem[rowIndex].BrgId = brg?.BrgId??string.Empty;
                FakturItemGrid.Refresh();
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

        private StokBalanceWarehouseModel BuildStok(int rowIndex)
        {
            var brgKey = new BrgModel(_listItem[rowIndex].BrgId);
            var fbk = Policy<StokBalanceModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new StokBalanceModel());
            var stok = fbk.Execute(() => _stokBalanceBuilder.Load(brgKey).Build());
            var result = stok.ListWarehouse.FirstOrDefault(x => x.WarehouseId == WarehouseIdText.Text)
                ?? new StokBalanceWarehouseModel { Qty = 0, }; 
            return result;
        }

        private void CleanRow(int rowIndex)
        {
            //_listItem[rowIndex].SetBrgName(string.Empty);
            //_listItem[rowIndex].SetCode(string.Empty);
            //FakturItemGrid.Refresh();
        }
        private void InitGrid()
        {
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturItemGrid.DataSource = binding;
            FakturItemGrid.Refresh();
            FakturItemGrid.Columns.SetDefaultCellStyle(Color.Beige);

            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            FakturItemGrid.Columns.Insert(1, buttonCol);

            //  TODO: Sesuaikan mapping kolom dari  FakturItem2Dto (DTO baru)
            var cols = FakturItemGrid.Columns;
            cols.GetCol("BrgId").Visible = true;
            cols.GetCol("BrgName").Visible = true;
            cols.GetCol("BrgCode").Visible = true;
            cols.GetCol("StokHargaStr").Visible = true;
            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyDetilStr").Visible = true;

            cols.GetCol("QtyBesar").Visible = false;
            cols.GetCol("SatBesar").Visible = false;
            cols.GetCol("Conversion").Visible = false;
            cols.GetCol("HrgSatBesar").Visible = false;
            
            cols.GetCol("QtyKecil").Visible = false;
            cols.GetCol("SatKecil").Visible = false;
            cols.GetCol("HrgSatKecil").Visible = false;
            cols.GetCol("QtyJual").Visible = false;
            cols.GetCol("HrgSat").Visible = false;
            cols.GetCol("SubTotal").Visible = true;

            cols.GetCol("QtyBonus").Visible = false;
            cols.GetCol("QtyPotStok").Visible = false;
            
            cols.GetCol("DiscInputStr").Visible = true;
            cols.GetCol("DiscDetilStr").Visible = true;
            cols.GetCol("DiscRp").Visible = false;
            cols.GetCol("PpnProsen").Visible = true;
            cols.GetCol("PpnRp").Visible = false;
            cols.GetCol("Total").Visible = true;

        //  hide
        FakturItemGrid.Columns.GetCol("DiscTotal").Visible = false;
            FakturItemGrid.Columns.GetCol("PpnRp").Visible = false;
            FakturItemGrid.Columns.GetCol("Find").Visible = false;
            //  width
            FakturItemGrid.Columns.GetCol("BrgId").Width = 50;
            FakturItemGrid.Columns.GetCol("Code").Width = 80;
            FakturItemGrid.Columns.GetCol("Find").Width = 20;
            FakturItemGrid.Columns.GetCol("BrgName").Width = 160;
            FakturItemGrid.Columns.GetCol("StokHarga").Width = 100;
            FakturItemGrid.Columns.GetCol("Qty").Width = 50;
            FakturItemGrid.Columns.GetCol("QtyDetil").Width = 80;
            FakturItemGrid.Columns.GetCol("SubTotal").Width = 80;
            FakturItemGrid.Columns.GetCol("Disc").Width = 65;
            FakturItemGrid.Columns.GetCol("DiscRp").Width = 100;
            FakturItemGrid.Columns.GetCol("Ppn").Width = 25;
            FakturItemGrid.Columns.GetCol("Total").Width = 80;
            //  right align
            FakturItemGrid.Columns.GetCol("StokHarga").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns.GetCol("QtyDetil").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns.GetCol("SubTotal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns.GetCol("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            //  multi-line
            FakturItemGrid.Columns.GetCol("StokHarga").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            FakturItemGrid.Columns.GetCol("QtyDetil").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            FakturItemGrid.Columns.GetCol("DiscRp").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //  number-format
            FakturItemGrid.Columns.GetCol("SubTotal").DefaultCellStyle.Format = "#,##0.00";
            FakturItemGrid.Columns.GetCol("Total").DefaultCellStyle.Format = "#,##0.00";
            FakturItemGrid.Columns.GetCol("BrgName").DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            //  auto-resize-rows
            FakturItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FakturItemGrid.AutoResizeRows();

        }
        #endregion

        #region TOTAL
        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            //DiscountText.Value = _listItem.Sum(x => x.DiscTotal);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
            SisaText.Value = GrandTotalText.Value - UangMukaText.Value;
        }
        private void DiscountLainText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        private void BiayaLainText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        private void UangMukaText_Validated(object sender, EventArgs e)
        {
            CalcTotal();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var mainform = (MainForm)this.Parent.Parent;
            var cmd = new SaveFakturRequest
            {
                FakturId = FakturIdText.Text,
                FakturDate = FakturDateText.Value.ToString("yyyy-MM-dd"),
                CustomerId = CustomerIdText.Text,
                SalesPersonId = SalesIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                RencanaKirimDate = TglRencanaKirimTextBox.Value.ToString("yyyy-MM-dd"),
                TermOfPayment = TermOfPaymentCombo.SelectedIndex,
                DueDate = DueDateText.Value.ToString("yyyy-MM-dd"),
                UserId = mainform.UserId.UserId,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName?.Length > 0
                select new SaveFakturRequestItem
                {
                    BrgId = c.BrgId,
                    StokHarga = c.StokHargaStr,
                    QtyString = c.QtyInputStr,
                    DiscountString = c.DiscInputStr,
                    PpnProsen = c.PpnProsen,
                }).ToList();
            cmd.ListBrg = listItem;
            var result = _saveFakturWorker.Execute(cmd);

            ClearForm();
            var fakturDb = _fakturBuilder
                .Load(result)
                .Build();

            LastIdLabel.Text = $"{result.FakturId} - {fakturDb.FakturCode}";
        }
        #endregion
    }
}