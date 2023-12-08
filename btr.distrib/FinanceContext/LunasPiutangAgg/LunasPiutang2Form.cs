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

namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    public partial class LunasPiutang2Form : Form
    {
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangLunasViewDal _piutangLunasViewDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IAddLunasPiutangWorker _addLunasPiutangWorker;
        
        private BindingList<PiutangLunasView> _listPiutangLunasView;
        private BindingList<LunasPiutangBayarView> _listLunasPiutangBayar;
        private BindingSource _bindingSource;
        private string _piutangId;


        public LunasPiutang2Form(IPiutangLunasViewDal piutangLunasViewDal,
            IPiutangBuilder piutangBuilder,
            IFakturBuilder fakturBuilder,
            IAddLunasPiutangWorker addLunasPiutangWorker)
        {
            _piutangLunasViewDal = piutangLunasViewDal;
            _piutangBuilder = piutangBuilder;
            _fakturBuilder = fakturBuilder;

            InitializeComponent();
            InitGrid();
            IniGridBayar();
            InitJenisBayarCombo();
            RegisterEventHandler();
            _addLunasPiutangWorker = addLunasPiutangWorker;
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
                    Nilai = piutang.Total
                }
            };
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
            var addLunasReq = new AddLunasPiutangRequest
            {
                PiutangId = _piutangId,
                Retur = ReturText.Value,
                Potongan = PotonganText.Value,
                Materai = MateraiText.Value,
                Admin = AdminText.Value,
                Nilai = NilaiPelunasanText.Value,
                JenisLunas = JenisBayarCombo.Text == "Tunai" ? JenisLunasEnum.Cash : JenisLunasEnum.CekBg,
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

        private void LoadPiutang(string piutangId)
        {
            var piutang = _piutangBuilder.Load(new PiutangModel(piutangId)).Build();
            var faktur = _fakturBuilder.Load(new FakturModel(piutang.PiutangId)).Build();
            var fc = faktur.FakturCode;
            _piutangId = piutang.PiutangId;
            CustomerText.Text = piutang.CustomerName;
            AddressText.Text = faktur.Address;

            FakturCodeText.Text = $@"{fc[0]}-{fc.Substring(1,3)}-{fc.Substring(4,4)}";
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
