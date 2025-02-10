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

            //  set margin page
            PageSettings pageSettings = new PageSettings
            {
                Margins = new Margins(25, 25, 25, 25) // Left, Right, Top, Bottom
            };
            TheViewer.SetPageSettings(pageSettings);
        }

        private void TheViewer_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        public void SetReportData(string reportName, List<ReportDataSource> listDatasource)
        {
            //var reportBasePath = _paramSistemDal
            //    .GetData(new ParamSistemModel("REPORT_BASE_PATH"))
            //    ?.ParamValue ?? string.Empty;
            //var clientId = _paramSistemDal.GetData(new ParamSistemModel("CLIENT_ID"))
            //    ?.ParamValue ?? string.Empty;

            var reportFileName = $"{reportName}.rdlc";
            var reportFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", reportFileName);

            listDatasource.ForEach(x => TheViewer.LocalReport.DataSources.Add(x));
            TheViewer.LocalReport.ReportPath = reportFileFullPath;

            TheViewer.ProcessingMode = ProcessingMode.Local;
            TheViewer.SetDisplayMode(DisplayMode.PrintLayout); // Print layout is generally non-interactive
            TheViewer.ZoomMode = ZoomMode.PageWidth; // Simplify 
            TheViewer.ZoomMode = ZoomMode.Percent;
            TheViewer.ZoomPercent = 150;

            TheViewer.RefreshReport();
        }
    }
}
