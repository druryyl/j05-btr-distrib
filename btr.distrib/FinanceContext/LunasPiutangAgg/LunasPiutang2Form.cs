using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.FakturAgg.Contracts;
using Polly;
using btr.application.FinanceContext.TagihanAgg;
using btr.application.FinanceContext.FakturPotBalanceAgg;

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
        
        private BindingList<PiutangLunasView> _listPiutangLunasView;
        private BindingList<LunasPiutangBayarView> _listLunasPiutangBayar;
        private BindingSource _bindingSource;
        private string _piutangId = string.Empty;

        public LunasPiutang2Form(IPiutangLunasViewDal piutangLunasViewDal,
            IPiutangBuilder piutangBuilder,
            IFakturBuilder fakturBuilder,
            IFakturDal fakturDal,
            IPiutangWriter piutangWriter,
            ITagihanFakturDal tagihanFakturDal,
            IFakturPotBalanceBuilder fakturPotBalanceBuilder,
            IFakturPotBalanceWriter fakturPotBalanceWriter)
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

            InitGrid();
            InitNumericUpDown();
            IniGridBayar();
            InitJenisBayarCombo();
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

        private void InitJenisBayarCombo()
        {
            var listJenisBayar = new List<string>
            {
                "Tunai",
                "BG / Transfer"
            };
            JenisBayarCombo.DataSource = listJenisBayar;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            ListPiutangGrid.QueryCellStyleInfo += ListPiutangGrid_QueryCellStyleInfo;
            ListPiutangGrid.TableControlCellDoubleClick += ListPiutangGrid_TableControlCellDoubleClick;
            ListPiutangGrid.TableControlCurrentCellActivated += ListPiutangGrid_TableControlCurrentCellActivated;
            ListPiutangGrid.TableControlCurrentCellValidated += ListPiutangGrid_TableControlCurrentCellValidated;
            
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            UpdateElementButton.Click += UpdateElementButton_Click;
            JenisBayarCombo.SelectedIndexChanged += JenisBayarCombo_SelectedIndexChanged;
            NilaiPelunasanText.KeyDown += NilaiPelunasanText_KeyDown;
            FakturCodeText.Validating += FakturCodeText_Validating;
            FakturCodeText.KeyDown += FakturCodeText_KeyDown;
            TagihanCombo.SelectedIndexChanged += TagihanCombo_SelectedIndexChanged;
            BayarGrid.CellFormatting += BayarGrid_CellFormatting;
        }

        private void DeleteElementButton_Click(object sender, EventArgs e)
        {
            var piutang = _piutangBuilder
                .Load(new PiutangModel(_piutangId))
                .Build();
            piutang.ListElement.Clear();
            piutang = _piutangBuilder
                .Attach(piutang)
                .AddMinusElement(PiutangElementEnum.Retur, 0)
                .AddMinusElement(PiutangElementEnum.Potongan, 0)
                .AddPlusElement(PiutangElementEnum.Materai, 0)
                .AddPlusElement(PiutangElementEnum.Admin, 0)
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
                .AddMinusElement(PiutangElementEnum.Retur, ReturText.Value)
                .AddMinusElement(PiutangElementEnum.Potongan, PotonganText.Value)
                .AddMinusElement(PiutangElementEnum.Materai, MateraiText.Value)
                .AddMinusElement(PiutangElementEnum.Admin, AdminText.Value)
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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteTagihan();
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

        private void NilaiPelunasanText_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as NumericUpDown;
            if (e.KeyCode == Keys.F1)
            {
                textBox.Focus();
                textBox.Select(0, 0);
                var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
                textBox.Value = piutang.Sisa; //piutang.Sisa - ReturText.Value - PotonganText.Value - MateraiText.Value - AdminText.Value; //;
            }
        }

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

        private void IniGridBayar()
        {
            BayarGrid.DataSource = _listLunasPiutangBayar;
            BayarGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.LavenderBlush,
                Font = new System.Drawing.Font("Lucida Console", 8.25F),
            };

            var cols = BayarGrid.Columns;
            RefreshGridBayar(new PiutangModel { ListLunas = new List<PiutangLunasModel>()});
            cols.GetCol("Tgl").DefaultCellStyle.Format = "ddd, dd-MMM-yyyy";
            cols.GetCol("Nilai").DefaultCellStyle.Format = "N0";
            cols.GetCol("Nilai").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //cols.GetCol("Tgl").Width = 120;
            //cols.GetCol("Keterangan").Width = 120;
            //cols.GetCol("Nilai").Width = 80;
            //cols.GetCol("Nilai").DefaultCellStyle.BackColor = System.Drawing.Color.LavenderBlush;

            cols.GetCol("JenisData").Visible = false;

            //  hide row header
            BayarGrid.RowHeadersVisible = false;
            //  readonly
            BayarGrid.ReadOnly = true;
            BayarGrid.AllowUserToAddRows = false;
            BayarGrid.AllowUserToDeleteRows = false;
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
                    BayarGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor= System.Drawing.Color.Gray;
                }
                else
                {
                    BayarGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
        }
        private void RefreshGridBayar(PiutangModel piutang)
        {
            piutang.RemoveNull();
            int noUrut = 1;
            _listLunasPiutangBayar = new BindingList<LunasPiutangBayarView>
            {
                new LunasPiutangBayarView(
                    noUrut,
                    piutang.PiutangDate,
                    "Tagihan",
                    piutang.Total,
                    1)
            };

            var retur = piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Retur)?.NilaiMinus ?? 0;
            var pot = piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Potongan)?.NilaiMinus ?? 0;
            var materai = piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Materai)?.NilaiMinus ?? 0;
            var admin = piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Admin)?.NilaiMinus ?? 0;
            var potBiayaLain = materai + admin + retur + pot;

            if(potBiayaLain != 0)
            {
                noUrut++;
                var caption = "Biaya Lain";
                if (potBiayaLain < 0)
                    caption = "   Potongan";
                _listLunasPiutangBayar.Add(new LunasPiutangBayarView(noUrut, piutang.PiutangDate, caption, potBiayaLain,2));
            }


            foreach (var pelunasan in piutang.ListLunas)
            {
                noUrut++;
                _listLunasPiutangBayar.Add(new LunasPiutangBayarView(
                    noUrut,
                    pelunasan.LunasDate,
                    $"   Pembayaran {pelunasan.JenisLunas}",
                    -pelunasan.Nilai,3));
            }

            noUrut++;
            _listLunasPiutangBayar.Add(new LunasPiutangBayarView(noUrut, DateTime.Now, "Sisa Tagihan", piutang.Sisa,1));

            BayarGrid.DataSource = _listLunasPiutangBayar;
            var cols = BayarGrid.Columns;
            cols.GetCol("No").Width = 30;
            cols.GetCol("Tgl").Width = 70;
            cols.GetCol("Keterangan").Width = 150;
            cols.GetCol("Nilai").Width = 80;
            //BayarGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Auto-resize all columns
            //BayarGrid.AutoResizeColumns(); // E
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
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
            foreach(var item in piutang.ListLunas)
            {
                item.NoUrut = i;
                i++;
            }

            _piutangWriter.Save(ref piutang);
            //var faktur = _fakturBuilder
            //    .Load(new FakturModel(_piutangId))
            //    .Build();

            RefreshGridBayar(piutang);
        }

        private void DeleteTagihan()
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


        private void ListPiutangGrid_TableControlCurrentCellValidated(object sender, GridTableControlEventArgs e)
        {
            ListPiutangGrid.CancelEdit();
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
            _listLunasPiutangBayar = new BindingList<LunasPiutangBayarView>();
            BayarGrid.DataSource = _listLunasPiutangBayar;
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
            PotonganText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Potongan )?.NilaiMinus ?? 0;
            MateraiText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Materai)?.NilaiMinus ?? 0;
            AdminText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Admin)?.NilaiMinus ?? 0;

            FakturCodeText.Text = fc;
            TglFakturText2.Text= piutang.PiutangDate.ToString("dd-MM-yyyy");
            SalesText.Text = faktur.SalesPersonName;
            LunasDateText.Value = DateTime.Now;

            SetTagihanComboDataSource(faktur);

            RefreshGridBayar(piutang);
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
            _listPiutangLunasView = new BindingList<PiutangLunasView>();
            _bindingSource = new BindingSource
            {
                DataSource = _listPiutangLunasView
            };
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

            var sumPotongan = new GridSummaryColumnDescriptor("Potongan", SummaryType.DoubleAggregate, "Potongan", "{Sum}");
            sumPotongan.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumPotongan.Appearance.AnySummaryCell.Format = "N0";
            sumPotongan.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColTerbayar = new GridSummaryColumnDescriptor("Terbayar", SummaryType.DoubleAggregate, "Terbayar", "{Sum}");
            sumColTerbayar.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumColTerbayar.Appearance.AnySummaryCell.Format = "N0";
            sumColTerbayar.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumColSisa = new GridSummaryColumnDescriptor("Sisa", SummaryType.DoubleAggregate, "Sisa", "{Sum}");
            sumColSisa.Appearance.AnySummaryCell.Interior = new BrushInfo(System.Drawing.Color.Khaki);
            sumColSisa.Appearance.AnySummaryCell.Format = "N0";
            sumColSisa.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColTotal, sumPotongan, sumColTerbayar, sumColSisa});
            ListPiutangGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            //  format number columns
            ListPiutangGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            ListPiutangGrid.TableDescriptor.Columns["Potongan"].Appearance.AnyRecordFieldCell.Format = "N0";
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
            ListPiutangGrid.Refresh();
        }
        #endregion

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
