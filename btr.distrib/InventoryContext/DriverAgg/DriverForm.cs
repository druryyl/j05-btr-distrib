using btr.application.InventoryContext.DriverAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.InventoryContext.KategoriAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.DriverAgg
{
    public partial class DriverForm : Form
    {
        private readonly IDriverDal _driverDal;
        private readonly IDriverWriter _driverWriter;

        private readonly IBrowser<DriverBrowserView> _driverBrowser;

        private IEnumerable<DriverFormGridDto> _listDriver;

        public DriverForm(IDriverDal driverDal,
            IBrowser<DriverBrowserView> driverBrowser,
            IDriverWriter driverWriter)
        {
            InitializeComponent();

            _driverDal = driverDal;
            _driverBrowser = driverBrowser;

            RegisterEventHandler();
            RefreshGrid();
            _driverWriter = driverWriter;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            DriverIdText.Validated += DriverIdText_Validated;
            DriverButton.Click += KategoriButton_Click;


            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Hapus driver?", "Driver", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var driver = _driverDal.GetData(new DriverModel(DriverIdText.Text))
                ?? throw new KeyNotFoundException("Driver not found");
            driver.IsAktif = false;
            _driverWriter.Save(driver);
            RefreshGrid();
            ClearForm();

        }

        #region GRID-CUSTOMER
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FilterListGrid(SearchText.Text);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FilterListGrid(SearchText.Text);
        }

        private void ListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var grid = (DataGridView)sender;
            var driverId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var driver = _driverDal.GetData(new DriverModel(driverId)) 
                ?? throw new KeyNotFoundException("Driver not found");
            ShowData(driver);
        }

        private void RefreshGrid()
        {
            var listDriver = _driverDal.ListData()?.ToList()
                ?? new List<DriverModel>();

            _listDriver = listDriver
                .Select(x => new DriverFormGridDto(x.DriverId,
                    x.DriverName)).ToList();
            ListGrid.DataSource = _listDriver;

            ListGrid.Columns.SetDefaultCellStyle(Color.Azure);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 150;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listDriver;
                return;
            }
            var listFilter = _listDriver.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region KATEGORI
        private void KategoriButton_Click(object sender, EventArgs e)
        {
            DriverIdText.Text = _driverBrowser.Browse(DriverIdText.Text);
            DriverIdText_Validated(DriverIdText, null);
        }

        private void DriverIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var driver = _driverDal.GetData(new DriverModel(textbox.Text))
                ?? throw new KeyNotFoundException("Driver not found");

            ShowData(driver);
        }

        private void ShowData(DriverModel driver)
        {
            DriverIdText.Text = driver.DriverId;
            DriverNameText.Text = driver.DriverName;
        }

        private void ClearForm()
        {
            DriverIdText.Clear();
            DriverNameText.Clear();
        }
        #endregion

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var driver = new DriverModel(DriverIdText.Text);
            if (DriverIdText.Text.Length != 0)
                driver = _driverDal.GetData(driver)
                    ?? throw new KeyNotFoundException("DriverID invalid. Update failed");

            driver.DriverName = DriverNameText.Text;
            driver.IsAktif = true;
            _driverWriter.Save(driver);
            ClearForm();
            RefreshGrid();
        }
        #endregion
    }

    public class DriverFormGridDto
    {
        public DriverFormGridDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }
        public string Name { get; }
    }

}
