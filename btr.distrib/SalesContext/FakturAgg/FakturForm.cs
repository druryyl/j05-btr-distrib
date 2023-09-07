using btr.application.SalesContext.FakturAgg.UseCases;
using btr.distrib.Browsers;
using btr.distrib.PrintDocs;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
using TextBox = System.Windows.Forms.TextBox;
using btr.application.SalesContext.FakturAgg.Workers;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturForm : Form
    {
        private readonly BindingList<FakturItemDto> _listItem = new BindingList<FakturItemDto>();
        private readonly IMediator _mediator;

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

        private readonly IFakturPrintDoc _fakturPrintDoc;

        public FakturForm(IMediator mediator,
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
            IFakturPrintDoc fakturPrintDoc
,
            ITglJamDal dateTime,
            IBrgDal brgDal,
            IFakturBuilder fakturBuilder,
            ISaveFakturWorker saveFakturWorker)
        {
            _mediator = mediator;

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

            _fakturPrintDoc = fakturPrintDoc;
            _dateTime = dateTime;

            InitializeComponent();
            InitGrid();
            ClearForm();
            RegisterEventHandler();
            _brgDal = brgDal;
            _fakturBuilder = fakturBuilder;
            _saveFakturWorker = saveFakturWorker;
        }

        private void RegisterEventHandler()
        {
            SalesPersonButton.Click += SalesPersonButton_Click;
            SalesIdText.Validated += SalesIdText_Validated;

            CustomerButton.Click += CustomerButton_Click;
            CustomerIdText.Validated += CustomerIdText_Validated;

            WarehouseIdText.Validated += WarehouseIdText_Validated;

            FakturItemGrid.CellContentClick += FakturItemGrid_CellContentClick;
            FakturItemGrid.CellValueChanged += FakturItemGrid_CellValueChanged;
            FakturItemGrid.CellValidated += FakturItemGrid_CellValidated;
            FakturItemGrid.KeyDown += FakturItemGrid_KeyDown;
            FakturItemGrid.EditingControlShowing += FakturItemGrid_EditingControlShowing;
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
            DiscountLainText.Value = 0;
            BiayaLainText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;

            _listItem.Clear();
            _listItem.Add(new FakturItemDto());
        }

        #region FAKTUR
        private async void FakturButton_Click(object sender, EventArgs e)
        {
            _fakturBrowser.Filter.Date = new Periode(DateTime.Now);

            FakturIdText.Text = _fakturBrowser.Browse(FakturIdText.Text);
            await FakturIdText_ValidatedAsync(FakturIdText, null);
        }
        private async Task FakturIdText_ValidatedAsync(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                ClearForm();

            await ValidateFaktur();
        }
        private async Task ValidateFaktur()
        {
            var textbox = FakturIdText;
            var policy = Policy<GetFakturResponse>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new GetFakturResponse { ListItem = new List<GetFakturResponseItem>() });
            var query = new GetFakturQuery(textbox.Text);
            Task<GetFakturResponse> QueryTask() => _mediator.Send(query);
            var result = await policy.ExecuteAsync(QueryTask);

            result.RemoveNull();
            CustomerNameTextBox.Text = result.CustomerName;

            FakturDateText.Value = result.FakturDate.ToDate();
            SalesIdText.Text = result.SalesPersonId;
            SalesPersonNameTextBox.Text = result.SalesPersonName;
            CustomerIdText.Text = result.CustomerId;
            CustomerNameTextBox.Text = result.CustomerName;
            PlafondTextBox.Value = result.Plafond;
            CreditBalanceTextBox.Value = result.CreditBalance;
            WarehouseIdText.Text = result.WarehouseId;
            WarehouseNameText.Text = result.WarehouseName;
            TglRencanaKirimTextBox.Value = result.TglRencanaKirim.ToDate();
            TermOfPaymentCombo.SelectedIndex = result.TermOfPayment;
            DueDateText.Value = result.DueDate.ToDate(DateFormatEnum.YMD);
            TotalText.Value = result.Total;
            GrandTotalText.Value = result.GrandTotal;
            UangMukaText.Value = result.UangMuka;
            SisaText.Value = result.KurangBayar;

            _listItem.Clear();

            foreach (var item in result.ListItem)
            {
                var qtyString = string.Join(";", item.ListQtyHarga.Select(x => x.Qty.ToString()));
                var discString = string.Join(";", item.ListDiscount.Select(x => x.DiscountProsen.ToString(CultureInfo.InvariantCulture)));
                var listQtyHarga = item.ListQtyHarga
                    .Where(x => x.HargaJual != 0)
                    .Select(x => new FakturItem2DtoStokHargaSatuan(x.Qty, x.HargaJual, x.Satuan));
                var newItem = new FakturItemDto()
                {
                    BrgId = item.BrgId,
                    Qty = qtyString,
                    Disc = discString,
                    ListStokHargaSatuan = listQtyHarga.ToList(),
                };
                newItem.SetBrgName(item.BrgName);
                newItem.SetCode(item.BrgCode);
                newItem.ReCalc();
                _listItem.Add(newItem);
            }
            _listItem.Add(new FakturItemDto());
            RefreshGrid();
        }
        #endregion

        #region SALES-PERSON
        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
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
        #endregion

        #region CUSTOMER
        private void CustomerButton_Click(object sender, EventArgs e)
        {
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
            var hrg = brg.ListHarga.FirstOrDefault()?.Harga ?? 0;            

            _listItem[rowIndex].SetBrgName(brg.BrgName);
            _listItem[rowIndex].SetCode(brg.BrgCode);
            _listItem[rowIndex].ListStokHargaSatuan = BuildStokHrgSatuan(stok.Qty, hrg, brg.ListSatuan).ToList();
            FakturItemGrid.Refresh();
            CalcTotal();
        }

        private static IEnumerable<FakturItem2DtoStokHargaSatuan> BuildStokHrgSatuan(
            int qty, decimal harga, IEnumerable<BrgSatuanModel> listSatuan)
        {
            var result = new List<FakturItem2DtoStokHargaSatuan>();
            var sisa = qty;
            foreach(var item in listSatuan.OrderByDescending(x => x.Conversion))
            {
                var thisQty = (int)(sisa / item.Conversion);
                var thisHrg = harga * item.Conversion;
                var newItem = new FakturItem2DtoStokHargaSatuan(thisQty, thisHrg, item.Satuan);
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
            _listItem[rowIndex].SetBrgName(string.Empty);
            _listItem[rowIndex].SetCode(string.Empty);
            FakturItemGrid.Refresh();
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

            RefreshGrid();

            //  hide
            //FakturItemGrid.Columns.GetCol("Code").Visible = false;
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
        private void RefreshGrid()
        {
            if (!_listItem.Any())
                _listItem.Add(new FakturItemDto());
        }
        #endregion

        #region TOTAL
        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.Total);
            GrandTotalText.Value = TotalText.Value - DiscountLainText.Value + BiayaLainText.Value;
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
                    QtyString = c.Qty,
                    DiscountString = c.Disc,
                    PpnProsen = c.Ppn,
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