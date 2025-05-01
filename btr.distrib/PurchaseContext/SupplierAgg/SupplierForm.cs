using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.PurchaseContext.SupplierAgg.Workers;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.PurchaseContext.SupplierAgg
{
    public partial class SupplierForm : Form
    {
        private readonly ISupplierDal _supplierDal;
        private readonly ISupplierBuilder _supplierBuilder;
        private IEnumerable<SupplierFormGridDto> _listSupplier;

        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly ISupplierWriter _supplierWriter;

        public SupplierForm(ISupplierDal supplierDal,
            ISupplierBuilder supplierBuilder,
            IBrowser<SupplierBrowserView> supplierBrowser,
            ISupplierWriter supplierWriter)
        {
            InitializeComponent();

            _supplierDal = supplierDal;

            _supplierBrowser = supplierBrowser;

            _supplierBuilder = supplierBuilder;
            _supplierWriter = supplierWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            SupplierIdText.Validated += SupplierIdText_Validated;
            SupplierButton.Click += SupplierButton_Click;

            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
            ExcelButton.Click += ExcelButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Export to Excel?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var listDb = _supplierDal.ListData()?.ToList() ?? new List<SupplierModel>();
            var listData = listDb
                .Select(x => new
                {
                    x.SupplierId,
                    x.SupplierCode,
                    x.SupplierName,
                    x.Address1,
                    x.Address2,
                    x.Kota,
                    x.KodePos,
                    x.ContactPerson,
                    x.NoTelp,
                    x.NoFax,
                    x.NoPkp,
                    x.Npwp
                }).ToList();

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"supplier-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("supplier-info")
                    .Cell($"B1")
                    .InsertTable(listData, false);
                var ws = wb.Worksheets.First();
                //  add row number at column A
                ws.Cell("A1").Value = "No";
                for (var i = 0; i < listData.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;

                //  border header
                ws.Range("A1:M1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  font bold header and background color light blue
                ws.Range("A1:M1").Style.Font.SetBold();
                ws.Range("A1:M1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                //  freeze header
                ws.SheetView.FreezeRows(1);
                //  border table
                ws.Range($"A2:M{listData.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A1:M{listData.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                ws.Range($"A1:M{listData.Count + 1}").Style.Font.SetFontName("Lucida Console");
                ws.Range($"A1:M{listData.Count + 1}").Style.Font.SetFontSize(9f);


                //  auto fit column
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
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
            var custId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var cust = _supplierBuilder.Load(new SupplierModel(custId)).Build();
            ShowData(cust);
            MainTab.SelectedTab = DetilPage;
        }

        private void InitGrid()
        {
            var listSupplier = _supplierDal.ListData()?.ToList()
                ?? new List<SupplierModel>();

            _listSupplier = listSupplier
                .Select(x => new SupplierFormGridDto(x.SupplierId,
                    x.SupplierCode,
                    x.SupplierName,
                    $"{x.Address1} {x.Kota}")).ToList();
            ListGrid.DataSource = _listSupplier;

            ListGrid.Columns.SetDefaultCellStyle(Color.MistyRose);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Code").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 200;
            ListGrid.Columns.GetCol("Alamat").Width = 250;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listSupplier;
                return;
            }
            var listFilter = _listSupplier.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            var listByAlamat = _listSupplier.Where(x => x.Alamat.ContainMultiWord(keyword)).ToList();
            listFilter.AddRange(listByAlamat);
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region CUSTOMER-ID
        private void SupplierButton_Click(object sender, EventArgs e)
        {
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }

        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var supplier = _supplierBuilder
                .Load(new SupplierModel(textbox.Text))
                .Build();

            ShowData(supplier);
        }

        private void ShowData(SupplierModel supplier)
        {
            SupplierIdText.Text = supplier.SupplierId;
            SupplierNameText.Text = supplier.SupplierName;
            SupplierCodeText.Text = supplier.SupplierCode;

            Alamat1Text.Text = supplier.Address1;
            Alamat2Text.Text = supplier.Address2;
            KotaText.Text = supplier.Kota;
            KodePosText.Text = supplier.KodePos;
            NoTelponText.Text = supplier.NoTelp;
            NoFaxText.Text = supplier.NoFax;

            NpwpText.Text = supplier.Npwp;
        }

        private void ClearForm()
        {
            SupplierIdText.Clear(); ;
            SupplierNameText.Clear(); ;
            SupplierCodeText.Clear(); ;
            Alamat1Text.Clear();
            Alamat2Text.Clear();
            KotaText.Clear();
            KodePosText.Clear();
            NoTelponText.Clear();
            NoFaxText.Clear();
            NpwpText.Clear();
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
            var supplier = new SupplierModel(SupplierIdText.Text);
            if (SupplierIdText.Text.Length == 0)
                supplier = _supplierBuilder.Create().Build();
            else
                supplier = _supplierBuilder.Load(supplier).Build();

            supplier = _supplierBuilder
                .Attach(supplier)
                .Name(SupplierNameText.Text)
                .Code(SupplierCodeText.Text)
                .Address(Alamat1Text.Text, Alamat2Text.Text, KotaText.Text)
                .KodePos(KodePosText.Text)
                .NoTelp(NoTelponText.Text)
                .NoFax(NoFaxText.Text)
                .Npwp(NpwpText.Text)
                .NoPkp(NoPkpText.Text)
                .Build();

            _supplierWriter.Save(ref supplier);
            ClearForm();
            InitGrid();
        }
        #endregion
    }

    public class SupplierFormGridDto
    {
        public SupplierFormGridDto(string id, string code,  string name, string alamat)
        {
            Id = id;
            Code = code;
            Name = name;
            Alamat = alamat;
        }
        public string Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Alamat { get; }
    }

}
