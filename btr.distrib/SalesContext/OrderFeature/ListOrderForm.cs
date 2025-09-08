using btr.application.SalesContext.OrderFeature;
using btr.application.SalesContext.OrderMapFeature;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.OrderAgg;
using btr.domain.SalesContext.OrderStatusFeature;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.OrderFeature
{
    public partial class ListOrderForm : Form
    {
        private readonly SortableBindingList<ListOrderDto> _listOrderView;
        private readonly BindingSource _orderViewBindingSource;

        private readonly IOrderDal _orderDal;
        private readonly IOrderSummaryDal _orderSummaryDal;
        private readonly IOrderMapDal _orderMapDal;
        private ContextMenu _gridContextMenu;

        public ListOrderForm(IOrderDal orderDal,
            IOrderMapDal orderMapDal,
            IOrderSummaryDal orderSummaryDal)
        {
            InitializeComponent();

            _orderDal = orderDal;
            _orderMapDal = orderMapDal;

            _listOrderView = new SortableBindingList<ListOrderDto>();
            _orderViewBindingSource = new BindingSource(_listOrderView, null);

            RegisterEventHandler();
            InitGrid();
            InitDatePicker();
            InitContextMenu();
            _orderSummaryDal = orderSummaryDal;
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
            ShowAllCheckBox.CheckedChanged += ShowAllCheckBox_CheckedChanged;
            ListRadioButton.CheckedChanged += ViewRadioButton_CheckedChanged;
            PeriodeStartDatePicker.ValueChanged += ViewRadioButton_CheckedChanged;
        }

        private void ViewRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ListRadioButton.Checked)
            {
                OrderGrid.Visible = true;
                SummaryGrid.Visible = false;
            }
            else
            {
                OrderGrid.Visible = false;
                SummaryGrid.Visible = true;
            }
        }

        private void ShowAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        public void RefreshData()
        {
            var isAll = ShowAllCheckBox.Checked;

            var periode = new Periode(PeriodeStartDatePicker.Value, PeriodeEndDatePicker.Value);
            var diffDate = periode.Tgl2 - periode.Tgl1;
            if (diffDate.Days > 31)
            {
                MessageBox.Show(@"Periode max 31 hari");
                return;
            }
            var listOrder = _orderDal.ListData(periode)?.ToList() ?? new List<OrderModel>();
            var listOrderMap = _orderMapDal.ListData(periode)?.ToList() ?? new List<OrderMapModel>();
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
                x.CustomerName, x.CustomerCode, x.CustomerAddress, x.ItemCount, x.TotalAmount, x.StatusSync, x.OrderNote)));
            foreach(var order in _listOrderView)
            {
                var orderMap = listOrderMap.FirstOrDefault(m => m.OrderId == order.OrderId);
                if (orderMap != null)
                {
                    order.SetFakturInfo(orderMap.FakturId, orderMap.FakturCode, orderMap.UserName, orderMap.FakturDate, orderMap.NilaiFaktur);
                }
            }
            
            if (isAll)
            {
                _orderViewBindingSource.DataSource = _listOrderView;
            }
            else
            {
                _orderViewBindingSource.DataSource = _listOrderView.Where(x => x.SyncStatus == "DOWNLOADED").ToList();
            }
            _listOrderView.RaiseListChangedEvents = false;
            _listOrderView.ResetBindings();
            _listOrderView.RaiseListChangedEvents = true;
            _listOrderView.ResetBindings();

            OrderGrid.Refresh();
            var summaryList = _orderSummaryDal.ListDataSummary(periode)?.ToList() ?? new List<OrderSummaryDto>();
            SummaryGrid.DataSource = summaryList;
            SummaryGrid.Refresh();
            SummaryGrid.Columns.SetDefaultCellStyle(Color.Cornsilk);
            //  auto size columns
            foreach (DataGridViewColumn col in SummaryGrid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
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
            OrderGrid.Columns["Faktur"].Width = 80;
            OrderGrid.Columns["NilaiFaktur"].Width = 80;
            OrderGrid.Columns["OrderNote"].Width = 200;

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
            OrderGrid.Columns["OrderNote"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            OrderGrid.Columns["Address"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["SyncStatus"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            OrderGrid.Columns["FakturId"].Visible = false;
            OrderGrid.Columns["FakturDate"].Visible = false;
            OrderGrid.Columns["FakturCode"].Visible = false;


            OrderGrid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0)
                    return;
                var row = OrderGrid.Rows[e.RowIndex];
                var item = (ListOrderDto)row.DataBoundItem;
                if (item.SyncStatus == "DOWNLOADED")
                    row.DefaultCellStyle.BackColor = Color.White;
                else
                    row.DefaultCellStyle.BackColor = Color.PaleGreen;
            };
            
            foreach (DataGridViewColumn col in OrderGrid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
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
            _gridContextMenu.MenuItems.Add(new MenuItem("Delete Order", DeleteOrder_OnClick));

            OrderGrid.ContextMenu = _gridContextMenu;
        }

        private void DeleteOrder_OnClick(object sender, EventArgs e)
        {
            var grid = OrderGrid;
            var orderKey = OrderModel.Key(grid.CurrentRow.Cells["OrderId"].Value.ToString());

            var thisOrder = _listOrderView.FirstOrDefault(x => x.OrderId == orderKey.OrderId);
            if (thisOrder.SyncStatus == "TERBIT FAKTUR")
            {
                MessageBox.Show("Order sudah terbit faktur, tidak bisa dihapus", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _orderDal.Delete(orderKey);
            _orderMapDal.Delete(orderKey);
            RefreshData();
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