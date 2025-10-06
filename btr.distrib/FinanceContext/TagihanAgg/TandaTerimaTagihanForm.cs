using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.FinanceContext.TagihanAgg;
using btr.distrib.Helpers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TandaTerimaTagihanForm : Form
    {
        private readonly SortableBindingList<TandaTerimaTagihanViewDto> _listTandaTerima;
        private readonly BindingSource _bindingSource = new BindingSource();
        private readonly ITagihanFakturDal _tagihanFakturDal;
        private readonly ITagihanBuilder _tagihanBuilder;
        private readonly ITagihanWriter _tagihanWriter;
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;

        public TandaTerimaTagihanForm(ITagihanFakturDal tagihanFakturDal,
            ITagihanBuilder tagihanBuilder,
            ITagihanWriter tagihanWriter,
            IPiutangBuilder piutangBuilder,
            IPiutangWriter piutangWriter)
        {
            InitializeComponent();

            _listTandaTerima = new SortableBindingList<TandaTerimaTagihanViewDto>();
            _bindingSource.DataSource = new BindingSource(_listTandaTerima, null);
            _tagihanFakturDal = tagihanFakturDal;
            _tagihanBuilder = tagihanBuilder;
            _tagihanWriter = tagihanWriter;

            InitEventHandler();
            InitGrid();
            _piutangBuilder = piutangBuilder;
            _piutangWriter = piutangWriter;
        }

        private void InitEventHandler()
        {
            ProsesButton.Click += ProsesButton_Click;
            TagihanGrid.CellContentClick += TagihanGrid_CellContentClick;
            CheckAllTTButton.Click += CheckAllTTButton_Click;
            CheckAllTUButton.Click += CheckAllTUButton_Click;
            UncheckAllButton.Click += UncheckAllButton_Click;
            ApplyButton.Click += ApplyButton_Click;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void Apply()
        {
            ApplyProgress.Visible = true;
            var listTagihanId = _listTandaTerima
                .Select(x => x.TagihanId)
                .Distinct()
                .ToList();

            ApplyProgress.Value = 0;
            ApplyProgress.Maximum = listTagihanId.Count;

            using (var trans = TransHelper.NewScope())
            {
                foreach (var item in listTagihanId)
                {
                    ApplyProgress.Value++;
                    var tagihan = _tagihanBuilder.Load(new TagihanModel(item)).Build();
                    var listFaktur = _listTandaTerima
                        .Where(x => x.TagihanId == item);
                    if (listFaktur.Any())
                    {
                        foreach (var faktur in listFaktur)
                        {
                            if (faktur.IsTandaTerima)
                                tagihan.TandaTerima(faktur.FakturId);
                            else
                                tagihan.BatalTandaTerima(faktur.FakturId);

                            if (faktur.IsTagihUlang)
                                tagihan.TagihUlang(faktur.FakturId);
                            else
                                tagihan.BatalTagihUlang(faktur.FakturId);
                        }
                    }
                    _tagihanWriter.Save(tagihan);
                }

                //var listTagihUlang = _listTandaTerima
                //    .Where(x => x.IsTagihUlang)?
                //    .ToList() ?? new List<TandaTerimaTagihanViewDto>();

                ApplyProgress.Value = 0;
                ApplyProgress.Maximum = _listTandaTerima.Count;

                //foreach (var item in listTagihUlang)
                //{
                //    ApplyProgress.Value++;
                //    var piutang = _piutangBuilder.Load(new PiutangModel(item.FakturId)).Build();
                //    piutang.TagihUlang();
                //    _piutangWriter.Save(ref piutang);
                //}

                foreach (var item in _listTandaTerima)
                {
                    ApplyProgress.Value++;
                    var piutang = _piutangBuilder.Load(new PiutangModel(item.FakturId)).Build();
                    if (item.IsTagihUlang)
                        piutang.TagihUlang();
                    else
                        piutang.Ditagihkan();

                    _piutangWriter.Save(ref piutang);
                }

                trans.Complete();
            }

            ApplyProgress.Visible = false;
        }

        private void UncheckAllButton_Click(object sender, EventArgs e)
        {
            _listTandaTerima.ToList().ForEach(x =>
            {
                x.IsTagihUlang = false;
                x.IsTandaTerima = false;
            });
        }

        private void CheckAllTUButton_Click(object sender, EventArgs e)
        {
            _listTandaTerima.ToList().ForEach(x =>
            {
                if (x.IsTagihUlang == false && x.IsTandaTerima == false)
                {
                    x.IsTagihUlang = true;
                }
            });
        }

        private void CheckAllTTButton_Click(object sender, EventArgs e)
        {
            _listTandaTerima.ToList().ForEach(x =>
            {
                if (x.IsTagihUlang == false && x.IsTandaTerima == false)
                {
                    x.IsTandaTerima = true;
                }
            });
        }

        private void TagihanGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            var isTT = (bool)grid.Rows[e.RowIndex].Cells["IsTandaTerima"].EditedFormattedValue;
            var isTU = (bool)grid.Rows[e.RowIndex].Cells["IsTagihUlang"].EditedFormattedValue;
            if (isTT == false && isTU == false)
            {
                return;
            }

            grid.EndEdit();
            ReCount();
        }

        private void ReCount()
        {
            var totFaktur = _listTandaTerima.Count;
            var totTandaTerima = _listTandaTerima.Count(x => x.IsTandaTerima);
            var totTagihUlang = _listTandaTerima.Count(x => x.IsTagihUlang);
            var summaryLabel = $" Jum. Faktur: {totFaktur:N0} | TT: {totTandaTerima:N0} | TU: {totTagihUlang:N0}";
            SummaryLabel.Text = summaryLabel;

            var totalNilai = _listTandaTerima.Sum(x => x.NilaiTotal);
            var totalTerbayar = _listTandaTerima.Sum(x => x.NilaiTerbayar);
            var totalTagih = _listTandaTerima.Sum(x => x.NilaiTagih);
            TotalAmountLabel.Text = $"Total Nilai = {totalNilai:N0} | Terbayar = {totalTerbayar:N0} | Ditagihkan = {totalTagih:N0}";
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
            ReCount();
        }

        private void Proses()
        {
            var periode = new Periode(Tgl1Date.Value);
            var listFaktur = _tagihanFakturDal.ListData(periode)?.ToList()
                ?? new List<TandaTerimaTagihanViewDto>();
            var keyword = SearchText.Text?.Trim().ToLower();
            if (keyword?.Length > 0)
            {
                listFaktur = listFaktur.Where(x =>
                    (x.FakturCode?.ToLower().Contains(keyword) ?? false)
                    || (x.CustomerName?.ToLower().Contains(keyword) ?? false)
                    || (x.Alamat?.ToLower().Contains(keyword) ?? false)
                    || (x.SalesPersonName?.ToLower().Contains(keyword) ?? false)
                ).ToList();
            }

            listFaktur = listFaktur
                .GroupBy(x => x.FakturId)
                .Select(g => g.OrderByDescending(x => x.TagihanId).First())
                .ToList();

            _listTandaTerima.Clear();
            foreach (var item in listFaktur)
            {
                _listTandaTerima.Add(item);
            }
        }

        private void InitGrid()
        {
            TagihanGrid.DataSource = _bindingSource;
            TagihanGrid.DefaultCellStyle.Font = new Font("Lucida Console", 8);

            var grid = TagihanGrid.Columns;
            grid["TagihanId"].Visible = false;
            grid["TagihanId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            grid["SalesPersonId"].Visible = false;
            grid["FakturId"].Visible = true;

            grid["SalesPersonName"].HeaderText = "Sales";
            grid["SalesPersonName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            grid["FakturCode"].HeaderText = "Faktur";
            grid["FakturCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            
            grid["FakturDate"].HeaderText = "Tanggal";
            grid["FakturDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            grid["FakturDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
            grid["FakturDate"].Visible = false;

            grid["CustomerName"].HeaderText = "Customer";
            grid["CustomerName"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            grid["Alamat"].DefaultCellStyle.Font = new Font("Segoe UI", 8);

            grid["NilaiTotal"].HeaderText = "Total";
            grid["NilaiTotal"].DefaultCellStyle.Format = "N0";
            grid["NilaiTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            grid["NilaiTerbayar"].HeaderText = "Terbayar";
            grid["NilaiTerbayar"].DefaultCellStyle.Format = "N0";
            grid["NilaiTerbayar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            grid["NilaiTagih"].HeaderText = "Tagih";
            grid["NilaiTagih"].DefaultCellStyle.Format = "N0";
            grid["NilaiTagih"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            grid["IsTandaTerima"].HeaderText = $"Tanda\nTerima";
            grid["IsTandaTerima"].Width = 100;
            grid["IsTandaTerima"].ReadOnly = false;
            grid["IsTandaTerima"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid["IsTandaTerima"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            grid["IsTagihUlang"].HeaderText = $"Tagih\nUlang";
            grid["IsTagihUlang"].Width = 100;
            grid["IsTagihUlang"].ReadOnly = false;
            grid["IsTagihUlang"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid["IsTagihUlang"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid["Keterangan"].HeaderText = "Keterangan";
            grid["Keterangan"].Width = 200;
            grid["TandaTerimaDate"].Visible = false;


            TagihanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            TagihanGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            TagihanGrid.AllowUserToAddRows = false;
            TagihanGrid.MultiSelect = false;
            //  set header color to light blue
            TagihanGrid.EnableHeadersVisualStyles = false;
            TagihanGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            TagihanGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TagihanGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //  set row header color to light blue
            TagihanGrid.RowHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            TagihanGrid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TagihanGrid.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            TagihanGrid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightBlue;
            TagihanGrid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            TagihanGrid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = TagihanGrid.Rows[e.RowIndex];
                var item = (TandaTerimaTagihanViewDto)row.DataBoundItem;
                if (item.IsTandaTerima) 
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                else  if (item.IsTagihUlang) 
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            };
            foreach (DataGridViewColumn col in TagihanGrid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            TagihanGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }
    }
}
