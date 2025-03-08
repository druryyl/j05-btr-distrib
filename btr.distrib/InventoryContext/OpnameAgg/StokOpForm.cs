using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using btr.application.BrgContext.BrgAgg;
using btr.application.BrgContext.KategoriAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using JetBrains.Annotations;

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public partial class StokOpForm : Form
    {
        private BindingList<StokOpItem> _listItem;
        private readonly BindingSource _bindingSource;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        
        private readonly IKategoriDal _kategoriDal;
        private readonly IWarehouseDal _warehouseDal;
        
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly ISaveStokOpWorker _saveStokOpWorker;
        private readonly IStokOpDal _stokOpDal;
        private readonly IBrgBuilder _brgBuilder;
        
        public StokOpForm(IBrgDal brgDal, 
            IKategoriDal kategoriDal, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgSatuanDal brgSatuanDal, 
            IWarehouseDal warehouseDal, 
            ISaveStokOpWorker saveStokOpWorker, 
            IStokOpDal stokOpDal, 
            IBrgBuilder brgBuilder)
        {
            InitializeComponent();
            _brgDal = brgDal;
            _kategoriDal = kategoriDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgSatuanDal = brgSatuanDal;
            _warehouseDal = warehouseDal;
            _saveStokOpWorker = saveStokOpWorker;
            _stokOpDal = stokOpDal;
            _brgBuilder = brgBuilder;
            _listItem = new BindingList<StokOpItem>();
            _bindingSource = new BindingSource
            {
                DataSource = _listItem
            };
            
            RegisterEventHandler(); 
            InitKetegoriCombo();
            InitWarehouseCombo();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            ListBrgButton.Click += ListBrgButton_Click;
            BrgGrid.CellValueChanged += BrgGrid_CellValueChanged;
            ExcelButton.Click += ExcelButton_Click;
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            PrintToExcel();
        }

        private void PrintToExcel()
        {
            var path = Path.GetTempPath();
            var baris = 1;
            var fileName = $"{path}\\StokOp_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("StokOp");
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "LAPORAN STOK OPNAME";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{PeriodeOpText.Value:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"Kategori Brg";
                ws.Cell($"B{baris}").Value = $"{KategoriCombo.Text}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);
                baris++;

                ws.Cell($"A{baris}").Value = $"Lokasi Gudang";
                ws.Cell($"B{baris}").Value = $"{WarehouseCombo.Text}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);
                baris++;
                baris++;
                var barisStart = baris;
                ws.Cell($"A{baris}").Value = "Kode Brg";
                ws.Cell($"B{baris}").Value = "Nama Brg";
                ws.Cell($"C{baris}").Value = "Qty-B Awal";
                ws.Cell($"D{baris}").Value = "Qty-K Awal";
                ws.Cell($"E{baris}").Value = "Qty-Pcs Awal";
                ws.Cell($"F{baris}").Value = "Qty-B Adjust";
                ws.Cell($"G{baris}").Value = "Qty-K Adjust";
                ws.Cell($"H{baris}").Value = "Qty-Pcs Adjust";
                ws.Cell($"I{baris}").Value = "Qty-B Akhir";
                ws.Cell($"J{baris}").Value = "Qty-K Akhir";
                ws.Cell($"K{baris}").Value = "Qty-Pcs Akhir";
                ws.Cell($"L{baris}").Value = "Hpp Satuan";
                ws.Cell($"M{baris}").Value = "Hpp Awal";
                ws.Cell($"N{baris}").Value = "Hpp Adjust";
                ws.Cell($"O{baris}").Value = "Hpp Akhir";
                baris++;

                foreach (var item in _listItem)
                {
                    ws.Cell($"A{baris}").Value = $"{item.BrgCode}";
                    ws.Cell($"B{baris}").Value = $"{item.BrgName}";
                    ws.Cell($"C{baris}").Value = item.QtyBesarAwal;
                    ws.Cell($"D{baris}").Value = item.QtyKecilAwal;
                    ws.Cell($"E{baris}").Value = item.QtyPcsAwal;

                    ws.Cell($"F{baris}").Value = item.QtyBesarAdjust;
                    ws.Cell($"G{baris}").Value = item.QtyKecilAdjust;
                    ws.Cell($"H{baris}").Value = item.QtyPcsAdjust;

                    ws.Cell($"I{baris}").Value = item.QtyBesarOpname;
                    ws.Cell($"J{baris}").Value = item.QtyKecilOpname;
                    ws.Cell($"K{baris}").Value = item.QtyPcsOpname;

                    ws.Cell($"L{baris}").Value = item.HppSatuan;
                    ws.Cell($"M{baris}").Value = item.HppAwal;
                    ws.Cell($"N{baris}").Value = item.HppAdjust;
                    ws.Cell($"O{baris}").Value = item.HppOpname;

                    baris++;
                }
                ws.Cell($"B{baris}").Value = $"Sub Total";
                ws.Cell($"M{baris}").Value = _listItem.Sum(x => x.HppAwal);
                ws.Cell($"N{baris}").Value = _listItem.Sum(x => x.HppAdjust);
                ws.Cell($"O{baris}").Value = _listItem.Sum(x => x.HppOpname);

                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"O{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"O{barisStart}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetBold(true);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"O{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetBold(true);

                //  format number column with thousand separator
                ws.Range(ws.Cell($"C{barisStart}"), ws.Cell($"O{baris}"))
                    .Style.NumberFormat.Format = "#,##0";
                //  set font number column to consolas 10 point
                ws.Range(ws.Cell($"C{barisStart}"), ws.Cell($"O{baris}"))
                    .Style.Font.SetFontName("Consolas")
                    .Font.SetFontSize(10);

                //  set background color columns Qty Awal to LemonChiffon
                ws.Range(ws.Cell($"C{barisStart}"), ws.Cell($"E{baris}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LemonChiffon);
                //  set background color columns Qty Adjust to Pink
                ws.Range(ws.Cell($"F{barisStart}"), ws.Cell($"H{baris}"))
                    .Style.Fill.SetBackgroundColor(XLColor.Pink);
                //  set background color columns Qty Akhir to LemonChiffon
                ws.Range(ws.Cell($"I{barisStart}"), ws.Cell($"K{baris}"))
                    .Style.Fill.SetBackgroundColor(XLColor.LemonChiffon);
                //  set background color columns Hpp to Beige
                ws.Range(ws.Cell($"L{barisStart}"), ws.Cell($"O{baris}"))
                    .Style.Fill.SetBackgroundColor(XLColor.Pink);
                //  set background color columns Hpp Satuan to white
                ws.Range(ws.Cell($"L{barisStart}"), ws.Cell($"L{baris}"))
                    .Style.Fill.SetBackgroundColor(XLColor.White);



                baris++;
                baris++;

                ws.Column("A").Width = 12;
                ws.Column("B").Width = 45;
                ws.Column("E").Width = 15;
                wb.SaveAs(fileName);
            }
            System.Diagnostics.Process.Start(fileName);
        }

        private void ListBrgButton_Click(object sender, EventArgs e)
        {
            ListBrg();
        }

        private void InitKetegoriCombo()
        {
            var listKategori = _kategoriDal.ListData()?.ToList()
                ?? new List<KategoriModel>();
            var ds = listKategori.OrderBy(x => x.KategoriName).ToList();
            KategoriCombo.DataSource = ds;
            KategoriCombo.DisplayMember = "KategoriName";
            KategoriCombo.ValueMember = "KategoriId";
        }
        
        private void InitWarehouseCombo()
        {
            var listWarehouse = _warehouseDal.ListData();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }
        
        #region GRID
        private void BrgGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var grid = (DataGridView)sender;

            var mainform = (MainForm)this.Parent.Parent;
            var user = mainform.UserId;

            if (e.ColumnIndex == grid.Columns.GetCol("QtyOpnameInputStr").Index)
            {
                NormalizeInput(_listItem[e.RowIndex].QtyOpnameInputStr);
                var reqStokOp = new SaveStokOpRequest
                {
                    BrgId = _listItem[e.RowIndex].BrgId,
                    StokOpId = _listItem[e.RowIndex].StokOpId,
                    WarehouseId = WarehouseCombo.SelectedValue.ToString(),
                    PeriodeOp = PeriodeOpText.Value.Date,
                    UserId = user.UserId,
                    QtyBesar = _listItem[e.RowIndex].QtyBesarOpname,
                    QtyKecil = _listItem[e.RowIndex].QtyKecilOpname,
                };
                var stokOp = _saveStokOpWorker.Execute(reqStokOp);

                _listItem[e.RowIndex].StokOpId = stokOp.StokOpId;
                _listItem[e.RowIndex].QtyBesarAwal = stokOp.QtyBesarAwal;
                _listItem[e.RowIndex].QtyKecilAwal = stokOp.QtyKecilAwal;
                _listItem[e.RowIndex].QtyPcsAwal = stokOp.QtyPcsAwal;
                
                _listItem[e.RowIndex].QtyBesarAdjust = stokOp.QtyBesarAdjust;
                _listItem[e.RowIndex].QtyKecilAdjust = stokOp.QtyKecilAdjust;
                _listItem[e.RowIndex].QtyPcsAdjust = stokOp.QtyPcsAdjust;

                BrgGrid.EndEdit();
                BrgGrid.Refresh();
            }
        }

        private void NormalizeInput(string qtyOpnameInputStr)
        {
            var brg = _brgBuilder.Load(new BrgModel(_listItem[BrgGrid.CurrentCell.RowIndex].BrgId)).Build();
            var conversion = brg.ListSatuan
                .DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 })
                .Max(x => x.Conversion);

            var qtys = ParseStringMultiNumber(qtyOpnameInputStr,2);

            if (qtys[1] > conversion)
            {
                var addedQty = Convert.ToInt16(qtys[0] / conversion);
                qtys[1] += addedQty;
                qtys[0] -= addedQty * conversion;
            }
            
            
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyBesarOpname = qtys[0];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyKecilOpname = qtys[1];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyPcsOpname = (qtys[0] * conversion) + qtys[1];
            _listItem[BrgGrid.CurrentCell.RowIndex].QtyOpnameInputStr = $"{qtys[0]};{qtys[1]}";
        }

        private static List<int> ParseStringMultiNumber(string str, int size)
        {
            if (str is null)
                str = string.Empty;

            var result = new List<int>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr.TakeWhile(item => x < result.Count))
            {
                if (int.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }

        private void ListBrg()
        {
            var listBrgItem = GenListBrg();
            listBrgItem = SetStokAwalAndConversion(listBrgItem);
            listBrgItem = UpdateFromPreviousInput(listBrgItem);

            _listItem = new BindingList<StokOpItem>(listBrgItem.ToList());
            _bindingSource.DataSource = _listItem;
            BrgGrid.DataSource = _bindingSource;
            BrgGrid.Refresh();
        }

        private IEnumerable<StokOpItem> GenListBrg()
        {
            var kategori = new KategoriModel(KategoriCombo.SelectedValue.ToString());
            var listBrg = _brgDal.ListData(kategori)?.ToList()
                          ?? new List<BrgModel>();

            var listBrgItem = listBrg
                .OrderBy(x => x.BrgCode)
                .Select(x => new StokOpItem
                {
                    BrgId = x.BrgId,
                    BrgCode = x.BrgCode,
                    BrgName = x.BrgName,
                    QtyBesarAwal = 0,
                    QtyKecilAwal =  0,
                    QtyPcsAwal =  0, 
                        
                    QtyBesarAdjust =  0,
                    QtyKecilAdjust =  0,
                    QtyPcsAdjust =  0,
                        
                    QtyBesarOpname =  0,
                    QtyKecilOpname =  0,
                    QtyPcsOpname =  0,
                        
                    QtyOpnameInputStr = string.Empty,
                    StokOpId = string.Empty,
                    Conversion = 1,

                    HppSatuan = x.Hpp,
                }).ToList();
            return listBrgItem;
        }
        
        private IEnumerable<StokOpItem> SetStokAwalAndConversion(IEnumerable<StokOpItem> listBrgItem)
        {
            var warehouse = new WarehouseModel(WarehouseCombo.SelectedValue.ToString());
            var listQty = _stokBalanceWarehouseDal.ListData(warehouse)?.ToList()
                          ?? new List<StokBalanceWarehouseModel>();
            var kategori = new KategoriModel(KategoriCombo.SelectedValue.ToString());
            var listConversion = _brgSatuanDal.ListData(kategori)?.ToList()
                                 ?? new List<BrgSatuanModel>();
            var fetchedListBrg = listBrgItem.ToList();
            foreach (var item in fetchedListBrg)
            {
                item.Conversion = listConversion
                    .Where(y => y.BrgId == item.BrgId)
                    .DefaultIfEmpty(new BrgSatuanModel { Conversion = 1 })
                    .Max(y => y.Conversion);
                
                var qty = listQty.FirstOrDefault(x => x.BrgId == item.BrgId);
                if (qty == null) continue;
                
                item.QtyBesarAwal = (int)Math.Floor((decimal)qty.Qty / item.Conversion);
                item.QtyKecilAwal = qty.Qty - (item.QtyBesarAwal * item.Conversion);
                item.QtyPcsAwal = qty.Qty;

                item.QtyBesarOpname = (int)Math.Floor((decimal)qty.Qty / item.Conversion);
                item.QtyKecilOpname = qty.Qty - (item.QtyBesarAwal * item.Conversion);
                item.QtyPcsOpname = qty.Qty;
                

                if (item.Conversion != 1) continue;
                item.QtyKecilAwal = item.QtyBesarAwal;
                item.QtyBesarAwal = 0;
                item.QtyKecilOpname = item.QtyBesarOpname;
                item.QtyBesarOpname = 0;
            }

            return fetchedListBrg;
        }

        private IEnumerable<StokOpItem> UpdateFromPreviousInput(IEnumerable<StokOpItem> listBrgItem2)
        {
            var listStokOp = _stokOpDal.ListData(new Periode(PeriodeOpText.Value.Date),
                    new WarehouseModel(WarehouseCombo.SelectedValue.ToString()))
                ?.ToList() ?? new List<StokOpModel>();
            var fetchedListBrg = listBrgItem2.ToList();
            foreach (var item in fetchedListBrg)
            {
                var stokOp = listStokOp.FirstOrDefault(y => y.BrgId == item.BrgId);
                if (stokOp == null) continue;
                
                item.QtyBesarAwal = stokOp.QtyBesarAwal;
                item.QtyKecilAwal = stokOp.QtyKecilAwal;
                item.QtyPcsAwal = stokOp.QtyPcsAwal;
                
                item.QtyBesarAdjust = stokOp.QtyBesarAdjust;
                item.QtyKecilAdjust = stokOp.QtyKecilAdjust;
                item.QtyPcsAdjust = stokOp.QtyPcsAdjust;
                
                item.QtyBesarOpname = stokOp.QtyBesarOpname;
                item.QtyKecilOpname = stokOp.QtyKecilOpname;
                item.QtyPcsOpname = stokOp.QtyPcsOpname;
                item.QtyOpnameInputStr = stokOp.QtyOpnameInputStr;
                item.StokOpId = stokOp.StokOpId;
            }

            return fetchedListBrg;
        }

        
        private void InitGrid()
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _listItem;
            BrgGrid.DataSource = bindingSource;
            BrgGrid.Refresh();

            var col = BrgGrid.Columns;
            col.SetDefaultCellStyle(System.Drawing.Color.Beige);

            col.GetCol("QtyPcsAwal").Visible = false;
            col.GetCol("QtyPcsAdjust").Visible = false;
            col.GetCol("QtyPcsOpname").Visible = false;
            col.GetCol("Conversion").Visible = false;

            foreach (DataGridViewColumn c in BrgGrid.Columns)
            {
                c.ReadOnly = true;
                c.DefaultCellStyle.BackColor = System.Drawing.Color.PowderBlue;
            }
            col.GetCol("QtyOpnameInputStr").ReadOnly = false;
            
            col.GetCol("QtyBesarAwal").DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon; 
            col.GetCol("QtyKecilAwal").DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
            col.GetCol("QtyBesarAdjust").DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
            col.GetCol("QtyKecilAdjust").DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
            col.GetCol("QtyBesarOpname").DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
            col.GetCol("QtyKecilOpname").DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
            col.GetCol("QtyOpnameInputStr").DefaultCellStyle.BackColor = System.Drawing.Color.White;

            col.GetCol("HppAwal").DefaultCellStyle.BackColor = System.Drawing.Color.Beige;
            col.GetCol("HppAdjust").DefaultCellStyle.BackColor = System.Drawing.Color.Beige;
            col.GetCol("HppOpname").DefaultCellStyle.BackColor = System.Drawing.Color.Beige;



            col.GetCol("BrgId").HeaderText = @"Id";
            col.GetCol("BrgCode").HeaderText = @"Brg Code";
            col.GetCol("BrgName").HeaderText = @"Brg Name";
            col.GetCol("QtyBesarAwal").HeaderText = @"Hpp";

            col.GetCol("QtyBesarAwal").HeaderText = @"Qty-B Awal"; 
            col.GetCol("QtyKecilAwal").HeaderText = @"Qty-K Awal"; 
            col.GetCol("QtyBesarAdjust").HeaderText = @"Qty-B Adjust"; 
            col.GetCol("QtyKecilAdjust").HeaderText = @"Qty-K Adjust"; 
            col.GetCol("QtyBesarOpname").HeaderText = @"Qty-B Akhir"; 
            col.GetCol("QtyKecilOpname").HeaderText = @"Qty-K Akhir";
            col.GetCol("StokOpId").HeaderText = @"Stok-Op ID";
            col.GetCol("QtyOpnameInputStr").HeaderText = @"Qty Opname";

            col.GetCol("HppSatuan").HeaderText = @"Hpp Satuan";
            col.GetCol("HppAwal").HeaderText = @"Hpp Awal";
            col.GetCol("HppAdjust").HeaderText = @"Hpp Adjust";
            col.GetCol("HppOpname").HeaderText = @"Hpp Akhir";

            col.GetCol("BrgId").Width = 80;
            col.GetCol("BrgCode").Width = 80;
            col.GetCol("BrgName").Width = 200;
            col.GetCol("QtyBesarAwal").Width = 50;
            col.GetCol("QtyKecilAwal").Width = 50;
            col.GetCol("QtyBesarAdjust").Width = 50;
            col.GetCol("QtyKecilAdjust").Width = 50;
            col.GetCol("QtyBesarOpname").Width = 50;
            col.GetCol("QtyKecilOpname").Width = 50;
            col.GetCol("QtyOpnameInputStr").Width = 70;

            col.GetCol("QtyOpnameInputStr").Width = 70;

        }
        #endregion
    }

    [PublicAPI]
    public class StokOpItem
    {
        public string BrgId { get;  set; }
        public string BrgCode { get;  set; }
        public string BrgName { get;  set; }

        public int QtyBesarAwal { get;  set; }
        public int QtyKecilAwal { get;  set; }
        public int QtyPcsAwal { get;  set; }
        
        public int QtyBesarAdjust { get;  set; }
        public int QtyKecilAdjust { get;  set; }
        public int QtyPcsAdjust { get; set; }
        
        public int QtyBesarOpname { get; set; }
        public int QtyKecilOpname { get; set; }
        public int QtyPcsOpname { get; set; }
        public string QtyOpnameInputStr { get; set; }
        
        public string StokOpId { get; set; }
        public int Conversion { get; set; }

        public decimal HppSatuan { get; set; }
        public decimal HppAwal {  get => HppSatuan * QtyPcsAwal; }
        public decimal HppOpname { get => HppSatuan * QtyPcsOpname; }
        public decimal HppAdjust {  get => HppSatuan * QtyPcsAdjust; }

    }

}
