using btr.domain.SalesContext.FakturAgg;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturAgg
{
    public partial class FakturPrintOutForm : Form
    {
        private FakturModel _faktur;

        public FakturPrintOutForm(FakturModel faktur)
        {
            InitializeComponent();
            reportViewer1.Print += ReportViewer1_Print;

            _faktur = faktur;
        }

        private void ReportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        private void FakturPrintOutForm_Load(object sender, EventArgs e)
        {
            List<FakturItemModel> items = _faktur.ListItem; // Get ListItem collection

            // Bind the FakturModel and ListItem to the ReportViewer
            reportViewer1.LocalReport.DataSources.Clear();

            // Bind the single FakturModel as MainFakturDataset
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FakturDataset", new List<FakturModel> { _faktur }));

            // Bind the ListItem collection as FakturItemsDataset
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FakturItemDataset", items));

            // Refresh the report
            reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
