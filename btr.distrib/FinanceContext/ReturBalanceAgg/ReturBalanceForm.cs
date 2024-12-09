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
using btr.domain.SalesContext.CustomerAgg;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.FakturPotBalanceAgg;
using btr.application.SalesContext.FakturAgg.Contracts;

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
            ListReturGrid.QueryCellStyleInfo += ListReturGrid_QueryCellStyleInfo;
            ListReturGrid.TableControlCellDoubleClick += ListReturGrid_TableControlCellDoubleClick;
            ListReturGrid.TableControlCurrentCellActivated += ListReturGrid_TableControlCurrentCellActivated;
            ListReturGrid.TableControlCurrentCellValidated += ListReturGrid_TableControlCurrentCellValidated;
        }

        private void InitGrid()
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

            //  ----------
            ListReturItemGrid.DataSource = _returGridDataSource;
            ListReturItemGrid.TableDescriptor.AllowEdit = false;
            ListReturItemGrid.TableDescriptor.AllowNew = false;
            ListReturItemGrid.TableDescriptor.AllowRemove = false;
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

        private void ListReturGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = System.Drawing.Color.LightPink;
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

            RefreshGridReturItem(retur);
            RefreshGridPiutang(retur);
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
                    $"{x.Qty:N0}", listBrg.FirstOrDefault(y => y.BrgId == x.BrgId)?.Satuan ?? ""));
            _returnItemGridDataSource = new BindingList<ReturItemViewDto>(datasource.ToList());
            ListReturItemGrid.DataSource = _returnItemGridDataSource;
        }

        private void RefreshGridPiutang(ReturJualModel retur)
        {
            //  ambil existing posting
            var returBalance= _returBalanceBuilder.LoadOrCreate(retur).Build();
            var listExisting = returBalance.ListPost
                .Select((x, idx) => new FakturPotViewDto(
                    idx + 1, x.FakturId, x.FakturCode,
                    $"{x.FakturDate:dd-MMM-yyyy}", x.NilaiFaktur, x.NilaiPotong, x.NilaiPost));

            //  ambil available faktur-pot
            var listFakturPot = _fakturPotBalanceDal.ListData(retur)?.ToList() 
                ?? new List<FakturPotBalanceModel>();
            var listAvailable = listFakturPot
                .Select((x, idx) => new FakturPotViewDto(
                    idx + 1, x.FakturId, x.FakturCode, $"{x.FakturDate:dd-MM-yyyy}",
                    x.NilaiFaktur, x.NilaiPotong, x.NilaiSumPost));
            

        }

        public void RefreshGridFakturPot()
        {


        }
    }

    public class ReturBalanceViewDto
    {
        public ReturBalanceViewDto(string id, string code, string tgl,
            string customer, string address, string jenis, string sales,
            decimal grandTotal, decimal posting)
        {
            ReturJualId = id;
            ReturJualCode = code;
            ReturJualDate = tgl;
            Customer = customer;
            Address = address;
            JenisRetur = jenis;
            Sales = sales;
            GrandTotal = grandTotal;
            Posting = posting;
        }
        public string ReturJualId { get; private set; }
        public string ReturJualCode { get; private set; }
        public string ReturJualDate { get; private set; }
        public string Customer { get; private set; }
        public string Address { get; private set; }
        public string Sales { get; private set; }
        public string JenisRetur { get; private set; }
        public decimal GrandTotal { get; private set; }
        public decimal Posting { get; private set; }
    }

    public class ReturItemViewDto
    {
        public ReturItemViewDto(int no, string brgId, string brgCode, string brgName, 
            string qty, string satuan)
        {
            No = no;
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
            Qty = qty;
            Satuan = satuan;
        }
        public int No { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string Qty { get; set; }
        public string Satuan { get; set; }
    }

    public class FakturPotViewDto
    {
        public FakturPotViewDto(int no, string fakturId, string fakturCode,
            string fakturDate, decimal nilaiFaktur, decimal nilaiPotongan, decimal nilaiPosting)
        {
            No = no;
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            NilaiFaktur = nilaiFaktur;
            NilaiPotongan = nilaiPotongan;
            NilaiPosting = nilaiPosting;
        }
        public int No { get; private set; }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public string FakturDate { get; private set; }

        public decimal NilaiFaktur { get; private set; }
        public decimal NilaiPotongan { get; private set; }
        public decimal NilaiPosting { get; set; }

        public void SetPosting(decimal posting) => NilaiPosting = posting;


    }
}
