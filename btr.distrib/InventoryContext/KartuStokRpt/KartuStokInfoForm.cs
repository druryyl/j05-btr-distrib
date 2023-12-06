using btr.application.BrgContext.BrgAgg;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.KartuStokRpt;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.KartuStokRpt;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mapster;
using Color = System.Drawing.Color;

namespace btr.distrib.InventoryContext.KartuStokRpt
{
    public partial class KartuStokInfoForm : Form
    {
        private readonly IKategoriDal _kategoriDal;
        private readonly IBrgDal _brgDal;
        private readonly IKartuStokDal _kartuStokDal;
        private readonly IWarehouseDal _warehouseDal;

        public KartuStokInfoForm(IKategoriDal kategoriDal,
            IBrgDal brgDal,
            IWarehouseDal warehouseDal,
            IKartuStokDal kartuStokDal)
        {
            InitializeComponent();
            _kategoriDal = kategoriDal;

            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            _kartuStokDal = kartuStokDal;
            Periode1Date.Value = DateTime.Now.AddDays(-7);
            Periode2Date.Value = DateTime.Now;
            RegisterEventHandler();
            InitGridBrg();
            InitGridKartuStok();
        }

        private void RegisterEventHandler()
        {
            ListBarangButton.Click += ListBarangButton_Click;
            BrgGrid.CellDoubleClick += BrgGrid_CellDoubleClick;
            KartuStokGrid.RowPrePaint += KartuStokGrid_RowPrePaint;
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
            var brgId = BrgGrid.Rows[e.RowIndex].Cells["BrgId"].Value.ToString();
            var periode = new Periode(Periode1Date.Value, Periode2Date.Value);
            GenKartuStok(periode, new BrgModel(brgId));
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
                .Select(x => new
                {
                    x.BrgId,
                    x.BrgCode,
                    x.BrgName,
                }).ToList();
            BrgGrid.DataSource = result;
            BrgGrid.Refresh();
        }

        private void InitGridBrg()
        {
            BrgGrid.AutoGenerateColumns = false;
            BrgGrid.Columns.Clear();
            BrgGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BrgId",
                HeaderText = @"BrgId",
                Name = "BrgId",
                Visible = true,
                ReadOnly = true
            });
            BrgGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BrgCode",
                HeaderText = @"Code",
                Name = "BrgCode",
                Width = 80,
                ReadOnly = true
            });
            BrgGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BrgName",
                HeaderText = @"Nama Barang",
                Name = "BrgName",
                Width = 200,
                ReadOnly = true

            });
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

            col.GetCol("WarehouseName").Width = 120;
            col.GetCol("MutasiDate").Width = 80;
            col.GetCol("JenisMutasi").Width = 100;
            col.GetCol("QtyAwal").Width = 50;
            col.GetCol("QtyMasuk").Width = 50;
            col.GetCol("QtyKeluar").Width = 50;
            col.GetCol("QtyAkhir").Width = 50;
            col.GetCol("Hpp").Width = 70;
            col.GetCol("HargaJual").Width = 70;
            col.GetCol("Keterangan").Width = 250;

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

            col.GetCol("QtyAwal").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("QtyMasuk").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("QtyKeluar").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F); 
            col.GetCol("QtyAkhir").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("Hpp").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("HargaJual").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("MutasiDate").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);
            col.GetCol("Keterangan").DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8.25F);

            col.GetCol("QtyAwal").DefaultCellStyle.BackColor = System.Drawing.Color.Bisque;
            col.GetCol("QtyMasuk").DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
            col.GetCol("QtyKeluar").DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
            col.GetCol("QtyAkhir").DefaultCellStyle.BackColor = System.Drawing.Color.Bisque;

            col.GetCol("Hpp").DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
            col.GetCol("HargaJual").DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;

            col.GetCol("JenisMutasi").Visible = false;
            col.GetCol("Hpp").Visible = false;
            col.GetCol("HargaJual").Visible = false;
        }

        private void GenKartuStok(Periode periode, IBrgKey brgKey)
        {
            var listKartuStok = _kartuStokDal.ListData(periode, brgKey)?.ToList()
                ?? new List<KartuStokView>();
            var listSaldoAwal = GetSaldoAwal(periode, brgKey);
            var listWarehouse = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();
            var result = new List<KartuStokItemInfoDto>();
            foreach(var warehouse in listWarehouse)
            {
                var saldoAwal = listSaldoAwal.FirstOrDefault(x => x.WarehouseId == warehouse.WarehouseId);
                result.Add(new KartuStokItemInfoDto
                {
                    WarehouseName = warehouse.WarehouseName,
                    JenisMutasi = "Saldo Awal",
                    QtyAwal = saldoAwal?.QtyAkhir ?? 0,
                    QtyMasuk = 0,
                    QtyKeluar = 0,
                    QtyAkhir = 0,
                    Hpp = 0,
                    HargaJual = 0,
                    Keterangan = "Saldo Awal"
                });
                var saldo = saldoAwal?.QtyAkhir ?? 0;
                var kartuStok = new KartuStokItemInfoDto();
                foreach(var item in listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId))
                {
                    kartuStok = new KartuStokItemInfoDto
                    {
                        WarehouseName = string.Empty,
                        MutasiDate = item.MutasiDate,
                        JenisMutasi = item.JenisMutasi,
                        QtyAwal = saldo,
                        QtyMasuk = item.QtyIn,
                        QtyKeluar = item.QtyOut,
                        QtyAkhir = saldo + item.QtyIn - item.QtyOut,
                        Hpp = item.Hpp,
                        HargaJual = item.HargaJual,
                        Keterangan = $"{item.JenisMutasi} : {item.ReffId} - {item.Keterangan}"
                    };
                    result.Add(kartuStok);
                    saldo += item.QtyIn - item.QtyOut;
                }

                var saldoAkhir = kartuStok.Adapt<KartuStokItemInfoDto>();
                saldoAkhir.JenisMutasi = "Saldo Akhir";
                saldoAkhir.Keterangan = "Saldo Akhir";
                saldoAkhir.QtyMasuk = listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId).Sum(x => x.QtyIn);
                saldoAkhir.QtyKeluar = listKartuStok.Where(x => x.WarehouseId == warehouse.WarehouseId).Sum(x => x.QtyOut);
                saldoAkhir.Hpp = 0;
                saldoAkhir.HargaJual = 0;
                result.Add(saldoAkhir);
            }
            KartuStokGrid.DataSource = result;
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
        public string JenisMutasi { get; set; }
        public DateTime MutasiDate { get; set; }
        public int QtyAwal { get; set; }
        public int QtyMasuk { get; set; }
        public int QtyKeluar { get; set; }
        public int QtyAkhir { get; set; }
        public decimal Hpp{ get; set; }
        public decimal HargaJual { get; set; }
        public string Keterangan { get; set; }
    }
}