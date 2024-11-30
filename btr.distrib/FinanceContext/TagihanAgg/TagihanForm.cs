using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.FinanceContext.TagihanAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.SalesContext.FakturAgg;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Domain;
using ClosedXML.Excel;
using JetBrains.Annotations;
using Mapster;
using Polly;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TagihanForm : Form
    {
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<TagihanDto> _listTagihan = new BindingList<TagihanDto>();
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturDal _fakturDal;
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly ITagihanBuilder _tagihanBuilder;
        private readonly ITagihanWriter _tagihanWriter;
        private readonly IBrowser<TagihanBrowserView> _tagihanBrowser;
        
        private readonly ISalesPersonDal _salesDal;
        public TagihanForm(ISalesPersonDal salesDal,
            IFakturBuilder fakturBuilder,
            IPiutangBuilder piutangBuilder,
            IFakturDal fakturDal,
            ITagihanBuilder tagihanBuilder,
            ITagihanWriter tagihanWriter,
            IBrowser<TagihanBrowserView> tagihanBrowser)
        {
            _salesDal = salesDal;
            _fakturBuilder = fakturBuilder;
            _piutangBuilder = piutangBuilder;
            _fakturDal = fakturDal;
            _tagihanBuilder = tagihanBuilder;
            _tagihanWriter = tagihanWriter;

            InitializeComponent();
            InitGrid();
            InitCombo();
            TglTagihText.Value = DateTime.Now;

            RegisterEventHandler();
            _tagihanBrowser = tagihanBrowser;
        }

        private void RegisterEventHandler()
        {
            FakturGrid.CellValidated += FakturGridOnCellValidated;
            SaveButton.Click += SaveButtonOnClick;
            TagihanButton.Click += TagihanButton_Click;
            TagihanIdText.Validating += TagihanIdText_Validating;

        }

        private void TagihanIdText_Validating(object sender, CancelEventArgs e)
        {
            var textbox = (TextBox)sender;
            var valid = true;
            if (textbox.Text.Length == 0)
                ClearForm();
            else
                valid = ValidateTagihan();

            if (!valid)
                e.Cancel = true;
        }

        private bool ValidateTagihan()
        {
            var textbox = TagihanIdText;
            var policy = Policy<TagihanModel>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .Fallback(null as TagihanModel, (r, c) =>
                {
                    MessageBox.Show(r.Exception.Message);
                });

            var tagihan = policy.Execute(() => _tagihanBuilder
                .Load(new TagihanModel(textbox.Text))
                .Build());
            if (tagihan is null)
                return false;

            tagihan.RemoveNull();

            TglTagihText.Value = tagihan.TagihanDate;
            SalesCombo.SelectedValue = tagihan.SalesPersonId;
            TotalTagihanText.Value = tagihan.TotalTagihan;

            _listTagihan.Clear();
            foreach (var item in tagihan.ListFaktur)
            {
                var newItem = item.Adapt<TagihanDto>();
                _listTagihan.Add(newItem);
            }

            return true;
        }

        private void TagihanButton_Click(object sender, EventArgs e)
        {
            _tagihanBrowser.Filter.Date = new Periode(DateTime.Now);

            TagihanIdText.Text = _tagihanBrowser.Browse(TagihanIdText.Text);
            TagihanIdText_Validating(TagihanIdText, null);
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            var tagihan = _tagihanBuilder
                .Create()
                .Sales(new SalesPersonModel(SalesCombo.SelectedValue.ToString()))
                .TglTagihan(TglTagihText.Value)
                .Build();
            tagihan = _listTagihan.Aggregate(tagihan, (current, item) => _tagihanBuilder.Attach(current)
                .AddFaktur(new FakturModel(item.FakturId), item.Nilai)
                .Build());
            _tagihanWriter.Save(tagihan);
            LastIdLabel.Text = tagihan.TagihanId;
            ClearForm();
            
            PrintToExcel(tagihan);
        }

        private void ClearForm()
        {
            TagihanIdText.Clear();
            SalesCombo.SelectedIndex = -1;
            TglTagihText.Value = DateTime.Now;
            _listTagihan.Clear();
            _bindingSource.ResetBindings(false);
        }

        private void FakturGridOnCellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != FakturGrid.Columns.GetCol("FakturCode").Index) return;
            _listTagihan[e.RowIndex].RemoveNull();
            if (string.IsNullOrEmpty(_listTagihan[e.RowIndex].FakturCode)) return;
            
            var fakturCode = _listTagihan[e.RowIndex].FakturCode;
            var faktur = GetFaktur(fakturCode);
            var piutang = GetPiutang(fakturCode);

            if (piutang == null)
            {
                MessageBox.Show("Faktur tidak ditemukan");
            }
            if (piutang.Sisa <= 0)
            {
                MessageBox.Show("Faktur sudah lunas");
                return;
            }
            
            if (faktur.SalesPersonId != SalesCombo.SelectedValue.ToString())
            {
                MessageBox.Show(@"Sales tidak sesuai dengan faktur");
                _listTagihan[e.RowIndex].FakturCode  = "";
                _listTagihan[e.RowIndex].FakturId  = "";
                _listTagihan[e.RowIndex].FakturDate = "";
                _listTagihan[e.RowIndex].CustomerId  = "";
                _listTagihan[e.RowIndex].CustomerName  = "";
                _listTagihan[e.RowIndex].Alamat = "";
                _listTagihan[e.RowIndex].Nilai = 0;
                return;
            }
            
            _listTagihan[e.RowIndex].FakturId  = faktur.FakturId;
            _listTagihan[e.RowIndex].FakturDate = $"{faktur.FakturDate:dd-MM-yyyy}";
            _listTagihan[e.RowIndex].CustomerId  = faktur.CustomerId;
            _listTagihan[e.RowIndex].CustomerName  = faktur.CustomerName;
            _listTagihan[e.RowIndex].Alamat  = faktur.Address;
            _listTagihan[e.RowIndex].Nilai  = faktur.GrandTotal;

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            TotalTagihanText.Value = _listTagihan.Sum(x => x.Nilai);
            FakturGrid.Refresh();
        }

        private void PrintToExcel(TagihanModel tagihan)
        {
            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("tagihan-piutang");
                var baris = 1;
                ws.Cell($"A{baris}").Value = "CV BINTANG TIMUR RAHAYU";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(12)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "Jl.Kaliurang Km 5.5 Gg. Durmo No.18"; 
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = "TAGIHAN PIUTANG PER-SALES";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"{tagihan.TagihanDate:dd MMMM yyyy}";
                ws.Cell($"A{baris}").Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(false)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Merge();
                baris++;

                ws.Cell($"A{baris}").Value = $"Sales: {tagihan.SalesPersonName}";
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Merge();
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true);

                baris++;
                baris++;

                ws.Cell($"A{baris}").Value = $"No";
                ws.Cell($"B{baris}").Value = $"Faktur";
                ws.Cell($"C{baris}").Value = $"Customer";
                ws.Cell($"D{baris}").Value = $"Tgl Faktur";
                ws.Cell($"E{baris}").Value = $"Total";
                ws.Cell($"F{baris}").Value = $"Bayar";
                //  add columns: Sisa, Tunai, Cek/Giro, Transfer, Meterai, Retur, Sewa
                ws.Cell($"G{baris}").Value = $"Sisa";
                ws.Cell($"H{baris}").Value = $"Tunai";
                ws.Cell($"I{baris}").Value = $"Cek/Giro";
                ws.Cell($"J{baris}").Value = $"Transfer";
                ws.Cell($"K{baris}").Value = $"Meterai";
                ws.Cell($"L{baris}").Value = $"Retur";
                ws.Cell($"M{baris}").Value = $"Sewa";
                
                ws.Range(ws.Cell($"A{baris}"), ws.Cell($"M{baris}")).Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                baris++;
                foreach (var item in tagihan.ListFaktur)
                {
                    var faktur = GetFaktur(item.FakturCode);
                    var piutang = GetPiutang(item.FakturCode);  
                    
                    // fill columns: No, Faktur, Customer, Tgl Faktur, Total
                    ws.Cell($"A{baris}").Value = $"{item.NoUrut}";
                    ws.Cell($"B{baris}").Value = $"{item.FakturCode}";
                    ws.Cell($"C{baris}").Value = $"{item.CustomerName}";
                    ws.Cell($"D{baris}").Value = $"{faktur.FakturDate:dd MMM yyyy}";
                    ws.Cell($"E{baris}").Value = faktur.GrandTotal;
                    ws.Cell($"F{baris}").Value = piutang.Terbayar;
                    ws.Cell($"G{baris}").Value = piutang.Sisa;
                    // set row height to 15
                    ws.Row(baris).Height = 18;
                    baris++;
                }
                // format column E to M to number with thousand separator and 0 decimal place
                ws.Range(ws.Cell($"E{baris - tagihan.ListFaktur.Count}"), ws.Cell($"M{baris - 1}")).Style
                    .NumberFormat.SetFormat("#,##0");
                //  set font for all data to 10
                ws.Range(ws.Cell($"A{baris - tagihan.ListFaktur.Count}"), ws.Cell($"M{baris - 1}")).Style
                    .Font.SetFontSize(10);
                //  outline border for all data
                ws.Range(ws.Cell($"A{baris - tagihan.ListFaktur.Count}"), ws.Cell($"M{baris - 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetInsideBorder(XLBorderStyleValues.Thin);
                
                //  add footer SUM for column E to G
                ws.Cell($"D{baris}").Value = $"TOTAL TAGIHAN";
                ws.Cell($"E{baris}").FormulaA1 = $"SUM(E{baris - tagihan.ListFaktur.Count}:E{baris - 1})";
                ws.Cell($"F{baris}").FormulaA1 = $"SUM(F{baris - tagihan.ListFaktur.Count}:F{baris - 1})";
                ws.Cell($"G{baris}").FormulaA1 = $"SUM(G{baris - tagihan.ListFaktur.Count}:G{baris - 1})";

                //  format footer SUM to number with thousand separator and 0 decimal place
                ws.Range(ws.Cell($"E{baris}"), ws.Cell($"G{baris}")).Style
                    .NumberFormat.SetFormat("#,##0");
                
                //  set font for footer SUM to 10 and bold
                ws.Range(ws.Cell($"D{baris}"), ws.Cell($"G{baris}")).Style
                    .Font.SetFontSize(10)
                    .Font.SetBold(true);
                //  set border for footer SUM
                ws.Range(ws.Cell($"D{baris}"), ws.Cell($"M{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetInsideBorder(XLBorderStyleValues.Thin);
                baris++;
                ws.Cell($"D{baris}").Value = $"TOTAL SETORAN";
                ws.Range(ws.Cell($"D{baris}"), ws.Cell($"M{baris}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetInsideBorder(XLBorderStyleValues.Thin);
                //  add another foot
                
                ws.Columns().AdjustToContents();
                //  set column H to M width to 15
                ws.Column(8).Width = 12;
                ws.Column(9).Width = 12;
                ws.Column(10).Width = 12;
                ws.Column(11).Width = 12;
                ws.Column(12).Width = 12;
                ws.Column(13).Width = 12;
                ws.Column(14).Width = 12;
                
                //  set page orientation to landscape
                ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                //  set page size to A4
                ws.PageSetup.PaperSize = XLPaperSize.A4Paper;
                //  set page margins
                ws.PageSetup.Margins.Left = 0.25;
                
                //  generate random name file in temp folder
                var filePath = $"{Environment.GetEnvironmentVariable("TEMP")}\\{Guid.NewGuid()}.xlsx";
                wb.SaveAs(filePath);
                
                //  print preview
                System.Diagnostics.Process.Start(filePath);
            }
        }
        
        private FakturModel GetFaktur(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var fakturHdr = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            var faktur = _fakturBuilder.Load(fakturHdr).Build();
            return faktur;
        }
        private PiutangModel GetPiutang(string fakturCode)
        {
            var fakturCodeKey = new FakturModel{FakturCode = fakturCode};
            var faktur = _fakturDal.GetData((IFakturCode)fakturCodeKey)
                ?? throw new Exception($"Faktur {fakturCode} tidak ditemukan");
            
            var piutangKey = new PiutangModel{PiutangId = faktur.FakturId};
            var piutang = _piutangBuilder.Load(piutangKey).Build();

            return piutang;
        }

        private void InitCombo()
        {
            var listSales = _salesDal.ListData()?.ToList() ?? new List<SalesPersonModel>();
            SalesCombo.DataSource = listSales.OrderBy(x => x.SalesPersonName).ToList();
            SalesCombo.DisplayMember = "SalesPersonName";
            SalesCombo.ValueMember = "SalesPersonId";
        }
        
        private void InitGrid()
        {
            _bindingSource.DataSource = _listTagihan;
            FakturGrid.DataSource = _bindingSource;
            FakturGrid.Refresh();
            
            var col = FakturGrid.Columns;
            col.GetCol("FakturCode").HeaderText = @"Faktur";
            col.GetCol("FakturDate").HeaderText = @"Tgl Faktur";
            col.GetCol("CustomerId").HeaderText = @"Cust Id";
            col.GetCol("CustomerName").HeaderText = @"Customer";
            col.GetCol("Alamat").HeaderText = @"Alamat";
            col.GetCol("Nilai").HeaderText = @"Total Tagihan";

            col.GetCol("FakturId").Visible = false;
            
            col.GetCol("FakturDate").DefaultCellStyle.Format = "dd MMM yyyy";
            col.GetCol("Nilai").DefaultCellStyle.Format = "N0";
            col.GetCol("Nilai").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            col.GetCol("FakturCode").Width = 80;
            col.GetCol("FakturDate").Width = 80;
            col.GetCol("CustomerId").Width = 80;
            col.GetCol("CustomerName").Width = 200;
            col.GetCol("Alamat").Width = 200;
            col.GetCol("Nilai").Width = 100;
            
            col.GetCol("FakturDate").ReadOnly = true;
            col.GetCol("CustomerId").ReadOnly = true;
            col.GetCol("CustomerName").ReadOnly = true;
            col.GetCol("Alamat").ReadOnly = true;
            col.GetCol("Nilai").ReadOnly = true;
            
            col.GetCol("FakturDate").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("CustomerId").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("CustomerName").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("Alamat").DefaultCellStyle.BackColor = Color.LightBlue;
            col.GetCol("Nilai").DefaultCellStyle.BackColor = Color.Beige;
        }
    }

    [PublicAPI]
    public class TagihanDto
    {
        public string FakturCode { get; set; }

        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public decimal Nilai { get; set; }
        public string FakturId { get; set; }
    }
}
