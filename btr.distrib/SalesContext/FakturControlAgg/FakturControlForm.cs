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
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SupportContext.ParamSistemAgg;
using Microsoft.Reporting.WinForms;
using btr.application.SupportContext.ParamSistemAgg;
using btr.application.SupportContext.UserAgg;
using System.Drawing;

namespace btr.distrib.SalesContext.FakturControlAgg
{
    [UsedImplicitly]
    public partial class FakturControlForm : Form
    {
        private readonly IListFaktorControlWorker _listFaktorControlWorker;
        private readonly IFakturControlStatusDal _fakturControlStatusDal;
        private readonly IFakturControlBuilder _builder;
        private readonly IFakturControlWriter _writer;
        private readonly IFakturBuilder _fakturBuilder;

        private readonly IVoidFakturWorker _voidFakturWorker;
        private readonly IReactivateFakturWorker _reactivateFakturWorker;
        private readonly IPiutangDal _piutangDal;
        private readonly ICustomerDal _customerDal;
        private readonly IParamSistemDal _paramSistemDal;
        private readonly IUserDal _userDal;

        private ContextMenu _gridContextMenu;

        private BindingList<FakturControlView> _listItem = new BindingList<FakturControlView>();
        public FakturControlForm(IListFaktorControlWorker listFaktorControlWorker,
            IFakturControlBuilder builder,
            IFakturControlWriter writer,
            IFakturControlStatusDal fakturControlStatusDal,
            IVoidFakturWorker voidFakturWorker,
            IReactivateFakturWorker reactivateFakturWorker,
            IPiutangDal piutangDal,
            IFakturBuilder fakturBuilder,
            ICustomerDal customerDal,
            IParamSistemDal paramSistemDal,
            IUserDal userDal)
        {
            InitializeComponent();

            _listFaktorControlWorker = listFaktorControlWorker;
            _fakturControlStatusDal = fakturControlStatusDal;
            _builder = builder;
            _writer = writer;

            _voidFakturWorker = voidFakturWorker;
            _reactivateFakturWorker = reactivateFakturWorker;
            _piutangDal = piutangDal;
            _fakturBuilder = fakturBuilder;
            _customerDal = customerDal;
            _paramSistemDal = paramSistemDal;
            _userDal = userDal;

            InitGrid();
            InitContextMenu();
            RefreshGrid();
            RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            SearchText.KeyDown += SearchText_KeyDown;
            SearchButton.Click += SearchButton_Click;
            ClearButton.Click += ClearButton_Click;
            PrintFakturJualButton.Click += PrintFakturButton_Click;
            PrintFakturKlaimButton.Click += PrintFakturKlaimButton_Click;

            FakturGrid.CellContentClick += FakturGrid_CellContentClick;
            FakturGrid.MouseClick += FakturGrid_MouseClick;
        }

        private void PrintFakturKlaimButton_Click(object sender, EventArgs e)
        {
            var fakturId = FakturGrid.CurrentRow.Cells["FakturId"].Value.ToString();
            PrintFaktur(fakturId, true);
        }

        private void PrintFakturButton_Click(object sender, EventArgs e)
        {
            var fakturId = FakturGrid.CurrentRow.Cells["FakturId"].Value.ToString();
            PrintFaktur(fakturId, false);
        }

        private void PrintFaktur(string fakturId, bool isKlaim)
        {
            var fakturKey = new FakturModel(fakturId);
            var faktur = _fakturBuilder.Load(fakturKey).Build();
            var customer = _customerDal.GetData(faktur);
            var user = _userDal.GetData(faktur) ?? new UserModel
            {
                UserId = faktur.UserId,
                UserName = faktur.UserId
            };
            var fakturPrintout = new FakturPrintOutDto(faktur, customer, user, isKlaim);

            var fakturJualDataset = new ReportDataSource("FakturJualDataset", new List<FakturPrintOutDto> { fakturPrintout });
            var fakturJualItemDataset = new ReportDataSource("FakturJualItemDataset", fakturPrintout.ListItem);
            var clientId = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ID"))?.ParamValue ?? string.Empty;

            var printOutTemplate = string.Empty;
            switch (clientId)
            {
                case "BTR-YK":
                    printOutTemplate = "FakturPrintOut-Yk";
                    break;
                case "BTR-MGL":
                    printOutTemplate = "FakturPrintOut-Mgl";
                    break;
                default:
                    break;
            }

            var listDataset = new List<ReportDataSource>
            {
                fakturJualDataset,
                fakturJualItemDataset
            };
            var rdlcViewerForm = new RdlcViewerForm();
            rdlcViewerForm.SetReportData(printOutTemplate, listDataset);
            rdlcViewerForm.ShowDialog();
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
            _gridContextMenu.MenuItems.Add(new MenuItem("Create Klaim", CreateKlaim_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("-", Separator_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print All", PrintAll_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print Kembali", PrintKembali_OnClick));
            _gridContextMenu.MenuItems.Add(new MenuItem("Print Belum Kembali", PrintBelumKembali_OnClick));

            FakturGrid.ContextMenu = _gridContextMenu;
        }

        private void CreateKlaim_OnClick(object sender, EventArgs e)
        {
            var grid = FakturGrid;
            if (grid.CurrentRow is null)
                return;
            var fakturKey = new FakturModel(grid.CurrentRow.Cells["FakturId"].Value.ToString());
            var mainMenu = (MainForm)this.Parent.Parent;
            mainMenu.ST1FakturButton_Click(null, null);
            var fakturForm = Application.OpenForms.OfType<FakturForm>().FirstOrDefault();
            fakturForm?.ShowFaktur(fakturKey.FakturId);
            fakturForm?.ShowKlaim();
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

            var clientName = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_NAME"))?.ParamValue ?? "CLIENT_NAME";
            var clientAddress = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ADDRESS"))?.ParamValue ?? "CLIENT_ADDRESS";

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("FakturControl");
                var baris = 1;
                ws.Cell($"A{baris}").Value = clientName;
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"F{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = clientAddress;
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
            var fakturControl = _fakturControlStatusDal.ListData(fakturKey).ToList();
            if (fakturControl.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.KembaliFaktur) != null)
            {
                MessageBox.Show(@"Faktur sudah kembali, tidak bisa di edit");
                return;
            }
            if (fakturControl.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Lunas) != null)
            {
                MessageBox.Show(@"Faktur sudah lunas, tidak bisa di edit");
                return;
            }

            var mainMenu = (MainForm)this.Parent.Parent;
            mainMenu.ST1FakturButton_Click(null, null);
            var fakturForm = Application.OpenForms.OfType<FakturForm>().FirstOrDefault();
            fakturForm?.ShowFaktur(fakturKey.FakturId);
        }


        private void FakturGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            if (grid.Columns[e.ColumnIndex].Name == "Posted" 
                || grid.Columns[e.ColumnIndex].Name == "Kembali")
            {
                var isReturn = IsReturnAfterCekRulePostedKembali(e.RowIndex, e.ColumnIndex);
                if (isReturn)
                    return;
            }

            FakturGrid.EndEdit();
            var statusFaktur = StatusFakturEnum.Unknown;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "Posted": 
                    statusFaktur = StatusFakturEnum.Posted; 
                    break;
                case "Kirim": 
                    statusFaktur = StatusFakturEnum.Kirim; 
                    break;
                case "Kembali": 
                    statusFaktur = StatusFakturEnum.KembaliFaktur; 
                    break;
                case "Lunas": 
                    statusFaktur = StatusFakturEnum.Lunas; 
                    break;
                case "Pajak": 
                    statusFaktur = StatusFakturEnum.Pajak; 
                    break;
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

        private bool IsReturnAfterCekRulePostedKembali(int rowIndex, int columnIndex)
        {
            var colPost = FakturGrid.Columns["Posted"].Index;
            var colKembali = FakturGrid.Columns["Kembali"].Index;
            var isPosted = _listItem[rowIndex].Posted;
            var isKembali = _listItem[rowIndex].Kembali;

            //  Un-Post: Tidak boleh jika sudah Kembali
            if (columnIndex == colPost)
                if (isKembali && isPosted)
                {
                    MessageBox.Show("Faktur sudah kembali, tidak boleh batalt");
                    FakturGrid.CancelEdit();
                    return true; 
                }
            if (columnIndex == colPost)
                if (isPosted)
                {
                    if (MessageBox.Show("Batal Faktur?", "Control", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        FakturGrid.CancelEdit();
                        return true;
                    }
                }

            //  Un-Kembali
            if (columnIndex == colKembali)
                if (isKembali)
                {
                    if (MessageBox.Show("Batal Kembali?", "Control", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        FakturGrid.CancelEdit();
                        return true;
                    }
                }

            return false;
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
                    break;
                case StatusFakturEnum.Lunas:
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
                    //var changeToCreditReq = new ChangeToCreditFakturRequest(fakturKey.FakturId, userKey.UserId);
                    //_changeToCreditFakturWorker.Execute(changeToCreditReq);
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
            var binding = new BindingSource
            {
                DataSource = _listItem
            };
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
            FakturGrid.Columns.SetDefaultCellStyle(Color.White);
            FakturGrid.Columns.GetCol("FakturId").Visible = false;
            FakturGrid.Columns.GetCol("IsHasKlaim").Visible = true;
            FakturGrid.Columns.GetCol("NoFakturPajak").Visible = false;
            FakturGrid.Columns.GetCol("FakturDate").DefaultCellStyle.Format = "ddd dd MMM yyyy";

            FakturGrid.Columns.GetCol("FakturId").Width = 80;
            FakturGrid.Columns.GetCol("FakturCode").Width = 70;
            FakturGrid.Columns.GetCol("FakturDate").Width = 100;
            FakturGrid.Columns.GetCol("CustomerName").Width = 130;
            FakturGrid.Columns.GetCol("Npwp").Width = 100;
            FakturGrid.Columns.GetCol("SalesPersonName").Width = 70;

            FakturGrid.Columns.GetCol("GrandTotal").Width = 80;
            FakturGrid.Columns.GetCol("Bayar").Width = 80;
            FakturGrid.Columns.GetCol("PotBiayaLain").Width = 80;
            FakturGrid.Columns.GetCol("Sisa").Width = 80;

            FakturGrid.Columns.GetCol("Posted").Width = 50;
            FakturGrid.Columns.GetCol("Kirim").Width = 50;
            FakturGrid.Columns.GetCol("Kirim").Visible  = false;

            FakturGrid.Columns.GetCol("Kembali").Width = 50;
            FakturGrid.Columns.GetCol("Lunas").Width = 50;
            FakturGrid.Columns.GetCol("Pajak").Width = 50;
            FakturGrid.Columns.GetCol("UserId").Width = 80;
            FakturGrid.Columns.GetCol("IsHasKlaim").Width = 50;

            FakturGrid.RowHeadersWidth = 55;

            FakturGrid.Columns.GetCol("GrandTotal").DefaultCellStyle.BackColor = Color.PaleTurquoise;
            FakturGrid.Columns.GetCol("Bayar").DefaultCellStyle.BackColor = Color.Pink;
            FakturGrid.Columns.GetCol("PotBiayaLain").DefaultCellStyle.BackColor = Color.Pink;
            FakturGrid.Columns.GetCol("Sisa").DefaultCellStyle.BackColor = Color.PaleTurquoise;

            FakturGrid.Columns.GetCol("FakturCode").HeaderText = @"Code";
            FakturGrid.Columns.GetCol("FakturDate").HeaderText = @"Tgl";
            FakturGrid.Columns.GetCol("CustomerName").HeaderText = @"Customer";
            FakturGrid.Columns.GetCol("Npwp").HeaderText = @"NPWP";
            FakturGrid.Columns.GetCol("SalesPersonName").HeaderText = @"Sales";
            FakturGrid.Columns.GetCol("UserId").HeaderText = @"Admin";
            FakturGrid.Columns.GetCol("PotBiayaLain").HeaderText = @"Pot/Biaya";
            FakturGrid.Columns.GetCol("IsHasKlaim").HeaderText = "Klaim";

            // conditional formatting row color; if lunas, then green
            FakturGrid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0)
                    return;
                var row = FakturGrid.Rows[e.RowIndex];
                var item = (FakturControlView)row.DataBoundItem;
                if (item.Kembali)
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                else
                    row.DefaultCellStyle.ForeColor = Color.Black;
                
                if (item.Lunas)
                    row.DefaultCellStyle.BackColor = Color.LemonChiffon;

                if (item.IsHasKlaim)
                    row.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            };

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

            var listStatusAll = _fakturControlStatusDal.ListData(periode)?.ToList() ?? new List<FakturControlStatusModel>();
            var listStatus = (
                from c in listStatusAll
                join d in listData on c.FakturId equals d.FakturId
                select c).ToList();

            var listPiutangKey = listStatus.Select(x => new PiutangModel(x.FakturId));
            var listPiutang = _piutangDal.ListData(listPiutangKey)?.ToList() ?? new List<PiutangModel>();
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
                item.SetPajak(item.NoFakturPajak.Trim().Length != 0);
                item.FormatFakturCode();

                var piutang = listPiutang.FirstOrDefault(x => x.PiutangId == item.FakturId);
                if (piutang is null)
                    continue;

                item.SetNilai(piutang.Total, piutang.Terbayar, piutang.Potongan);
                if (item.Sisa >= -1 && item.Sisa <=1)
                    item.SetLunas(true);
                //else
                //    item.SetLunas(false);

            }

            var binding = new BindingSource
            {
                DataSource = _listItem
            };
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
                .Union(listCustomerId)
                .Union(listFakturCode)
                .Union(listSales)
                .Union(listAdmin)
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
