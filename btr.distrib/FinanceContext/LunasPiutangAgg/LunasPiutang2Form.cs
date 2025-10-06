using btr.application.FinanceContext.FakturPotBalanceAgg;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.FinanceContext.TagihanAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.SalesRuteAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.FinanceContext.TagihanAgg;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using Polly;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    public partial class LunasPiutang2Form : Form
    {
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
        private readonly IPiutangLunasViewDal _piutangLunasViewDal;
        private readonly ITagihanFakturDal _tagihanFakturDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturPotBalanceBuilder _fakturPotBalanceBuilder;
        private readonly IFakturPotBalanceWriter _fakturPotBalanceWriter;
        private readonly IFakturDal _fakturDal;
        private readonly ITglJamDal _dateTime;
        private readonly ISalesRuteBuilder _salesRuteBuilder;
        private readonly ISalesPersonDal _salesPersonDal;

        private readonly BindingList<LunasPiutangTagihanViewDto> _listTagihan;
        private readonly BindingSource _tagihanBindingSource;
        private BindingList<LunasPiutangBayarView> _listLunasBayar;

        private string _piutangId = string.Empty;

        public LunasPiutang2Form(IPiutangLunasViewDal piutangLunasViewDal,
            IPiutangBuilder piutangBuilder,
            IFakturBuilder fakturBuilder,
            IFakturDal fakturDal,
            IPiutangWriter piutangWriter,
            ITagihanFakturDal tagihanFakturDal,
            IFakturPotBalanceBuilder fakturPotBalanceBuilder,
            IFakturPotBalanceWriter fakturPotBalanceWriter,
            ITglJamDal dateTime,
            ISalesRuteBuilder salesRuteBuilder,
            ISalesPersonDal salesPersonDal)
        {
            InitializeComponent();

            _piutangLunasViewDal = piutangLunasViewDal;
            _piutangBuilder = piutangBuilder;
            _fakturBuilder = fakturBuilder;
            _fakturDal = fakturDal;
            _piutangWriter = piutangWriter;
            _tagihanFakturDal = tagihanFakturDal;
            _fakturPotBalanceBuilder = fakturPotBalanceBuilder;
            _fakturPotBalanceWriter = fakturPotBalanceWriter;
            _salesRuteBuilder = salesRuteBuilder;
            _dateTime = dateTime;
            _salesPersonDal = salesPersonDal;

            _listTagihan = new BindingList<LunasPiutangTagihanViewDto>();
            _tagihanBindingSource = new BindingSource(_listTagihan, null);

            InitNumericUpDown();
            IniGridBayar();
            InitTagihanGrid();
            InitJenisBayarCombo();
            InitSalesCombo();
            RegisterEventHandler();
        }

        private void InitNumericUpDown()
        {
            ReturText.ResetText();
            PotonganText.ResetText();
            MateraiText.ResetText();
            AdminText.ResetText();
            NilaiPelunasanText.ResetText();
        }
        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            UpdateElementButton.Click += UpdateElementButton_Click;
            JenisBayarCombo.SelectedIndexChanged += JenisBayarCombo_SelectedIndexChanged;
            NilaiPelunasanText.KeyDown += NilaiPelunasanText_KeyDown;
            FakturCodeText.Validating += FakturCodeText_Validating;
            FakturCodeText.KeyDown += FakturCodeText_KeyDown;
            TagihanCombo.SelectedIndexChanged += TagihanCombo_SelectedIndexChanged;
            BayarGrid.CellFormatting += BayarGrid_CellFormatting;

            TagihanGrid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = TagihanGrid.Rows[e.RowIndex];
                var item = (LunasPiutangTagihanViewDto)row.DataBoundItem;
                LoadPiutang(item.FakturId);
            };
        }

        #region PEMBAYARAN-HEADER
        private void LoadTagihan(string tagihanId)
        {
            ClearTagihan();
            var piutang = _piutangBuilder
                .Load(new PiutangModel(_piutangId))
                .Build();
            var tagihan = piutang.ListLunas.FirstOrDefault(x => x.TagihanId == tagihanId);

            if (tagihan == null)
                return;

            NilaiPelunasanText.Value = tagihan.Nilai;
            JenisBayarCombo.SelectedIndex = (int)tagihan.JenisLunas;
            LunasDateText.Value = tagihan.LunasDate;
            NoRekBgText.Text = tagihan.NoRekBg;
            BankNameText.Text = tagihan.NamaBank;
            AtasNamaText.Text = tagihan.AtasNamaBank;
            JatuhTempBgText.Value = tagihan.JatuhTempoBg;
        }
        private void ClearTagihan()
        {
            NilaiPelunasanText.Value = 0;
            JenisBayarCombo.SelectedIndex = 0;
            LunasDateText.Value = DateTime.Now;
            NoRekBgText.Clear(); 
            BankNameText.Clear();
            AtasNamaText.Clear();
            JatuhTempBgText.Value = DateTime.Now;
        }
        private void FakturCodeText_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.KeyCode == Keys.Enter) 
                //  set focus to next control
                SelectNextControl(textBox, true, true, true, true);
        }
        private void FakturCodeText_Validating(object sender, CancelEventArgs e)
        {
            IFakturCode fakturCode = new FakturModel { FakturCode = FakturCodeText.Text };
            var fakturKey = _fakturDal.GetData(fakturCode) ?? new FakturModel(string.Empty);    
            var fallback = Policy
                .Handle<KeyNotFoundException>()
                .Fallback(() =>
                {
                    if (FakturCodeText.Text == string.Empty)
                    {
                        ClearInputForm();                        
                        return;
                    }
                    MessageBox.Show(@"Faktur not found");
                    e.Cancel = true;
                });
            fallback.Execute(() => LoadPiutang(fakturKey.FakturId));
        }
        private void ClearInputForm()
        {
            CustomerText.Text = string.Empty;
            AddressText.Text = string.Empty;

            FakturCodeText.Text = string.Empty;
            TglFakturText2.Text = DateTime.Now.ToString("dd-MM-yyyy");
            JatuhTempoText2.Text = DateTime.Now.ToString("dd-MM-yyyy");
            SalesText.Text = string.Empty;
            LunasDateText.Value = DateTime.Now;
            TagihanCombo.DataSource = new List<TagihanView>
            {
                new TagihanView("-", "---tidak ada tagihan---")
            };

            ReturText.Value = 0;
            PotonganText.Value = 0;
            MateraiText.Value = 0;
            AdminText.Value = 0;
            NilaiPelunasanText.Value = 0;

            ReturText.ResetText();
            PotonganText.ResetText();
            MateraiText.ResetText();
            AdminText.ResetText();
            NilaiPelunasanText.ResetText();


            JenisBayarCombo.SelectedIndex = 0;
            JatuhTempBgText.Value = DateTime.Now;
            NoRekBgText.Text = string.Empty;
            BankNameText.Text = string.Empty;
            AtasNamaText.Text = string.Empty;
            _listLunasBayar = new BindingList<LunasPiutangBayarView>();
            BayarGrid.DataSource = _listLunasBayar;
            BayarGrid.Refresh();
        }
        private void LoadPiutang(string piutangId)
        {
            ClearInputForm();
            var piutang = _piutangBuilder.Load(new PiutangModel(piutangId)).Build();
            var faktur = _fakturBuilder.Load(new FakturModel(piutang.PiutangId)).Build();
            var fc = faktur.FakturCode;
            _piutangId = piutang.PiutangId;
            CustomerText.Text = piutang.CustomerName;
            AddressText.Text = faktur.Address;

            ReturText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Retur)?.NilaiMinus ?? 0;
            PotonganText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Potongan)?.NilaiMinus ?? 0;
            MateraiText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Materai)?.NilaiMinus ?? 0;
            AdminText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Admin)?.NilaiMinus ?? 0;

            FakturCodeText.Text = fc;
            TglFakturText2.Text = piutang.PiutangDate.ToString("dd-MM-yyyy");
            SalesText.Text = faktur.SalesPersonName;
            LunasDateText.Value = DateTime.Now;

            SetTagihanComboDataSource(faktur);

            RefreshGridBayar(piutang);
        }

        #endregion

        #region PERMBAYARAN-ELEMENT
        private void NilaiPelunasanText_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as NumericUpDown;
            if (e.KeyCode == Keys.F1)
            {
                var currentTagihan = TagihanCombo.SelectedValue.ToString();
                textBox.Focus();
                textBox.Select(0, 0);
                var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
                var nilaiPelunasanSaatIni = piutang.ListLunas.FirstOrDefault(x => x.TagihanId == currentTagihan)?.Nilai??0;
                textBox.Value = piutang.Sisa + nilaiPelunasanSaatIni; 
            }
        }
        private void DeleteElementButton_Click(object sender, EventArgs e)
        {
            var piutang = _piutangBuilder
                .Load(new PiutangModel(_piutangId))
                .Build();
            piutang.ListElement.Clear();
            piutang = _piutangBuilder
                .Attach(piutang)
                .AddMinusElement(PiutangElementEnum.Retur, 0, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Potongan, 0, _dateTime.Now)
                .AddPlusElement(PiutangElementEnum.Materai, 0, _dateTime.Now)
                .AddPlusElement(PiutangElementEnum.Admin, 0, _dateTime.Now)
                .Build();
            piutang.ListElement.RemoveAll(x => x.NilaiPlus + x.NilaiMinus == 0);
            _piutangWriter.Save(ref piutang);
            RefreshGridBayar(piutang);
        }
        private void UpdateElementButton_Click(object sender, EventArgs e)
        {
            //  piutang
            var piutang = _piutangBuilder
                .Load(new PiutangModel(_piutangId))
                .Build();
            piutang.ListElement.Clear();
            piutang = _piutangBuilder
                .Attach(piutang)
                .AddMinusElement(PiutangElementEnum.Retur, ReturText.Value, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Potongan, PotonganText.Value, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Materai, MateraiText.Value, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Admin, AdminText.Value, _dateTime.Now)
                .Build();
            _piutangWriter.Save(ref piutang);
            RefreshGridBayar(piutang);

            //  faktur-pot-balance
            var nilaiRetur = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Retur)
                ?? new PiutangElementModel { NilaiMinus = 0 };
            var faktur = _fakturBuilder.Load(new FakturModel(piutang.PiutangId)).Build();
            var fakturPotBalance = _fakturPotBalanceBuilder
                .LoadOrCreate(faktur).Build();
            fakturPotBalance.NilaiPotong = nilaiRetur.NilaiMinus;
            _ = _fakturPotBalanceWriter.Save(fakturPotBalance);

        }
        #endregion

        #region PEMBAYARAN-BAYAR
        private void JenisBayarCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selected = comboBox?.SelectedItem.ToString();
            if (selected == "Tunai")
            {
                JatuhTempBgText.Enabled = false;
                NoRekBgText.ReadOnly = true;
                BankNameText.ReadOnly = true;
                AtasNamaText.ReadOnly = true;
            }
            else
            {
                JatuhTempBgText.Enabled = true;
                NoRekBgText.ReadOnly = false;
                BankNameText.ReadOnly = false;
                AtasNamaText.ReadOnly = false;
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SavePembayaran();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeletePembayaran();
        }
        private void TagihanCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TagihanCombo.DataSource == null)
                return;

            var tagihanId = TagihanCombo.SelectedValue.ToString();
            if (tagihanId == "-")
                return;
            LoadTagihan(tagihanId);
        }
        private void InitJenisBayarCombo()
        {
            var listJenisBayar = new List<string>
            {
                "Tunai",
                "BG / Transfer"
            };
            JenisBayarCombo.DataSource = listJenisBayar;
        }
        private void SavePembayaran()
        {
            var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
            var tagihanId = TagihanCombo.SelectedValue.ToString();
            piutang.ListLunas.RemoveAll(x => x.TagihanId == tagihanId);

            if (JenisBayarCombo.SelectedIndex == 0)
                piutang = _piutangBuilder
                    .Attach(piutang)
                    .AddLunasCash(NilaiPelunasanText.Value, LunasDateText.Value, tagihanId)
                    .Build();
            else
                piutang = _piutangBuilder
                    .Attach(piutang)
                    .AddLunasBg(NilaiPelunasanText.Value, LunasDateText.Value, tagihanId,
                                JatuhTempBgText.Value, BankNameText.Text, NoRekBgText.Text, AtasNamaText.Text)
                    .Build();

            piutang.ListLunas.RemoveAll(x => x.Nilai == 0);
            piutang.ListLunas = piutang.ListLunas.OrderBy(x => x.TagihanId)?.ToList();
            int i = 1;
            foreach (var item in piutang.ListLunas)
            {
                item.NoUrut = i;
                i++;
            }

            if (piutang.Sisa <= 1)
                piutang.StatusPiutang = StatusPiutangEnum.Lunas;
            else
                piutang.StatusPiutang = StatusPiutangEnum.Tercatat; // agar bisa ditagihkan lagi

            _piutangWriter.Save(ref piutang);
            RefreshGridBayar(piutang);
        }
        private void DeletePembayaran()
        {
            var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
            var tagihanId = TagihanCombo.SelectedValue.ToString();
            piutang = _piutangBuilder
                .Attach(piutang)
                .RemoveLunas(tagihanId)
                .Build();
            _piutangWriter.Save(ref piutang);
            RefreshGridBayar(piutang);
            LoadTagihan(tagihanId);
        }
        private void SetTagihanComboDataSource(IFakturKey faktur)
        {
            SaveButton.Enabled = true;
            DeleteButton.Enabled = true;

            var listTagihan = _tagihanFakturDal.ListData(faktur)?.ToList();
            if (listTagihan is null)
            {
                TagihanCombo.DataSource = null;
                TagihanCombo.Text = "--[tidak ada tagihan]--";
                SaveButton.Enabled = false;
                DeleteButton.Enabled = false;
                return;
            }

            var datasource = listTagihan.Select((x, y) => new TagihanView(
                x.TagihanId, $"[{y + 1:D0}] {x.TagihanDate:dd-MMM} {x.SalesPersonName}"))
                .ToList();

            TagihanCombo.DataSource = datasource;
            TagihanCombo.DisplayMember = "TagihanDisplay";
            TagihanCombo.ValueMember = "TagihanId";
            LoadTagihan(datasource.First().TagihanId);
        }
        #endregion

        #region PEMBAYARAN-GRID
        private void IniGridBayar()
        {
            BayarGrid.DataSource = _listLunasBayar;
            BayarGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.LavenderBlush,
                Font = new System.Drawing.Font("Consolas", 8.25F),
            };

            var cols = BayarGrid.Columns;
            RefreshGridBayar(new PiutangModel { ListLunas = new List<PiutangLunasModel>()});
            cols.GetCol("Tgl").DefaultCellStyle.Format = "ddd, dd-MMM-yyyy";
            cols.GetCol("Nilai").DefaultCellStyle.Format = "N0";
            cols.GetCol("Nilai").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            cols.GetCol("JenisData").Visible = false;

            //  hide row header
            BayarGrid.RowHeadersVisible = false;
            //  readonly
            BayarGrid.ReadOnly = true;
            BayarGrid.AllowUserToAddRows = false;
            BayarGrid.AllowUserToDeleteRows = false;
        }
        private void RefreshGridBayar(PiutangModel piutang)
        {
            piutang.RemoveNull();
            int noUrut = 1;
            _listLunasBayar = new BindingList<LunasPiutangBayarView>
            {
                new LunasPiutangBayarView(
                    noUrut,
                    piutang.PiutangDate,
                    "Tagihan",
                    piutang.Total,
                    1)
            };

            foreach (var item in piutang.ListElement ?? new List<PiutangElementModel>())
            {
                var nilaiElement = item.NilaiPlus - item.NilaiMinus;
                if (nilaiElement == 0)
                    continue;
                noUrut++;
                LunasPiutangBayarView newElement;
                if (nilaiElement < 0)
                    newElement = new LunasPiutangBayarView
                        (
                            noUrut,
                            piutang.PiutangDate,
                            $"   {item.ElementName}",
                            nilaiElement,
                            2
                        );
                else
                    newElement = new LunasPiutangBayarView
                        (
                            noUrut,
                            piutang.PiutangDate,
                            $"{item.ElementName}",
                            nilaiElement,
                            2
                        );
                _listLunasBayar.Add(newElement);
            }

            foreach (var pelunasan in piutang.ListLunas)
            {
                noUrut++;
                _listLunasBayar.Add(new LunasPiutangBayarView(
                    noUrut,
                    pelunasan.LunasDate,
                    $"   Pembayaran {pelunasan.JenisLunas}",
                    -pelunasan.Nilai, 3));
            }

            noUrut++;
            _listLunasBayar.Add(new LunasPiutangBayarView(noUrut, DateTime.Now, "Sisa Tagihan", piutang.Sisa, 1));

            BayarGrid.DataSource = _listLunasBayar;
            var cols = BayarGrid.Columns;
            cols.GetCol("No").Width = 30;
            cols.GetCol("Tgl").Width = 70;
            cols.GetCol("Keterangan").Width = 150;
            cols.GetCol("Nilai").Width = 80;
        }
        private void BayarGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var conditionValue = BayarGrid.Rows[e.RowIndex].Cells["JenisData"].Value?.ToString(); // Replace with your column name

            if (!string.IsNullOrEmpty(conditionValue))
            {
                if (conditionValue == "1") // Replace with your condition
                {
                    BayarGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
                else if (conditionValue == "2") // Another condition
                {
                    BayarGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    BayarGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
        }
        #endregion

        #region TAGIHAN-GRID
        private void InitTagihanGrid()
        {
            TagihanGrid.DataSource = _tagihanBindingSource;
            TagihanGrid.Columns.SetDefaultCellStyle(Color.Azure);
            
            var grid = TagihanGrid.Columns;
            grid["TagihanId"].Visible = false;
            grid["FakturId"].Visible = false;

            grid["FakturDate"].HeaderText = "Tgl";
            grid["FakturDate"].Width = 80;
            grid["FakturDate"].DefaultCellStyle.Format = "dd-MM-yyyy";

            grid["FakturCode"].HeaderText = "Faktur";
            grid["FakturCode"].Width = 70;
            
            grid["SalesName"].Visible = false;
            grid["CustomerName"].Visible = false;
            grid["Alamat"].Visible = false;

            grid["Customer"].HeaderText = "Customer";
            grid["Customer"].Width = 150;
            grid["Customer"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            grid["Customer"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            grid["NilaiTotal"].HeaderText = "Total";
            grid["NilaiTotal"].Width = 80;
            grid["NilaiTerbayar"].HeaderText = "Terbayar";
            grid["NilaiTerbayar"].Width = 80;
            grid["NilaiTagih"].HeaderText = "Tagih";
            grid["NilaiTagih"].Width = 80;
            grid["NilaiPelunasan"].HeaderText = "Pelunasan";
            grid["NilaiPelunasan"].Width = 80;

            grid["IsTandaTerima"].Visible= false;
            grid["Keterangan"].Width = 120;
            grid["Keterangan"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            foreach (DataGridViewColumn col in TagihanGrid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            TagihanGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            TagihanGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            
            TagihanGrid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = TagihanGrid.Rows[e.RowIndex];
                var item = (LunasPiutangTagihanViewDto)row.DataBoundItem;
                if (!item.IsTandaTerima)
                    row.DefaultCellStyle.ForeColor = Color.Red;
                else
                    row.DefaultCellStyle.ForeColor = Color.Black;

                if (item.NilaiPelunasan > 0)
                    //  set backcolor to B4DEBD
                    row.DefaultCellStyle.BackColor = Color.FromArgb(180, 222, 189);
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            };
        }
        #endregion

        #region SEARCH-BUTTON
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchTagihan();
        }
        private void SearchTagihan()
        {
            if (SalesCombo.SelectedValue is null)
                return;
            var sales = new SalesPersonModel(SalesCombo.SelectedValue.ToString());
            var listAllTagihan = new List<TandaTerimaTagihanViewDto>();
            var periode = new Periode(Tgl1DatePicker.Value, Tgl2DatePicker.Value);
            var listCustomer = ListCustomerByRute(sales, GetSelectedHari());
            if (listCustomer.Count() > 0)
                foreach (var item in listCustomer)
                {
                    var listTagihanThisCustomer = ListTagihanPerCustomer(item, periode);
                    if (listTagihanThisCustomer.Count == 0)
                        continue;
                    if (!IgnoreTTCheckBox.Checked)
                        listTagihanThisCustomer.RemoveAll(x => x.IsTandaTerima == false);

                    listTagihanThisCustomer.RemoveAll(x => x.IsTagihUlang);
                    listAllTagihan.AddRange(listTagihanThisCustomer);
                }
            else
            {
                listAllTagihan = ListTagihanPerSales(sales, periode);
                if (!IgnoreTTCheckBox.Checked)
                    listAllTagihan.RemoveAll(x => x.IsTandaTerima == false);
                listAllTagihan.RemoveAll(x => x.IsTagihUlang);
            }

            if (SearchText.Text.Trim().Length > 0)
            {
                var search = SearchText.Text.Trim().ToLower();
                listAllTagihan.RemoveAll(x => !(x.FakturCode.ToLower().Contains(search)
                    || x.CustomerName.ToLower().Contains(search)
                    || x.Alamat.ToLower().Contains(search)));
            }

            _listTagihan.Clear();
            foreach (var item in listAllTagihan.OrderBy(x => x.CustomerName).ThenBy(x => x.FakturCode))
            {
                _listTagihan.Add(new LunasPiutangTagihanViewDto(item));
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
        private List<TandaTerimaTagihanViewDto> ListTagihanPerCustomer(ICustomerKey customer, Periode periode)
        {
            var listTagihan = _tagihanFakturDal.ListData(periode)?.ToList() ?? new List<TandaTerimaTagihanViewDto>();
            var result = listTagihan
                .Where(x => x.CustomerId == customer.CustomerId)
                .ToList();
            return result;
        }
        private List<TandaTerimaTagihanViewDto> ListTagihanPerSales(ISalesPersonKey sales, Periode periode)
        {
            var listTagihan = _tagihanFakturDal.ListData(periode)?.ToList() ?? new List<TandaTerimaTagihanViewDto>();
            var result = listTagihan
                .Where(x => x.SalesPersonId == sales.SalesPersonId)
                .ToList();
            return result;
        }
        #endregion

        #region SALES-COMBO
        private void InitSalesCombo()
        {
            var listSales = _salesPersonDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            SalesCombo.DataSource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesCombo.DisplayMember = "SalesPersonName";
            SalesCombo.ValueMember = "SalesPersonId";
        }
        #endregion


    }

    internal class LunasPiutangTagihanViewDto
    {
        public LunasPiutangTagihanViewDto(TandaTerimaTagihanViewDto tandaTerima)
        {
            TagihanId = tandaTerima.TagihanId;
            FakturId = tandaTerima.FakturId;
            FakturDate = tandaTerima.FakturDate;
            FakturCode = tandaTerima.FakturCode;
            
            SalesName = tandaTerima.SalesPersonName;
            CustomerName = tandaTerima.CustomerName;
            Alamat = tandaTerima.Alamat;
            
            NilaiTotal = tandaTerima.NilaiTotal;
            NilaiTerbayar = tandaTerima.NilaiTerbayar;
            NilaiTagih = tandaTerima.NilaiTagih;
            NilaiPelunasan = tandaTerima.NilaiPelunasan;

            Keterangan = tandaTerima.Keterangan;
            IsTandaTerima = tandaTerima.IsTandaTerima;
        }
        public string TagihanId { get; set; }
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string FakturCode { get; set; }
        public string SalesName { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public string Customer => $"{CustomerName}\n{Alamat}";
        public decimal NilaiTotal { get; set; }
        public decimal NilaiTerbayar { get; set; }
        public decimal NilaiTagih { get; set; }
        public decimal NilaiPelunasan { get; set; }
        public string Keterangan { get; set; }
        public bool IsTandaTerima { get; set; }
    }

    public class LunasPiutangBayarView
    {

        public LunasPiutangBayarView(int no, DateTime tgl, string keterangan, decimal nilai, int jenisData)
        {
            No = no;
            Tgl = tgl.ToString("dd-MM-yyyy");
            Keterangan = keterangan;
            Nilai = nilai;
            JenisData = jenisData;
        }
        public int No { get; set; }
        public string Tgl { get; set; }
        public string Keterangan { get; set; }
        public decimal Nilai { get; set; }
        public int JenisData { get; set; }
    }

    public class TagihanView
    {
        public TagihanView(string tagihanId, string tagihanDisplay)
        {
            TagihanId = tagihanId;
            TagihanDisplay = tagihanDisplay;
        }

        public string TagihanId { get; set; }

        public string TagihanDisplay { get; set; }
    }
}
