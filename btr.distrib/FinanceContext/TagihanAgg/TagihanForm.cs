using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.TagihanAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.SalesRuteAgg;
using btr.application.SupportContext.ParamSistemAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.ParamSistemAgg;
using btr.nuna.Domain;
using Microsoft.Reporting.WinForms;
using Polly;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TagihanForm : Form
    {
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<TagihanFakturDto> _listTagihan = new BindingList<TagihanFakturDto>();
        private readonly IBrowser<TagihanBrowserView> _tagihanBrowser;

        private readonly IFakturDal _fakturDal;
        private readonly IParamSistemDal _paramSistemDal;
        private readonly IPiutangDal _piutangDal;
        private readonly ISalesRuteDal _salesRuteDal;
        private readonly ISalesPersonDal _salesDal;

        private readonly ITagihanBuilder _tagihanBuilder;
        private readonly ITagihanWriter _tagihanWriter;
        private readonly ISalesRuteBuilder _salesRuteBuilder;

        public TagihanForm(ISalesPersonDal salesDal,
            IFakturDal fakturDal,
            ITagihanBuilder tagihanBuilder,
            ITagihanWriter tagihanWriter,
            IBrowser<TagihanBrowserView> tagihanBrowser,
            IParamSistemDal paramSistemDal,
            IPiutangDal piutangDal,
            ISalesRuteDal salesRuteDal,
            ISalesRuteBuilder salesRuteBuilder)
        {
            InitializeComponent();

            _salesDal = salesDal;
            _fakturDal = fakturDal;
            _tagihanBuilder = tagihanBuilder;
            _tagihanWriter = tagihanWriter;
            _tagihanBrowser = tagihanBrowser;
            _paramSistemDal = paramSistemDal;
            _piutangDal = piutangDal;
            TglTagihText.Value = DateTime.Now;
            _salesRuteDal = salesRuteDal;

            InitGrid();
            InitCombo();
            RegisterEventHandler();
            _salesRuteBuilder = salesRuteBuilder;
        }

        private void RegisterEventHandler()
        {
            TagihanButton.Click += TagihanButton_Click;
            TagihanIdText.Validating += TagihanIdText_Validating;

            SalesCombo.SelectedValueChanged += SalesCombo_SelectedValueChanged;
            ListFakturButton.Click += ListFakturButton_Click;

            FakturGrid.CellValueChanged += FakturGrid_CellValidated;
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            FakturGrid.KeyDown += FakturGrid_KeyDown;

            SaveButton.Click += SaveButton_Click;
        }

        private void InitCombo()
        {
            var listSales = _salesDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            SalesCombo.DataSource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesCombo.DisplayMember = "SalesPersonName";
            SalesCombo.ValueMember = "SalesPersonId";

        }

        private void InitGrid()
        {
            _bindingSource.DataSource = _listTagihan;
            FakturGrid.DataSource = _bindingSource;
            FakturGrid.Columns.SetDefaultCellStyle(Color.LemonChiffon);
            FakturGrid.Refresh();

            var col = FakturGrid.Columns;
            col.GetCol("FakturCode").HeaderText = @"Faktur";
            col.GetCol("FakturDate").HeaderText = @"TglFaktur";
            col.GetCol("CustomerCode").HeaderText = @"CustCode";
            col.GetCol("CustomerName").HeaderText = @"Customer";
            col.GetCol("Alamat").HeaderText = @"Alamat";
            col.GetCol("NilaiTotal").HeaderText = @"Total";
            col.GetCol("NilaiTerbayar").HeaderText = @"Terbayar";
            col.GetCol("NilaiTagih").HeaderText = @"Tagihanr";

            col.GetCol("FakturId").Visible = false;
            col.GetCol("CustomerId").Visible = false;


            col.GetCol("FakturDate").DefaultCellStyle.Format = "dd MMM yyyy";
            col.GetCol("NilaiTotal").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTotal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("NilaiTerbayar").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTerbayar").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("NilaiTagih").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTagih").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            col.GetCol("FakturCode").Width = 60;
            col.GetCol("FakturDate").Width = 80;
            col.GetCol("JatuhTempo").Width = 80;
            col.GetCol("CustomerCode").Width = 70;
            col.GetCol("CustomerName").Width = 150;
            col.GetCol("Alamat").Width = 180;
            col.GetCol("NilaiTotal").Width = 75;
            col.GetCol("NilaiTerbayar").Width = 75;
            col.GetCol("NilaiTagih").Width = 75;

            col.GetCol("FakturDate").ReadOnly = true;
            col.GetCol("JatuhTempo").ReadOnly = true;
            col.GetCol("CustomerCode").ReadOnly = true;
            col.GetCol("CustomerName").ReadOnly = true;
            col.GetCol("Alamat").ReadOnly = true;
            col.GetCol("NilaiTotal").ReadOnly = true;
            col.GetCol("NilaiTerbayar").ReadOnly = true;
            col.GetCol("NilaiTagih").ReadOnly = true;

            col.GetCol("FakturDate").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("JatuhTempo").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("CustomerCode").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("CustomerName").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("Alamat").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("NilaiTotal").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("NilaiTerbayar").DefaultCellStyle.BackColor = Color.Beige;
            col.GetCol("NilaiTagih").DefaultCellStyle.BackColor = Color.Beige;
        }

        private void TagihanIdText_Validating(object sender, CancelEventArgs e)
        {
            var textbox = (TextBox)sender;
            var valid = true;
            if (textbox.Text.Length == 0)
                ClearForm();
            else
                valid = ValidateTagihan();

            if (!valid)
                e.Cancel = true;
        }

        private void TagihanButton_Click(object sender, EventArgs e)
        {
            _tagihanBrowser.Filter.Date = new Periode(DateTime.Now);

            TagihanIdText.Text = _tagihanBrowser.Browse(TagihanIdText.Text);
            TagihanIdText_Validating(TagihanIdText, null);
        }

        private void SalesCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (SalesCombo.SelectedValue == null)
                return;
            var salesId = SalesCombo.SelectedValue.ToString();
            if (salesId == string.Empty)
                return;
            var listRute = _salesRuteDal.ListData(new SalesPersonModel(salesId));
            SalesRuteCombo.DataSource = listRute;
            SalesRuteCombo.DisplayMember = "ShortName";
            SalesRuteCombo.ValueMember= "SalesRuteId";
        }

        private void ListFakturButton_Click(object sender, EventArgs e)
        {
            if (SalesRuteCombo.SelectedValue is null)
                return;

            var salesRuteId = SalesRuteCombo.SelectedValue.ToString();
            if (salesRuteId == string.Empty)
                return;

            var salesRuteKey = new SalesRuteModel(salesRuteId);
            var salesRute = _salesRuteBuilder.Load(salesRuteKey).Build();
            var listAllPiutang = new List<PiutangModel>();
            foreach(var item in salesRute.ListCustomer)
            {
                var listPiutangThisCustomer = ListPiutangPerCustomer(item);
                if (listPiutangThisCustomer.Count == 0)
                    continue;
                listAllPiutang.AddRange(listPiutangThisCustomer);
            }
            foreach(var item in listAllPiutang.OrderBy(x => x.CustomerCode).ThenBy(x => x.DueDate))
            {
                var newTagihanItem = new TagihanFakturDto
                {
                    FakturCode = item.FakturCode,
                    FakturDate = $"{item.PiutangDate:dd MMM yyyy}",
                    JatuhTempo = $"{item.DueDate:dd MMM yyyy}",
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerCode = item.CustomerCode,
                    Alamat = item.Address,
                    NilaiTotal = item.Total,
                    NilaiTerbayar = item.Terbayar,
                    NilaiTagih = item.Sisa,
                    FakturId = item.PiutangId,
                };
                if (_listTagihan.Where(x => x.FakturCode == item.FakturCode).Any())
                    continue;
                _listTagihan.Add(newTagihanItem);
            }
            RefreshGrid();
        }

        private List<PiutangModel> ListPiutangPerCustomer(ICustomerKey customerKey)
        {
            var listPiutang = _piutangDal.ListData(customerKey)?.ToList() ?? new List<PiutangModel>();
            listPiutang.RemoveAll(x => x.Sisa < 1);
            listPiutang.RemoveAll(x => x.DueDate.Date > TglTagihText.Value.Date);
            return listPiutang;
        }

        private void FakturGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _listTagihan.RemoveAt(grid.CurrentCell.RowIndex);
                    grid.Refresh();
                    break;
            }
        }

        private void FakturGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != FakturGrid.Columns.GetCol("FakturCode").Index) return;
            _listTagihan[e.RowIndex].RemoveNull();
            if (string.IsNullOrEmpty(_listTagihan[e.RowIndex].FakturCode)) return;

            var fakturCode = _listTagihan[e.RowIndex].FakturCode;
            var countFakturCode = _listTagihan.Count(x => x.FakturCode == fakturCode);
            if (countFakturCode > 1)
            {
                MessageBox.Show("Faktur sudah diinputkan");
                _listTagihan.RemoveAt(e.RowIndex);
                RefreshGrid();
                return;
            }


            var faktur = GetFaktur(fakturCode);
            var piutang = GetPiutang(fakturCode);

            if (piutang == null)
            {
                MessageBox.Show("Faktur tidak ditemukan");
            }
            if (piutang.Sisa <= 1)
            {
                MessageBox.Show("Faktur sudah lunas");
                return;
            }

            if (faktur.SalesPersonId != SalesCombo.SelectedValue.ToString())
            {
                MessageBox.Show(@"Sales tidak sesuai dengan faktur");
                _listTagihan[e.RowIndex].FakturCode = "";
                _listTagihan[e.RowIndex].FakturId = "";
                _listTagihan[e.RowIndex].FakturDate = "";
                _listTagihan[e.RowIndex].CustomerId = "";
                _listTagihan[e.RowIndex].CustomerName = "";
                _listTagihan[e.RowIndex].Alamat = "";
                _listTagihan[e.RowIndex].NilaiTotal = 0;
                _listTagihan[e.RowIndex].NilaiTerbayar = 0;
                _listTagihan[e.RowIndex].NilaiTagih = 0;
                return;
            }

            _listTagihan[e.RowIndex].FakturId = faktur.FakturId;
            _listTagihan[e.RowIndex].FakturDate = $"{faktur.FakturDate:dd-MMM-yyyy}";
            _listTagihan[e.RowIndex].JatuhTempo= $"{faktur.DueDate:dd-MMM-yyyy}";
            _listTagihan[e.RowIndex].CustomerId = faktur.CustomerId;
            _listTagihan[e.RowIndex].CustomerCode = faktur.CustomerCode;
            _listTagihan[e.RowIndex].CustomerName = faktur.CustomerName;
            _listTagihan[e.RowIndex].Alamat = faktur.Address;
            _listTagihan[e.RowIndex].NilaiTotal = piutang.Total + piutang.Potongan;
            _listTagihan[e.RowIndex].NilaiTerbayar = piutang.Terbayar;
            _listTagihan[e.RowIndex].NilaiTagih = piutang.Sisa;

            RefreshGrid();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var tagihan = new TagihanModel();
            if (TagihanIdText.Text.Trim().Length == 0)
                tagihan = _tagihanBuilder
                    .Create()
                    .Sales(new SalesPersonModel(SalesCombo.SelectedValue.ToString()))
                    .TglTagihan(TglTagihText.Value)
                    .Build();
            else
                tagihan = _tagihanBuilder
                    .Load(new TagihanModel(TagihanIdText.Text))
                    .Sales(new SalesPersonModel(SalesCombo.SelectedValue.ToString()))
                    .TglTagihan(TglTagihText.Value)
                    .Build();

            tagihan.ListFaktur.Clear();
            tagihan.TotalTagihan = 0;
            tagihan = _listTagihan.Aggregate(tagihan, (current, item) => _tagihanBuilder.Attach(current)
                .AddFaktur(new FakturModel(item.FakturId), item.NilaiTotal, item.NilaiTerbayar, item.NilaiTagih)
                .Build());
            _tagihanWriter.Save(tagihan);
            LastIdLabel.Text = tagihan.TagihanId;
            ClearForm();

            var tagihanPrintout = new TagihanPrintOutDto(tagihan);
            PrintRdlc(tagihanPrintout);
        }

        private void ClearForm()
        {
            TagihanIdText.Clear();
            SalesCombo.SelectedIndex = -1;
            TglTagihText.Value = DateTime.Now;
            _listTagihan.Clear();
            _bindingSource.ResetBindings(false);
        }

        private void RefreshGrid()
        {
            var totalTagihan = _listTagihan.Sum(x => x.NilaiTagih);
            TotalTagihanLabel.Text = $"{totalTagihan:N0}";
            JumlahFakturLabel.Text = $"{_listTagihan.Count:N0}";
            FakturGrid.Refresh();
        }

        private bool ValidateTagihan()
        {
            var textbox = TagihanIdText;
            var policy = Policy<TagihanModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as TagihanModel, (r, c) =>
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var tagihan = policy.Execute(() => _tagihanBuilder
                .Load(new TagihanModel(textbox.Text))
                .Build());
            if (tagihan is null)
                return false;

            tagihan.RemoveNull();

            TglTagihText.Value = tagihan.TagihanDate;
            SalesCombo.SelectedValue = tagihan.SalesPersonId;
            TotalTagihanLabel.Text = $"{tagihan.TotalTagihan:N0}";

            _listTagihan.Clear();
            foreach (var item in tagihan.ListFaktur)
            {
                var newItem = new TagihanFakturDto();
                newItem.FakturId = item.FakturId;
                newItem.FakturCode = item.FakturCode;
                newItem.FakturDate = $"{item.FakturDate:dd-MMM-yyyy}";
                newItem.CustomerId = item.CustomerId;
                newItem.CustomerName = item.CustomerName;
                newItem.Alamat = item.Alamat;
                newItem.NilaiTerbayar = item.NilaiTerbayar;
                newItem.NilaiTotal = item.NilaiTotal;
                newItem.NilaiTagih = item.NilaiTagih;
                _listTagihan.Add(newItem);
            }

            return true;
        }

        private void PrintRdlc(TagihanPrintOutDto tagihan)
        {
            var tagihanJualDataset = new ReportDataSource("TagihanDataset", new List<TagihanPrintOutDto> { tagihan });
            var tagihanJualItemDataset = new ReportDataSource("TagihanItemDataset", tagihan.ListItem);
            var clientId = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ID"))?.ParamValue ?? string.Empty;

            var printOutTemplate = string.Empty;
            switch (clientId)
            {
                case "BTR-YK":
                    printOutTemplate = "TagihanPrintOut-Yk";
                    break;
                case "BTR-MGL":
                    printOutTemplate = "TagihanPrintOut-Mgl";
                    break;
                default:
                    break;
            }

            var listDataset = new List<ReportDataSource>
            {
                tagihanJualDataset,
                tagihanJualItemDataset
            };
            var rdlcViewerForm = new RdlcViewerForm();
            rdlcViewerForm.SetReportData(printOutTemplate, listDataset, true);
            rdlcViewerForm.ShowDialog();
        }

        private FakturModel GetFaktur(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var fakturHdr = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            return fakturHdr;
        }

        private PiutangModel GetPiutang(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var faktur = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            
            var piutangKey = new PiutangModel{PiutangId = faktur.FakturId};
            var piutang = _piutangDal.GetData(piutangKey) ?? throw new KeyNotFoundException("Piutang not found");
            //var piutang = _piutangBuilder.Load(piutangKey).Build();

            return piutang;
        }
    }
}
