using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.RuteAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.RuteAgg
{
    public partial class RuteForm : Form
    {
        private readonly BindingList<CustomerView> _listCustomerView;
        private readonly BindingList<RuteItemView> _listRuteItemView;
        private readonly BindingSource _listCustomerViewBindingSource;
        private readonly BindingSource _listRuteItemViewBindingSource;

        private readonly IBrowser<RuteBrowserView> _ruteBrowser;
        private readonly ICustomerDal _customerDal;
        private readonly IRuteDal _ruteDal;
        private readonly IRuteItemDal _ruteItemDal;
        private readonly IRuteWriter _ruteWriter;
        private readonly IRuteBuilder _ruteBuilder;

        public RuteForm(
            IBrowser<RuteBrowserView> warehouseBrowser,
            ICustomerDal customerDal,
            IRuteDal ruteDal,
            IRuteItemDal ruteItemDal,
            IRuteBuilder ruteBuilder,
            IRuteWriter ruteWriter)
        {
            InitializeComponent();

            _listCustomerView = new BindingList<CustomerView>();
            _listRuteItemView = new BindingList<RuteItemView>();
            _listCustomerViewBindingSource = new BindingSource(_listCustomerView, null);
            _listRuteItemViewBindingSource = new BindingSource(_listRuteItemView, null);

            _ruteBrowser = warehouseBrowser;
            _ruteDal = ruteDal;
            _ruteItemDal = ruteItemDal;
            _customerDal = customerDal;
            _ruteBuilder = ruteBuilder;
            _ruteWriter = ruteWriter;

            InitGrid();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            AddNewRuteButton.Click += AddNewRuteButton_Click;
            SaveButton.Click += SaveButton_Click;
            RuteIdButton.Click += RuteIdButton_Click;
            RuteIdText.Validated += RuteIdText_Validated;
            SearchText.KeyDown += (s, e) => 
            {
                if (e.KeyCode == Keys.Enter)
                    RefreshListCustomer();
            };


            CustomerGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            RuteItemGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            CustomerGrid.CellContentClick += CustomerGrid_CellContentClick;
            // delete row of RuteItemGrid when user press delete key
            RuteItemGrid.KeyDown += RuteItemGrid_Keydown;
        }

        private void RuteItemGrid_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var grid = (DataGridView)sender;
                if (grid.CurrentCell is DataGridViewCheckBoxCell)
                    return;
                var rowNo = grid.CurrentCell.RowIndex;
                var customerId = _listRuteItemView[rowNo].CustomerId;

                //  remove rute item
                var rute = _ruteBuilder.Load(new RuteModel(RuteIdText.Text)).Build();
                rute.ListCustomer.RemoveAll(x => x.CustomerId == customerId);

                // remove rute id from customer
                var customer = _customerDal.GetData(new CustomerModel(customerId))
                    ?? throw new KeyNotFoundException("Customer invalid");
                customer.RuteId = string.Empty;

                using (var trans = TransHelper.NewScope())
                {
                    _ruteWriter.Save(rute);
                    _customerDal.Update(customer);
                    trans.Complete();
                }

                RefreshRute();
                RefreshListCustomer();

            }
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CustomerGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;
            
            grid.EndEdit();
            if (grid.Columns[e.ColumnIndex].Name == "IsRute")
            {
                var isRuteState = _listCustomerView[e.RowIndex].IsRute;
                if (isRuteState == true)
                    SetRute(e.RowIndex);
                else
                    ResetRute(e.RowIndex);
            }
        }

        private void SetRute(int rowNo)
        {
            var ruteId = RuteIdText.Text;
            if (ruteId == string.Empty)
            {
                SystemSounds.Exclamation.Play();
                _listCustomerView[rowNo].IsRute = false;
                CustomerGrid.Refresh();
                return;
            }
            _listCustomerView[rowNo].IsRute = true;
            _listCustomerView[rowNo].SetRute(ruteId);

            var customerId = _listCustomerView[rowNo].CustomerId;
            var customer = _customerDal.GetData(new CustomerModel(customerId))
                ?? throw new KeyNotFoundException("Customer invalid");
            customer.RuteId = ruteId;

            var rute = _ruteBuilder
                .Load(new RuteModel(ruteId))
                .AddItem(new CustomerModel(customerId))
                .Build();
            using (var trans = TransHelper.NewScope())
            {
                _ruteWriter.Save(rute);
                _customerDal.Update(customer);
                trans.Complete();
            }
            RefreshRute();
                
        }

        private void ResetRute(int rowNo)
        {
            var ruteId = _listCustomerView[rowNo].RuteId;
            _listCustomerView[rowNo].SetRute(string.Empty);
            var customerId = _listCustomerView[rowNo].CustomerId;
            var customer = _customerDal.GetData(new CustomerModel(customerId))
                ?? throw new KeyNotFoundException("Customer invalid");
            customer.RuteId = string.Empty;
            var rute = _ruteBuilder
                .Load(new RuteModel(ruteId))
                .Build();
            rute.ListCustomer.RemoveAll(x => x.CustomerId == customerId);

            using (var trans = TransHelper.NewScope())
            {
                _ruteWriter.Save(rute);
                _customerDal.Update(customer);
                trans.Complete();
            }

            RefreshRute();
        }

        public void RefreshRute()
        {
            var rute = _ruteDal.GetData(new RuteModel { RuteId = RuteIdText.Text });
            RuteIdText.Text = rute.RuteId;
            RuteCodeText.Text = rute.RuteCode;
            RuteNameText.Text = rute.RuteName;

            var listItem = _ruteItemDal.ListData(new RuteItemModel { RuteId = rute.RuteId });
            _listRuteItemView.Clear();
            foreach (var item in listItem)
            {
                _listRuteItemView.Add(new RuteItemView(item.CustomerId, item.NoUrut, item.CustomerName, item.CustomerCode, item.Address));
            }
            RuteItemGrid.Refresh();
        }

        private void RuteIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            RefreshRute();
        }

        private void RuteIdButton_Click(object sender, EventArgs e)
        {
            RuteIdText.Text = _ruteBrowser.Browse(RuteIdText.Text);
            RuteIdText_Validated(RuteIdText, null);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (RuteNameText.Text == string.Empty)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            var rute = new RuteModel
            {
                RuteId = RuteIdText.Text,
                RuteCode = RuteCodeText.Text,
                RuteName = RuteNameText.Text,
                ListCustomer = _listRuteItemView.Select((x,y) => new RuteItemModel
                {
                    CustomerId = x.CustomerId,
                    NoUrut = y + 1
                }).ToList()
            };
            rute = _ruteWriter.Save(rute);
            ClearForm();
        }

        private void AddNewRuteButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            RuteIdText.Text = string.Empty;
            RuteCodeText.Text = string.Empty;
            RuteNameText.Text = string.Empty;
            _listRuteItemView.Clear();
            RuteItemGrid.Refresh();
        }

        private void InitGrid()
        {
            RefreshListCustomer();
            CustomerGrid.DataSource = _listCustomerViewBindingSource;
            var grid = CustomerGrid;
            grid.Columns["CustomerId"].Visible = false;
            grid.Columns["CustomerCode"].HeaderText = "Kode";
            grid.Columns["CustomerName"].HeaderText = "Nama";
            grid.Columns["Address"].HeaderText = "Alamat";
            grid.Columns["Wilayah"].HeaderText = "Wilayah";
            grid.Columns["IsRute"].HeaderText = "Rute";

            grid.Columns["CustomerCode"].Width = 70;
            grid.Columns["CustomerName"].Width = 120;
            grid.Columns["Address"].Width = 170;
            grid.Columns["Wilayah"].Width = 80;
            grid.Columns["IsRute"].Width = 40;
            grid.Columns["RuteId"].Width = 50;


            RuteItemGrid.DataSource = _listRuteItemViewBindingSource;
            grid = RuteItemGrid;
            grid.Columns["CustomerId"].Visible = false;
            grid.Columns["NoUrut"].Visible = false;

            grid.Columns["NoUrut"].HeaderText = "No";
            grid.Columns["CustomerName"].HeaderText = "Nama";
            grid.Columns["CustomerCode"].HeaderText = "Kode";
            grid.Columns["Address"].HeaderText = "Alamat";

            grid.Columns["NoUrut"].Width = 30;
            grid.Columns["CustomerName"].Width = 120;
            grid.Columns["CustomerCode"].Width = 70;
            grid.Columns["Address"].Width = 170;
        }

        private void RefreshListCustomer()
        {
            var keyword = SearchText.Text;
            var listCustomer = _customerDal.ListData()?.ToList() ?? new List<CustomerModel>();
            var listCustomerDisplay = listCustomer
                .Where(x => x.CustomerName.ContainMultiWord(keyword) 
                    || x.CustomerCode.ContainMultiWord(keyword)
                    || x.Address1.ContainMultiWord(keyword)
                    || x.WilayahName.ContainMultiWord(keyword))
                .ToList();
            _listCustomerView.Clear();
            foreach (var customer in listCustomerDisplay
                                        .OrderBy(x => x.WilayahName)
                                        .ThenBy(x => x.CustomerName))
                _listCustomerView.Add(new CustomerView(customer.CustomerId, 
                    customer.CustomerName, customer.CustomerCode, 
                    customer.Address1, customer.WilayahName, customer.RuteId));
        }
    }

    public class CustomerView
    {
        public CustomerView(string customerId, string customerName, string customerCode, string address, string wilayah, string ruteId)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerCode = customerCode;
            Address = address;
            Wilayah = wilayah;
            RuteId = ruteId;
            if (RuteId != string.Empty)
                IsRute = true;
            else
                IsRute = false;
        }
        public string CustomerId { get; private set; }
        public string Wilayah { get; private set; }
        public string CustomerCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Address { get; private set; }
        public bool IsRute { get; set; }
        public string RuteId { get; private set; }

        public void SetRute(string ruteId)
        {
            RuteId = ruteId;
        }
    }

    public class RuteItemView
    {
        public RuteItemView(string customerId, int noUrut, string customerName, 
            string customerCode, string address)
        {
            CustomerId = customerId;
            NoUrut = noUrut;
            CustomerName = customerName;
            CustomerCode = customerCode;
            Address = address;
        }
        public string CustomerId { get; }
        public int NoUrut { get; }
        public string CustomerCode { get; }
        public string CustomerName { get; }
        public string Address { get; }
    }
}

/* DRAG-DROP FEATURE
private void InitializeDataGridViews()
{
    // Setup DataGridView1
    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dataGridView1.AllowUserToAddRows = false;
    dataGridView1.AllowDrop = true;

    // Setup DataGridView2
    dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dataGridView2.AllowUserToAddRows = false;
    dataGridView2.AllowDrop = true;

    // Create columns
    dataGridView1.Columns.Add("ID", "ID");
    dataGridView1.Columns.Add("Name", "Name");
    dataGridView2.Columns.Add("ID", "ID");
    dataGridView2.Columns.Add("Name", "Name");

    // Add sample data
    dataGridView1.Rows.Add("1", "Alice");
    dataGridView1.Rows.Add("2", "Bob");
    dataGridView1.Rows.Add("3", "Charlie");

    // Assign event handlers
    dataGridView1.MouseDown += DataGridView_MouseDown;
    dataGridView2.MouseDown += DataGridView_MouseDown;
    dataGridView1.DragEnter += DataGridView_DragEnter;
    dataGridView2.DragEnter += DataGridView_DragEnter;
    dataGridView1.DragDrop += DataGridView_DragDrop;
    dataGridView2.DragDrop += DataGridView_DragDrop;
}

private void DataGridView_MouseDown(object sender, MouseEventArgs e)
{
    DataGridView dgv = sender as DataGridView;
    if (dgv.SelectedRows.Count > 0)
    {
        DataGridViewRow row = dgv.SelectedRows[0];
        dgv.DoDragDrop(row, DragDropEffects.Move);
    }
}

private void DataGridView_DragEnter(object sender, DragEventArgs e)
{
    if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
    {
        e.Effect = DragDropEffects.Move;
    }
}

private void DataGridView_DragDrop(object sender, DragEventArgs e)
{
    DataGridView targetDgv = sender as DataGridView;
    DataGridViewRow draggedRow = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

    if (draggedRow != null)
    {
        DataGridView sourceDgv = draggedRow.DataGridView;

        // Clone row and add to target DataGridView
        int index = targetDgv.Rows.Add();
        for (int i = 0; i < draggedRow.Cells.Count; i++)
        {
            targetDgv.Rows[index].Cells[i].Value = draggedRow.Cells[i].Value;
        }

        // Remove row from source DataGridView
        sourceDgv.Rows.Remove(draggedRow);
    }
}
*/
