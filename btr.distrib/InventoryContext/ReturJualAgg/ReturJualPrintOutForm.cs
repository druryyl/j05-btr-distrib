using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    public partial class ReturJualPrintOutForm : Form
    {
        private ReturJualPrintOutDto _retJual;


        public ReturJualPrintOutForm(ReturJualPrintOutDto retJual)
        {
            InitializeComponent();
            reportViewer1.Print += ReportViewer1_Print;

            //  set margin page
            PageSettings pageSettings = new PageSettings();
            pageSettings.Margins = new Margins(25, 25, 25, 25); // Left, Right, Top, Bottom
            reportViewer1.SetPageSettings(pageSettings);

            _retJual = retJual;            
        }

        private void ReportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        private async void ReturJualPrintOutForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                reportViewer1.Invoke((MethodInvoker)delegate
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout); // Print layout is generally non-interactive
                    reportViewer1.ZoomMode = ZoomMode.PageWidth; // Simplify 
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ReturJualDataset", new List<ReturJualPrintOutDto> { _retJual }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ReturJualItemDataset", _retJual.ListItem));
                    reportViewer1.RefreshReport();
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 150;
                });
            });
        }
    }
}
