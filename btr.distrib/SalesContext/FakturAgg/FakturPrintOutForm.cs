using btr.domain.SalesContext.FakturAgg;
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

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturPrintOutForm : Form
    {
        private FakturPrintOutDto _faktur;

        public FakturPrintOutForm(FakturPrintOutDto faktur)
        {
            InitializeComponent();
            reportViewer1.Print += ReportViewer1_Print;

            //  set margin page
            PageSettings pageSettings = new PageSettings();
            pageSettings.Margins = new Margins(25, 25, 25, 25); // Left, Right, Top, Bottom
            reportViewer1.SetPageSettings(pageSettings);

            _faktur = faktur;
        }

        private void ReportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        private async void FakturPrintOutForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                reportViewer1.Invoke((MethodInvoker)delegate
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout); // Print layout is generally non-interactive
                    reportViewer1.ZoomMode = ZoomMode.PageWidth; // Simplify 
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FakturJualDataset", new List<FakturPrintOutDto> { _faktur }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FakturJualItemDataset", _faktur.ListItem));
                    reportViewer1.RefreshReport();
                });
            });
        }
    }
}
