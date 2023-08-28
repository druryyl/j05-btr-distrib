using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Workers;
using btr.application.SalesContext.WilayahAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.SalesPersonAgg
{
    public partial class SalesPersonForm : Form
    {
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWilayahDal _wilayahDal;

        private readonly IBrowser<SalesPersonBrowserView> _salesPersonBrowser;
        private readonly IBrowser<WilayahBrowserView> _wilayahBrowser;

        private readonly ISalesPersonBuilder _salesPersonBuilder;
        private readonly ISalesPersonWriter _salesPersonWriter;

        private IEnumerable<SalesPersonFormGridDto> _listSalesPerson;

        public SalesPersonForm(ISalesPersonDal salesPersonDal,
            IWilayahDal wilayahDal,
            IBrowser<SalesPersonBrowserView> salesPersonBrowser,
            IBrowser<WilayahBrowserView> wilayahBrowser,
            ISalesPersonBuilder salesPersonBuilder,
            ISalesPersonWriter salesPersonWriter
            )
        {
            InitializeComponent();

            _salesPersonDal = salesPersonDal;
            _wilayahDal = wilayahDal;

            _wilayahBrowser = wilayahBrowser;
            _salesPersonBrowser = salesPersonBrowser;

            _salesPersonBuilder = salesPersonBuilder;
            _salesPersonWriter = salesPersonWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            SalesPersonIdText.Validated += SalesPersonIdText_Validated;
            SalesPersonButton.Click += SalesPersonButton_Click;

            WilayahIdText.Validated += WilayahIdText_Validated;
            WilayahButton.Click += WilayahButton_Click;

            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
        }

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

            var wilayah = _wilayahDal.GetData(new WilayahModel(textbox.Text))
                ?? new WilayahModel { WilayahName = string.Empty };
            WilayahNameText.Text = wilayah.WilayahName;
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
            var salesPersonId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var salesPerson = _salesPersonBuilder.Load(new SalesPersonModel(salesPersonId)).Build();
            ShowData(salesPerson);
        }

        private void InitGrid()
        {
            var listSalesPerson = _salesPersonDal.ListData()?.ToList()
                ?? new List<SalesPersonModel>();

            _listSalesPerson = listSalesPerson
                .Select(x => new SalesPersonFormGridDto(x.SalesPersonId,
                    x.SalesPersonName,
                    x.WilayahName)).ToList();
            ListGrid.DataSource = _listSalesPerson;

            ListGrid.Columns.SetDefaultCellStyle(Color.PowderBlue);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 100;
            ListGrid.Columns.GetCol("Wilayah").Width = 100;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listSalesPerson;
                return;
            }
            var listFilter = _listSalesPerson.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            var listByWilayah = _listSalesPerson.Where(x => x.Wilayah.ContainMultiWord(keyword)).ToList();
            listFilter.AddRange(listByWilayah);
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region SALESPERSON-ID
        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            SalesPersonIdText.Text = _salesPersonBrowser.Browse(SalesPersonIdText.Text);
            SalesPersonIdText_Validated(SalesPersonIdText, null);
        }

        private void SalesPersonIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var salesPerson = _salesPersonBuilder
                .Load(new SalesPersonModel(textbox.Text))
                .Build();

            ShowData(salesPerson);
        }

        private void ShowData(SalesPersonModel salesPerson)
        {
            SalesPersonIdText.Text = salesPerson.SalesPersonId;
            SalesPersonNameText.Text = salesPerson.SalesPersonName;
        }

        private void ClearForm()
        {
            SalesPersonIdText.Clear();
            SalesPersonNameText.Clear();
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
            var salesPerson = new SalesPersonModel(SalesPersonIdText.Text);
            if (SalesPersonIdText.Text.Length == 0)
                salesPerson = _salesPersonBuilder.CreateNew().Build();
            else
                salesPerson = _salesPersonBuilder.Load(salesPerson).Build();

            salesPerson = _salesPersonBuilder
                .Attach(salesPerson)
                .Name(SalesPersonNameText.Text)
                .Wilayah(new WilayahModel(WilayahIdText.Text))
                .Build();

            _salesPersonWriter.Save(ref salesPerson);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class SalesPersonFormGridDto
    {
        public SalesPersonFormGridDto(string id, string name, string wilayah)
        {
            Id = id;
            Name = name;
            Wilayah = wilayah;
        }
        public string Id { get; }
        public string Name { get; }
        public string Wilayah { get; }
    }
}
