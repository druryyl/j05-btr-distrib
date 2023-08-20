using btr.application.InventoryContext.StokAgg.UseCases;
using btr.application.InventoryContext.WarehouseAgg.UseCases;
using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.application.SalesContext.FakturAgg.UseCases;
using btr.application.SalesContext.SalesPersonAgg.UseCases;
using btr.distrib.Browsers;
using btr.distrib.PrintDocs;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using Mapster;
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

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturForm : Form
    {
        private readonly BindingList<FakturItemDto> _listItem = new BindingList<FakturItemDto>();
        private readonly IMediator _mediator;
        private readonly IFakturBrowser _fakturBrowser;
        private readonly IBrgStokBrowser _brgStokBrowser;
        private readonly IWarehouseBrowser _warehouseBrowser;
        private readonly IFakturPrintDoc _fakturPrintDoc;

        public FakturForm(IMediator mediator,
            IFakturBrowser fakturBrowser,
            IBrgStokBrowser brgStokBrowser,
            IFakturPrintDoc fakturPrintDoc,
            IWarehouseBrowser warehouseBrowser)
        {
            InitializeComponent();
            InitGrid();
            ClearForm();
            RegisterEventHandler();

            _mediator = mediator;
            _fakturBrowser = fakturBrowser;
            _brgStokBrowser = brgStokBrowser;
            _fakturPrintDoc = fakturPrintDoc;
            _warehouseBrowser = warehouseBrowser;
        }

        private void RegisterEventHandler()
        {
            WarehouseIdText.Validated += WarehouseIdText_Validated;
        }


        private void ClearForm()
        {
            FakturIdText.Text = string.Empty;
            FakturDateText.Value = DateTime.Now;
            SalesIdText.Text = string.Empty;
            SalesPersonNameTextBox.Text = string.Empty;
            CustomerIdText.Text = string.Empty;
            CustomerNameTextBox.Text = string.Empty;
            PlafondTextBox.Value = 0;
            CreditBalanceTextBox.Value = 0;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;
            TglRencanaKirimTextBox.Value= DateTime.Now.AddDays(1);
            TermOfPaymentComboBox.SelectedIndex = 0;

            TotalText.Value = 0;
            DiscountLainText.Value = 0;
            BiayaLainText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;

            _listItem.Clear();
            _listItem.Add(new FakturItemDto(_mediator));
        }

        #region FAKTUR
        private async void FakturButton_Click(object sender, EventArgs e)
        {
            var form = new BrowserForm<ListFakturResponse, string>(_fakturBrowser, FakturIdText.Text, x => x.CustomerName);
            var resultDialog = form.ShowDialog();
            if (resultDialog == DialogResult.OK)
            {
                FakturIdText.Text = form.ReturnedValue;
                await ValidateFaktur();
            }
            SalesIdText.Focus();
        }
        private async void FakturIdText_Validating(object sender, CancelEventArgs e)
        {
            await ValidateFaktur();
        }
        private void FakturIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                ClearForm();
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
                    .Select(x => new FakturItemDtoStokHargaSatuan(x.Qty, x.HargaJual, x.Satuan));
                var newItem = new FakturItemDto(_mediator)
                {
                    BrgId = item.BrgId,
                    Qty = qtyString,
                    Disc = discString,
                    ListStokHargaSatuan = listQtyHarga.ToList(),
                };
                newItem.SetBrgName(item.BrgName);
                newItem.ReCalc();
                _listItem.Add(newItem);
            }
            _listItem.Add(new FakturItemDto(_mediator));
            RefreshGrid();
        }
        #endregion

        #region SALES-PERSON
        private async void SalesPersonButton_ClickAsync(object sender, EventArgs e)
        {
            var query = new ListDataSalesPersonQuery();
            var list = await _mediator.Send(query);
            var form = new BrowserForm<ListDataSalesPersonResponse, string>(list, SalesIdText.Text, x => x.SalesPersonName);
            var resultDialog = form.ShowDialog();
            if (resultDialog == DialogResult.OK)
            {
                SalesIdText.Text = form.ReturnedValue;
                await ValidateSalesPerson();
            }
            CustomerIdText.Focus();
        }

        private async void SalesIdText_Validating(object sender, CancelEventArgs e)
        {
            await ValidateSalesPerson();
        }

        private async Task ValidateSalesPerson()
        {
            var textbox = SalesIdText;
            var policy = Policy<GetSalesPersonResponse>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new GetSalesPersonResponse());
            var query = new GetSalesPersonQuery(textbox.Text);
            Task<GetSalesPersonResponse> QueryFunc() => _mediator.Send(query);

            var result = await policy.ExecuteAsync(QueryFunc);
            result.RemoveNull();
            SalesPersonNameTextBox.Text = result.SalesPersonName;
        }
        #endregion

        #region CUSTOMER
        private async void CustomerButton_Click(object sender, EventArgs e)
        {
            var query = new ListCustomerQuery();
            var list = await _mediator.Send(query);
            var form = new BrowserForm<ListCustomerResponse, string>(list, CustomerIdText.Text, x => x.CustomerName);
            var resultDialog = form.ShowDialog();
            if (resultDialog == DialogResult.OK)
            {
                CustomerIdText.Text = form.ReturnedValue;
                await ValidateCustomer();
            }
            WarehouseIdText.Focus();
        }
        private async void CustomerIdText_Validating(object sender, CancelEventArgs e)
        {
            await ValidateCustomer();
        }
        private async Task ValidateCustomer()
        {
            var textbox = CustomerIdText;
            var policy = Policy<CustomerModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new CustomerModel());

            var query = new GetCustomerQuery(textbox.Text);
            Task<CustomerModel> QueryFunc() => _mediator.Send(query);

            var result = await policy.ExecuteAsync(QueryFunc);
            result.RemoveNull();
            CustomerNameTextBox.Text = result.CustomerName;
        }
        #endregion

        #region WAREHOUSE
        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        private async void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var fallback = Policy<WarehouseModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new WarehouseModel());
            var query = new GetWarehouseQuery(textbox.Text);
            var warehouse = await fallback.ExecuteAsync(() => _mediator.Send(query));
            WarehouseNameText.Text = warehouse.WarehouseName;
        }
        #endregion

        #region GRID
        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns["Find"]?.Index && e.RowIndex >= 0)
            {
                if (WarehouseIdText.Text.Length == 0)
                    return;

                var defaultBrgId = grid.CurrentCell.Value?.ToString() ?? string.Empty;
                var warehouse = WarehouseIdText.Text;
                _brgStokBrowser.BrowserQueryArgs = new[] {warehouse };
                var form = new BrowserForm<ListBrgStokResponse, string>(_brgStokBrowser, defaultBrgId, x => x.BrgName);
                var resultDialog = form.ShowDialog();
                if (resultDialog == DialogResult.OK)
                    if (grid.CurrentRow != null)
                        grid.CurrentRow.Cells["BrgId"].Value = form.ReturnedValue;
            }
            if (_listItem.Last().BrgName.Length != 0)
                _listItem.Add(new FakturItemDto(_mediator));
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

        private void InitGrid()
        {
            RefreshGrid();

            foreach (DataGridViewColumn col in FakturItemGrid.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Consolas", 8.25f);
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                if (col.ReadOnly)
                    col.DefaultCellStyle.BackColor = Color.Beige;
            }
            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            FakturItemGrid.Columns.Insert(1, buttonCol);

            //  hide
            FakturItemGrid.Columns.GetCol("DiscTotal").Visible = false;
            FakturItemGrid.Columns.GetCol("PpnRp").Visible = false;
            //  width
            FakturItemGrid.Columns.GetCol("BrgId").Width = 50;
            FakturItemGrid.Columns.GetCol("Find").Width = 20;
            FakturItemGrid.Columns.GetCol("BrgName").Width = 150;
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
            //  auto-resize-rows
            FakturItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FakturItemGrid.AutoResizeRows();
        }
        private void RefreshGrid()
        {
            if (!_listItem.Any())
                _listItem.Add(new FakturItemDto(_mediator));

            var binding = new BindingSource
            {
                DataSource = _listItem,
            };
            FakturItemGrid.DataSource = binding;
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
        private async void SaveButton_Click(object sender, EventArgs e)
        {
            string result;
            if (FakturIdText.Text.Length == 0)
                result = await CreateFakturAsync();
            else
                result = await UpdateFakturAsync();

            if (MessageBox.Show(@"Cetak Faktur ?", @"Faktur", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var response = await _mediator.Send(new GetFakturQuery(result));
                var faktur = response.Adapt<FakturModel>();
                _fakturPrintDoc.CreateDoc(faktur);
                //_fakturPrintDoc.PrintDoc();
            }

            ClearForm();
            LastIdLabel.Text = result;
        }
        private async Task<string> CreateFakturAsync()
        {
            var cmd = new CreateFakturCommand
            {
                FakturDate = FakturDateText.Value.ToString("yyyy-MM-dd"),
                CustomerId = CustomerIdText.Text,
                SalesPersonId = SalesIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                RencanaKirimDate = TglRencanaKirimTextBox.Value.ToString("yyyy-MM-dd"),
                TermOfPayment = TermOfPaymentComboBox.SelectedIndex,
                UserId = string.Empty,
            };

            var listItem = ( 
                from c in _listItem
                where c.BrgName.Length > 0
                select new CreateFakturCommandItem
                {
                    BrgId = c.BrgId,
                    QtyString = c.Qty,
                    DiscountString = c.Disc,
                    PpnProsen = c.Ppn,
                }).ToList();
            cmd.ListBrg = listItem;

            var response = await _mediator.Send(cmd);
            return response.FakturId;
        }
        private async Task<string> UpdateFakturAsync()
        {
            var cmd = new UpdateFakturCommand
            {
                FakturId = FakturIdText.Text,
                FakturDate = FakturDateText.Value.ToString("yyyy-MM-dd"),
                CustomerId = CustomerIdText.Text,
                SalesPersonId = SalesIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                RencanaKirimDate = TglRencanaKirimTextBox.Value.ToString("yyyy-MM-dd"),
                TermOfPayment = TermOfPaymentComboBox.SelectedIndex,
                UserId = string.Empty,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName.Length > 0
                select new UpdateFakturCommandItem
                {
                    BrgId = c.BrgId,
                    QtyString = c.Qty,
                    DiscountString = c.Disc,
                    PpnProsen = c.Ppn,
                }).ToList();
            cmd.ListBrg = listItem;

            var response = await _mediator.Send(cmd);
            return response.FakturId;
        }
        #endregion
    }
}