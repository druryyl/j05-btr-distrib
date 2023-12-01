using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.DocAgg;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.DocAgg;
using System.Drawing.Printing;
using btr.application.BrgContext.BrgAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.BrgContext.KategoriAgg;

namespace btr.distrib.PrintDocs
{
    public partial class PrintManagerForm : Form
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDocDal _docDal;
        private readonly IDocBuilder _docBuilder;
        private readonly IDocWriter _docWriter;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturPrintDoc _fakturPrinter;
        private int refreshCounter = 0;

        private readonly IBrgDal _brgDal;
        private readonly ISupplierDal _supplierDal;
        private readonly IKategoriDal _kategoriDal;


        public PrintManagerForm(IWarehouseDal warehouseDal,
            IDocDal docDal,
            IDocBuilder docBuilder,
            IFakturBuilder fakturBuilder,
            IFakturPrintDoc fakturPrinter,
            IDocWriter docWriter,
            IBrgDal brgDal,
            ISupplierDal supplierDal,
            IKategoriDal kategoriDal)
        {
            _warehouseDal = warehouseDal;
            _docDal = docDal;
            _docBuilder = docBuilder;
            _fakturBuilder = fakturBuilder;
            _fakturPrinter = fakturPrinter;
            _docWriter = docWriter;

            InitializeComponent();
            InitWarehouse();
            InitGrid();
            InitPrgBar();

            RegisterEventHandler();
            //ListDoc();
            PrintTimer.Enabled = false;

            _brgDal = brgDal;
            _supplierDal = supplierDal;
            _kategoriDal = kategoriDal;

        }

        private void RegisterEventHandler()
        {
            WarehouseCombo.SelectedValueChanged += WarehouseCombo_SelectedValueChanged;
            PrintTimer.Tick += PrintTimer_Tick;
            GridAtas.CellDoubleClick += GridAtas_CellDoubleClick;
            GridBawah.CellDoubleClick += GridBawah_CellDoubleClick;
        }

        private void InitWarehouse()
        {
            var listWarehouse = _warehouseDal.ListData()?.ToList()
                ?? new List<WarehouseModel>();
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.DisplayMember = "WarehouseName";
            WarehouseCombo.ValueMember = "WarehouseId";
        }

        private void InitGrid()
        {
            GridAtas.DataSource = new List<PrintManagerDto>();
            GridAtas.Columns.GetCol("Id").Width = 100;
            GridAtas.Columns.GetCol("TglJam").Width = 100;
            GridAtas.Columns.GetCol("Description").Width = 200;
            GridAtas.Columns.GetCol("Status").Width = 80;
            GridAtas.Columns.SetDefaultCellStyle(Color.Azure);

            GridBawah.DataSource = new List<PrintManagerDto>();
            GridBawah.Columns.GetCol("Id").Width = 100;
            GridBawah.Columns.GetCol("TglJam").Width = 100;
            GridBawah.Columns.GetCol("Description").Width = 200;
            GridBawah.Columns.GetCol("Status").Width = 80;
            GridBawah.Columns.SetDefaultCellStyle(Color.Beige);
        }

        private void StartPrint(int rowIndex)
        {
            var id = GridAtas.Rows[rowIndex].Cells[0].Value.ToString();
            var faktur = _fakturBuilder.Load(new FakturModel(id)).Build();
            _fakturPrinter.DefaultPrinter = GetPrinterName();
            _fakturPrinter.CreateDoc(faktur);
            DocModel doc = null;
            try
            {
                _fakturPrinter.PrintDoc();
                if (_fakturPrinter.IsPrinted)
                    doc = _docBuilder
                        .LoadOrCreate(new DocModel(id))
                        .Print()
                        .Build();
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message.Substring(0, ex.Message.Length > 255 ? 255 : ex.Message.Length);
                doc = _docBuilder
                    .LoadOrCreate(new DocModel(id))
                    .ErrorPrint(errMsg)
                    .Build();
            }

            if (doc is null)
                return;

            _docWriter.Save(ref doc);
        }

        private void GridAtas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            StartPrint(e.RowIndex);
            ListDoc();
        }

        private void GridBawah_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = GridBawah.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (MessageBox.Show($@"Re-Print Document {id}?", @"Re-Print", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var doc = _docBuilder
                .LoadOrCreate(new DocModel(id))
                .Queue()
                .Build();
            _docWriter.Save(ref doc);
            ListDoc();
        }

        private void RefreshNowButton_Click(object sender, EventArgs e)
        {
            refreshCounter = 10;
            ListDoc();
        }

        private void InitPrgBar()
        {
            RefreshPrgBar.Maximum = 10;
            RefreshPrgBar.Minimum = 0;
        }

        private void PrintTimer_Tick(object sender, EventArgs e)
        {
            RefreshPrgBar.Value = refreshCounter;

            if (refreshCounter == 10)
            {
                PrintTimer.Enabled = false;
                ListDoc();
                PrinterLabel.Text = GetPrinterName();
                PrintTimer.Enabled = true;
            }

            refreshCounter++;
            if (refreshCounter >= 11)
                refreshCounter = 0;
        }

        private void WarehouseCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            ListDoc();
        }

        private void ListDoc()
        {
            var hMin3 = DateTime.Now.AddDays(-3);
            Tgl1Text.Value = hMin3;
            Tgl2Text.Value = DateTime.Now;
            var listDocAll = _docDal.ListData(new Periode(Tgl1Text.Value, Tgl2Text.Value))?.ToList()
                ?? new List<DocModel>();
            var listAtas = listDocAll
                .Where(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())
                .Where(x => x.DocPrintStatus == DocPrintStatusEnum.Queued)
                .OrderBy(x => x.DocId)
                .Select(x => new PrintManagerDto(x.DocId, x.Code, x.DocDate.ToString("dd-MMM HH:mm:ss"),
                    $"{x.DocType} {x.DocDesc}",x.DocPrintStatus.ToString()))
                .ToList();
            GridAtas.DataSource = listAtas;

            var listBawah = listDocAll
                .Where(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())
                .Where(x => x.DocPrintStatus != DocPrintStatusEnum.Queued)
                .OrderBy(x => x.DocId)
                .Select(x => new PrintManagerDto(x.DocId, x.Code, x.DocDate.ToString("dd-MMM HH:mm:ss"),
                    $"{x.DocType} {x.DocDesc}", x.DocPrintStatus.ToString()))
                .ToList();
            GridBawah.DataSource = listBawah;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var listBrg = _brgDal.ListData();
            var listSup = _supplierDal.ListData();
            var listKat = _kategoriDal.ListData();

            var result =(
                from c1 in listBrg
                join c2 in listSup on c1.SupplierId equals c2.SupplierId
                join c3 in listKat on c1.KategoriId equals c3.KategoriId
                group c1 by new
                {
                    c1.SupplierId,
                    c2.SupplierName,
                    c1.KategoriId,
                    c3.KategoriName
                } into g
                select new SupplierKategoriDto
                {
                    Supplier = g.Key.SupplierName,
                    Kategori = g.Key.KategoriName,
                    Jum = g.Count()
                }).ToList();

            var result2 = new List<SupplierKategoriDto>();
            foreach(var item in listSup.OrderBy(x => x.SupplierName))
            {
                var top3 = result
                    .Where(x => x.Supplier == item.SupplierName)
                    .OrderByDescending(x => x.Jum).Take(3).ToList();
                result2.AddRange(top3);
            }

            GridBawah.DataSource = result2;
            GridBawah.Refresh();

        }
    }

    public class SupplierKategoriDto
    {
        public string Supplier { get; set; }
        public string Kategori { get; set; }
        public int Jum { get; set; }
    }

    public class PrintManagerDto
    {
        public PrintManagerDto(string id, string code, string tglJam, string description, string status)
        {
            Id = id;
            Code = code;
            TglJam = tglJam;
            Description = description;
            Status = status;
        }
        public string Id { get; }
        public string Code { get; set; }
        public string TglJam { get; }
        public string Description { get; }
        public string Status { get; }
    }
}
