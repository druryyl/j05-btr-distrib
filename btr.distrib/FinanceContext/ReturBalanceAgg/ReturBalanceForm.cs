using btr.application.FinanceContext.ReturBalanceAgg;
using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Domain;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Syncfusion.Grouping;
using btr.application.InventoryContext.ReturJualAgg.Workers;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.application.FinanceContext.FakturPotBalanceAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using Syncfusion.Windows.Forms.Grid;
using System.Windows.Forms.DataVisualization.Charting;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace btr.distrib.FinanceContext.ReturBalanceAgg
{
    public partial class ReturBalanceForm : Form
    {
        private readonly IReturJualDal _returJualDal;
        private readonly IReturBalanceDal _returBalanceDal;
        private readonly IReturBalanceBuilder _returBalanceBuilder;
        private readonly IReturJualBuilder _returJualBuilder;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly IFakturPotBalanceDal _fakturPotBalanceDal;
        private readonly IFakturPotBalanceBuilder _fakturPotBalanceBuilder;
        private readonly IFakturPotBalanceBuilder _fakturPotBalanceWriter;
        private readonly IFakturDal _fakturDal;
        
        private BindingList<ReturBalanceViewDto> _returGridDataSource;
        private BindingList<ReturItemViewDto> _returnItemGridDataSource;
        private BindingSource _returItemBindingSource;
        private readonly FakturPotHeaderViewDto _fakturPotHeader;


        public ReturBalanceForm(IReturJualDal returJualDal,
            IReturBalanceDal returBalanceDal,
            IReturJualBuilder returJualBuilder,
            IBrgSatuanDal brgSatuanDal,
            IReturBalanceBuilder returBalanceBuilder,
            IFakturPotBalanceDal fakturPotBalanceDal,
            IFakturPotBalanceBuilder fakturPotBalanceBuilder,
            IFakturPotBalanceBuilder fakturPotBalanceWriter,
            IFakturDal fakturDal)
        {
            InitializeComponent();

            _returGridDataSource = new BindingList<ReturBalanceViewDto>();
            _returnItemGridDataSource = new BindingList<ReturItemViewDto>();
            _returItemBindingSource = new BindingSource();
            _fakturPotHeader = new FakturPotHeaderViewDto { ListDetil = new BindingList<FakturPotDetilViewDto>()};

            _returJualDal = returJualDal;
            _returBalanceDal = returBalanceDal;
            _returJualBuilder = returJualBuilder;
            _brgSatuanDal = brgSatuanDal;
            _returBalanceBuilder = returBalanceBuilder;
            _fakturPotBalanceDal = fakturPotBalanceDal;
            _fakturPotBalanceBuilder = fakturPotBalanceBuilder;
            _fakturPotBalanceWriter = fakturPotBalanceWriter;
            _fakturDal = fakturDal;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            
            ListReturGrid.QueryCellStyleInfo += AllGrid_QueryCellStyleInfo;
            ListReturItemGrid.QueryCellStyleInfo += AllGrid_QueryCellStyleInfo;
            ListFakturPotGrid.QueryCellStyleInfo += AllGrid_QueryCellStyleInfo;

            ListReturGrid.TableControlCellDoubleClick += ListReturGrid_TableControlCellDoubleClick;
            ListReturGrid.TableControlCurrentCellActivated += ListReturGrid_TableControlCurrentCellActivated;
            ListReturGrid.TableControlCurrentCellValidated += ListReturGrid_TableControlCurrentCellValidated;

            ListFakturPotGrid.TableControlCheckBoxClick += ListFakturPotGrid_TableControlCheckBoxClick;

        }

        private void ListFakturPotGrid_TableControlCheckBoxClick(object sender, GridTableControlCellClickEventArgs e)
        {
            var grid = (GridGroupingControl)sender;
            var currentCell = e.TableControl.CurrentCell;
            var record = e.TableControl.Table.CurrentRecord;

            if (currentCell != null && record != null)
            {
                // Get the field name of the current cell
                string fieldName = e.TableControl.TableDescriptor.Columns[currentCell.ColIndex].MappingName;

                if (fieldName == "Post")
                {
                    // Directly update the bound object's property
                    var item = record.GetData() as FakturPotDetilViewDto;
                    if (item != null)
                    {
                        bool newValue = !(bool)record.GetValue("Post");
                        item.Post = newValue; // Set the property directly

                        // Refresh the grid to reflect the change
                        record.SetValue("Post", newValue);
                        grid.Refresh();
                    }
                }
            }
        }


        private void InitGrid()
        {
            InitGridRetur();
            InitGridReturItem();
            InitGridFakturPot();

        }

        private void InitGridRetur()
        {
            ListReturGrid.DataSource = _returGridDataSource;
            ListReturGrid.TableDescriptor.AllowEdit = false;
            ListReturGrid.TableDescriptor.AllowNew = false;
            ListReturGrid.TableDescriptor.AllowRemove = false;
            ListReturGrid.ShowGroupDropArea = true;

            ListReturGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListReturGrid.TableDescriptor.Columns["Posting"].Appearance.AnyRecordFieldCell.Format = "N0";

            ListReturGrid.TableDescriptor.VisibleColumns.Remove("ReturJualId");
            ListReturGrid.TableDescriptor.Columns["ReturJualCode"].HeaderText = "Ret-Code";
            ListReturGrid.TableDescriptor.Columns["ReturJualDate"].HeaderText = "Tgl Retur";
            ListReturGrid.TableDescriptor.Columns["JenisRetur"].HeaderText = "Jenis";
            ListReturGrid.TableDescriptor.Columns["GrandTotal"].HeaderText = "Nilai Retur";
            ListReturGrid.TableDescriptor.Columns["Posting"].HeaderText = "Nilai Posting";

            ListReturGrid.TableDescriptor.Columns["ReturJualCode"].Width = 60;
            ListReturGrid.TableDescriptor.Columns["ReturJualDate"].Width = 50;
            ListReturGrid.TableDescriptor.Columns["Customer"].Width = 120;
            ListReturGrid.TableDescriptor.Columns["Address"].Width = 100;
            ListReturGrid.TableDescriptor.Columns["JenisRetur"].Width = 50;
            ListReturGrid.TableDescriptor.Columns["Sales"].Width = 70;
            ListReturGrid.TableDescriptor.Columns["GrandTotal"].Width = 70;
            ListReturGrid.TableDescriptor.Columns["Posting"].Width = 70;

        }

        private void InitGridReturItem()
        {
            _returItemBindingSource.DataSource = _returnItemGridDataSource;
            ListReturItemGrid.DataSource = _returItemBindingSource;
            ListReturItemGrid.TableDescriptor.AllowEdit = false;
            ListReturItemGrid.TableDescriptor.AllowNew = false;
            ListReturItemGrid.TableDescriptor.AllowRemove = false;

            //ListReturItemGrid.TableDescriptor.VisibleColumns.Remove("BrgId");

            ListReturItemGrid.TableDescriptor.Columns["No"].Width = 30;
            ListReturItemGrid.TableDescriptor.Columns["BrgCode"].Width = 70;
            ListReturItemGrid.TableDescriptor.Columns["BrgName"].Width = 200;
            ListReturItemGrid.TableDescriptor.Columns["Qty"].Width = 30;
            ListReturItemGrid.TableDescriptor.Columns["Satuan"].Width = 40;
            ListReturItemGrid.TableDescriptor.Columns["SubTotal"].Width = 70;

            ListReturItemGrid.TableDescriptor.Columns["Satuan"].HeaderText = "Sat";

            ListReturItemGrid.TableDescriptor.Columns["SubTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
        }

        private void InitGridFakturPot()
        {
            ListFakturPotGrid.DataSource = _fakturPotHeader.ListDetil;
            ListFakturPotGrid.TableDescriptor.AllowNew = false;
            ListFakturPotGrid.TableDescriptor.AllowRemove = false;

            ListFakturPotGrid.TableDescriptor.VisibleColumns.Remove("FakturId");
            //ListFakturPotGrid.TableDescriptor.VisibleColumns.Remove("NilaiReturBalance");

            ListFakturPotGrid.TableDescriptor.Columns["NilaiFaktur"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListFakturPotGrid.TableDescriptor.Columns["NilaiPotongan"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListFakturPotGrid.TableDescriptor.Columns["NilaiPosting"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListFakturPotGrid.TableDescriptor.Columns["NilaiFaktur"].HeaderText = "Grand Total";
            ListFakturPotGrid.TableDescriptor.Columns["NilaiPotongan"].HeaderText = "Potongan";
            ListFakturPotGrid.TableDescriptor.Columns["NilaiPosting"].HeaderText = "Posting";

        }

        private void ListReturGrid_TableControlCurrentCellValidated(object sender, GridTableControlEventArgs e)
        {
            ListReturGrid.CancelEdit();
        }

        private void ListReturGrid_TableControlCurrentCellActivated(object sender, GridTableControlEventArgs e)
        {
            ListReturGrid.CancelEdit();
        }

        private void ListReturGrid_TableControlCellDoubleClick(object sender, GridTableControlCellClickEventArgs e)
        {
            var table = e.TableControl.Table;
            Element el = table.DisplayElements[e.Inner.RowIndex];
            Record r = el.ParentRecord;
            int dataRowPos = table.UnsortedRecords.IndexOf(r);
            if (dataRowPos < 0) return;

            var retur = _returGridDataSource[dataRowPos];
            LoadRetur(retur.ReturJualId);
        }

        private void AllGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = System.Drawing.Color.LightPink;
            }
            if (e.Style.CellType == GridCellTypeName.TextBox)
            {
                e.Style.WrapText = true;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshGridRetur();
        }

        private void LoadRetur(string returJualId)
        {
            var retur = _returJualBuilder.Load(new ReturJualModel(returJualId)).Build();
            ReturJualIdText.Text = retur.ReturJualId;
            ReturJualCodeText.Text = retur.ReturJualCode;
            ReturJualDateText.Text = retur.ReturJualDate.ToString("dd-MMM-yyyy");
            CustomerNameText.Text = retur.CustomerName;
            CustomerAddressText.Text = retur.Address;
            SalesText.Text = retur.SalesPersonName;
            JenisReturText.Text = retur.JenisRetur;
            TotalReturTextBox.Text = $"{retur.GrandTotal:N0}";

            var returBalance = _returBalanceBuilder.LoadOrCreate(retur).Build();
            var returBalanceVal = returBalance.NilaiRetur - returBalance.NilaiSumPost;
            BalanceTextBox.Text = $"{returBalanceVal:N0}";

            RefreshGridReturItem(retur);
            LoadFakturPot(returBalance);
        }

        private void LoadFakturPot(ReturBalanceModel returBalance)
        {
            _fakturPotHeader.NilaiReturBalance = returBalance.NilaiRetur - returBalance.NilaiSumPost;
            _fakturPotHeader.ListDetil.Clear();

            var listExisting = returBalance.ListPost
                .Select((x, idx) => new FakturPotDetilViewDto(
                    idx + 1, x.FakturId, x.FakturCode,
                    $"{x.FakturDate:dd-MMM-yyyy}", x.NilaiFaktur, x.NilaiPotong, x.NilaiPost, true))
                ?.ToList()
                ?? new List<FakturPotDetilViewDto>();

            //  ambil available faktur-pot
            var listFakturPot = _fakturPotBalanceDal.ListData(returBalance)?.ToList()
                ?? new List<FakturPotBalanceModel>();
            var listAvailable = listFakturPot
                .Select((x, idx) => new FakturPotDetilViewDto(
                    idx + 1, x.FakturId, x.FakturCode, $"{x.FakturDate:dd-MM-yyyy}",
                    x.NilaiFaktur, x.NilaiPotong, x.NilaiSumPost, false))
                ?.ToList()
                ?? new List<FakturPotDetilViewDto>();

            var listGabung = listExisting.Union(listAvailable);
            var datasource = listGabung.Select((x, idx) => new FakturPotDetilViewDto(
                    idx + 1, x.FakturId, x.FakturCode, x.FakturDate, x.NilaiFaktur, x.NilaiPotongan, x.NilaiPosting, x.Post));

            _fakturPotHeader.ListDetil = new BindingList<FakturPotDetilViewDto>(datasource.ToList());
            ListFakturPotGrid.DataSource = _fakturPotHeader.ListDetil;
            _fakturPotHeader.NilaiReturBalance = returBalance.NilaiRetur - returBalance.NilaiSumPost;
        }

        private void RefreshGridRetur()
        {
            var periode = new Periode(Tgl1Text.Value, Tgl2Text.Value);
            var listRetur = _returJualDal.ListData(periode)?.ToList()
                ?? new List<ReturJualModel>();
            var listBalance = _returBalanceDal.ListData(listRetur.Select(x => x))?.ToList()
                ?? new List<ReturBalanceModel>();
            var datasource =
                from r in listRetur
                join b in listBalance on r.ReturJualId equals b.ReturJualId
                into rb
                from b in rb.DefaultIfEmpty()
                select new ReturBalanceViewDto(
                    r.ReturJualId, r.ReturJualCode, $"{r.ReturJualDate:dd-MMM}", 
                    r.CustomerName, r.Address,
                    r.JenisRetur, r.SalesPersonName, r.GrandTotal, b?.NilaiSumPost ?? 0);

            _returGridDataSource = new BindingList<ReturBalanceViewDto>(datasource.ToList());
            ListReturGrid.DataSource = _returGridDataSource;
        }

        private void RefreshGridReturItem(ReturJualModel retur)
        {
            var listBrg = _brgSatuanDal.ListData(retur.ListItem)?.ToList()
                ?? new List<BrgSatuanModel>();
            listBrg.RemoveAll(x => x.Conversion > 1);

            var datasource = retur.ListItem.Select((x, idx) =>
                new ReturItemViewDto(idx + 1, x.BrgId, x.BrgCode, x.BrgName,
                    x.Qty, listBrg.FirstOrDefault(y => y.BrgId == x.BrgId)?.Satuan ?? "",
                    x.SubTotal - x.DiscRp + x.PpnRp));
            _returnItemGridDataSource.Clear();
            foreach(var item in datasource)
                _returnItemGridDataSource.Add(item);
            var longestRow = _returnItemGridDataSource.OrderByDescending(obj => obj.BrgName.Length).FirstOrDefault()?.No ?? 1;
            ListReturItemGrid.TableControl.Model.RowHeights.ResizeToFit(GridRangeInfo.Row(longestRow+2));
        }
    }
}
