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
using btr.distrib.SalesContext.AlokasiFpAgg;
using btr.distrib.PurchaseContext.InvoiceAgg;
using btr.distrib.InventoryContext.MutasiAgg;
using btr.distrib.ReportingContext;
using btr.distrib.InventoryContext.ImportOpnameAgg;
using btr.distrib.FinanceContext.LunasPiutangAgg;
using btr.distrib.InventoryContext.StokBalanceRpt;
using btr.distrib.SalesContext.FakturInfoRpt;
using btr.distrib.InventoryContext.ReturJualAgg;
using btr.distrib.InventoryContext.OmzetSupplierRpt;
using btr.distrib.PurchaseContext.InvoiceInfo;
using btr.distrib.InventoryContext.KartuStokRpt;
using btr.distrib.FinanceContext.PiutangSalesWilayahRpt;
using btr.distrib.FinanceContext.PenerimaanPelunasanSalesRpt;
using btr.distrib.InventoryContext.StokBrgSupplierRpt;
using btr.distrib.SalesContext.FakturPerSupplierRpt;
using btr.distrib.SalesContext.FakturPerCustomerRpt;
using btr.application.PurchaseContext.InvoiceHarianDetilRpt;
using btr.distrib.FinanceContext.TagihanAgg;
using btr.distrib.PurchaseContext.InvoiceHarianDetilRpt;
using btr.distrib.InventoryContext.AdjustmentAgg;
using btr.distrib.SalesContext.FakturPajakRpt;
using btr.distrib.SalesContext.FakturCashRpt;
using btr.distrib.InventoryContext.ReturJualRpt;
using btr.distrib.FinanceContext.ReturBalanceAgg;
using btr.distrib.SalesContext.DriverFakturRpt;
using btr.distrib.InventoryContext.DriverAgg;
using btr.application.InventoryContext.StokAgg;
using DocumentFormat.OpenXml.Drawing.Charts;
using btr.distrib.FinanceContext.FpKeluaranAgg;
using btr.distrib.InventoryContext.StokPeriodikRpt;
using btr.distrib.PurchaseContext.PostingStokAgg;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        private readonly ServiceProvider _servicesProvider;
        public IUserKey UserId { get; private set; }

        public MainForm(IServiceCollection servicesCollection)
        {
            InitializeComponent();
            var mdi = Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdi != null)
                mdi.BackColor = Color.White;
            
            _servicesProvider = servicesCollection.BuildServiceProvider();
            
        }

        public void SetUser(string user)
        {
            var dbOpt =  _servicesProvider.GetRequiredService<IOptions<DatabaseOptions>>();
            var server = ConnStringHelper.Server;
            var db = ConnStringHelper.Database;
            LoginStatus.Text = $@"User ID: {user}";
            ServerDbStatus.Text = $@"Connected Database: {db}@{server}";
            UserId = new UserModel(user);
            if (dbOpt.Value.ServerName == "JUDE7")
                this.BackgroundImage = null;
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

        public void ST1FakturButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturForm>())
                return;

            var form = _servicesProvider.GetRequiredService<FakturForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void ST2ControlFakturButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturControlForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturControlForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void ST3FakturPajakButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<AlokasiFpForm>())
                return;
            var form = _servicesProvider.GetRequiredService<AlokasiFpForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF1FakturInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF2FakturBrgInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturBrgInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturBrgInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF3FakturPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPerSupplierForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturPerSupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF4FakturPerCustomerButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPerCustomerForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturPerCustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SM1CustomerButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CustomerForm>())
                return;
            var form = _servicesProvider.GetRequiredService<CustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SM2SalesPersonButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesPersonForm>())
                return;
            var form = _servicesProvider.GetRequiredService<SalesPersonForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SM3WilayahButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<WilayahForm>())
                return;
            var form = _servicesProvider.GetRequiredService<WilayahForm>();
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

        private void IM1BrgButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<BrgForm>())
                return;
            var form = _servicesProvider.GetRequiredService<BrgForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PM1SupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SupplierForm>())
                return;
            var form = _servicesProvider.GetRequiredService<SupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IM3KategoriButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<KategoriForm>())
                return;
            var form = _servicesProvider.GetRequiredService<KategoriForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IM2WarehouseButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<WarehouseForm>())
                return;
            var form = _servicesProvider.GetRequiredService<WarehouseForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT2PrintButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<PrintManagerForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PrintManagerForm>();
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

        private void IT1OpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokOpForm>())
                return;
            var form = _servicesProvider.GetRequiredService<StokOpForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void IT3PackingButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<Packing2Form>())
                return;
            var form = _servicesProvider.GetRequiredService<Packing2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF1StokBalanceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBalanceInfo2Form>())
                return;
            var form = _servicesProvider.GetRequiredService<StokBalanceInfo2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PT1InvoiceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceForm>())
                return;
            var form = _servicesProvider.GetRequiredService<InvoiceForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT4MutasiButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<MutasiForm>())
                return;
            var form = _servicesProvider.GetRequiredService<MutasiForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void InvoiceReportButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CustomerChartRpt>())
                return;
            var form = _servicesProvider.GetRequiredService<CustomerChartRpt>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ImportExcelOpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ImportOpnameForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ImportOpnameForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT1LunasPiutangButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<LunasPiutang2Form>())
                return;
            var form = _servicesProvider.GetRequiredService<LunasPiutang2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void RT15ReturJualButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ReturJualForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT3ReturJualButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ReturJualForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void P1InvoiceInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<InvoiceInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void P2InvoiceBrgInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceBrgInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<InvoiceBrgInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF3OmzetPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OmzetSupplierInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<OmzetSupplierInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ParamSistemButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ParamSistemForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ParamSistemForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF2KartuStokButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<KartuStokInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<KartuStokInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF1PiutangSalesWilayahButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PiutangSalesWilayahForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PiutangSalesWilayahForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF2PenerimaanSalesButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PenerimaanPelunasanSalesForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PenerimaanPelunasanSalesForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF7OmzetSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OmzetSupplierInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<OmzetSupplierInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void P3InvoiceHarianButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceHarianDetilForm>())
                return;
            var form = _servicesProvider.GetRequiredService<InvoiceHarianDetilForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void FT2TagihanSalesButton_Click_1(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<TagihanForm>())
                return;
            var form = _servicesProvider.GetRequiredService<TagihanForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT6Adjustment_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<AdjustmentForm>())
                return;
            var form = _servicesProvider.GetRequiredService<AdjustmentForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF5FakturPajakButton_DoubleClick(object sender, EventArgs e)
        {

        }

        private void SF5FakturPajakButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPajakInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturPajakInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF6FakturCashButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturCashInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FakturCashInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF4StokPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBrgSupplierForm>())
                return;
            var form = _servicesProvider.GetRequiredService<StokBrgSupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RF1ReturJualInfo_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJjualReportForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ReturJjualReportForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RT2PostingRetur_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturBalanceForm>())
                return;
            var form = _servicesProvider.GetRequiredService<ReturBalanceForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();

        }

        private void SF8DriverFaktur_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DriverFakturInfoForm>())
                return;
            var form = _servicesProvider.GetRequiredService<DriverFakturInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DriverForm>())
                return;
            var form = _servicesProvider.GetRequiredService<DriverForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT3FpKeluaranButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FpKeluaranForm>())
                return;
            var form = _servicesProvider.GetRequiredService<FpKeluaranForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF5StokPeriodikButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokPeriodikForm>())
                return;
            var form = _servicesProvider.GetRequiredService<StokPeriodikForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PT2PostingStok_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PostingStokForm>())
                return;
            var form = _servicesProvider.GetRequiredService<PostingStokForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
    }
}
