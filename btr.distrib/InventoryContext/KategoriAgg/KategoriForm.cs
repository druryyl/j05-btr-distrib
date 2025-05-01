using btr.application.BrgContext.KategoriAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.KategoriAgg
{
    public partial class KategoriForm : Form
    {
        private readonly IKategoriDal _kategoriDal;

        private readonly IBrowser<KategoriBrowserView> _kategoriBrowser;

        private readonly IKategoriBuilder _kategoriBuilder;
        private readonly IKategoriWriter _kategoriWriter;

        private IEnumerable<KategoriFormGridDto> _listKategori;

        public KategoriForm(IKategoriDal kategoriDal,
            IBrowser<KategoriBrowserView> kategoriBrowser,
            IKategoriBuilder kategoriBuilder,
            IKategoriWriter kategoriWriter
            )
        {
            InitializeComponent();

            _kategoriDal = kategoriDal;
            _kategoriBrowser = kategoriBrowser;

            _kategoriBuilder = kategoriBuilder;
            _kategoriWriter = kategoriWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            SearchText.KeyDown += SearchText_KeyDown;

            KategoriIdText.Validated += KategoriIdText_Validated;
            KategoriButton.Click += KategoriButton_Click;


            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
            ExcelButton.Click += ExcelButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Export to Excel?", @"Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var listDb = _kategoriDal.ListData()?.ToList() ?? new List<KategoriModel>();
            var listData = listDb
                .Select(x => new
                {
                    x.KategoriId,
                    x.Code,
                    x.KategoriName,
                    x.SupplierId,
                    x.SupplierName
                }).ToList();

            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"kategori-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("kategori-info")
                    .Cell($"B1")
                    .InsertTable(listData, false);
                var ws = wb.Worksheets.First();
                //  add row number at column A
                ws.Cell("A1").Value = "No";
                for (var i = 0; i < listData.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;

                //  border header
                ws.Range("A1:F1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  font bold header and background color light blue
                ws.Range("A1:F1").Style.Font.SetBold();
                ws.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                //  freeze header
                ws.SheetView.FreezeRows(1);
                //  border table
                ws.Range($"A2:F{listData.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range($"A1:F{listData.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                ws.Range($"A1:F{listData.Count + 1}").Style.Font.SetFontName("Lucida Console");
                ws.Range($"A1:F{listData.Count + 1}").Style.Font.SetFontSize(9f);


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
            var kategoriId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var kategori = _kategoriBuilder.Load(new KategoriModel(kategoriId)).Build();
            ShowData(kategori);
        }

        private void InitGrid()
        {
            var listKategori = _kategoriDal.ListData()?.ToList()
                ?? new List<KategoriModel>();

            _listKategori = listKategori
                .Select(x => new KategoriFormGridDto(x.KategoriId,
                    x.Code,
                    x.KategoriName,
                    x.SupplierName)).ToList();
            ListGrid.DataSource = _listKategori;

            ListGrid.Columns.SetDefaultCellStyle(Color.Azure);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Code").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 150;
        }

        private void FilterListGrid(string keyword)
        {
            if (keyword.Length == 0)
            {
                ListGrid.DataSource = _listKategori;
                return;
            }
            var listFilter = _listKategori.Where(x => x.Name.ContainMultiWord(keyword)).ToList();
            ListGrid.DataSource = listFilter;
        }
        #endregion

        #region KATEGORI
        private void KategoriButton_Click(object sender, EventArgs e)
        {
            KategoriIdText.Text = _kategoriBrowser.Browse(KategoriIdText.Text);
            KategoriIdText_Validated(KategoriIdText, null);
        }

        private void KategoriIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var kategori = _kategoriBuilder
                .Load(new KategoriModel(textbox.Text))
                .Build();

            ShowData(kategori);
        }

        private void ShowData(KategoriModel kategori)
        {
            KategoriIdText.Text = kategori.KategoriId;
            KategoriNameText.Text = kategori.KategoriName;
        }

        private void ClearForm()
        {
            KategoriIdText.Clear();
            KategoriNameText.Clear();
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
            var kategori = new KategoriModel(KategoriIdText.Text);
            if (KategoriIdText.Text.Length == 0)
                kategori = _kategoriBuilder.Create().Build();
            else
                kategori = _kategoriBuilder.Load(kategori).Build();

            kategori = _kategoriBuilder
                .Attach(kategori)
                .Name(KategoriNameText.Text)
                .Build();

            _kategoriWriter.Save(ref kategori);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class KategoriFormGridDto
    {
        public KategoriFormGridDto(string id, string code, string name, string supplierName)
        {
            Id = id;
            Code = code;
            Name = name;
            SupplierName = SupplierName;
        }
        public string Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string SupplierName { get; }
    }
}
