using btr.application.SalesContext.WilayahAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.domain.SalesContext.WilayahAgg;

namespace btr.distrib.SalesContext.WilayahAgg
{
    public partial class WilayahForm : Form
    {
        private readonly IWilayahDal _wilayahDal;

        private readonly IBrowser<WilayahBrowserView> _wilayahBrowser;

        private readonly IWilayahBuilder _wilayahBuilder;
        private readonly IWilayahWriter _wilayahWriter;

        private IEnumerable<WilayahFormGridDto> _listWilayah;

        public WilayahForm(IWilayahDal wilayahDal,
            IBrowser<WilayahBrowserView> wilayahBrowser,
            IWilayahBuilder wilayahBuilder,
            IWilayahWriter wilayahWriter
            )
        {
            InitializeComponent();

            _wilayahDal = wilayahDal;
            _wilayahBrowser = wilayahBrowser;

            _wilayahBuilder = wilayahBuilder;
            _wilayahWriter = wilayahWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            WilayahIdText.Validated += WilayahIdText_Validated;
            WilayahButton.Click += WilayahButton_Click;


            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
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
            var wilayahId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var wilayah = _wilayahBuilder.Load(new WilayahModel(wilayahId)).Build();
            ShowData(wilayah);
        }

        private void InitGrid()
        {
            var listWilayah = _wilayahDal.ListData()?.ToList()
                ?? new List<WilayahModel>();

            _listWilayah = listWilayah
                .Select(x => new WilayahFormGridDto(x.WilayahId,
                    x.WilayahName)).ToList();
            ListGrid.DataSource = _listWilayah;

            ListGrid.Columns.SetDefaultCellStyle(Color.MintCream);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 150;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listWilayah;
                return;
            }
            var listFilter = _listWilayah.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region WILAYAH-ID
        private void WilayahButton_Click(object sender, EventArgs e)
        {
            WilayahIdText.Text = _wilayahBrowser.Browse(WilayahIdText.Text);
            WilayahIdText_Validated(WilayahIdText, null);
        }

        private void WilayahIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var wilayah = _wilayahBuilder
                .Load(new WilayahModel(textbox.Text))
                .Build();

            ShowData(wilayah);
        }

        private void ShowData(WilayahModel wilayah)
        {
            WilayahIdText.Text = wilayah.WilayahId;
            WilayahNameText.Text = wilayah.WilayahName;
        }

        private void ClearForm()
        {
            WilayahIdText.Clear();
            WilayahNameText.Clear();
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
            var wilayah = new WilayahModel(WilayahIdText.Text);
            if (WilayahIdText.Text.Length == 0)
                wilayah = _wilayahBuilder.Create().Build();
            else
                wilayah = _wilayahBuilder.Load(wilayah).Build();

            wilayah = _wilayahBuilder
                .Attach(wilayah)
                .Name(WilayahNameText.Text)
                .Build();

            _wilayahWriter.Save(ref wilayah);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class WilayahFormGridDto
    {
        public WilayahFormGridDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }
        public string Name { get; }
    }
}
