using btr.application.SalesContext.OrderFeature;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.OrderAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace btr.distrib.SalesContext.OrderFeature
{
    public partial class ListOrderForm : Form
    {
        private readonly BindingList<ListOrderDto> _listOrderView;
        private readonly BindingSource _orderViewBindingSource;

        private readonly IOrderDal _orderDal;
        private ContextMenu _gridContextMenu;

        public ListOrderForm(IOrderDal orderDal)
        {
            InitializeComponent();
            
            _orderDal = orderDal;

            _listOrderView = new BindingList<ListOrderDto>();
            _orderViewBindingSource = new BindingSource(_listOrderView, null);

            RegisterEventHandler();
            InitGrid();
            InitDatePicker();
            InitContextMenu();
        }

        private void InitDatePicker()
        {
            PeriodeStartDatePicker.Value = DateTime.Now.AddDays(-3);
            PeriodeEndDatePicker.Value = DateTime.Now;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            OrderGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            OrderGrid.MouseClick += OrderGrid_MouseClick;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        public void RefreshData()
        {
            var periode = new Periode(PeriodeStartDatePicker.Value, PeriodeEndDatePicker.Value);
            var diffDate = periode.Tgl2 - periode.Tgl1;
            if (diffDate.Days > 31)
            {
                MessageBox.Show(@"Periode max 31 hari");
                return;
            }
            var listOrder = _orderDal.ListData(periode)?.ToList() ?? new List<OrderModel>();
            _listOrderView.Clear();

            if (SearchTextBox.Text.Length > 0)
            {
                var keyword = SearchTextBox.Text.ToLower();
                listOrder = listOrder
                    .Where(x => x.OrderId.ToLower() == keyword
                        || x.CustomerName.ContainMultiWord(keyword)
                        || x.CustomerAddress.ContainMultiWord(keyword)
                        || x.SalesName.ContainMultiWord(keyword)
                        || x.UserEmail.ContainMultiWord(keyword))
                    .ToList();
            }

            listOrder.ForEach(x => _listOrderView.Add(new ListOrderDto(
                x.OrderId, x.OrderDate.ToDate(DateFormatEnum.YMD), x.SalesName, x.UserEmail, x.OrderLocalId,
                x.CustomerName, x.CustomerCode, x.CustomerAddress, x.TotalAmount, x.StatusSync)));
        }

        public void InitGrid()
        {
            OrderGrid.DataSource = _orderViewBindingSource;
            OrderGrid.Columns["ID"].Width = 110;
            OrderGrid.Columns["OrderDate"].Width = 80;
            OrderGrid.Columns["Sales"].Width = 60;
            OrderGrid.Columns["Customer"].Width = 220;
            OrderGrid.Columns["TotalAmount"].Width = 80;
            OrderGrid.Columns["SyncStatus"].Width = 100;

            OrderGrid.Columns["CustomerCode"].Visible = false;
            OrderGrid.Columns["OrderId"].Visible = false;
            OrderGrid.Columns["Email"].Visible = false;
            OrderGrid.Columns["LocalID"].Visible = false;

            OrderGrid.Columns["CustomerName"].Visible = false;
            OrderGrid.Columns["CustomerCode"].Visible = false;
            OrderGrid.Columns["Address"].Visible = false;

            OrderGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            OrderGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            OrderGrid.Columns.SetDefaultCellStyle(Color.Cornsilk);
            OrderGrid.Columns["Sales"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["Email"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            OrderGrid.Columns["CustomerName"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["CustomerCode"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["Customer"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["ID"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            OrderGrid.Columns["Address"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["SyncStatus"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

        }
        private void OrderGrid_MouseClick(object sender, MouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.Button == MouseButtons.Right)
            {
                _gridContextMenu.Show(grid, e.Location);
            }
        }
        private void InitContextMenu()
        {
            _gridContextMenu = new ContextMenu();
            _gridContextMenu.MenuItems.Add(new MenuItem("Create Faktur", CreatetFaktur_OnClick));

            OrderGrid.ContextMenu = _gridContextMenu;
        }

        private void CreatetFaktur_OnClick(object sender, EventArgs e)
        {
            var grid = OrderGrid;
            var orderKey = OrderModel.Key(grid.CurrentRow.Cells["OrderId"].Value.ToString());

            var mainMenu = (MainForm)this.Parent.Parent;
            mainMenu.ST1FakturButton_Click(null, null);
            var fakturForm = Application.OpenForms.OfType<FakturForm>().FirstOrDefault();
            fakturForm?.LoadOrder(orderKey.OrderId);
        }
    }
}