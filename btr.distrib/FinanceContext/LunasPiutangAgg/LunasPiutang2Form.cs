using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.DataSource;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    public partial class LunasPiutang2Form : Form
    {
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangLunasViewDal _piutangLunasViewDal;
        private readonly IFakturBuilder _fakturBuilder;
        
        private BindingList<PiutangLunasView> _listPiutangLunasView;
        private BindingSource _bindingSource;

        public LunasPiutang2Form(IPiutangLunasViewDal piutangLunasViewDal,
            IPiutangBuilder piutangBuilder,
            IFakturBuilder fakturBuilder)
        {
            _piutangLunasViewDal = piutangLunasViewDal;
            _piutangBuilder = piutangBuilder;
            _fakturBuilder = fakturBuilder;

            InitializeComponent();
            InitGrid();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            ListPiutangGrid.QueryCellStyleInfo += ListPiutangGrid_QueryCellStyleInfo;
            ListPiutangGrid.TableControlCellDoubleClick += ListPiutangGrid_TableControlCellDoubleClick;
            ListPiutangGrid.TableControlCurrentCellActivated += ListPiutangGrid_TableControlCurrentCellActivated;
            ListPiutangGrid.TableControlCurrentCellValidated += ListPiutangGrid_TableControlCurrentCellValidated;

        }

        private void ListPiutangGrid_TableControlCurrentCellValidated(object sender, GridTableControlEventArgs e)
        {
            ListPiutangGrid.CancelEdit();

        }

        private void LoadPiutang(string piutangId)
        {
            var piutang = _piutangBuilder.Load(new PiutangModel(piutangId)).Build();
            var faktur = _fakturBuilder.Load(new FakturModel(piutang.PiutangId)).Build();
            var fc = faktur.FakturCode;
            var pt = piutang.PiutangId;
            PiutangIdLabel.Text = $"{pt.Substring(0,4)}-{pt.Substring(4, 3)}-{pt.Substring(7, 6)}";
            FakturCodeLabel.Text = $"{fc[0]}-{fc.Substring(1,3)}-{fc.Substring(4,4)}";
            TglFakturLabel.Text= piutang.PiutangDate.ToString("dd MMM yyyy");
            CustomerNameLabel.Text = piutang.CustomerName;
            AlamatLabel.Text = faktur.Address;
            TglTempoLabel.Text = piutang.DueDate.ToString("dd MMM yyyy");
            SalesLabel.Text = faktur.SalesPersonName;
            AdminLabel.Text = faktur.UserId;

            TotalLabel.Text= piutang.Total.ToString("N2", CultureInfo.InvariantCulture);
            TerbayarLabel.Text = piutang.Terbayar.ToString("N2", CultureInfo.InvariantCulture);
            SisaLabel.Text = piutang.Sisa.ToString("N2", CultureInfo.InvariantCulture);

        }

        #region GRID
        private void ListPiutangGrid_TableControlCurrentCellActivated(object sender, GridTableControlEventArgs e)
        {
            ListPiutangGrid.CancelEdit();
        }

        private void ListPiutangGrid_TableControlCellDoubleClick(object sender, GridTableControlCellClickEventArgs e)
        {
            var table = e.TableControl.Table;
            Element el = table.DisplayElements[e.Inner.RowIndex];
            Record r = el.ParentRecord;
            int dataRowPos = table.UnsortedRecords.IndexOf(r);
            if (dataRowPos < 0) return;

            var faktur = _listPiutangLunasView[dataRowPos];
            LoadPiutang(faktur.PiutangId);

            //try
            //{
            //}
            //catch (ArgumentOutOfRangeException) { }
            //catch (Exception)
            //{
            //    throw;
            //}

        }

        private void ListPiutangGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = System.Drawing.Color.LightPink;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void InitGrid()
        {
            //RefreshGrid();
            _listPiutangLunasView = new BindingList<PiutangLunasView>();
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _listPiutangLunasView;
            ListPiutangGrid.DataSource = _bindingSource;

            //  readonly
            ListPiutangGrid.TableDescriptor.AllowEdit = false;
            ListPiutangGrid.TableDescriptor.AllowNew = false;
            ListPiutangGrid.TableDescriptor.AllowRemove = false;
            ListPiutangGrid.ShowGroupDropArea = true;

            //  summary
            var sumColTotal = new GridSummaryColumnDescriptor("Total", SummaryType.DoubleAggregate, "Total", "{Sum}");
            sumColTotal.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumColTotal.Appearance.AnySummaryCell.Format = "N0";
            sumColTotal.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTerbayar = new GridSummaryColumnDescriptor("Terbayar", SummaryType.DoubleAggregate, "Terbayar", "{Sum}");
            sumColTerbayar.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumColTerbayar.Appearance.AnySummaryCell.Format = "N0";
            sumColTerbayar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColSisa = new GridSummaryColumnDescriptor("Sisa", SummaryType.DoubleAggregate, "Sisa", "{Sum}");
            sumColSisa.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumColSisa.Appearance.AnySummaryCell.Format = "N0";
            sumColSisa.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumColTerbayar, sumColSisa});
            ListPiutangGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            //  format number columns
            ListPiutangGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListPiutangGrid.TableDescriptor.Columns["Terbayar"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListPiutangGrid.TableDescriptor.Columns["Sisa"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListPiutangGrid.TableDescriptor.Columns["PiutangDate"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";

            //  column layout
            ListPiutangGrid.TableDescriptor.VisibleColumns.Remove("PiutangId");
            ListPiutangGrid.TableDescriptor.Columns["PiutangDate"].Width = 100;
            ListPiutangGrid.TableDescriptor.Columns["Customer"].Width = 150;
            ListPiutangGrid.TableDescriptor.Columns["Address"].Width = 200;
        }

        private void RefreshGrid()
        {
            var tgl1 = Tgl1Text.Value;
            var tgl2 = Tgl2Text.Value;
            var listPiutang = _piutangLunasViewDal.ListData(new Periode(tgl1, tgl2))?.ToList()
                ?? new List<PiutangLunasView>();
            listPiutang.ForEach(x => x.PiutangDate = x.PiutangDate.Date);
            _listPiutangLunasView = new BindingList<PiutangLunasView>(listPiutang);
            _bindingSource.DataSource = _listPiutangLunasView;
            //ListPiutangGrid.DataSource = listPiutang;
            ListPiutangGrid.Refresh();
        }
        #endregion

        private void CustomerText_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
