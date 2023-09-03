using btr.distrib.InventoryContext.BrgAgg;
using btr.distrib.InventoryContext.KategoriAgg;
using btr.distrib.InventoryContext.WarehouseAgg;
using btr.distrib.PrintDocs;
using btr.distrib.PurchaseContext.PurchaseOrderAgg;
using btr.distrib.PurchaseContext.SupplierAgg;
using btr.distrib.SalesContext.CustomerAgg;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SalesContext.SalesPersonAgg;
using btr.distrib.SalesContext.UserAgg;
using btr.distrib.SalesContext.WilayahAgg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
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

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<SupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<SalesPersonForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void WilayahButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<WilayahForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void KategoriButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<KategoriForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<WarehouseForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<PrintManagerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void TestingButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<TestPlayground>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<AboutForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void UserButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<UserForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
    }
}
