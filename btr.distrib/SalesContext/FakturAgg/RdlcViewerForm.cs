using btr.application.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.ParamSistemAgg;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class RdlcViewerForm : Form
    {
        private readonly IParamSistemDal _paramSistemDal;
        private string _reportFilePath = string.Empty;

        public RdlcViewerForm(IParamSistemDal paramSistemDal, string reportName)
        {
            InitializeComponent();
            _paramSistemDal = paramSistemDal;
            var reportBasePath = _paramSistemDal
                .GetData(new ParamSistemModel("REPORT_BASE_PATH"))
                ?.ParamValue ?? string.Empty;
            var clientId = _paramSistemDal .GetData(new ParamSistemModel("CLIENT_ID"))
                ?.ParamValue ?? string.Empty;
            _reportFilePath = Path.Combine(reportBasePath, clientId, $"{reportName}.rdlc");
        }

        private void RdlcViewerForm_Load(object sender, EventArgs e)
        {
            TheViewer.LocalReport.ReportPath = _reportFilePath;
            ReportDataSource rds = new ReportDataSource();
            TheViewer.RefreshReport();
        }
    }
}
