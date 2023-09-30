using btr.distrib.InventoryContext.BrgAgg;
using btr.distrib.InventoryContext.KategoriAgg;
using btr.distrib.InventoryContext.OpnameAgg;
using btr.distrib.InventoryContext.PackingAgg;
using btr.distrib.InventoryContext.WarehouseAgg;
using btr.distrib.PrintDocs;
using btr.distrib.PurchaseContext.PurchaseOrderAgg;
using btr.distrib.PurchaseContext.SupplierAgg;
using btr.distrib.SalesContext.CustomerAgg;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SalesContext.FakturControlAgg;
using btr.distrib.SalesContext.SalesPersonAgg;
using btr.distrib.SalesContext.WilayahAgg;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.distrib.InventoryContext.StokBalanceRpt;
using btr.distrib.SalesContext.AlokasiFpAgg;
using btr.distrib.PurchaseContext.InvoiceAgg;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        private readonly ServiceProvider _servicesProvider;
        public IUserKey UserId { get; private set; }

        public MainForm(ServiceCollection servicesCollection)
        {
            InitializeComponent();
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;
            _servicesProvider = servicesCollection.BuildServiceProvider();
        }

        public void SetUser(string user)
        {
            var dbOpt =  _servicesProvider.GetRequiredService<IOptions<DatabaseOptions>>();

            LoginStatus.Text = $"User ID: {user}";
            ServerDbStatus.Text = $"Connected Database: {dbOpt.Value.DbName}@{dbOpt.Value.ServerName}";
            UserId = new UserModel(user);
        }
        private bool BringMdiChildToFrontIfLoaded<T>() where T : Form
        {
            var loadedForm = this.MdiChildren.OfType<T>().FirstOrDefault();
            if (loadedForm != null)
            {
                loadedForm.BringToFront();
                return true;
            }
            return false;
        }

        private void FakturButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturForm>())
                return;

            var form = _servicesProvider.GetRequiredService<FakturForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void PoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PurchaseOrderForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PurchaseOrderForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void BrgButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<BrgForm>())
                return;
            var form = _servicesProvider.GetRequiredService<BrgForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void OutletButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CustomerForm>())
                return;
            var form = _servicesProvider.GetRequiredService<CustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SupplierForm>())
                return;
            var form = _servicesProvider.GetRequiredService<SupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesPersonForm>())
                return;
            var form = _servicesProvider.GetRequiredService<SalesPersonForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void WilayahButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<WilayahForm>())
                return;
            var form = _servicesProvider.GetRequiredService<WilayahForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void KategoriButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<KategoriForm>())
                return;
            var form = _servicesProvider.GetRequiredService<KategoriForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void WarehouseButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<WarehouseForm>())
                return;
            var form = _servicesProvider.GetRequiredService<WarehouseForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<PrintManagerForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PrintManagerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void TestingButton_Click(object sender, EventArgs e)
        {
            if (!this.UserId.UserId.ToLower().StartsWith("jude"))
            {
                MessageBox.Show("Only for programmer");
                return;
            }

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
            if (BringMdiChildToFrontIfLoaded<UserForm>())
                return;
            var form = _servicesProvider.GetRequiredService<UserForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void OpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OpnameForm>())
                return;
            var form = _servicesProvider.GetRequiredService<OpnameForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ControlFakturButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturControlForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturControlForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PackingButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PackingForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PackingForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FakturPajakButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<AlokasiFpForm>())
                return;
            var form = _servicesProvider.GetRequiredService<AlokasiFpForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void StokBalanceButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<StokBalanceInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void InvoiceButton_Click(object sender, EventArgs e)
        {
            var form = _servicesProvider.GetRequiredService<InvoiceForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
    }
}
