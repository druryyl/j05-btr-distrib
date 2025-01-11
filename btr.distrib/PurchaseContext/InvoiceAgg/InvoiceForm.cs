using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using Mapster;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.distrib.PrintDocs;
using btr.application.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.ParamSistemAgg;

namespace btr.distrib.PurchaseContext.InvoiceAgg
{
    public partial class InvoiceForm : Form
    {
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<WarehouseBrowserView> _warehouseBrowser;
        private readonly IBrowser<BrgStokBrowserView> _brgStokBrowser;
        private readonly IBrowser<InvoiceBrowserView> _invoiceBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgDal _brgDal;
        private readonly ITglJamDal _dateTime;
        private readonly IParamSistemDal _paramSistemDal;

        private readonly ICreateInvoiceItemWorker _createItemWorker;
        private readonly ISaveInvoiceWorker _saveInvoiceWorker;
        private readonly IVoidInvoiceWorker _voidInvoiceWorker;

        private readonly IBrgBuilder _brgBuilder;
        private readonly IInvoiceBuilder _invoiceBuilder;
        private readonly IInvoicePrintDoc _invoicePrinter;
        private readonly IGenStokInvoiceWorker _genStokInvoiceWorker;

        private readonly BindingList<InvoiceItemDto> _listItem = new BindingList<InvoiceItemDto>();
        private decimal _ppnProsen;
        private decimal _dppProsen;


        public InvoiceForm(IBrowser<SupplierBrowserView> supplierBrowser,
            IBrowser<WarehouseBrowserView> warehouseBrowser,
            IBrowser<BrgStokBrowserView> brgStokBrowser,
            ISupplierDal supplierDal,
            IWarehouseDal warehouseDal,
            ICreateInvoiceItemWorker createItemWorker,
            IBrgBuilder brgBuilder,
            IBrgDal brgDal,
            ISaveInvoiceWorker saveInvoiceWorker,
            IInvoiceBuilder invoiceBuilder,
            ITglJamDal dateTime,
            IBrowser<InvoiceBrowserView> invoiceBrowser,
            IInvoicePrintDoc invoicePrinter, IGenStokInvoiceWorker genStokInvoiceWorker, IParamSistemDal paramSistemDal, IVoidInvoiceWorker voidInvoiceWorker)
        {
            InitializeComponent();

            _supplierBrowser = supplierBrowser;
            _warehouseBrowser = warehouseBrowser;

            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgDal = brgDal;

            _brgBuilder = brgBuilder;
            _createItemWorker = createItemWorker;
            _brgStokBrowser = brgStokBrowser;
            _saveInvoiceWorker = saveInvoiceWorker;
            _invoiceBuilder = invoiceBuilder;
            _dateTime = dateTime;
            _invoiceBrowser = invoiceBrowser;
            _invoicePrinter = invoicePrinter;
            _genStokInvoiceWorker = genStokInvoiceWorker;
            _paramSistemDal = paramSistemDal;

            RegisterEventHandler();
            InitGrid();
            InitParamSistem();
            ClearForm();
            _voidInvoiceWorker = voidInvoiceWorker;
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
            InvoiceButton.Click += InvoiceButton_Click;
            SupplierButton.Click += SupplierButton_Click;
            SupplierIdText.Validated += SupplierIdText_Validated;

            WarehouseButton.Click += WarehouseButton_Click;
            WarehouseIdText.Validated += WarehouseIdText_Validated;

            InvoiceItemGrid.CellContentClick += InvoiceItemGrid_CellContentClick;
            InvoiceItemGrid.CellValueChanged += InvoiceItemGrid_CellValueChanged;
            InvoiceItemGrid.CellValidated += InvoiceItemGrid_CellValidated;
            InvoiceItemGrid.CellValidating += InvoiceItemGrid_CellValidating; 
            InvoiceItemGrid.KeyDown += InvoiceItemGrid_KeyDown;
            InvoiceItemGrid.EditingControlShowing += InvoiceItemGrid_EditingControlShowing;

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
            PrintInvoice(InvoiceIdText.Text);
        }
        private void PrintInvoiceRdlc(string invoiceId)
        {
            var invoice = _invoiceBuilder.Load(new InvoiceModel(invoiceId)).Build();
            var supplier = _supplierDal.GetData(invoice);
            var invoicePrintOut = new InvoicePrintOutDto(invoice, supplier);
            var form = new InvoicePrintOutForm(invoicePrintOut);
            form.ShowDialog();
        }
        private void PrintInvoice(string invoiceId)
        {
            var invoice = _invoiceBuilder.Load(new InvoiceModel(invoiceId)).Build();
            _invoicePrinter.DefaultPrinter = GetPrinterName();
            _invoicePrinter.CreateDoc(invoice);
            _invoicePrinter.PrintDoc();
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

        private void InvoiceItemGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.CurrentCell.ColumnIndex != grid.Columns.GetCol("BrgId").Index) return;
            
            var x = _listItem[e.RowIndex].BrgId;
            if (grid.CurrentRow == null) return;
            
            var y = grid.CurrentRow.Cells["BrgId"].Value?.ToString()??string.Empty;
            if (x != y)
                _listItem[e.RowIndex].HrgInputStr = string.Empty;
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void InvoiceButton_Click(object sender, EventArgs e)
        {
            _invoiceBrowser.Filter.Date = new Periode(_dateTime.Now);

            InvoiceIdText.Text = _invoiceBrowser.Browse(InvoiceIdText.Text);
            LoadInvoice();
        }
        private void LoadInvoice()
        {
            var textbox = InvoiceIdText;
            var policy = Policy<InvoiceModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as InvoiceModel, (r, c) =>
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var invoice = policy.Execute(() => _invoiceBuilder
                .Load(new InvoiceModel(textbox.Text))
                .Build());
            if (invoice is null) return;
            if (invoice.IsVoid)
            {
                MessageBox.Show("Invoice sudah dihapus");
                return;
            }

            invoice.RemoveNull();
            SupplierNameText.Text = invoice.SupplierName;
            SupplierIdText.Text = invoice.SupplierId;

            InvoiceDateText.Value = invoice.InvoiceDate;
            InvoiceCodeText.Text = invoice.InvoiceCode;
            WarehouseIdText.Text = invoice.WarehouseId;
            WarehouseNameText.Text = invoice.WarehouseName;
            TermOfPaymentCombo.SelectedIndex = (int)invoice.TermOfPayment;
            DueDateText.Value = invoice.DueDate;
            TotalText.Value = invoice.Total;
            DiscountText.Value = invoice.Disc;
            TaxText.Value = invoice.Tax;
            GrandTotalText.Value = invoice.GrandTotal;
            UangMukaText.Value = invoice.UangMuka;
            SisaText.Value = invoice.KurangBayar;

            _listItem.Clear();
            foreach (var newItem in invoice.ListItem.Select(item => item.Adapt<InvoiceItemDto>()))
                _listItem.Add(newItem);

            if (invoice.IsVoid)
                ShowAsVoid(invoice);
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
        private void InvoiceItemGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex != grid.Columns["Find"].Index)
                return;

            BrowseBrg(e.RowIndex);
        }

        private void InvoiceItemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == grid.Columns.GetCol("BrgId").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("QtyInputStr").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("DiscInputStr").Index)
                CalcTotal();
            if (e.ColumnIndex == grid.Columns.GetCol("PpnProsen").Index)
                CalcTotal();
        }

        private void InvoiceItemGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "BrgId":
                case "QtyInputStr":
                case "HrgInputStr":
                case "DiscInputStr":
                    if (grid.CurrentCell.Value is null)
                        return;
                    ValidateRow(e.RowIndex);
                    break;
            }
        }

        private void InvoiceItemGrid_KeyDown(object sender, KeyEventArgs e)
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
        private void InvoiceItemGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                InvoiceItemGrid.EndEdit();
                BrowseBrg(InvoiceItemGrid.CurrentCell.RowIndex);
            }
        }
        #endregion

        private void BrowseBrg(int rowIndex)
        {
            var brgId = _listItem[rowIndex].BrgId;
            _brgStokBrowser.Filter.StaticFilter1 = WarehouseIdText.Text;
            _brgStokBrowser.Filter.UserKeyword = _listItem[rowIndex].BrgId;
            brgId = _brgStokBrowser.Browse(brgId);
            _listItem[rowIndex].BrgId = brgId;
            ValidateRow(rowIndex);
        }

        private void ValidateRow(int rowIndex)
        {
            var brg = BuildBrg(rowIndex);
            if (brg == null)
            {
                return;
            }

            var req = new CreateInvoiceItemRequest(
                _listItem[rowIndex].BrgId,
                _listItem[rowIndex].HrgInputStr,
                _listItem[rowIndex].QtyInputStr,
                _listItem[rowIndex].DiscInputStr,
                _listItem[rowIndex].DppProsen == 0 ? _dppProsen : _listItem[rowIndex].DppProsen,
                _listItem[rowIndex].PpnProsen == 0 ? _ppnProsen : _listItem[rowIndex].PpnProsen);
            var item = _createItemWorker.Execute(req);
            _listItem[rowIndex] = item.Adapt<InvoiceItemDto>();
            InvoiceItemGrid.Refresh();
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
                InvoiceItemGrid.Refresh();
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
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            InvoiceItemGrid.DataSource = binding;
            InvoiceItemGrid.Refresh();
            InvoiceItemGrid.Columns.SetDefaultCellStyle(Color.Beige);

            DataGridViewButtonColumn buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = @"Find", // Set the column header text
                Text = "...", // Set the button text
                Name = "Find" // Set the button text
            };
            buttonCol.DefaultCellStyle.BackColor = Color.Brown;
            InvoiceItemGrid.Columns.Insert(1, buttonCol);

            var cols = InvoiceItemGrid.Columns;
            cols.GetCol("BrgId").Visible = true;
            cols.GetCol("BrgId").Width = 50;

            cols.GetCol("Find").Width = 20;

            cols.GetCol("BrgCode").Visible = true;
            cols.GetCol("BrgCode").Width = 80;

            cols.GetCol("BrgName").Visible = true;
            cols.GetCol("BrgName").Width = 160;
            cols.GetCol("BrgName").DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            cols.GetCol("HrgInputStr").Visible = true;
            cols.GetCol("HrgInputStr").Width = 110;
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
            InvoiceItemGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            InvoiceItemGrid.AutoResizeRows();
        }

        private void CalcTotal()
        {
            TotalText.Value = _listItem.Sum(x => x.SubTotal);
            DiscountText.Value = _listItem.Sum(x => x.DiscRp);
            DppText.Value = _listItem.Sum(x => x.DppRp);
            TaxText.Value = _listItem.Sum(x => x.PpnRp);
            GrandTotalText.Value = _listItem.Sum(x => x.Total);
            SisaText.Value = GrandTotalText.Value - UangMukaText.Value;
        }

        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            //  simpan kondisi barang sebelum simpan
            //  untuk deteksi apa perlu gen-stok
            var result = SaveInvoice();

            PrintInvoiceRdlc(result.InvoiceId);

            ClearForm();
        }

        private InvoiceModel SaveInvoice()
        {
            var mainform = (MainForm)this.Parent.Parent;
            var cmd = new SaveInvoiceRequest
            {
                InvoiceId = InvoiceIdText.Text,
                InvoiceDate = InvoiceDateText.Value.ToString("yyyy-MM-dd"),
                InvoiceCode = InvoiceCodeText.Text,
                SupplierId = SupplierIdText.Text,
                WarehouseId = WarehouseIdText.Text,
                TermOfPayment = TermOfPaymentCombo.SelectedIndex,
                DueDate = DueDateText.Value.ToString("yyyy-MM-dd"),
                UserId = mainform.UserId.UserId,
            };

            var listItem = (
                from c in _listItem
                where c.BrgName?.Length > 0
                select new SaveInvoiceRequestItem
                {
                    BrgId = c.BrgId,
                    HrgInputStr = c.HrgInputStr,
                    QtyString = c.QtyInputStr,
                    DiscountString = c.DiscInputStr,
                    DppProsen = c.DppProsen,
                    PpnProsen = c.PpnProsen,
                }).ToList();
            cmd.ListBrg = listItem;
            var result = _saveInvoiceWorker.Execute(cmd);
            LastIdLabel.Text = result.InvoiceId;
            return result;
        }
        
        private void ClearForm()
        {
            InvoiceIdText.Text = string.Empty;
            InvoiceDateText.Value = _dateTime.Now;
            InvoiceCodeText.Text = string.Empty;

            SupplierIdText.Text = string.Empty;
            SupplierNameText.Text = string.Empty;
            WarehouseIdText.Text = string.Empty;
            WarehouseNameText.Text = string.Empty;
            TermOfPaymentCombo.SelectedIndex = 0;

            TotalText.Value = 0;
            DiscountText.Value = 0;
            DppText.Value = 0;
            TaxText.Value = 0;
            UangMukaText.Value = 0;
            SisaText.Value = 0;

            _listItem.Clear();
            var newItem = new InvoiceItemDto();
            newItem.SetPpnProsen(_ppnProsen);
            _listItem.Add(newItem);

            ShowAsActive();
        }
        private void ShowAsVoid(InvoiceModel invoice)
        {
            this.BackColor = Color.RosyBrown;
            foreach (var item in this.Controls)
                if (item is Panel panel)
                    panel.BackColor = Color.MistyRose;

            CancelLabel.Text = $@"Invoice sudah DIBATALKAN \noleh {invoice.UserIdVoid} \npada {invoice.VoidDate:ddd, dd MMM yyyy}";
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
            if (InvoiceIdText.Text == string.Empty)
                return;

            if (MessageBox.Show("Delete Invoice?", "Invoice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Delete();
        }

        private void Delete()
        {
            var mainMenu = this.Parent.Parent;
            var user = ((MainForm)mainMenu).UserId;
            var req = new VoidInvoiceRequest(InvoiceIdText.Text, user.UserId);
            _voidInvoiceWorker.Execute(req);

            ClearForm();
        }
        #endregion

    }
}
