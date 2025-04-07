using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.KartuStokRpt;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.KartuStokRpt;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mapster;
using System.ComponentModel;
using System.Drawing;
using ClosedXML.Excel;

namespace btr.distrib.InventoryContext.KartuStokRpt
{
    public partial class KartuStokInfoForm : Form
    {
        private readonly IBrgDal _brgDal;
        private readonly IKartuStokDal _kartuStokDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly BindingList<BrgKartuDto> _listBrgKartu;
        private readonly BindingSource _listKartuBindingSource;
        private List<KartuStokItemInfoDto> _listItemKartuStok;

        public KartuStokInfoForm(IBrgDal brgDal,
            IWarehouseDal warehouseDal,
            IKartuStokDal kartuStokDal)
        {
            InitializeComponent();
            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            _kartuStokDal = kartuStokDal;
            _listBrgKartu = new BindingList<BrgKartuDto>();
            _listKartuBindingSource = new BindingSource();
            _listKartuBindingSource.DataSource = _listBrgKartu;

            PeriodeCalender.SelectionStart = DateTime.Now.AddDays(-6);
            PeriodeCalender.SelectionEnd = DateTime.Now.AddDays(1);
            
            RegisterEventHandler();
            InitGridBrg();
            InitGridKartuStok();
        }

        private void RegisterEventHandler()
        {
            ListBarangButton.Click += ListBarangButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            BrgGrid.CellDoubleClick += BrgGrid_CellDoubleClick;
            KartuStokGrid.RowPrePaint += KartuStokGrid_RowPrePaint;
            PencatatanRadioButton.CheckedChanged += SortingRadioButton_CheckedChanged;
            PengakuanRadioButton.CheckedChanged -= SortingRadioButton_CheckedChanged;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            GenToExcel();
        }

        private void SortingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GenKartuStok();
        }

        private void KartuStokGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var cellValue = grid.Rows[e.RowIndex].Cells["JenisMutasi"].Value;
            Color color;
            switch (cellValue)
            {
                case "Saldo Awal":
                    color = System.Drawing.Color.LightBlue;
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = color;
                    break;
                case "Saldo Akhir":
                    color = System.Drawing.Color.Pink;
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = color;
                    break;
            }
        }

        private void BrgGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            GenKartuStok();
        }

        private void ListBarangButton_Click(object sender, EventArgs e)
        {
            var keyWord = SearchBrgText.Text.ToLower();
            var listBrg = _brgDal.ListData()?.ToList()
                ?? new List<BrgModel>();
            var listNamaBrg = listBrg.Where(x => x.BrgName.ToLower().ContainMultiWord(keyWord));
            var listCode = listBrg.Where(x => x.BrgCode.ToLower().StartsWith(keyWord));
            var listId = listBrg.Where(x => x.BrgId.ToLower().StartsWith(keyWord));
            var result = listNamaBrg.Union(listCode).Union(listId)
                .OrderBy(x => x.BrgCode)
                .Select(x => new BrgKartuDto(x.BrgId,x.BrgCode,x.BrgName))
                .ToList();
            _listBrgKartu.Clear();
            result.ForEach(x => _listBrgKartu.Add(x));
            BrgGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void InitGridBrg()
        {
            BrgGrid.DataSource = _listKartuBindingSource;

            BrgGrid.Columns["BrgId"].Visible = false;
            BrgGrid.Columns["BrgCode"].Visible = false;
            BrgGrid.Columns["BrgName"].Visible = false;
            BrgGrid.RowHeadersVisible = false;
            BrgGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            BrgGrid.Columns["BrgCodeName"].Width = 200;
        }

        private void InitGridKartuStok()
        {
            KartuStokGrid.DataSource = new List<KartuStokItemInfoDto>();
            KartuStokGrid.Refresh();
            KartuStokGrid.AutoGenerateColumns = false;

            var col = KartuStokGrid.Columns;
            col.GetCol("WarehouseName").HeaderText = @"Gudang";
            col.GetCol("MutasiDate").HeaderText = @"Tanggal";
            col.GetCol("JenisMutasi").HeaderText = @"Jenis Mutasi";
            col.GetCol("QtyAwal").HeaderText = @"Qty Awal";
            col.GetCol("QtyMasuk").HeaderText = @"Qty Masuk";
            col.GetCol("QtyKeluar").HeaderText = @"Qty Keluar";
            col.GetCol("QtyAkhir").HeaderText = @"Qty Saldo";
            col.GetCol("Hpp").HeaderText = @"HPP";
            col.GetCol("HargaJual").HeaderText = @"Harga Jual";
            col.GetCol("Keterangan").HeaderText = @"Keterangan";

            col.GetCol("WarehouseName").Width = 80;
            col.GetCol("No").Width = 40;
            col.GetCol("MutasiDate").Width = 80;
            col.GetCol("JenisMutasi").Width = 100;
            col.GetCol("QtyAwal").Width = 40;
            col.GetCol("QtyMasuk").Width = 40;
            col.GetCol("QtyKeluar").Width = 40;
            col.GetCol("QtyAkhir").Width = 40;
            col.GetCol("Hpp").Width = 70;
            col.GetCol("HargaJual").Width = 70;
            col.GetCol("Keterangan").Width = 350;

            foreach(DataGridViewColumn item in KartuStokGrid.Columns)
                item.ReadOnly = true;

            col.GetCol("QtyAwal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("QtyMasuk").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("QtyKeluar").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("QtyAkhir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("Hpp").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.GetCol("HargaJual").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            col.GetCol("QtyAwal").DefaultCellStyle.Format = "N0";
            col.GetCol("QtyMasuk").DefaultCellStyle.Format = "N0";
            col.GetCol("QtyKeluar").DefaultCellStyle.Format = "N0";
            col.GetCol("QtyAkhir").DefaultCellStyle.Format = "N0";
            col.GetCol("Hpp").DefaultCellStyle.Format = "N0";
            col.GetCol("HargaJual").DefaultCellStyle.Format = "N0";
            col.GetCol("MutasiDate").DefaultCellStyle.Format = "dd-MMM HH:mm";

            col.GetCol("QtyAwal").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("QtyMasuk").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("QtyKeluar").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F); 
            col.GetCol("QtyAkhir").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("Hpp").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("HargaJual").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("MutasiDate").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            col.GetCol("Keterangan").DefaultCellStyle.Font = new System.Drawing.Font("Lucida Console", 8.25F);

            col.GetCol("QtyAwal").DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
            col.GetCol("QtyAkhir").DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;

            col.GetCol("Hpp").DefaultCellStyle.BackColor = Color.LightGreen;
            col.GetCol("HargaJual").DefaultCellStyle.BackColor = Color.LightGreen;

            col.GetCol("JenisMutasi").Visible = false;
            col.GetCol("Hpp").Visible = false;
            col.GetCol("HargaJual").Visible = false;
        }

        private void GenToExcel()
        {
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"kartu-stok-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }
            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("kartu-stok-summary")
                .Cell($"B1")
                    .InsertTable(_listItemKartuStok, false);
                var ws = wb.Worksheets.First();

                //  set format row header: font bold, background lightblue, border medium
                ws.Range(ws.Cell("A1"), ws.Cell($"M1")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightBlue)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  set format row data: font consolas 9, border medium, border inside hair
                ws.Range(ws.Cell("A2"), ws.Cell($"M{_listItemKartuStok.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                //  add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < _listItemKartuStok.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;
                ws.Range(ws.Cell("A2"), ws.Cell($"M{_listItemKartuStok.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";

                //  format numeric column  
                ws.Range(ws.Cell("H2"), ws.Cell($"M{_listItemKartuStok.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##";
                //  format date column
                ws.Range(ws.Cell("C2"), ws.Cell($"C{_listItemKartuStok.Count + 1}"))
                    .Style.NumberFormat.Format = "dd-MMM-yyyy";

                //  format numeric column DiscTotal with 2 decimal places but hide zero
                ws.Range(ws.Cell("P2"), ws.Cell($"S{_listItemKartuStok.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.00_);(#,##0.00);-";

                //  set backcolor numeric column
                ws.Range(ws.Cell("H2"), ws.Cell($"L{_listItemKartuStok.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightGreen);
                ws.Range(ws.Cell("M2"), ws.Cell($"M{_listItemKartuStok.Count + 1}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LightBlue);

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void GenKartuStok()
        {
            var brgId = BrgGrid.CurrentRow.Cells["BrgId"].Value.ToString();
            var tgl1 = PeriodeCalender.SelectionStart;
            var tgl2 = PeriodeCalender.SelectionEnd;
            var periode = new Periode(tgl1, tgl2);
            var brgKey = new BrgModel(brgId);
            //GenKartuStok(periode, new BrgModel(brgId));

            var listKartuStokRaw = _kartuStokDal.ListData(periode, brgKey)?.ToList()
                ?? new List<KartuStokView>();
            //  sum QtyIn, QtyOut grouped by ReffId
            List<KartuStokView> listKartuStok;
            if (PencatatanRadioButton.Checked)
                listKartuStok = listKartuStokRaw
                    .Select(x => new KartuStokView
                    {
                        ReffId = x.ReffId,
                        JenisMutasi = x.JenisMutasi,
                        WarehouseId = x.WarehouseId,
                        QtyIn = x.QtyIn,
                        QtyOut = x.QtyOut,
                        Hpp = x.Hpp,
                        HargaJual = x.HargaJual,
                        Keterangan = x.Keterangan,
                        MutasiDate = x.MutasiDate
                    }).ToList();
            else
                listKartuStok = listKartuStokRaw
                    .OrderBy(x => x.MutasiDate)
                    .Select(x => new KartuStokView
                    {
                        ReffId = x.ReffId,
                        JenisMutasi = x.JenisMutasi,
                        WarehouseId = x.WarehouseId,
                        QtyIn = x.QtyIn,
                        QtyOut = x.QtyOut,
                        Hpp = x.Hpp,
                        HargaJual = x.HargaJual,
                        Keterangan = x.Keterangan,
                        MutasiDate = x.MutasiDate
                    }).ToList();

            var listSaldoAwal = GetSaldoAwal(periode, brgKey);
            var listWarehouse = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();
            _listItemKartuStok = new List<KartuStokItemInfoDto>();
            foreach(var warehouse in listWarehouse)
            {
                var saldoAwal = listSaldoAwal.FirstOrDefault(x => x.WarehouseId == warehouse.WarehouseId);
                _listItemKartuStok.Add(new KartuStokItemInfoDto
                {
                    WarehouseName = warehouse.WarehouseName,
                    JenisMutasi = "Saldo Awal",
                    QtyAwal = $"{saldoAwal?.QtyAkhir.ToString("N0") ?? string.Empty}",
                    QtyMasuk = string.Empty,
                    QtyKeluar = string.Empty,
                    QtyAkhir = string.Empty,
                    Hpp = 0,
                    HargaJual = 0,
                    Keterangan = "Saldo Awal"
                });
                var saldo = saldoAwal?.QtyAkhir ?? 0;
                var kartuStok = new KartuStokItemInfoDto();
                var noUrut = 1;
                foreach (var item in listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId))
                {
                    var qtyIn = item.QtyIn == 0 ? string.Empty : $"{item.QtyIn:N0}";
                    var qtyOut = item.QtyOut == 0 ? string.Empty : $"{item.QtyOut:N0}";
                    kartuStok = new KartuStokItemInfoDto
                    {
                        No = noUrut,
                        WarehouseName = string.Empty,
                        MutasiDate = item.MutasiDate,
                        JenisMutasi = item.JenisMutasi,
                        QtyAwal = string.Empty, //$"{saldo:N0}",
                        QtyMasuk = qtyIn,
                        QtyKeluar = qtyOut,
                        QtyAkhir = $"{(saldo + item.QtyIn - item.QtyOut):N0}",
                        Hpp = item.Hpp,
                        HargaJual = item.HargaJual,
                        Keterangan = $"{item.JenisMutasi} : {item.ReffId} - {item.Keterangan}"
                    };
                    _listItemKartuStok.Add(kartuStok);
                    saldo += item.QtyIn - item.QtyOut;
                    noUrut++;
                }

                var saldoAkhir = kartuStok.Adapt<KartuStokItemInfoDto>();
                //noUrut++;
                saldoAkhir.No = noUrut;
                saldoAkhir.JenisMutasi = "Saldo Akhir";
                saldoAkhir.Keterangan = "Saldo Akhir";
                var saldoQtyMasuk = listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId).Sum(x => x.QtyIn);
                var saldoQtyKeluar = listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId).Sum(x => x.QtyOut);
                saldoAkhir.QtyMasuk = $"{saldoQtyMasuk:N0}";
                saldoAkhir.QtyKeluar = $"{saldoQtyKeluar:N0}";
                saldoAkhir.Hpp = 0;
                saldoAkhir.HargaJual = 0;
                _listItemKartuStok.Add(saldoAkhir);
            }
            KartuStokGrid.DataSource = _listItemKartuStok;
        }

        private IEnumerable<KartuStokStokAwalView> GetSaldoAwal(Periode periode, IBrgKey brgKey)
        {
            var listWarehouse = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();
            var result = listWarehouse.Select(x => _kartuStokDal.GetSaldoAwal(periode, brgKey, x) ?? 
                new KartuStokStokAwalView
                {
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.WarehouseName,
                    QtyMasuk = 0,
                    QtyKeluar = 0,
                    QtyAkhir = 0
                });
            return result;
        }

    }

    public class KartuStokItemInfoDto 
    {
        public string WarehouseName { get; set; }
        public int No { get; set; }
        public string JenisMutasi { get; set; }
        public DateTime MutasiDate { get; set; }
        public string QtyAwal { get; set; }
        public string QtyMasuk { get; set; }
        public string QtyKeluar { get; set; }
        public string QtyAkhir { get; set; }
        public decimal Hpp{ get; set; }
        public decimal HargaJual { get; set; }
        public string Keterangan { get; set; }
    }

    public class BrgKartuDto
    {
        public BrgKartuDto(string id, string code, string name)
        {
            BrgId = id;
            BrgCode = code;
            BrgName = name;
            BrgCodeName = $"{name}\n    {code}";
        }
        public string BrgId { get; private set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string BrgCodeName { get; private set; }
    }
}