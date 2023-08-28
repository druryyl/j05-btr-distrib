using btr.distrib.InventoryContext.BrgAgg;
using btr.distrib.PurchaseContext.PurchaseOrderAgg;
using btr.distrib.SalesContext.CustomerAgg;
using btr.distrib.SalesContext.FakturAgg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        private readonly FakturForm _fakturForm;
        private readonly ServiceProvider _servicesProvider;

        public MainForm(ServiceCollection servicesCollection)
        {
            InitializeComponent();
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;
            _servicesProvider = servicesCollection.BuildServiceProvider();
        }

        private void FakturButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<FakturForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PoButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<PurchaseOrderForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void BrgButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<BrgForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void OutletButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<CustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
    }
}
