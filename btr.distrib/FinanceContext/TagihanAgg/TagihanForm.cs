using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
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
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.ParamSistemAgg;
using btr.nuna.Domain;
using JetBrains.Annotations;
using Microsoft.Reporting.WinForms;
using Polly;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TagihanForm : Form
    {
        private readonly BindingSource _fakturBindingSource = new BindingSource();
        private readonly BindingSource _searchResultBindingSource = new BindingSource();

        private readonly SortableBindingList<TagihanFakturDto> _listTagihan = new SortableBindingList<TagihanFakturDto>();
        private readonly SortableBindingList<SearchResultDto> _listResult = new SortableBindingList<SearchResultDto>();
        
        private readonly IBrowser<TagihanBrowserView> _tagihanBrowser;

        private readonly IFakturDal _fakturDal;
        private readonly IParamSistemDal _paramSistemDal;
        private readonly IPiutangDal _piutangDal;
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
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
            ISalesRuteBuilder salesRuteBuilder,
            IPiutangBuilder piutangBuilder,
            IPiutangWriter piutangWriter)
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
            _salesRuteBuilder = salesRuteBuilder;
            _piutangBuilder = piutangBuilder;
            _piutangWriter = piutangWriter;

            InitTagihanGrid();
            InitSearchResultGrid();
            InitCombo();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            TagihanButton.Click += TagihanButton_Click;
            TagihanIdText.Validating += TagihanIdText_Validating;

            SalesCombo.SelectedValueChanged += SalesCombo_SelectedValueChanged;
            SearchButton.Click += SearchButton_Click;
            SearchButton.KeyDown += SearchButton_KeyDown;

            FakturGrid.CellValueChanged += FakturGrid_CellValidated;
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            FakturGrid.KeyDown += FakturGrid_KeyDown;

            SearchResultGrid.CellContentClick += SearchResultGrid_CellContentClick;
            SaveButton.Click += SaveButton_Click;
        }

        #region TAGIHAN-ID
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
            //TotalTagihanLabel.Text = $"{tagihan.TotalTagihan:N0}";

            _listTagihan.Clear();
            foreach (var item in tagihan.ListFaktur)
            {
                var newItem = new TagihanFakturDto
                {
                    FakturId = item.FakturId,
                    FakturCode = item.FakturCode,
                    FakturDate = item.FakturDate,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    Alamat = item.Alamat,
                    NilaiTerbayar = item.NilaiTerbayar,
                    NilaiTotal = item.NilaiTotal,
                    NilaiTagih = item.NilaiTagih,
                    IsTandaTerima = item.IsTandaTerima,
                    Keterangan = item.Keterangan,
                    TandaTerimaDate = item.TandaTerimaDate
                };
                _listTagihan.Add(newItem);
            }

            _listResult.Clear();
            _searchResultBindingSource.ResetBindings(true);
            SearchResultGrid.Refresh();

            return true;
        }
        private void ClearForm()
        {
            TagihanIdText.Clear();
            SalesCombo.SelectedIndex = -1;
            TglTagihText.Value = DateTime.Now;
            _listTagihan.Clear();
            _fakturBindingSource.ResetBindings(false);

            _listResult.Clear();
            _searchResultBindingSource.ResetBindings(true);
            SearchResultGrid.Refresh();
        }        
        #endregion

        #region COMBO-SALES
        private void InitCombo()
        {
            var listSales = _salesDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            SalesCombo.DataSource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesCombo.DisplayMember = "SalesPersonName";
            SalesCombo.ValueMember = "SalesPersonId";
        }
        private void SalesCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (SalesCombo.SelectedValue == null)
                return;
            var salesId = SalesCombo.SelectedValue.ToString();
            if (salesId == string.Empty)
                return;

        }
        #endregion

        #region BUTTON SEARCH
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchFaktur();
            RefreshGrid();
        }
        private void SearchButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void SearchFaktur()
        {
            if (SalesCombo.SelectedValue is null)
                return;
            var sales = new SalesPersonModel(SalesCombo.SelectedValue.ToString());
            var listAllPiutang = new List<PiutangModel>();
            var periode = new Periode(Tgl1DatePicker.Value, Tgl2DatePicker.Value);
            var listCustomer = ListCustomerByRute(sales, GetSelectedHari());
            if (listCustomer.Count() > 0)
                foreach (var item in listCustomer)
                {
                    var listPiutangThisCustomer = ListPiutangPerCustomer(item, periode);
                    if (listPiutangThisCustomer.Count == 0)
                        continue;
                    listPiutangThisCustomer.RemoveAll(x => x.StatusPiutang != StatusPiutangEnum.Tercatat);
                    listAllPiutang.AddRange(listPiutangThisCustomer);
                }
            else
            {
                listAllPiutang = ListPiutangPerSales(sales, periode);
                listAllPiutang.RemoveAll(x => x.StatusPiutang != StatusPiutangEnum.Tercatat);
            }
            
            if (SearchText.Text.Trim().Length > 0)
            {
                var search = SearchText.Text.Trim().ToLower();
                listAllPiutang.RemoveAll(x => !(x.FakturCode.ToLower().Contains(search)
                    || x.CustomerName.ToLower().Contains(search)
                    || x.CustomerCode.ToLower().Contains(search)
                    || x.Address.ToLower().Contains(search)));
            }

            _listResult.Clear();
            foreach (var item in listAllPiutang.OrderBy(x => x.CustomerCode).ThenBy(x => x.DueDate))
            {
                var newTagihanItem = new TagihanFakturDto
                {
                    FakturCode = item.FakturCode,
                    FakturDate = item.PiutangDate,
                    JatuhTempo = item.DueDate,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerCode = item.CustomerCode,
                    Alamat = item.Address,
                    NilaiTotal = item.Total,
                    NilaiTerbayar = item.Terbayar,
                    NilaiTagih = item.Sisa,
                    FakturId = item.PiutangId,
                    IsTandaTerima = false,
                    Keterangan = string.Empty,
                    TandaTerimaDate = new DateTime(3000, 1, 1)
                };
                _listResult.Add(new SearchResultDto(newTagihanItem));
            }
        }
        private IEnumerable<string> GetSelectedHari()
        {
            var listHari = new List<string>();
            if (Sen1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sen1CheckBox));
            if (Sel1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sel1CheckBox));
            if (Rab1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Rab1CheckBox));
            if (Kam1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Kam1CheckBox));
            if (Jum1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Jum1CheckBox));
            if (Sab1CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sab1CheckBox));

            if (Sen2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sen2CheckBox));
            if (Sel2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sel2CheckBox));
            if (Rab2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Rab2CheckBox));
            if (Kam2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Kam2CheckBox));
            if (Jum2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Jum2CheckBox));
            if (Sab2CheckBox.Checked) listHari.Add(GetHariFromCheckBox(Sab2CheckBox));

            if (listHari.Count == 0)
                listHari = new List<string> { "H11", "H12", "H13", "H14", "H15", "H16", "H21", "H22", "H23", "H24", "H25", "H26" };
            return listHari;
        }
        private string GetHariFromCheckBox(CheckBox checkBox)
        {
            switch (checkBox.Name)
            {
                case "Sen1CheckBox": return "H11";
                case "Sel1CheckBox": return "H12";
                case "Rab1CheckBox": return "H13";
                case "Kam1CheckBox": return "H14";
                case "Jum1CheckBox": return "H15";
                case "Sab1CheckBox": return "H16";

                case "Sen2CheckBox": return "H21";
                case "Sel2CheckBox": return "H22";
                case "Rab2CheckBox": return "H23";
                case "Kam2CheckBox": return "H24";
                case "Jum2CheckBox": return "H25";
                case "Sab2CheckBox": return "H26";

                default:
                    throw new NotImplementedException();
            }
        }
        private IEnumerable<ICustomerKey> ListCustomerByRute(SalesPersonModel sales, IEnumerable<string> listHari)
        {
            var listRute = new List<SalesRuteModel>();
            foreach (var hari in listHari)
            {
                var rute = _salesRuteBuilder.LoadOrCreate(sales, new HariRuteModel(hari)).Build();
                listRute.Add(rute);
            }

            var listCustomer = new List<ICustomerKey>();
            foreach (var rute in listRute)
                foreach (var customer in rute.ListCustomer)
                    if (!listCustomer.Any(x => x.CustomerId == customer.CustomerId))
                        listCustomer.Add(customer);

            return listCustomer;
        }
        private List<PiutangModel> ListPiutangPerSales(ISalesPersonKey sales, Periode periode)
        {
            var listPiutang = _piutangDal.ListData(sales, periode)?.ToList() ?? new List<PiutangModel>();
            listPiutang.RemoveAll(x => x.Sisa < 1);
            //listPiutang.RemoveAll(x => x.DueDate.Date > TglTagihText.Value.Date);
            return listPiutang;
        }
        private List<PiutangModel> ListPiutangPerCustomer(ICustomerKey customerKey, Periode periode)
        {
            var listPiutang = _piutangDal.ListData(customerKey, periode)?.ToList() ?? new List<PiutangModel>();
            listPiutang.RemoveAll(x => x.Sisa < 1);
            //listPiutang.RemoveAll(x => x.DueDate.Date > TglTagihText.Value.Date);
            return listPiutang;
        }
        #endregion

        #region TAGIHAN-GRID
        private void InitTagihanGrid()
        {
            _fakturBindingSource.DataSource = _listTagihan;
            FakturGrid.DataSource = _fakturBindingSource;
            FakturGrid.Columns.SetDefaultCellStyle(Color.LemonChiffon);
            FakturGrid.Refresh();

            var col = FakturGrid.Columns;
            col.GetCol("FakturCode").HeaderText = @"Faktur";
            col.GetCol("FakturCode").DefaultCellStyle.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            col.GetCol("FakturDate").HeaderText = @"Tgl";
            col.GetCol("CustomerCode").HeaderText = @"CustCode";
            col.GetCol("CustomerName").HeaderText = @"Customer";
            col.GetCol("Alamat").HeaderText = @"Alamat";
            col.GetCol("NilaiTotal").HeaderText = @"Total";
            col.GetCol("NilaiTerbayar").HeaderText = @"Terbayar";
            col.GetCol("NilaiTagih").HeaderText = @"Tagihanr";

            col.GetCol("FakturId").Visible = false;
            col.GetCol("CustomerId").Visible = false;
            col.GetCol("JatuhTempo").Visible = false;
            col.GetCol("CustomerCode").Visible = false;

            col.GetCol("FakturDate").DefaultCellStyle.Format = "dd-MMM";
            col.GetCol("NilaiTotal").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTotal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("NilaiTerbayar").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTerbayar").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("NilaiTagih").DefaultCellStyle.Format = "N0";
            col.GetCol("NilaiTagih").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            col.GetCol("FakturCode").Width = 60;
            col.GetCol("FakturDate").Width = 50;
            col.GetCol("JatuhTempo").Width = 80;
            col.GetCol("CustomerCode").Width = 70;
            col.GetCol("CustomerName").Width = 150;
            col.GetCol("CustomerName").DefaultCellStyle.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular);

            col.GetCol("Alamat").Width = 180;
            col.GetCol("Alamat").DefaultCellStyle.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular);
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

            col.GetCol("IsTandaTerima").Visible = false;
            col.GetCol("TandaTerimaDate").Visible = false;
            col.GetCol("Keterangan").Visible = false;

            foreach (DataGridViewColumn columns in FakturGrid.Columns)
            {
                columns.SortMode = DataGridViewColumnSortMode.Automatic;
            }
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
                _listTagihan[e.RowIndex].FakturDate = new DateTime(3000,1,1);
                _listTagihan[e.RowIndex].CustomerId = "";
                _listTagihan[e.RowIndex].CustomerName = "";
                _listTagihan[e.RowIndex].Alamat = "";
                _listTagihan[e.RowIndex].NilaiTotal = 0;
                _listTagihan[e.RowIndex].NilaiTerbayar = 0;
                _listTagihan[e.RowIndex].NilaiTagih = 0;
                _listTagihan[e.RowIndex].IsTandaTerima = false;
                _listTagihan[e.RowIndex].Keterangan = "";
                _listTagihan[e.RowIndex].TandaTerimaDate = new DateTime(3000, 1, 1);
                return;
            }

            _listTagihan[e.RowIndex].FakturId = faktur.FakturId;
            _listTagihan[e.RowIndex].FakturDate = faktur.FakturDate;
            _listTagihan[e.RowIndex].JatuhTempo = faktur.DueDate;
            _listTagihan[e.RowIndex].CustomerId = faktur.CustomerId;
            _listTagihan[e.RowIndex].CustomerCode = faktur.CustomerCode;
            _listTagihan[e.RowIndex].CustomerName = faktur.CustomerName;
            _listTagihan[e.RowIndex].Alamat = faktur.Address;
            _listTagihan[e.RowIndex].NilaiTotal = piutang.Total + piutang.Potongan;
            _listTagihan[e.RowIndex].NilaiTerbayar = piutang.Terbayar;
            _listTagihan[e.RowIndex].NilaiTagih = piutang.Sisa;
            _listTagihan[e.RowIndex].IsTandaTerima = false;
            _listTagihan[e.RowIndex].Keterangan = "";
            _listTagihan[e.RowIndex].TandaTerimaDate = new DateTime(3000, 1, 1);

            RefreshGrid();
        }
        private FakturModel GetFaktur(string fakturCode)
        {
            var fakturCodeKey = new FakturModel { FakturCode = fakturCode };
            var fakturHdr = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            return fakturHdr;
        }
        private PiutangModel GetPiutang(string fakturCode)
        {
            var fakturCodeKey = new FakturModel { FakturCode = fakturCode };
            var faktur = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");

            var piutangKey = new PiutangModel { PiutangId = faktur.FakturId };
            var piutang = _piutangDal.GetData(piutangKey) ?? throw new KeyNotFoundException("Piutang not found");
            //var piutang = _piutangBuilder.Load(piutangKey).Build();

            return piutang;
        }
        #endregion

        #region SEARCH-GRID
        private void InitSearchResultGrid()
        {
            _searchResultBindingSource.DataSource = _listResult;
            SearchResultGrid.DataSource = _searchResultBindingSource;
            SearchResultGrid.Refresh(); 
            
            var grid = SearchResultGrid.Columns;
            grid["FakturId"].Visible = false;
            grid["FakturDate"].Visible = false;
            grid["FakturCode"].Visible = false;
            grid["CustomerId"].Visible = false;
            grid["CustomerName"].Visible = false;
            grid["Alamat"].Visible = false;

            grid["Faktur"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid["Faktur"].Width = 60;

            grid["Customer"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid["Customer"].Width = 170;

            grid["IsSelected"].HeaderText = "Pilih";
            grid["NilaiFaktur"].HeaderText = "Nilai";
            grid["NilaiFaktur"].DefaultCellStyle.Format = "N0";
            grid["NilaiFaktur"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["NilaiFaktur"].Width = 70;
            grid["IsSelected"].Width = 40;

            SearchResultGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            SearchResultGrid.AutoResizeRows();
            SearchResultGrid.AllowUserToResizeRows = true;
            SearchResultGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            SearchResultGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;

            foreach (DataGridViewColumn columns in FakturGrid.Columns)
                columns.SortMode = DataGridViewColumnSortMode.Automatic;
        }
        private void SearchResultGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != SearchResultGrid.Columns.GetCol("IsSelected").Index) return;
            SearchResultGrid.EndEdit();

            var item = _listResult[e.RowIndex];
            if (item.IsSelected)
            {
                if (_listTagihan.Where(x => x.FakturCode == item.FakturCode).Any())
                {
                    MessageBox.Show("Faktur sudah diinputkan");
                    item.IsSelected = false;
                    SearchResultGrid.Refresh();
                    return;
                }
                var newTagihanItem = new TagihanFakturDto
                {
                    FakturCode = item.FakturCode,
                    FakturDate = item.FakturDate,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    Alamat = item.Alamat,
                    NilaiTotal = item.NilaiFaktur,
                    NilaiTerbayar = 0,
                    NilaiTagih = item.NilaiFaktur,
                    FakturId = item.FakturId,
                    IsTandaTerima = false,
                    Keterangan = string.Empty,
                    TandaTerimaDate = new DateTime(3000, 1, 1)
                };
                _listTagihan.Add(newTagihanItem);
            }
            else
            {
                var idx = _listTagihan.IndexOf(_listTagihan.First(x => x.FakturCode == item.FakturCode));
                if (idx >= 0)
                {
                    _listTagihan.RemoveAt(idx);
                }
            }
            RefreshGrid();
        }
        #endregion

        #region SAVE-BUTTON
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
                .AddFaktur(new FakturModel(item.FakturId), item.NilaiTotal, item.NilaiTerbayar, item.NilaiTagih,
                    item.IsTandaTerima, item.Keterangan, item.TandaTerimaDate)
                .Build());

            var listPiutang = new List<PiutangModel>();
            foreach (var faktur in tagihan.ListFaktur)
            {
                var piutangKey = new PiutangModel { PiutangId = faktur.FakturId };
                var piutang = _piutangBuilder.Load(piutangKey).Build();
                piutang.StatusPiutang = StatusPiutangEnum.Ditagihkan;
                listPiutang.Add(piutang);
            }

            _tagihanWriter.Save(tagihan);
            foreach (var piutang in listPiutang)
            {
                var piutangX = piutang;
                _piutangWriter.Save(ref piutangX);
            }

            LastIdLabel.Text = tagihan.TagihanId;
            ClearForm();

            var tagihanPrintout = new TagihanPrintOutDto(tagihan);
            PrintRdlc(tagihanPrintout);
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
        #endregion

        private void RefreshGrid()
        {
            var totalTagihan = _listTagihan.Sum(x => x.NilaiTagih);
            FakturGrid.Refresh();
            SearchResultGrid.Refresh();
        }

        private class SearchResultDto
        {
            public SearchResultDto(TagihanFakturDto faktur)
            {
                FakturId = faktur.FakturId;
                FakturDate = faktur.FakturDate;
                FakturCode = faktur.FakturCode;
                CustomerId = faktur.CustomerId;
                CustomerName = faktur.CustomerName;
                Alamat = faktur.Alamat;
                NilaiFaktur = faktur.NilaiTotal;
            }
            public string FakturId { get; private set; }
            public string FakturCode { get; private set; }
            public DateTime FakturDate { get; private set; }
            public string CustomerId { get; private set; }
            public string CustomerName { get; private set; }
            public string Alamat { get; private set; }
            public string Faktur => $"{FakturCode}\n  {FakturDate:dd-MM}";
            public string Customer => $"{CustomerName}\n{Alamat}";
            public decimal NilaiFaktur { get; private set; }
            public bool IsSelected { get; set; }
        }
    }
}
