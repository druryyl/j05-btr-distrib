using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class RdlcViewerForm : Form
    {

        public RdlcViewerForm()
        {
            InitializeComponent();
            TheViewer.Print += TheViewer_Print;
            PageSettings pageSettings = new PageSettings
            {
                // Left, Right, Top, Bottom
                Margins = new Margins(25, 25, 25, 25)
            };
            TheViewer.SetPageSettings(pageSettings);
        }

        private void TheViewer_Print(object sender, ReportPrintEventArgs e)
        {
            //this.Close();
        }

        public void SetReportData(string reportName, List<ReportDataSource> listDatasource)
        {
            var reportFileName = $"{reportName}.rdlc";
            var reportFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", reportFileName);

            listDatasource.ForEach(x => TheViewer.LocalReport.DataSources.Add(x));
            TheViewer.LocalReport.ReportPath = reportFileFullPath;

            TheViewer.ProcessingMode = ProcessingMode.Local;
            TheViewer.SetDisplayMode(DisplayMode.PrintLayout);
            TheViewer.ZoomMode = ZoomMode.PageWidth;
            TheViewer.ZoomMode = ZoomMode.Percent;
            TheViewer.ZoomPercent = 100;

            TheViewer.RefreshReport();
        }
    }
}
