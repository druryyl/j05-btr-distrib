using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.PurchaseContext.PostingStokAgg
{
    public partial class PostingStokForm : Form
    {
        private readonly IInvoiceDal _invoiceDal;
        private readonly BindingList<PostingStokDto> _listInvoice;
        private readonly BindingSource _bindingSource;
        private readonly IGenStokInvoiceWorker _genStokWorker;
        private readonly InvoiceBuilder _invoiceBuilder;

        public PostingStokForm(IInvoiceDal invoiceDal, 
            IGenStokInvoiceWorker genStokWorker, 
            InvoiceBuilder invoiceBuilder)
        {
            InitializeComponent();

            _invoiceDal = invoiceDal;
            _listInvoice = new BindingList<PostingStokDto>();
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _listInvoice;
            _genStokWorker = genStokWorker;

            InitGrid();
            RegisterEventHandler();
            _invoiceBuilder = invoiceBuilder;
        }

        private void RegisterEventHandler()
        {

            // Register the event handler
            InvoiceGrid.RowEnter += InvoiceGrid_RowEnter;
            SearchButton.Click += SearchButton_Click;
            PostingButton.Click += PostingButton_Click;
        }

        private void PostingButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Posting Stok", "Konfirmasi", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            if (InvoiceIdText.Text == string.Empty)
                return;

            PostingStok();
            Search();
            MessageBox.Show("Posting Stok Selesai");
        }

        private void PostingStok()
        {
            var invoiceId = InvoiceIdText.Text;
            var invoiceKey = new InvoiceModel(invoiceId);
            var genStokReq = new GenStokInvoiceRequest(invoiceId);
            var invoice = _invoiceBuilder
                .Load(invoiceKey)
                .IsPosted(true)
                .Build();
            using (var trans = TransHelper.NewScope())
            {
                _invoiceDal.Update(invoice);
                _genStokWorker.Execute(genStokReq);
                trans.Complete();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var periode = new Periode(
                InvoiceCalender.SelectionStart,
                InvoiceCalender.SelectionStart);
            var listInvoice = _invoiceDal.ListData(periode)?.ToList()
                ?? new List<InvoiceModel>();
            _listInvoice.Clear();
            listInvoice.ForEach(x => _listInvoice.Add(new PostingStokDto(x.InvoiceId, x.InvoiceCode, x.InvoiceDate, x.SupplierName, x.GrandTotal, x.IsStokPosted)));

            foreach (DataGridViewRow row in InvoiceGrid.Rows)
            {
                if (row.Cells["IsPosted"].Value.ToString() == "True")
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }

        private void InitGrid()
        {
            InvoiceGrid.DataSource = _bindingSource;
            InvoiceGrid.DefaultCellStyle.Font = new Font("Lucida Console", 8);
            var grid = InvoiceGrid.Columns;
            grid["InvoiceId"].Visible = false;

            grid["InvoiceCode"].HeaderText = "Invoice Code";
            grid["InvoiceDate"].HeaderText = "Invoice Date";
            grid["SupplierName"].HeaderText = "Supplier Name";
            grid["GrandTotal"].HeaderText = "Grand Total";
            grid["IsPosted"].HeaderText = "Posted";

            grid["InvoiceCode"].Width = 100;
            grid["InvoiceDate"].Width = 100;
            grid["SupplierName"].Width = 200;
            grid["GrandTotal"].Width = 100;
            grid["IsPosted"].Width = 50;

            grid["InvoiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid["SupplierName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid["GrandTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["IsPosted"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid["GrandTotal"].DefaultCellStyle.Format = "N2";
        }
        private void InvoiceGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = InvoiceGrid.Rows[e.RowIndex];
            var invoiceId = selectedRow.Cells["InvoiceId"].Value.ToString();
            ShowInvoice(invoiceId);
        }

        private void ShowInvoice(string invoiceId)
        {
            var invoice = _invoiceDal.GetData(new InvoiceModel(invoiceId));
            if (invoice is null)
                return;

            InvoiceIdText.Text = $"{invoice.InvoiceId}";
            InvoiceCodeText.Text = $"{invoice.InvoiceCode}";
            InvoiceDateText.Text = $"{invoice.InvoiceDate:ddd, dd MMM yyyy}";
            SupplierNameText.Text = $"{invoice.SupplierName}";
            GrandTotalText.Text = $"{invoice.GrandTotal:N0}";
        }
    }

    public class PostingStokDto
    {

        public string InvoiceId { get; private set; }

        public PostingStokDto(string id, string code, DateTime tgl, 
            string name,  decimal total, bool isPosted)
        {
            InvoiceId = id;
            InvoiceCode = code;
            InvoiceDate = tgl.ToString("dd-MM-yyyy");
            SupplierName = name;
            GrandTotal = total;
            IsPosted = isPosted;
        }

        public string InvoiceCode { get; set; }
        public string InvoiceDate { get; private set; }
        public string SupplierName { get; private set; }
        public decimal GrandTotal { get; private set; }
        public bool IsPosted { get; private set; }
    }
}
