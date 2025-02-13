﻿using btr.distrib.SalesContext.FakturAgg;
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

namespace btr.distrib.PurchaseContext.InvoiceAgg
{
    public partial class InvoicePrintOutForm : Form
    {
        private InvoicePrintOutDto _invoice;

        public InvoicePrintOutForm(InvoicePrintOutDto invoice)
        {
            InitializeComponent();

            //  set margin page
            PageSettings pageSettings = new PageSettings();
            pageSettings.Margins = new Margins(25, 25, 25, 25); // Left, Right, Top, Bottom
            reportViewer1.SetPageSettings(pageSettings);

            _invoice = invoice;
        }

        private void ReportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

        private async void InvoicePrintOutForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                reportViewer1.Invoke((MethodInvoker)delegate
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout); // Print layout is generally non-interactive
                    reportViewer1.ZoomMode = ZoomMode.PageWidth; // Simplify 
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("InvoiceBeliDataset", new List<InvoicePrintOutDto> { _invoice }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("InvoiceBeliItemDataset", _invoice.ListItem));
                    reportViewer1.RefreshReport();
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 150;
                });
            });
            this.reportViewer1.RefreshReport();
        }
    }
}
