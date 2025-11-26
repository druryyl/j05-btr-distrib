using btr.application.SalesContext.OrderFeature;
using btr.application.SalesContext.OrderMapFeature;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.OrderAgg;
using btr.domain.SalesContext.OrderStatusFeature;
using btr.nuna.Domain;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
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
            ExcelButton.Click += ExcelButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            if (ListRadioButton.Checked)
            {
                ExportOrdersToExcel(_listOrderView.ToList());
            }
            else
            {
                var periode = new Periode(PeriodeStartDatePicker.Value, PeriodeEndDatePicker.Value);
                var summaryList = _orderSummaryDal.ListDataSummary(periode)?.ToList() ?? new List<OrderSummaryDto>();
                ExportOrderSummaryToExcel(summaryList);
            }
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

        private void ExportOrdersToExcel(List<ListOrderDto> listOrders)
        {
            if (listOrders == null || !listOrders.Any())
            {
                MessageBox.Show(@"No data to export.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(@"Export to Excel?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"orders-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("orders-info");

                // Create header row
                var headers = new[]
                {
            "No", "Order ID", "Order Date", "Sales", "Email", "Local ID",
            "Customer Name", "Customer Code", "Address", "Item Count",
            "Total Amount", "Sync Status", "Order Note", "Faktur Code",
            "Faktur Date", "Nilai Faktur", "Username"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(1, i + 1).Value = headers[i];
                }

                // Fill data rows
                for (int i = 0; i < listOrders.Count; i++)
                {
                    var order = listOrders[i];
                    int row = i + 2; // Start from row 2 (after header)

                    ws.Cell(row, 1).Value = i + 1; // No
                    ws.Cell(row, 2).Value = order.OrderId;
                    ws.Cell(row, 3).Value = order.OrderDate;
                    ws.Cell(row, 4).Value = order.Sales;
                    ws.Cell(row, 5).Value = order.Email;
                    ws.Cell(row, 6).Value = order.LocalID;
                    ws.Cell(row, 7).Value = order.CustomerName;
                    ws.Cell(row, 8).Value = order.CustomerCode;
                    ws.Cell(row, 9).Value = order.Address;
                    ws.Cell(row, 10).Value = order.ItemCount;
                    ws.Cell(row, 11).Value = order.TotalAmount;
                    ws.Cell(row, 12).Value = order.SyncStatus;
                    ws.Cell(row, 13).Value = order.OrderNote;
                    ws.Cell(row, 14).Value = order.FakturCode;
                    ws.Cell(row, 15).Value = order.FakturDate;
                    ws.Cell(row, 16).Value = order.NilaiFaktur;
                    ws.Cell(row, 17).Value = order.UserrName;
                }

                // Format header row
                var headerRange = ws.Range($"A1:Q{listOrders.Count + 1}"); // Q is column 17
                var headerRow = ws.Range("A1:Q1");

                // Header styling
                headerRow.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                headerRow.Style.Font.SetBold();
                headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;

                // Table border styling
                ws.Range($"A2:Q{listOrders.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A1:Q{listOrders.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                // Font styling
                headerRange.Style.Font.SetFontName("Lucida Console");
                headerRange.Style.Font.SetFontSize(9f);

                // Freeze header row
                ws.SheetView.FreezeRows(1);

                // Auto-fit columns
                ws.Columns().AdjustToContents();

                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        private void ExportOrderSummaryToExcel(List<OrderSummaryDto> orderSummaryList)
        {
            if (orderSummaryList == null || !orderSummaryList.Any())
            {
                MessageBox.Show(@"No data to export.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(@"Export to Excel?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"order-summary-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("order-summary");

                // Create header row
                var headers = new[]
                {
            "No", "Order Date", "User Email", "Order Count", "Item Count", "Total Amount"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(1, i + 1).Value = headers[i];
                }

                // Fill data rows
                for (int i = 0; i < orderSummaryList.Count; i++)
                {
                    var summary = orderSummaryList[i];
                    int row = i + 2; // Start from row 2 (after header)

                    ws.Cell(row, 1).Value = i + 1; // No
                    ws.Cell(row, 2).Value = summary.OrderDate;
                    ws.Cell(row, 3).Value = summary.UserEmail;
                    ws.Cell(row, 4).Value = summary.OrderCount;
                    ws.Cell(row, 5).Value = summary.OrderItemCount;
                    ws.Cell(row, 6).Value = summary.TotalAmountSum;
                }

                // Format header row
                var headerRange = ws.Range($"A1:F{orderSummaryList.Count + 1}");
                var headerRow = ws.Range("A1:F1");

                // Header styling
                headerRow.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                headerRow.Style.Font.SetBold();
                headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;

                // Table border styling
                ws.Range($"A2:F{orderSummaryList.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A1:F{orderSummaryList.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                // Font styling
                headerRange.Style.Font.SetFontName("Lucida Console");
                headerRange.Style.Font.SetFontSize(9f);

                // Freeze header row
                ws.SheetView.FreezeRows(1);

                // Auto-fit columns
                ws.Columns().AdjustToContents();

                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
    }
}