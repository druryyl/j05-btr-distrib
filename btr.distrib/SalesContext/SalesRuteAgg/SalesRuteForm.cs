using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.RuteAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.SalesRuteAgg;
using btr.distrib.Helpers;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.SalesPersonAgg
{
    public partial class SalesRuteForm : Form
    {
        private readonly BindingList<CustomerViewDto> _listCustomerView;
        private readonly BindingSource _customerViewBindingSource;

        private readonly BindingList<CustomerViewDto> _listRuteItemView;
        private readonly BindingSource _ruteItemViewBindingSource;

        private string _hariRuteId;
        private readonly Dictionary<string, string> _hariDictionary;
        private DataGridViewRow dragRow = null;

        private readonly ISalesPersonDal _salesPersonDal;
        private readonly ICustomerDal _customerDal;
        private readonly IHariRuteDal _hariRuteDal;

        private readonly ISalesRuteBuilder _salesRuteBuilder;
        private readonly ISalesRuteWriter _salesRuteWriter;

        public SalesRuteForm(ISalesPersonDal salesPersonDal,
            ICustomerDal customerDal,
            IHariRuteDal hariRuteDal,
            ISalesRuteBuilder salesRuteBuilder)
        {
            InitializeComponent();
            _salesPersonDal = salesPersonDal;
            _customerDal = customerDal;
            _hariRuteDal = hariRuteDal;

            _listCustomerView = new BindingList<CustomerViewDto>();
            _customerViewBindingSource = new BindingSource(_listCustomerView, null);
            _listRuteItemView = new BindingList<CustomerViewDto>();
            _ruteItemViewBindingSource = new BindingSource(_listRuteItemView, null);
            _hariDictionary = new Dictionary<string, string>();

            RegisterEventHandler();
            InitComboBox();
            InitGrid();
            InitRadioButton();
            _salesRuteBuilder = salesRuteBuilder;
        }

        private void RegisterEventHandler()
        {
            CustomerGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            CustomerGrid.CellDoubleClick += CustomerGrid_CellDoubleClick;

            RuteItemGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            RuteItemGrid.MouseDown += RuteItemGrid_MouseDown;
            RuteItemGrid.MouseMove += RuteItemGrid_MouseMove;
            RuteItemGrid.DragOver += RuteItemGrid_DragOver;
            RuteItemGrid.DragDrop += RuteItemGrid_DragDrop;
            RuteItemGrid.QueryContinueDrag += RuteItemGrid_QueryContinueDrag;
            RuteItemGrid.CellFormatting += RuteItemGrid_CellFormatting;

            H11Radio.CheckedChanged += HariRadio_CheckedChanged;
            H12Radio.CheckedChanged += HariRadio_CheckedChanged;
            H13Radio.CheckedChanged += HariRadio_CheckedChanged;
            H14Radio.CheckedChanged += HariRadio_CheckedChanged;
            H15Radio.CheckedChanged += HariRadio_CheckedChanged;
            H16Radio.CheckedChanged += HariRadio_CheckedChanged;
            H21Radio.CheckedChanged += HariRadio_CheckedChanged;
            H22Radio.CheckedChanged += HariRadio_CheckedChanged;
            H23Radio.CheckedChanged += HariRadio_CheckedChanged;
            H24Radio.CheckedChanged += HariRadio_CheckedChanged;
            H25Radio.CheckedChanged += HariRadio_CheckedChanged;
            H26Radio.CheckedChanged += HariRadio_CheckedChanged;
        }


        private void InitComboBox()
        {
            var listSales = _salesPersonDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            listSales = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesComboBox.DataSource = listSales;
            SalesComboBox.DisplayMember = "SalesPersonName";
            SalesComboBox.ValueMember = "SalesPersonId";
            SalesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void InitGrid()
        {
            InitGridCustomer();
            InitGridRuteItem();
        }
        private void InitGridCustomer()
        {
            CustomerGrid.DataSource = _customerViewBindingSource;
            PopulateCustomer();
            CustomerGrid.Columns["CustomerId"].Visible = false;
            CustomerGrid.Columns["CustomerCode"].HeaderText = "Kode";
            CustomerGrid.Columns["CustomerName"].HeaderText = "Nama";
            CustomerGrid.Columns["Address"].HeaderText = "Alamat";

            CustomerGrid.Columns["Wilayah"].Width = 70;
            CustomerGrid.Columns["CustomerCode"].Width = 70;
            CustomerGrid.Columns["CustomerName"].Width = 120;
            CustomerGrid.Columns["Address"].Width = 168;
            CustomerGrid.Columns["Hari"].Width = 70;
        }
        private void InitGridRuteItem()
        {
            RuteItemGrid.DataSource = _ruteItemViewBindingSource;
            RuteItemGrid.Columns["CustomerId"].Visible = false;
            RuteItemGrid.Columns["Hari"].Visible = false;
            RuteItemGrid.Columns["Wilayah"].Visible = false;

            RuteItemGrid.Columns["CustomerCode"].HeaderText = "Kode";
            RuteItemGrid.Columns["CustomerName"].HeaderText = "Nama";
            RuteItemGrid.Columns["Address"].HeaderText = "Alamat";

            RuteItemGrid.Columns["Wilayah"].Width = 70;
            RuteItemGrid.Columns["CustomerCode"].Width = 70;
            RuteItemGrid.Columns["CustomerName"].Width = 120;
            RuteItemGrid.Columns["Address"].Width = 168;
            RuteItemGrid.Columns["Hari"].Width = 70;

            RuteItemGrid.AllowDrop = true;
            RuteItemGrid.MultiSelect = false;
            RuteItemGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void InitRadioButton()
        {
            H11Radio.Tag = "H11";
            H12Radio.Tag = "H12";
            H13Radio.Tag = "H13";
            H14Radio.Tag = "H14";
            H15Radio.Tag = "H15";
            H16Radio.Tag = "H16";
            H21Radio.Tag = "H21";
            H22Radio.Tag = "H22";
            H23Radio.Tag = "H23";
            H24Radio.Tag = "H24";
            H25Radio.Tag = "H25";
            H26Radio.Tag = "H26";

            var listHariRute = _hariRuteDal.ListData()?.ToList() ?? new List<HariRuteModel>();
            listHariRute.ForEach(x => _hariDictionary.Add(x.HariRuteId, x.HariRuteName));

            _hariRuteId = "H11";
            H11Radio.Checked = true;
            HariRadio_CheckedChanged(H11Radio, null);
        }

        #region ALL-CUSTOMER-GRID
        private void CustomerGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            PilihCustomerAsRuteItem(e.RowIndex);
        }
        private void PilihCustomerAsRuteItem(int rowIndex)
        {
            var selectedCustomer = _listCustomerView[rowIndex];
            if (selectedCustomer == null) 
                return;
            if (_listRuteItemView.Any(x => x.CustomerId == selectedCustomer.CustomerId))
            {
                SystemSounds.Exclamation.Play();
                return;
            }
            var newRuteItem = new CustomerViewDto(
                selectedCustomer.CustomerId,
                selectedCustomer.CustomerCode,
                selectedCustomer.CustomerName,
                selectedCustomer.Address,
                selectedCustomer.Wilayah);
            _listRuteItemView.Add(newRuteItem);
            RuteItemGrid.Refresh();
            Save();
        }
        #endregion

        #region RUTE-ITEM-GRID (DRAG-DROP-FEATURE)
        private void RuteItemGrid_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = RuteItemGrid.HitTest(e.X, e.Y);
            if (hitTest.Type == DataGridViewHitTestType.Cell)
            {
                dragRow = RuteItemGrid.Rows[hitTest.RowIndex];
                if (dragRow.IsNewRow) dragRow = null;
            }
        }
        private void RuteItemGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && dragRow != null)
            {
                RuteItemGrid.DoDragDrop(dragRow, DragDropEffects.Move);
            }
        }
        private void RuteItemGrid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void RuteItemGrid_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = RuteItemGrid.PointToClient(new Point(e.X, e.Y));
            var hitTest = RuteItemGrid.HitTest(clientPoint.X, clientPoint.Y);
            if (hitTest.Type == DataGridViewHitTestType.Cell && dragRow != null)
            {
                int targetRowIndex = hitTest.RowIndex;
                if (targetRowIndex == RuteItemGrid.NewRowIndex)
                    return;
                MoveRow(dragRow.Index, targetRowIndex);
            }
        }
        private void RuteItemGrid_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Cancel || e.EscapePressed)
            {
                RuteItemGrid.ClearSelection();
                dragRow = null;
            }
        }
        private void RuteItemGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Skip column headers and new row
            if (e.RowIndex < 0 || e.RowIndex == RuteItemGrid.NewRowIndex)
                return;

            // Calculate color group (0 or 1)
            int colorGroup = (e.RowIndex / 5) % 2;

            // Set colors
            e.CellStyle.BackColor = colorGroup == 0 ? Color.PowderBlue : Color.White;
            e.CellStyle.ForeColor = Color.Black;
        }
        private void MoveRow(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return;

            var selectedCustomer = _listRuteItemView[fromIndex].Clone();
            if (fromIndex < toIndex)
            {
                _listRuteItemView.Insert(toIndex + 1, selectedCustomer);
                _listRuteItemView.RemoveAt(fromIndex);
            }
            else
            {
                _listRuteItemView.RemoveAt(fromIndex);
                _listRuteItemView.Insert(toIndex, selectedCustomer);
            }

            RuteItemGrid.Refresh();

            DataGridViewRow row = RuteItemGrid.Rows[toIndex];
            RuteItemGrid.ClearSelection();
            row.Selected = true;
            SystemSounds.Beep.Play();
            Save();
        }
        #endregion


        private void HariRadio_CheckedChanged(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;
            if (!radio.Checked)
                return;
            _hariRuteId = radio.Tag.ToString();
            HariLabel.Text = _hariDictionary[_hariRuteId];
        }

        private void PopulateCustomer()
        {
            var listCustomer = _customerDal.ListData()?.ToList() ?? new List<CustomerModel>();
            _listCustomerView.Clear();

            listCustomer = listCustomer
                .OrderBy(x => x.WilayahName)
                .ThenBy(x => x.CustomerCode)
                .ToList();

            listCustomer
                .ForEach(x => _listCustomerView.Add(new CustomerViewDto(
                x.CustomerId, x.CustomerCode, x.CustomerName,
                x.Address1, x.WilayahName)));
            CustomerGrid.Refresh();
        }

        private void Save()
        {
            var salesPersonKey = new SalesPersonModel(SalesComboBox.SelectedValue.ToString());
            var hariRuteKey = new HariRuteModel(_hariRuteId);
            var salesRute = _salesRuteBuilder.LoadOrCreate(salesPersonKey, hariRuteKey).Build();
            salesRute.ListCustomer = _listRuteItemView
                .Select((x, y) => new SalesRuteItemModel
                {
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    NoUrut = y,
                    CustomerCode = x.CustomerCode,
                }).ToList();
            _salesRuteWriter.Save(salesRute);
        }
    }

}
