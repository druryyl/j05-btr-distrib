using btr.distrib.InventoryContext.BrgAgg;
using btr.distrib.InventoryContext.KategoriAgg;
using btr.distrib.InventoryContext.OpnameAgg;
using btr.distrib.InventoryContext.PackingAgg;
using btr.distrib.InventoryContext.WarehouseAgg;
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
using btr.distrib.FinanceContext.TagihanAgg;
using btr.distrib.PurchaseContext.InvoiceHarianDetilRpt;
using btr.distrib.InventoryContext.AdjustmentAgg;
using btr.distrib.SalesContext.FakturPajakRpt;
using btr.distrib.SalesContext.FakturCashRpt;
using btr.distrib.InventoryContext.ReturJualRpt;
using btr.distrib.FinanceContext.ReturBalanceAgg;
using btr.distrib.SalesContext.DriverFakturRpt;
using btr.distrib.InventoryContext.DriverAgg;
using btr.distrib.FinanceContext.FpKeluaranAgg;
using btr.distrib.InventoryContext.StokPeriodikRpt;
using btr.distrib.PurchaseContext.PostingStokAgg;
using btr.distrib.InventoryContext.MutasiRpt;
using btr.distrib.FinanceContext;
using btr.distrib.SalesContext.SalesReplacementFeat;
using btr.distrib.PurchaseContext.ReturBeliFeature;
using btr.distrib.PurchaseContext.ReturBeliInfo;
using btr.distrib.SalesContext.OrderFeature;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        public ServiceProvider ThisServicesProvider { get; private set; }
        public IUserKey UserId { get; private set; }

        public MainForm(IServiceCollection servicesCollection)
        {
            InitializeComponent();
            var mdi = Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdi != null)
                mdi.BackColor = Color.White;
            
            ThisServicesProvider = servicesCollection.BuildServiceProvider();
            
        }

        public void SetUser(string user)
        {
            var dbOpt =  ThisServicesProvider.GetRequiredService<IOptions<DatabaseOptions>>();
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

            var form = ThisServicesProvider.GetRequiredService<FakturForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void ST2ControlFakturButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturControlForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturControlForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void ST3FakturPajakButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<AlokasiFpForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<AlokasiFpForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF1FakturInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF2FakturBrgInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturBrgInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturBrgInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF3FakturPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPerSupplierForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturPerSupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SF4FakturPerCustomerButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPerCustomerForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturPerCustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SM1CustomerButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CustomerForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<CustomerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SM2SalesPersonButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesPersonForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesPersonForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void SM3WilayahButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<WilayahForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<WilayahForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IM1BrgButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<BrgForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<BrgForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PM1SupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SupplierForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IM3KategoriButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<KategoriForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<KategoriForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IM2WarehouseButton_Click(object sender, EventArgs e)
        {if (BringMdiChildToFrontIfLoaded<WarehouseForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<WarehouseForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        //private void IT2PrintButton_Click(object sender, EventArgs e)
        //{if (BringMdiChildToFrontIfLoaded<PrintManagerForm>())
        //        return;
        //    var form = ThisServicesProvider.GetRequiredService<PrintManagerForm>();
        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    form.MdiParent = this;
        //    form.Show();
        //}

        private void AboutButton_Click(object sender, EventArgs e)
        {
            var form = ThisServicesProvider.GetRequiredService<AboutForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        private void UserButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<UserForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<UserForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT1OpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokOpForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokOpForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void IT3PackingButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<Packing2Form>())
                return;
            var form = ThisServicesProvider.GetRequiredService<Packing2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF1StokBalanceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBalanceInfo2Form>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokBalanceInfo2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PT1InvoiceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT4MutasiButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<MutasiForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<MutasiForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void InvoiceReportButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CustomerChartRpt>())
                return;
            var form = ThisServicesProvider.GetRequiredService<CustomerChartRpt>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ImportExcelOpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ImportOpnameForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ImportOpnameForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT1LunasPiutangButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<LunasPiutang2Form>())
                return;
            var form = ThisServicesProvider.GetRequiredService<LunasPiutang2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void RT15ReturJualButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT3ReturJualButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void P1InvoiceInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void P2InvoiceBrgInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceBrgInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceBrgInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF3OmzetPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OmzetSupplierInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<OmzetSupplierInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ParamSistemButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ParamSistemForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ParamSistemForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF2KartuStokButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<KartuStokInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<KartuStokInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF1PiutangSalesWilayahButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PiutangSalesWilayahForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<PiutangSalesWilayahForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF2PenerimaanSalesButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PenerimaanPelunasanSalesForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<PenerimaanPelunasanSalesForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF7OmzetSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OmzetSupplierInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<OmzetSupplierInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void P3InvoiceHarianButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceHarianDetilForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceHarianDetilForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void FT2TagihanSalesButton_Click_1(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<TagihanForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<TagihanForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IT6Adjustment_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<AdjustmentForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<AdjustmentForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF5FakturPajakButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturPajakInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturPajakInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void SF6FakturCashButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FakturCashInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FakturCashInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF4StokPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBrgSupplierForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokBrgSupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RF1ReturJualInfo_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualReportForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualReportForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RF2ReturBrgInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualBrgReportForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualBrgReportForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }


        private void RT2PostingRetur_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturBalanceForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturBalanceForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();

        }

        private void SF8DriverFaktur_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DriverFakturInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<DriverFakturInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DriverForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<DriverForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT3FpKeluaranButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FpKeluaranForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FpKeluaranForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF5StokPeriodikButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokPeriodikForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokPeriodikForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PT2PostingStok_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PostingStokForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<PostingStokForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF6StokMutasiButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<MutasiInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<MutasiInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF3FpKeluaranButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FpKeluaranInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FpKeluaranInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RuteButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesRuteForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesRuteForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF8KartuStokSummary_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<KartuStokSummaryForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<KartuStokSummaryForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FF4PelunasanInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PelunasanInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<PelunasanInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void IF7StokOpnameButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokOpInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokOpInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ST3SalesReplacementMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesReplacementForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesReplacementForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PT3ReturBeliMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturBeliForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturBeliForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void PF4ReturBeliDetil_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturBeliBrgInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturBeliBrgInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void ST4OrderMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ListOrderForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ListOrderForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void RF3ReturJualPerSupplierInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualPerSupplierReportForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualPerSupplierReportForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show(); 
        }

        private void FT4TandaTerimaButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<TandaTerimaTagihanForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<TandaTerimaTagihanForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }

        private void FT5PiutangTracker_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<PiutangTrackerForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<PiutangTrackerForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
    }
}
