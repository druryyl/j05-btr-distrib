using btr.distrib.SalesContext.FakturAgg;
using btr.domain.FinanceContext.TagihanAgg;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public partial class TagihanPrintOutForm : Form
    {
        private TagihanPrintOutDto _tagihan;

        public TagihanPrintOutForm(TagihanPrintOutDto tagihan)
        {
            InitializeComponent();
            reportViewer1.Print += ReportViewer1_Print;

            //  set margin page
            PageSettings pageSettings = new PageSettings();
            pageSettings.Margins = new Margins(25, 25, 25, 25); // Left, Right, Top, Bottom
            PaperSize f4Size = new PaperSize("F4", 827, 1300);  // 8.27in x 13in (measured in hundredths of an inch)
            pageSettings.PaperSize = f4Size; 
            pageSettings.Landscape = true;
            reportViewer1.SetPageSettings(pageSettings);

            _tagihan = tagihan;
        }

        private void ReportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        private async void TagihanPrintOutForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                reportViewer1.Invoke((MethodInvoker)delegate
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout); // Print layout is generally non-interactive
                    reportViewer1.ZoomMode = ZoomMode.PageWidth; // Simplify 
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("TagihanDataset", new List<TagihanPrintOutDto> { _tagihan }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("TagihanItemDataset", _tagihan.ListItem));
                    reportViewer1.RefreshReport();
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 150;
                });
            });
        }
    }
}
