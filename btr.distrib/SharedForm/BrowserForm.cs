using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class BrowserForm<T, TKey> : Form
    {
        public IEnumerable<T> ListData { get; set; }
        public string ReturnedValue { get; set; }

        private IEnumerable<T> ListDataFiltered { get; set; }

        //  property selector untuk field yang akan di-filter
        private readonly Func<T, TKey> _propertySelector = null;
        private readonly IDateBrowser<T> _dateBrowser = null;
        private readonly IStringBrowser<T> _stringBrowser = null;
        private readonly string _filter2nd;

        //  constructor standard
        public BrowserForm(IEnumerable<T> listData, string returnedValue, Func<T, TKey> propertySelector)
        {
            InitializeComponent();
            HideDateInput();

            ListData = listData;
            _propertySelector = propertySelector;
            RefreshGrid();
            ReturnedValue = returnedValue;
        }

        //  constructor dengan date filter
        public BrowserForm(IDateBrowser<T> browseDate, string returnedValue, Func<T, TKey> propertySelector)
        {
            InitializeComponent();

            _dateBrowser = browseDate;
            ListData = Task.Run(() => _dateBrowser.Browse(new Periode(DateTime.Now))).GetAwaiter().GetResult();
            _propertySelector = propertySelector;
            RefreshGrid();
            ReturnedValue = returnedValue;
        }

        public BrowserForm(IStringBrowser<T> browser, string returnedValue, string filter2nd, Func<T, TKey> propertySelector)
        {
            InitializeComponent();
            HideDateInput();

            _stringBrowser = browser;
            _filter2nd = filter2nd;
            ListData = new List<T>();
            _propertySelector = propertySelector;
            RefreshGrid();
            ReturnedValue = returnedValue;
        }

        private async Task RePopulateListData()
        {
            if (_dateBrowser != null)
            {
                ListData = await _dateBrowser.Browse(new Periode(FilterDate1TextBox.Value, FilterDate2TextBox.Value));
                return;
            }
            if (_stringBrowser != null)
            {
                if (FilterTextBox.Text.Length == 0)
                    ListData = new List<T>();
                else
                    ListData = await _stringBrowser.Browse(FilterTextBox.Text, _filter2nd);
                return;
            }
        }

        private void RefreshGrid()
        {
            var keyword = FilterTextBox.Text.Trim();
            if (keyword.Length == 0)
            {
                ListDataFiltered = ListData;
            }
            else
            {
                var keywords = keyword.ToLower().Split(' ');
                ListDataFiltered = ListData
                    .Where(x => keywords.All(word => _propertySelector(x).ToString().ToLower().Contains(word)))
                    .ToList();
            }

            var binding = new BindingSource();
            binding.DataSource = ListDataFiltered;
            BrowserGrid.DataSource = binding;
            BrowserGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            BrowserGrid.AutoResizeColumns();
        }

        private void BrowserGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int allColWidth = 0;
            int colIndex = 0;
            foreach (DataGridViewColumn col in BrowserGrid.Columns)
            {
                allColWidth += (int)col.FillWeight;
                colIndex++;
            }
            int allColWidth2 =
                BrowserGrid.Columns.Cast<DataGridViewColumn>()
                    .Sum(x => x.Width)
                    + (BrowserGrid.RowHeadersVisible ? BrowserGrid.RowHeadersWidth : 0);

            var maxWidth = 1200;
            var minWidth = 300;
            var margin = 20;
            if (allColWidth2 <= maxWidth)
                this.Width = allColWidth2 + margin;

            if (allColWidth2 <= minWidth)
                this.Width = minWidth + margin;
        }

        private void BrowserGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var dataGrid = (DataGridView)sender;
            ReturnedValue = dataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            DialogResult = DialogResult.OK;
        }

        private void FilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _ = RePopulateListData();
                RefreshGrid();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            _ = RePopulateListData();
            RefreshGrid();
        }

        private void HideDateInput()
        {
            FilterDate1TextBox.Visible = false;
            FilterDate2TextBox.Visible = false;
            SearchButton.Visible = false;
            const int shiftUp = 30;
            panel1.Size = new Size(panel1.Size.Width, panel1.Size.Height - shiftUp);
            BrowserGrid.Location = new Point(BrowserGrid.Location.X, BrowserGrid.Location.Y - shiftUp);
            BrowserGrid.Size = new Size(BrowserGrid.Width, BrowserGrid.Height + shiftUp);
        }

    }
}
