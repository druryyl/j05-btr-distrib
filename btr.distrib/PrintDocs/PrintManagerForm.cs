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

        public PrintManagerForm(IWarehouseDal warehouseDal,
            IDocDal docDal,
            IDocBuilder docBuilder,
            IFakturBuilder fakturBuilder,
            IFakturPrintDoc fakturPrinter,
            IDocWriter docWriter)
        {
            _warehouseDal = warehouseDal;
            _docDal = docDal;
            _docBuilder = docBuilder;

            InitializeComponent();
            InitWarehouse();
            InitGrid();
            InitPrgBar();

            RegisterEventHandler();
            ListDoc();
            _fakturBuilder = fakturBuilder;
            _fakturPrinter = fakturPrinter;
            _docWriter = docWriter;
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
            if (MessageBox.Show($"Re-Print Document {id}?", "Re-Print", MessageBoxButtons.YesNo) == DialogResult.No)
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
            var listDocAll = _docDal.ListData(new Periode(hMin3, DateTime.Now))
                ?? new List<DocModel>();
            var listAtas = listDocAll
                .Where(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())
                .Where(x => x.DocPrintStatus == DocPrintStatusEnum.Queued)
                .OrderBy(x => x.DocId)
                .Select(x => new PrintManagerDto(x.DocId, x.DocDate.ToString("dd-MMM HH:mm:ss"),
                    $"{x.DocType} {x.DocDesc}",x.DocPrintStatus.ToString()))
                .ToList();
            GridAtas.DataSource = listAtas;

            var listBawah = listDocAll
                .Where(x => x.WarehouseId == WarehouseCombo.SelectedValue.ToString())
                .Where(x => x.DocPrintStatus != DocPrintStatusEnum.Queued)
                .OrderBy(x => x.DocId)
                .Select(x => new PrintManagerDto(x.DocId, x.DocDate.ToString("dd-MMM HH:mm:ss"),
                    $"{x.DocType} {x.DocDesc}", x.DocPrintStatus.ToString()))
                .ToList();
            GridBawah.DataSource = listBawah;
        }
    }

    public class PrintManagerDto
    {
        public PrintManagerDto(string id, string tglJam, string description, string status)
        {
            Id = id;
            TglJam = tglJam;
            Description = description;
            Status = status;
        }
        public string Id { get; }
        public string TglJam { get; }
        public string Description { get; }
        public string Status { get; }
    }
}
