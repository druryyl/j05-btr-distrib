using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Workers;
using btr.application.SalesContext.WilayahAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.domain.SalesContext.WilayahAgg;
using btr.distrib.InventoryContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using ClosedXML.Excel;

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
            ISalesPersonWriter salesPersonWriter)
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
            ExcelButton.Click += ExcelButton_Click;
        }
        private void ExcelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Export to Excel?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var listSales = _salesPersonDal.ListData()?.ToList() ?? new List<SalesPersonModel>();

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"salesperson-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("salesperson-info")
                    .Cell($"B1")
                    .InsertTable(listSales, false);
                var ws = wb.Worksheets.First();
                //  add row number at column A
                ws.Cell("A1").Value = "No";
                for (var i = 0; i < listSales.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;

                //  border header
                ws.Range("A1:G1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  font bold header and background color light blue
                ws.Range("A1:G1").Style.Font.SetBold();
                ws.Range("A1:G1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                //  freeze header
                ws.SheetView.FreezeRows(1);
                //  border table
                ws.Range($"A2:G{listSales.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A1:G{listSales.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                ws.Range($"A1:G{listSales.Count + 1}").Style.Font.SetFontName("Lucida Console");
                ws.Range($"A1:G{listSales.Count + 1}").Style.Font.SetFontSize(9f);


                //  auto fit column
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
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
                    x.SalesPersonCode,
                    x.SalesPersonName,
                    x.WilayahName,
                    x.Email)).ToList();
            ListGrid.DataSource = _listSalesPerson;

            ListGrid.Columns.SetDefaultCellStyle(Color.PowderBlue);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Code").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 100;
            ListGrid.Columns.GetCol("Wilayah").Width = 100;
            ListGrid.Columns.GetCol("Email").Width = 120;
            ListGrid.SetAlternatingRowColors();
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
            var listByEmail = _listSalesPerson.Where(x => x.Email.ContainMultiWord(keyword)).ToList();
            listFilter.AddRange(listByWilayah.Union(listByEmail));
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
            SalesPersonCodeText.Text = salesPerson.SalesPersonCode;
            WilayahIdText.Text = salesPerson.WilayahId;
            WilayahNameText.Text = salesPerson.WilayahName;
            EmailText.Text = salesPerson.Email;
        }

        private void ClearForm()
        {
            SalesPersonIdText.Clear();
            SalesPersonNameText.Clear();
            SalesPersonCodeText.Clear();
            WilayahIdText.Clear();
            WilayahNameText.Clear();
            EmailText.Clear();
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
                .Code(SalesPersonCodeText.Text)
                .Wilayah(new WilayahModel(WilayahIdText.Text))
                .Email(EmailText.Text)
                .Build();

            _salesPersonWriter.Save(ref salesPerson);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class SalesPersonFormGridDto
    {
        public SalesPersonFormGridDto(string id, string code, string name, string wilayah, string email)
        {
            Id = id;
            Code = code;
            Name = name;
            Wilayah = wilayah;
            Email = email;
        }
        public string Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Wilayah { get; }
        public string Email { get; }
        }
}
