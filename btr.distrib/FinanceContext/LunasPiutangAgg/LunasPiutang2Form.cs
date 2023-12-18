using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.UseCases;
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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SupportContext.UserAgg;
using Polly;
using DocumentFormat.OpenXml.Wordprocessing;

namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    public partial class LunasPiutang2Form : Form
    {
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
        private readonly IPiutangLunasViewDal _piutangLunasViewDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturDal _fakturDal;
        private readonly IAddLunasPiutangWorker _addLunasPiutangWorker;
        private readonly IRemoveLunasPiutangWorker _removeLunasPiutangWorker;
        
        private BindingList<PiutangLunasView> _listPiutangLunasView;
        private BindingList<LunasPiutangBayarView> _listLunasPiutangBayar;
        private BindingSource _bindingSource;
        private string _piutangId;


        public LunasPiutang2Form(IPiutangLunasViewDal piutangLunasViewDal,
            IPiutangBuilder piutangBuilder,
            IFakturBuilder fakturBuilder,
            IAddLunasPiutangWorker addLunasPiutangWorker, IFakturDal fakturDal,
            IRemoveLunasPiutangWorker removeLunasPiutangWorker, IPiutangWriter piutangWriter)
        {
            _piutangLunasViewDal = piutangLunasViewDal;
            _piutangBuilder = piutangBuilder;
            _fakturBuilder = fakturBuilder;
            _addLunasPiutangWorker = addLunasPiutangWorker;
            _fakturDal = fakturDal;
            _removeLunasPiutangWorker = removeLunasPiutangWorker;

            InitializeComponent();
            InitGrid();
            IniGridBayar();
            InitJenisBayarCombo();
            RegisterEventHandler();
            _piutangWriter = piutangWriter;
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
            JenisBayarCombo.SelectedIndexChanged += JenisBayarCombo_SelectedIndexChanged;
            NilaiPelunasanText.KeyDown += NilaiPelunasanText_KeyDown;
            FakturCodeText.Validating += FakturCodeText_Validating;
            FakturCodeText.KeyDown += FakturCodeText_KeyDown;
            BayarGrid.CellDoubleClick += BayarGrid_CellDoubleClick;
        }

        private void FakturCodeText_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.KeyCode == Keys.Enter) 
                //  set focus to next control
                SelectNextControl(textBox, true, true, true, true);
        }

        private void BayarGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.RowIndex == -1) return;
            if (e.RowIndex == 0) return;
            if (e.RowIndex == grid?.Rows.Count - 1) return;
            
            var deleted = _listLunasPiutangBayar[e.RowIndex];
            _listLunasPiutangBayar.Remove(deleted);
            BayarGrid.Refresh();

            if (deleted.Keterangan == "Potongan")
                RemovePotongan();
            else
                RemovePelunasan(deleted, e.RowIndex);

            NoRekBgText.Text = string.Empty;
            BankNameText.Text = string.Empty;
            AtasNamaText.Text = string.Empty;
        }

        private void RemovePotongan()
        {
            var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();

            ReturText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Retur)?.NilaiMinus ?? 0;
            PotonganText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Potongan)?.NilaiMinus ?? 0;
            MateraiText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Materai)?.NilaiMinus ?? 0;
            AdminText.Value = piutang.ListElement.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Admin)?.NilaiMinus ?? 0;

            piutang = _piutangBuilder
                .Attach(piutang)
                .ClearElement()
                .Build();

            _piutangWriter.Save(ref piutang);
            RefreshGridBayar(piutang);
            RefreshGrid();
        }

        private void RemovePelunasan(LunasPiutangBayarView deletedItem, int noUrut)
        {
            var removeLunasReq = new RemoveLunasPiutangRequest
            {
                PiutangId = _piutangId,
                NoUrut = noUrut
            };
            _removeLunasPiutangWorker.Execute(removeLunasReq);
            NilaiPelunasanText.Value = Math.Abs(deletedItem.Nilai);
            JenisBayarCombo.SelectedIndex = deletedItem.Keterangan == "Pelunasan Cash" ? 0 : 1;
            JatuhTempBgText.Value = deletedItem.Tgl;

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
                var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
                textBox.Value = piutang.Sisa - ReturText.Value - PotonganText.Value - MateraiText.Value - AdminText.Value;
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
                BackColor = System.Drawing.Color.Thistle,
                Font = new System.Drawing.Font("Consolas", 8.25F),

            };
            var cols = BayarGrid.Columns;
            RefreshGridBayar(new PiutangModel { ListLunas = new List<PiutangLunasModel>()});
            cols.GetCol("Tgl").DefaultCellStyle.Format = "ddd, dd-MMM-yyyy";
            cols.GetCol("Nilai").DefaultCellStyle.Format = "N0";
            cols.GetCol("Nilai").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            cols.GetCol("Tgl").Width = 120;
            cols.GetCol("Keterangan").Width = 120;
            cols.GetCol("Nilai").Width = 80;
            cols.GetCol("Nilai").DefaultCellStyle.BackColor = System.Drawing.Color.LavenderBlush;

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
            _listLunasPiutangBayar = new BindingList<LunasPiutangBayarView>
            {
                new LunasPiutangBayarView
                {
                    Tgl = piutang.PiutangDate,
                    Keterangan = "Tagihan",
                    Nilai = piutang.Total,
                }
            };
            var potongan = piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Retur)?.NilaiMinus ?? 0;
            potongan += piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Potongan)?.NilaiMinus ?? 0;
            potongan += piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Materai)?.NilaiMinus ?? 0;
            potongan += piutang.ListElement?.FirstOrDefault(x => x.ElementTag == PiutangElementEnum.Admin)?.NilaiMinus ?? 0;
            
            if (potongan > 0)
                _listLunasPiutangBayar.Add(new LunasPiutangBayarView
                {
                    Tgl = piutang.PiutangDate,
                    Keterangan = $"Potongan",
                    Nilai = -potongan
                });
            
            foreach (var pelunasan in piutang.ListLunas)
            {
                _listLunasPiutangBayar.Add(new LunasPiutangBayarView
                {
                    Tgl = pelunasan.LunasDate,
                    Keterangan = $"Pelunasan {pelunasan.JenisLunas}",
                    Nilai = -pelunasan.Nilai
                });
            }
            _listLunasPiutangBayar.Add(new LunasPiutangBayarView
            {
                Tgl = DateTime.Now,
                Keterangan = "Sisa Tagihan",
                Nilai = piutang.Sisa
            });
            BayarGrid.DataSource = _listLunasPiutangBayar;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var addLunasReq = new AddLunasPiutangRequest
            {
                PiutangId = _piutangId,
                Retur = ReturText.Value,
                Potongan = PotonganText.Value,
                Materai = MateraiText.Value,
                Admin = AdminText.Value,
                Nilai = NilaiPelunasanText.Value,
                JenisLunas = JenisBayarCombo.Text == @"Tunai" ? JenisLunasEnum.Cash : JenisLunasEnum.CekBg,
                LunasDate = LunasDateText.Value,
                JatuhTempoBg = JatuhTempBgText.Value,
                NoBgRek = NoRekBgText.Text,
                NamaBank = BankNameText.Text,
                AtasNamaBank = AtasNamaText.Text,
            };
            _addLunasPiutangWorker.Execute(addLunasReq);
            RefreshGrid();
            var piutang = _piutangBuilder.Load(new PiutangModel(_piutangId)).Build();
            RefreshGridBayar(piutang);
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
            TglFakturText.Value= DateTime.Now;
            JatuhTempoText.Value = DateTime.Now;
            SalesText.Text = string.Empty;
            LunasDateText.Value = DateTime.Now;
            
            ReturText.Value = 0;
            PotonganText.Value = 0;
            MateraiText.Value = 0;
            AdminText.Value = 0;
            NilaiPelunasanText.Value = 0;
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
            TglFakturText.Text= piutang.PiutangDate.ToString("dd MMM yyyy");
            JatuhTempoText.Text = piutang.DueDate.ToString("dd MMM yyyy");
            SalesText.Text = faktur.SalesPersonName;
            LunasDateText.Value = DateTime.Now;
            RefreshGridBayar(piutang);
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
        public DateTime Tgl { get; set; }
        public string Keterangan { get; set; }
        public decimal Nilai { get; set; }
    }
}
