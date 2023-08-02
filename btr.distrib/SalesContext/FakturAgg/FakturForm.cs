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
        private List<FakturItemDto> _listItem = new List<FakturItemDto>();
        private readonly IMediator _mediator;
        private readonly IFakturBrowser _fakturBrowser;

        public FakturForm(IMediator mediator, IFakturBrowser fakturBrowser)
        {
            InitializeComponent();
            InitGrid();

            _mediator = mediator;
            _fakturBrowser = fakturBrowser;
        }

        private void HideNumericUpDownButton()
        {
            foreach (var item in this.Controls.OfType<NumericUpDown>().ToList())
                ControlStyle.HideUpDownButtons(item);
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
            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn();
            buttonCol.HeaderText = "Find"; // Set the column header text
            buttonCol.Text = "..."; // Set the button text
            buttonCol.Name = "Find"; // Set the button text
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
            var binding = new BindingSource
            {
                DataSource = _listItem
            };
            FakturItemGrid.DataSource = binding;
        }

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
            TotalTextBox.Value = (decimal)result.Total;
            GrandTotalTextBox.Value = (decimal)result.GrandTotal;

            _listItem.Clear();

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
                _listItem.Add(newItem);
            }
            RefreshGrid();
        }

        private void FakturForm_Paint(object sender, PaintEventArgs e)
        {
            HideNumericUpDownButton();
        }
    }
}
