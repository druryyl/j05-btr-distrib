using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.ReturBeliAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SupportContext.ParamSistemAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.application.SupportContext.UserAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.PurchaseContext.ReturBeliAgg;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using Mapster;
using Microsoft.Reporting.WinForms;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.PurchaseContext.ReturBeliFeature
{
    public partial class ReturBeliForm : Form
    {
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<ReturBeliBrowserView> _returBeliBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;
        private readonly ITglJamDal _dateTime;
        private readonly IParamSistemDal _paramSistemDal;
        private readonly IUserDal _userDal;

        private readonly ICreateReturBeliItemWorker _createItemWorker;
        private readonly ISaveReturBeliWorker _saveReturBeliWorker;
        private readonly IVoidReturBeliWorker _voidReturBeliWorker;
        private readonly IGenStokReturBeliWorker _genStokReturBeliWorker;

        private readonly IBrgBuilder _brgBuilder;
        private readonly IReturBeliBuilder _returBeliBuilder;

        private readonly BindingList<ReturBeliItemDto> _listItem = new BindingList<ReturBeliItemDto>();
        private decimal _ppnProsen;
        private decimal _dppProsen;

        public ReturBeliForm(IBrowser<SupplierBrowserView> supplierBrowser, IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser, IBrowser<ReturBeliBrowserView> returBeliBrowser, ISupplierDal supplierDal,
            IWarehouseDal warehouseDal, IBrgDal brgDal, ITglJamDal dateTime, IParamSistemDal paramSistemDal, IUserDal userDal,
            ICreateReturBeliItemWorker createItemWorker, ISaveReturBeliWorker saveReturBeliWorker, IVoidReturBeliWorker voidReturBeliWorker,
            IBrgBuilder brgBuilder, IReturBeliBuilder returBeliBuilder, IGenStokReturBeliWorker genStokReturBeliWorker)
        {
            InitializeComponent();
            _supplierBrowser = supplierBrowser;
            _warehouseBrowser = warehouseBrowser;
            _brgStokBrowser = brgStokBrowser;
            _returBeliBrowser = returBeliBrowser;
            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;
            _dateTime = dateTime;
            _paramSistemDal = paramSistemDal;
            _userDal = userDal;
            _createItemWorker = createItemWorker;
            _saveReturBeliWorker = saveReturBeliWorker;
            _voidReturBeliWorker = voidReturBeliWorker;
            _brgBuilder = brgBuilder;
            _returBeliBuilder = returBeliBuilder;

            RegisterEventHandler();
            InitGrid();
            InitParamSistem();
            ClearForm();
            _genStokReturBeliWorker = genStokReturBeliWorker;
        }

        private void InitParamSistem()
        {
            var paramKey = new ParamSistemModel("SISTEM_PPN_PROSEN");
            var paramPpn = _paramSistemDal.GetData(paramKey).ParamValue ?? "0";
            _ppnProsen = Convert.ToDecimal(paramPpn);

            paramKey = new ParamSistemModel("SISTEM_DPP_PROSEN");
            var paramDpp = _paramSistemDal.GetData(paramKey).ParamValue ?? "0";
            _dppProsen = Convert.ToDecimal(paramDpp);
        }

        private void RegisterEventHandler()
        {
            InvoiceButton.Click += ReturBeliButton_Click;
            SupplierButton.Click += SupplierButton_Click;
            SupplierIdText.Validated += SupplierIdText_Validated;

            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.Validated += WarehouseIdText_Validated;

            ReturBeliItemGrid.CellContentClick += ReturBeliItemGrid_CellContentClick;
            ReturBeliItemGrid.CellValueChanged += ReturBeliItemGrid_CellValueChanged;
            ReturBeliItemGrid.CellValidating += ReturBeliItemGrid_CellValidating;
            ReturBeliItemGrid.KeyDown += ReturBeliItemGrid_KeyDown;
            ReturBeliItemGrid.EditingControlShowing += ReturBeliItemGrid_EditingControlShowing;
            ReturBeliItemGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;

            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            NewButton.Click += NewButton_Click;
            PrintButton.Click += PrintButton_Click;
        }

        private void PerbaikanStokButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            PrintReturBeliRdlc(ReturBeliIdText.Text);
        }
        private void PrintReturBeliRdlc(string returBeliId)
        {
            var returBeli = _returBeliBuilder.Load(new ReturBeliModel(returBeliId)).Build();
            var supplier = _supplierDal.GetData(returBeli);
            var user = _userDal.GetData(returBeli) ?? new UserModel
            {
                UserId = returBeli.UserId,
                UserName = returBeli.UserId
            };
            var returBeliPrintOut = new ReturBeliPrintOutDto(returBeli, supplier, user);

            var returBeliDataset = new ReportDataSource("ReturBeliBeliDataset", new List<ReturBeliPrintOutDto> { returBeliPrintOut });
            var returBeliItemDataset = new ReportDataSource("ReturBeliBeliItemDataset", returBeliPrintOut.ListItem);
            var clientId = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ID"))?.ParamValue ?? string.Empty;

            var printOutTemplate = string.Empty;
            switch (clientId)
            {
                case "BTR-YK":
                    printOutTemplate = "ReturBeliPrintOut-Yk";
                    break;
                case "BTR-MGL":
                    printOutTemplate = "ReturBeliPrintOut-Mgl";
                    break;
                default:
                    break;
            }

            var listDataset = new List<ReportDataSource>
            {
                returBeliDataset,
                returBeliItemDataset
            };
            var rdlcViewerForm = new RdlcViewerForm();
            rdlcViewerForm.SetReportData(printOutTemplate, listDataset);
            rdlcViewerForm.ShowDialog();
        }
        private static string GetPrinterName()
        {
            string defaultPrinterName;

            try
            {
                var printDocument = new PrintDocument();
                defaultPrinterName = "Printer : " + printDocument.PrinterSettings.PrinterName;
            }
            catch (Exception ex)
            {
                defaultPrinterName = "Printer Error : " + ex.Message;
            }
            return defaultPrinterName;
        }

        private void ReturBeliItemGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex != grid.Columns.GetCol("BrgId").Index) return;

            var x = _listItem[e.RowIndex].BrgId;
            if (grid.CurrentRow == null) return;

            var y = grid.CurrentRow.Cells["BrgId"].Value?.ToString() ?? string.Empty;
            if (x != y)
                _listItem[e.RowIndex].HrgInputStr = string.Empty;
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ReturBeliButton_Click(object sender, EventArgs e)
        {
            _returBeliBrowser.Filter.Date = new Periode(_dateTime.Now);

            ReturBeliIdText.Text = _returBeliBrowser.Browse(ReturBeliIdText.Text);
            LoadReturBeli();
        }
        private void LoadReturBeli()
        {
            var textbox = ReturBeliIdText;
            var policy = Policy<ReturBeliModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as ReturBeliModel, (r, c) =>
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var returBeli = policy.Execute(() => _returBeliBuilder
                .Load(new ReturBeliModel(textbox.Text))
                .Build());
            if (returBeli is null) return;
            if (returBeli.IsVoid)
            {
                MessageBox.Show("ReturBeli sudah dihapus");
                return;
            }

            returBeli.RemoveNull();
            SupplierNameText.Text = returBeli.SupplierName;
            SupplierIdText.Text = returBeli.SupplierId;

            ReturBeliDateText.Value = returBeli.ReturBeliDate;
            ReturBeliCodeText.Text = returBeli.ReturBeliCode;
            WarehouseIdText.Text = returBeli.WarehouseId;
            WarehouseNameText.Text = returBeli.WarehouseName;
            TotalText.Value = returBeli.Total;
            DiscountText.Value = returBeli.Disc;
            TaxText.Value = returBeli.Tax;
            GrandTotalText.Value = returBeli.GrandTotal;

            _listItem.Clear();
            foreach (var newItem in returBeli.ListItem
                .OrderBy(x => x.NoUrut)
                .Select(item => item.Adapt<ReturBeliItemDto>()))
            {
                _listItem.Add(newItem);
                ValidateRow(_listItem.Count - 1, true);
            }

            if (returBeli.IsVoid)
                ShowAsVoid(returBeli);
            else
                ShowAsActive();
            CalcTotal();
        }

        #region SUPPLIER
        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var supplier = _supplierDal.GetData(new SupplierModel(textbox.Text));
            SupplierNameText.Text = supplier?.SupplierName ?? string.Empty;
        }

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            _supplierBrowser.Filter.UserKeyword = SupplierIdText.Text;
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }
        #endregion

        #region WAREHOUSE
        private void WarehouseIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var warehouse = _warehouseDal.GetData(new WarehouseModel(textbox.Text));
            WarehouseNameText.Text = warehouse?.WarehouseName ?? string.Empty;
        }

        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            _warehouseBrowser.Filter.UserKeyword = WarehouseIdText.Text;
            WarehouseIdText.Text = _warehouseBrowser.Browse(WarehouseIdText.Text);
            WarehouseIdText_Validated(WarehouseIdText, null);
        }
        #endregion

        #region GRID
        private void ReturBeliItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex != grid.Columns["Find"].Index)
                return;

            BrowseBrg(e.RowIndex);
        }

        private void ReturBeliItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "BrgId":
                    if (grid.CurrentCell.Value is null)
                        return;
                    ValidateRow(e.RowIndex, true);
                    break;
                case "QtyInputStr":
                case "DiscInputStr":
                case "HrgInputStr":
                    if (grid.CurrentCell.Value is null)
                        return;
                    ValidateRow(e.RowIndex, false);
                    break;
            }

            CalcTotal();
        }

        private void ReturBeliItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.KeyCode == Keys.F1)
            {
                if (grid.CurrentCell.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                    BrowseBrg(grid.CurrentCell.RowIndex);
            }

            if (e.KeyCode == Keys.Delete)
            {
                _listItem.RemoveAt(grid.CurrentCell.RowIndex);
                grid.Refresh();
            }
        }

        #region browse-brg-saat-cell-aktif
        private void ReturBeliItemGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex != 1)
                return;

            if (e.Control is TextBox textBox)
            {
                textBox.KeyDown -= TextBox_KeyDown;
                textBox.KeyDown += TextBox_KeyDown;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                ReturBeliItemGrid.EndEdit();
                BrowseBrg(ReturBeliItemGrid.CurrentCell.RowIndex);
            }
        }
        #endregion

        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId;
            _brgStokBrowser.Filter.StaticFilter1 = WarehouseIdText.Text;
            _brgStokBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            var brgIdPilih = _brgStokBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgIdPilih;

            if (brgId != brgIdPilih)
                ValidateRow(rowIndex, true);
        }

        private void ValidateRow(int rowIndex, bool isGetHarga)
        {
            var brg = BuildBrg(rowIndex);
            if (brg == null)
            {
                return;
            }

            var req = new CreateReturBeliItemRequest(
                _listItem[rowIndex].BrgId,
                _listItem[rowIndex].HrgInputStr,
                _listItem[rowIndex].QtyInputStr,
                _listItem[rowIndex].DiscInputStr,
                _listItem[rowIndex].DppProsen == 0 ? _dppProsen : _listItem[rowIndex].DppProsen,
                _listItem[rowIndex].PpnProsen == 0 ? _ppnProsen : _listItem[rowIndex].PpnProsen,
                isGetHarga);
            var item = _createItemWorker.Execute(req);
            _listItem[rowIndex] = item.Adapt<ReturBeliItemDto>();
            ReturBeliItemGrid.Refresh();
            CalcTotal();
        }

        private BrgModel BuildBrg(int rowIndex)
        {
            var id = _listItem[rowIndex].BrgId ?? string.Empty;
            if (id.Length == 0)
                return null;

            var brgKey = new BrgModel(id);
            var fbk = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as BrgModel);
            var brg = fbk.Execute(() => _brgBuilder.Load(brgKey).Build());

            if (brg is null)
            {
                brg = GetBrgByCode(id);
                _listItem[rowIndex].BrgId = brg?.BrgId ?? string.Empty;
                ReturBeliItemGrid.Refresh();
            }

            return brg;
        }

        private BrgModel GetBrgByCode(string id)
        {
            var result = _brgDal.GetData(id);
            if (result is null) return null;

            result = _brgBuilder.Load(result).Build();
            return result;
        }

        private void InitGrid()
        {
            var binding = new BindingSource
            {
                DataSource = _listItem
            };
            ReturBeliItemGrid.DataSource = binding;
            ReturBeliItemGrid.Refresh();
            ReturBeliItemGrid.Columns.SetDefaultCellStyle(Color.Beige);

            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            ReturBeliItemGrid.Columns.Insert(1, buttonCol);

            var cols = ReturBeliItemGrid.Columns;
            cols.GetCol("BrgId").Visible = true;
            cols.GetCol("BrgId").Width = 50;

            cols.GetCol("Find").Width = 20;

            cols.GetCol("BrgCode").Visible = true;
            cols.GetCol("BrgCode").Width = 80;

            cols.GetCol("BrgName").Visible = true;
            cols.GetCol("BrgName").Width = 160;
            cols.GetCol("BrgName").DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            cols.GetCol("HrgInputStr").Visible = true;
            cols.GetCol("HrgInputStr").Width = 130;
            cols.GetCol("HrgInputStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HrgInputStr").HeaderText = @"Hrg Beli";
            cols.GetCol("HrgInputStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("HrgDetilStr").Visible = false;
            cols.GetCol("HrgDetilStr").Width = 110;
            cols.GetCol("HrgDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("HrgDetilStr").HeaderText = @"Detil Harga";
            cols.GetCol("HrgDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;


            cols.GetCol("QtyInputStr").Visible = true;
            cols.GetCol("QtyInputStr").Width = 50;
            cols.GetCol("QtyInputStr").HeaderText = @"Qty";

            cols.GetCol("QtyDetilStr").Visible = true;
            cols.GetCol("QtyDetilStr").Width = 80;
            cols.GetCol("QtyDetilStr").HeaderText = @"Qty Desc";
            cols.GetCol("QtyDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("QtyDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            cols.GetCol("QtyBesar").Visible = false;
            cols.GetCol("SatBesar").Visible = false;
            cols.GetCol("Conversion").Visible = false;
            cols.GetCol("HppSatBesar").Visible = false;

            cols.GetCol("QtyKecil").Visible = false;
            cols.GetCol("SatKecil").Visible = false;
            cols.GetCol("HppSatKecil").Visible = false;

            cols.GetCol("QtyBeli").Visible = false;
            cols.GetCol("HppSat").Visible = false;

            cols.GetCol("SubTotal").Visible = true;
            cols.GetCol("SubTotal").Width = 70;

            cols.GetCol("QtyBonus").Visible = false;
            cols.GetCol("QtyPotStok").Visible = false;

            cols.GetCol("DiscInputStr").Visible = true;
            cols.GetCol("DiscInputStr").Width = 65;
            cols.GetCol("DiscInputStr").HeaderText = @"Disc";

            cols.GetCol("DiscDetilStr").Visible = true;
            cols.GetCol("DiscDetilStr").Width = 90;
            cols.GetCol("DiscDetilStr").HeaderText = @"Disc Rp";
            cols.GetCol("DiscDetilStr").DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            cols.GetCol("DiscDetilStr").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            cols.GetCol("DiscRp").Visible = false;

            cols.GetCol("DppProsen").Visible = false;
            cols.GetCol("DppRp").Visible = false;

            cols.GetCol("PpnProsen").Visible = true;
            cols.GetCol("PpnProsen").Width = 50;
            cols.GetCol("PpnProsen").HeaderText = @"Ppn";

            cols.GetCol("PpnRp").Visible = false;

            cols.GetCol("Total").Visible = true;
            cols.GetCol("Total").Width = 80;

            //  auto-resize-rows
            ReturBeliItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ReturBeliItemGrid.AutoResizeRows();
        }

        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            DiscountText.Value = _listItem.Sum(x => x.DiscRp);
            DppText.Value = _listItem.Sum(x => x.DppRp);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
        }

        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var result = SaveReturBeli();
            _genStokReturBeliWorker.Execute(new GenStokReturBeliRequest(result.ReturBeliId));

            PrintReturBeliRdlc(result.ReturBeliId);

            ClearForm();
        }

        private ReturBeliModel SaveReturBeli()
        {
            var mainform = (MainForm)this.Parent.Parent;
            var cmd = new SaveReturBeliRequest
            {
                ReturBeliId = ReturBeliIdText.Text,
                ReturBeliDate = ReturBeliDateText.Value.ToString("yyyy-MM-dd"),
                ReturBeliCode = ReturBeliCodeText.Text,
                SupplierId = SupplierIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                UserId = mainform.UserId.UserId,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName?.Length > 0
                select new SaveReturBeliRequestItem
                {
                    BrgId = c.BrgId,
                    HrgInputStr = c.HrgInputStr,
                    QtyString = c.QtyInputStr,
                    DiscountString = c.DiscInputStr,
                    DppProsen = c.DppProsen,
                    PpnProsen = c.PpnProsen,
                }).ToList();
            cmd.ListBrg = listItem;
            var result = _saveReturBeliWorker.Execute(cmd);
            LastIdLabel.Text = result.ReturBeliId;
            return result;
        }

        private void ClearForm()
        {
            ReturBeliIdText.Text = string.Empty;
            ReturBeliDateText.Value = _dateTime.Now;
            ReturBeliCodeText.Text = string.Empty;

            SupplierIdText.Text = string.Empty;
            SupplierNameText.Text = string.Empty;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;

            TotalText.Value = 0;
            DiscountText.Value = 0;
            DppText.Value = 0;
            TaxText.Value = 0;

            _listItem.Clear();
            var newItem = new ReturBeliItemDto();
            newItem.SetPpnProsen(_ppnProsen);
            _listItem.Add(newItem);

            ShowAsActive();
        }

        private void ShowAsVoid(ReturBeliModel returBeli)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            CancelLabel.Text = $@"ReturBeli sudah DIBATALKAN \noleh {returBeli.UserIdVoid} \npada {returBeli.VoidDate:ddd, dd MMM yyyy}";
            VoidPanel.Visible = true;
            SaveButton.Visible = false;
        }

        private void ShowAsActive()
        {
            this.BackColor = Color.DarkSeaGreen;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.Honeydew;

            VoidPanel.Visible = false;
            SaveButton.Visible = true;
        }
        #endregion

        #region DELETE
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ReturBeliIdText.Text == string.Empty)
                return;

            if (MessageBox.Show("Delete ReturBeli?", "ReturBeli", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Delete();
        }

        private void Delete()
        {
            var mainMenu = this.Parent.Parent;
            var user = ((MainForm)mainMenu).UserId;
            var req = new VoidReturBeliRequest(ReturBeliIdText.Text, user.UserId);
            _voidReturBeliWorker.Execute(req);

            ClearForm();
        }
        #endregion

    }
}
