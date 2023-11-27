using btr.application.FinanceContext.PiutangAgg.UseCases;
using btr.application.SalesContext.FakturAgg.UseCases;
using btr.application.SalesContext.FakturControlAgg;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace btr.distrib.SalesContext.FakturControlAgg
{
    [UsedImplicitly]
    public partial class FakturControlForm : Form
    {
        private readonly IListFaktorControlWorker _listFaktorControlWorker;
        private readonly IFakturControlStatusDal _fakturControlStatusDal;
        private readonly IFakturControlBuilder _builder;
        private readonly IFakturControlWriter _writer;
        
        private readonly IVoidFakturWorker _voidFakturWorker;
        private readonly IChangeToCashFakturWorker _changeToCashFakturWorker;
        private readonly IChangeToCreditFakturWorker _changeToCreditFakturWorker;
        private readonly IReactivateFakturWorker _reactivateFakturWorker;
        private readonly ICreatePiutangWorker _createPiutangWorker;

        private ContextMenu _gridContextMenu;

        private BindingList<FakturControlView> _listItem = new BindingList<FakturControlView>();
        public FakturControlForm(IListFaktorControlWorker listFaktorControlWorker,
            IFakturControlBuilder builder,
            IFakturControlWriter writer,
            IFakturControlStatusDal fakturControlStatusDal,
            IVoidFakturWorker voidFakturWorker,
            IChangeToCashFakturWorker changeToCashFakturWorker,
            IChangeToCreditFakturWorker changeToCreditFakturWorker,
            IReactivateFakturWorker reactivateFakturWorker,
            ICreatePiutangWorker createPiutangWorker)
        {
            _listFaktorControlWorker = listFaktorControlWorker;
            _fakturControlStatusDal = fakturControlStatusDal;
            _builder = builder;
            _writer = writer;

            InitializeComponent();
            InitGrid();
            InitContextMenu();
            RefreshGrid();
            RegisterEventHandler();
            _voidFakturWorker = voidFakturWorker;
            _changeToCashFakturWorker = changeToCashFakturWorker;
            _changeToCreditFakturWorker = changeToCreditFakturWorker;
            _reactivateFakturWorker = reactivateFakturWorker;
            _createPiutangWorker = createPiutangWorker;
        }

        private void RegisterEventHandler()
        {
            SearchText.KeyDown += SearchText_KeyDown;
            SearchButton.Click += SearchButton_Click;
            FakturGrid.CellContentClick += FakturGrid_CellContentClick;
            FakturGrid.MouseClick += FakturGrid_MouseClick;
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                RefreshGrid();
        }

        private void FakturGrid_MouseClick(object sender, MouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.Button == MouseButtons.Right)
            {
                _gridContextMenu.Show(grid, e.Location);
            }
        }

        private void InitContextMenu()
        {
            _gridContextMenu = new ContextMenu();
            _gridContextMenu.MenuItems.Add(new MenuItem("Edit Faktur", EditFaktur_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("-", Separator_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print All", PrintAll_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print Kembali", PrintKembali_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print Belum Kembali", PrintBelumKembali_OnClick));


            FakturGrid.ContextMenu = _gridContextMenu;
        }

        private void PrintBelumKembali_OnClick(object sender, EventArgs e)
        {
            var listPrint = _listItem
                .Where(x => x.Kembali == false)
                .ToList();
            PrintFakturControl(listPrint, "FAKTUR CONTROL - LIST BELUM KEMBALI");
        }

        private void PrintAll_OnClick(object sender, EventArgs e)
        {
            var listPrint = _listItem.ToList();
            PrintFakturControl(listPrint, "FAKTUR CONTROL - LIST ALL");
        }

        private void PrintKembali_OnClick(object sender, EventArgs e)
        {
            var listPrint = _listItem
            .Where(x => x.Kembali).ToList();
            PrintFakturControl(listPrint, "FAKTUR CONTROL - LIST KEMBALI");
        }

        private void PrintFakturControl(IEnumerable<FakturControlView> listData, string header)
        {
            var fetched = listData.ToList();
            var path = Path.GetTempPath();
            var filename = $"{path}\\FakturControl_{DateTime.Now:yymmdd_HHmmss}.xlsx";

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("FakturControl");
                var baris = 1;
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = header; // "FAKTUR CONTROL LIST";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{Tgl1Text.Value:dd MMMM yyyy} s/d {Tgl2Text.Value:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;
                baris++;

                var barisStart = baris;
                ws.Cell($"A{baris}").Value = "No";
                ws.Cell($"B{baris}").Value = "Tgl";
                ws.Cell($"C{baris}").Value = "FakturCode";
                ws.Cell($"D{baris}").Value = "Sales";
                ws.Cell($"E{baris}").Value = "Customer";
                ws.Cell($"F{baris}").Value = "Nilai Penjualan";
                baris++;

                int i = 1;
                foreach (var item in fetched)
                {
                    ws.Cell($"A{baris}").Value = i;
                    ws.Cell($"B{baris}").Value = item.FakturDate.Date;
                    ws.Cell($"C{baris}").Value = $"{item.FakturCode}";
                    ws.Cell($"D{baris}").Value = $"{item.SalesPersonName}";
                    ws.Cell($"E{baris}").Value = $"{item.CustomerName}";
                    ws.Cell($"F{baris}").Value = item.GrandTotal;
                    baris++;
                    i++;
                }
                ws.Cell($"E{baris}").Value = $"Grand Total";
                ws.Cell($"F{baris}").Value = fetched.Sum(x => x.GrandTotal);

                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"F{baris-1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{barisStart}"), ws.Cell($"F{barisStart}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Font.SetBold(true);
                ws.Range(ws.Cell($"F{barisStart + 1}"), ws.Cell($"F{baris}")).Style.NumberFormat.NumberFormatId = 3;

                ws.Range(ws.Cell($"E{baris}"), ws.Cell($"F{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Double)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                    .Font.SetBold(true);

                ws.Cell($"E{baris}").Value = $"Grand Total";
                ws.Cell($"F{baris}").Value = fetched.Sum(x => x.GrandTotal);

                ws.Column("A").Width = 4;
                ws.Column("B").Width = 10;
                ws.Column("C").Width = 10;
                ws.Column("D").Width = 10;
                ws.Column("E").Width = 35;
                ws.Column("F").Width = 15;
                workbook.SaveAs(filename);
            }
            System.Diagnostics.Process.Start(filename);

        }

        private void Separator_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditFaktur_OnClick(object sender, EventArgs e)
        {
            var grid = FakturGrid;
            if (grid.CurrentRow is null)
                return;

            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            var mainMenu = (MainForm)this.Parent.Parent;
            mainMenu.FakturButton_Click(null, null);
            var fakturForm = Application.OpenForms.OfType<FakturForm>().FirstOrDefault();
            fakturForm?.ShowFaktur(fakturKey.FakturId);
        }


        private void FakturGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            FakturGrid.EndEdit();
            var statusFaktur = StatusFakturEnum.Unknown;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "Posted": statusFaktur = StatusFakturEnum.Posted; break;
                case "Kirim": statusFaktur = StatusFakturEnum.Kirim; break;
                case "Kembali": statusFaktur = StatusFakturEnum.KembaliFaktur; break;
                case "Lunas": statusFaktur = StatusFakturEnum.Lunas; break;
                case "Pajak": statusFaktur = StatusFakturEnum.Pajak; break;
            }

            var isChecked = (bool)grid.CurrentCell.Value;
            var mainMenu = this.Parent.Parent;
            var user = ((MainForm)mainMenu).UserId;
            var faktur = new FakturModel(_listItem[e.RowIndex].FakturId);
            if (isChecked)
                FakturProses(faktur, statusFaktur, user);
            else
                FakturRollback(faktur, statusFaktur, user);
        }

        private void FakturProses(IFakturKey fakturKey, StatusFakturEnum statusFaktur, IUserKey userKey)
        {
            switch (statusFaktur)
            {
                //  re-gen stok
                case StatusFakturEnum.Posted:
                    var reactivateFakturRequest = new ReactivateFakturRequest(fakturKey.FakturId, userKey.UserId);
                    _reactivateFakturWorker.Execute(reactivateFakturRequest);
                    break;
                //  
                case StatusFakturEnum.Kirim:
                    var fakturControl = _builder
                        .LoadOrCreate(fakturKey)
                        .Kirim(userKey)
                        .Build();
                    _writer.Save(fakturControl);

                    break;
                case StatusFakturEnum.KembaliFaktur:
                    fakturControl = _builder
                        .LoadOrCreate(fakturKey)
                        .KembaliFaktur(userKey)
                        .Build();
                    _writer.Save(fakturControl);
                    _createPiutangWorker.Execute(fakturControl);
                    break;
                case StatusFakturEnum.Lunas:
                    var changeToCashReq = new ChangeToCashFakturRequest(fakturKey.FakturId, userKey.UserId);
                    _changeToCashFakturWorker.Execute(changeToCashReq);
                    break;
                case StatusFakturEnum.Pajak:
                    break;
                case StatusFakturEnum.Unknown:
                default:
                    break;
            }

        }

        private void FakturRollback(IFakturKey fakturKey, StatusFakturEnum statusFaktur, IUserKey userKey)
        {
            switch (statusFaktur)
            {
                //  void faktur
                case StatusFakturEnum.Posted:
                    var voidFakturRequest = new VoidFakturRequest(fakturKey.FakturId, userKey.UserId);
                    _voidFakturWorker.Execute(voidFakturRequest);
                    break;

                //  batal kirim
                case StatusFakturEnum.Kirim:
                    var faktur = _builder
                        .LoadOrCreate(fakturKey)
                        .CancelKirim()
                        .Build();
                    _writer.Save(faktur);
                    break;
                
                //  batal kembali
                case StatusFakturEnum.KembaliFaktur:
                    faktur = _builder
                        .LoadOrCreate(fakturKey)
                        .CancelKembaliFaktur()
                        .Build();
                    _writer.Save(faktur);
                    break;
                
                case StatusFakturEnum.Lunas:
                    var changeToCreditReq = new ChangeToCreditFakturRequest(fakturKey.FakturId, userKey.UserId);
                    _changeToCreditFakturWorker.Execute(changeToCreditReq);
                    break;
                case StatusFakturEnum.Pajak:
                    break;
                case StatusFakturEnum.Unknown:
                default:
                    break;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void InitGrid()
        {
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
            FakturGrid.Columns.SetDefaultCellStyle(System.Drawing.Color.Beige);
            FakturGrid.Columns.GetCol("FakturId").Visible = false;
            FakturGrid.Columns.GetCol("FakturDate").DefaultCellStyle.Format = "ddd dd MMM yyyy";

            FakturGrid.Columns.GetCol("FakturId").Width = 80;
            FakturGrid.Columns.GetCol("FakturCode").Width = 80;
            FakturGrid.Columns.GetCol("FakturDate").Width = 100;
            FakturGrid.Columns.GetCol("CustomerName").Width = 150;
            FakturGrid.Columns.GetCol("Npwp").Width = 100;
            FakturGrid.Columns.GetCol("SalesPersonName").Width = 80;

            FakturGrid.Columns.GetCol("GrandTotal").Width = 80;
            FakturGrid.Columns.GetCol("Bayar").Width = 80;
            FakturGrid.Columns.GetCol("Sisa").Width = 80;

            FakturGrid.Columns.GetCol("Posted").Width = 50;
            FakturGrid.Columns.GetCol("Kirim").Width = 50;
            FakturGrid.Columns.GetCol("Kirim").Visible  = false;

            FakturGrid.Columns.GetCol("Kembali").Width = 50;
            FakturGrid.Columns.GetCol("Lunas").Width = 50;
            FakturGrid.Columns.GetCol("Pajak").Width = 50;
            FakturGrid.Columns.GetCol("NoFakturPajak").Width = 80;
            FakturGrid.Columns.GetCol("UserId").Width = 80;
            FakturGrid.RowHeadersWidth = 55;

            FakturGrid.Columns.GetCol("GrandTotal").DefaultCellStyle.BackColor = System.Drawing.Color.PaleTurquoise;
            FakturGrid.Columns.GetCol("Bayar").DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
            FakturGrid.Columns.GetCol("Sisa").DefaultCellStyle.BackColor = System.Drawing.Color.PaleTurquoise;

            FakturGrid.Columns.GetCol("FakturDate").HeaderText = @"Tgl";
            FakturGrid.Columns.GetCol("CustomerName").HeaderText = @"Customer";
            FakturGrid.Columns.GetCol("Npwp").HeaderText = @"NPWP";
            FakturGrid.Columns.GetCol("SalesPersonName").HeaderText = @"Sales";
            FakturGrid.Columns.GetCol("NoFakturPajak").HeaderText = @"Faktur Pajak";
            FakturGrid.Columns.GetCol("UserId").HeaderText = @"Admin";
        }

        private void RefreshGrid()
        {
            var periode = new Periode(Tgl1Text.Value, Tgl2Text.Value);
            var diffDate = periode.Tgl2 - periode.Tgl1;
            if (diffDate.Days > 31)
            {
                MessageBox.Show(@"Periode max 31 hari");
                return;
            }    

            var listData = _listFaktorControlWorker.Execute(periode);
            if (SearchText.Text.Length > 0)
                listData = FilterFaktur(listData, SearchText.Text);

            var listTemp = listData.Select(x => x.Adapt<FakturControlView>()).ToList();
            _listItem = new BindingList<FakturControlView>(listTemp);

            var listStatus = _fakturControlStatusDal.ListData(periode)?.ToList() ?? new List<FakturControlStatusModel>();
            foreach(var item in _listItem)
            {
                item.Posted = listStatus
                    .Where(x => x.FakturId == item.FakturId)
                    .FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Posted) != null;

                item.Kirim = listStatus
                    .Where(x => x.FakturId == item.FakturId)
                    .FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Kirim) != null;
                item.Kembali = listStatus
                    .Where(x => x.FakturId == item.FakturId)
                    .FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.KembaliFaktur) != null;
                item.FormatFakturCode();

            }

            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
            foreach (DataGridViewRow row in FakturGrid.Rows)
                row.HeaderCell.Value = $"{(row.Index + 1):N0}";
            
        }

        private IEnumerable<FakturControlModel> FilterFaktur(IEnumerable<FakturControlModel> listFaktur, string keyword)
        {
            var listData = listFaktur.ToList();
            // if keyword 2nd char is '-', then search by faktur code
            if (keyword.Length > 1 && keyword[1] == '-')
            {
                return SearchByFakturCode(keyword, listData);
            }
            var listFakturCode = listData.Where(x => x.FakturCode.ToLower().StartsWith(keyword.ToLower())).ToList();



            var listCustomerName = listData.Where(x => x.CustomerName.ToLower().ContainMultiWord(SearchText.Text.ToLower())).ToList();
            var listCustomerId = listData.Where(x => x.CustomerCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listSales = listData.Where(x => x.SalesPersonName.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listAdmin = listData.Where(x => string.Equals(x.UserId, keyword, StringComparison.CurrentCultureIgnoreCase));
            var result = listCustomerName
                .Concat(listCustomerId)
                .Concat(listFakturCode)
                .Concat(listSales)
                .Concat(listAdmin)
                .OrderBy(x => x.FakturDate);

            return result.ToList();
        }

        private static IEnumerable<FakturControlModel> SearchByFakturCode(string keyword, IEnumerable<FakturControlModel> listData)
        {
            var fetched = listData.ToList();
            keyword = keyword.ToUpper();

            var noUrut = keyword.Substring(2);
            noUrut = noUrut.Length <= 4 
                ? noUrut.PadLeft(4, '0') 
                : keyword.Substring(keyword.Length - 4);
            
            var result = fetched
                .Where(x => x.FakturCode[0] == keyword[0])
                .Where(x => x.FakturCode.Substring(4,4) == noUrut)
                .ToList();
            
            return result;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            SearchText.Clear();
            RefreshGrid();
        }
    }
}
