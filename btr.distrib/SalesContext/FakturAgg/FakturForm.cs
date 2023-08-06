using btr.application.InventoryContext.StokAgg.UseCases;
using btr.application.InventoryContext.WarehouseAgg.UseCases;
using btr.application.SalesContext.CustomerAgg.UseCases;
using btr.application.SalesContext.FakturAgg.UseCases;
using btr.application.SalesContext.SalesPersonAgg.UseCases;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturForm : Form
    {
        private BindingList<FakturItemDto> ListItem = new BindingList<FakturItemDto>();
        private readonly IMediator _mediator;
        private readonly IFakturBrowser _fakturBrowser;
        private readonly IBrgStokBrowser _brgStokBrowser;

        public FakturForm(IMediator mediator, IFakturBrowser fakturBrowser, IBrgStokBrowser brgStokBrowser)
        {
            InitializeComponent();
            InitGrid();
            ClearForm();

            _mediator = mediator;
            _fakturBrowser = fakturBrowser;
            _brgStokBrowser = brgStokBrowser;
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

            ListItem.Clear();
            ListItem.Add(new FakturItemDto(_mediator));
        }

        #region FAKTUR
        private async void FakturButton_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd");
            var query = new ListFakturQuery(now, now);
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
            Task<GetFakturResponse> queryTask() => _mediator.Send(query);
            var result = await policy.ExecuteAsync(queryTask);

            result.RemoveNull();
            CustomerNameTextBox.Text = result.CustomerName;

            FakturDateText.Value = result.FakturDate.ToDate();
            SalesIdText.Text = result.SalesPersonId;
            SalesPersonNameTextBox.Text = result.SalesPersonName;
            CustomerIdText.Text = result.CustomerId;
            CustomerNameTextBox.Text = result.CustomerName;
            PlafondTextBox.Value = (decimal)result.Plafond;
            CreditBalanceTextBox.Value = (decimal)result.CreditBalance;
            WarehouseIdText.Text = result.WarehouseId;
            WarehouseNameText.Text = result.WarehouseName;
            TglRencanaKirimTextBox.Value = result.TglRencanaKirim.ToDate();
            TotalText.Value = result.Total;
            GrandTotalText.Value = result.GrandTotal;
            UangMukaText.Value = result.UangMuka;
            SisaText.Value = result.KurangBayar;

            ListItem.Clear();

            foreach (var item in result.ListItem)
            {
                var qtyString = string.Join(";", item.ListQtyHarga.Select(x => x.Qty.ToString()));
                var discString = string.Join(";", item.ListDiscount.Select(x => x.DiscountProsen.ToString()));
                var listQtyHarga = item.ListQtyHarga
                    .Where(x => x.HargaJual != 0)
                    .Select(x => new FakturItemStokHargaSatuan(x.Qty, x.HargaJual, x.Satuan));
                var newItem = new FakturItemDto(_mediator)
                {
                    BrgId = item.BrgId,
                    Qty = qtyString,
                    Disc = discString,
                    ListStokHargaSatuan = listQtyHarga.ToList(),
                };
                newItem.SetBrgName(item.BrgName);
                newItem.ReCalc();
                ListItem.Add(newItem);
            }
            ListItem.Add(new FakturItemDto(_mediator));
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
            Task<GetSalesPersonResponse> queryFunc() => _mediator.Send(query);

            var result = await policy.ExecuteAsync(queryFunc);
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
            var policy = Policy<GetCustomerResponse>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new GetCustomerResponse());

            var query = new GetCustomerQuery(textbox.Text);
            Task<GetCustomerResponse> queryFunc() => _mediator.Send(query);

            var result = await policy.ExecuteAsync(queryFunc);
            result.RemoveNull();
            CustomerNameTextBox.Text = result.CustomerName;
        }
        #endregion

        #region WAREHOUSE
        private async void WarehouseButton_Click(object sender, EventArgs e)
        {
            var query = new ListWarehouseQuery();
            var list = await _mediator.Send(query);
            var form = new BrowserForm<ListWarehouseResponse, string>(list, WarehouseIdText.Text, x => x.WarehouseName);
            var resultDialog = form.ShowDialog();
            if (resultDialog == DialogResult.OK)
            {
                WarehouseIdText.Text = form.ReturnedValue;
                await ValidateWarehouse();
            }
            TglRencanaKirimTextBox.Focus();
        }
        private async void WarehouseIdText_Validating(object sender, CancelEventArgs e)
        {
            await ValidateWarehouse();
        }
        private async Task ValidateWarehouse()
        {
            var textbox = WarehouseIdText;
            var policy = Policy<GetWarehouseResponse>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new GetWarehouseResponse());
            var query = new GetWarehouseQuery(textbox.Text);
            Task<GetWarehouseResponse> queryFunc() => _mediator.Send(query);

            var result = await policy.ExecuteAsync(queryFunc);
            result.RemoveNull();
            WarehouseNameText.Text = result.WarehouseName;
        }
        #endregion

        #region GRID
        private void FakturItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns["Find"].Index && e.RowIndex >= 0)
            {
                if (WarehouseIdText.Text.Length == 0)
                    return;

                var defaultBrgId = grid.CurrentCell.Value?.ToString() ?? string.Empty;
                var warehouse = WarehouseIdText.Text;
                _brgStokBrowser.BrowserQueryArgs = new string[] {warehouse };
                var form = new BrowserForm<ListBrgStokResponse, string>(_brgStokBrowser, defaultBrgId, x => x.BrgName);
                var resultDialog = form.ShowDialog();
                if (resultDialog == DialogResult.OK)
                    grid.CurrentRow.Cells["BrgId"].Value = form.ReturnedValue;
            }
            if (ListItem.Last().BrgName.Length != 0)
                ListItem.Add(new FakturItemDto(_mediator));
        }
        private void FakturItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns["BrgId"].Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns["Qty"].Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns["Disc"].Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns["Ppn"].Index)
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
                HeaderText = "Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            FakturItemGrid.Columns.Insert(1, buttonCol);

            //  hide
            FakturItemGrid.Columns["DiscTotal"].Visible = false;
            FakturItemGrid.Columns["PpnRp"].Visible = false;
            //  width
            FakturItemGrid.Columns["BrgId"].Width = 50;
            FakturItemGrid.Columns["Find"].Width = 20;
            FakturItemGrid.Columns["BrgName"].Width = 150;
            FakturItemGrid.Columns["StokHarga"].Width = 100;
            FakturItemGrid.Columns["Qty"].Width = 50;
            FakturItemGrid.Columns["QtyDetil"].Width = 80;
            FakturItemGrid.Columns["SubTotal"].Width = 80;
            FakturItemGrid.Columns["Disc"].Width = 65;
            FakturItemGrid.Columns["DiscRp"].Width = 100;
            FakturItemGrid.Columns["Ppn"].Width = 25;
            FakturItemGrid.Columns["Total"].Width = 80;
            //  right align
            FakturItemGrid.Columns["StokHarga"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns["QtyDetil"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns["SubTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            FakturItemGrid.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            //  multi-line
            FakturItemGrid.Columns["StokHarga"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            FakturItemGrid.Columns["QtyDetil"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            FakturItemGrid.Columns["DiscRp"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //  number-format
            FakturItemGrid.Columns["SubTotal"].DefaultCellStyle.Format = "#,##0.00";
            FakturItemGrid.Columns["Total"].DefaultCellStyle.Format = "#,##0.00";
            //  auto-resize-rows
            FakturItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FakturItemGrid.AutoResizeRows();
        }
        private void RefreshGrid()
        {
            if (!ListItem.Any())
                ListItem.Add(new FakturItemDto(_mediator));

            var binding = new BindingSource
            {
                DataSource = ListItem,
            };
            FakturItemGrid.DataSource = binding;
        }
        #endregion

        #region TOTAL
        private void CalcTotal()
        {
            TotalText.Value = ListItem.Sum(x => x.Total);
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

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            string result;
            if (FakturIdText.Text.Length == 0)
                result = await CreateFakturAsync();
            else
                result = await UpdateFakturAsync();
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
                from c in ListItem
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
                from c in ListItem
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

    }
}