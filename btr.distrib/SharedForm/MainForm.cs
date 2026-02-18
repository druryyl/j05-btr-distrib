using btr.application.SupportContext.RoleFeature;
using btr.application.SupportContext.UserAgg;
using btr.distrib.FinanceContext;
using btr.distrib.FinanceContext.FpKeluaranAgg;
using btr.distrib.FinanceContext.LunasPiutangAgg;
using btr.distrib.FinanceContext.PenerimaanPelunasanSalesRpt;
using btr.distrib.FinanceContext.PiutangSalesWilayahRpt;
using btr.distrib.FinanceContext.ReturBalanceAgg;
using btr.distrib.FinanceContext.TagihanAgg;
using btr.distrib.InventoryContext.AdjustmentAgg;
using btr.distrib.InventoryContext.BrgAgg;
using btr.distrib.InventoryContext.DriverAgg;
using btr.distrib.InventoryContext.ImportOpnameAgg;
using btr.distrib.InventoryContext.KartuStokRpt;
using btr.distrib.InventoryContext.KategoriAgg;
using btr.distrib.InventoryContext.MutasiAgg;
using btr.distrib.InventoryContext.MutasiRpt;
using btr.distrib.InventoryContext.OmzetSupplierRpt;
using btr.distrib.InventoryContext.OpnameAgg;
using btr.distrib.InventoryContext.PackingAgg;
using btr.distrib.InventoryContext.ReturJualAgg;
using btr.distrib.InventoryContext.ReturJualRpt;
using btr.distrib.InventoryContext.StokBalanceRpt;
using btr.distrib.InventoryContext.StokBrgSupplierRpt;
using btr.distrib.InventoryContext.StokPeriodikRpt;
using btr.distrib.InventoryContext.WarehouseAgg;
using btr.distrib.PurchaseContext.InvoiceAgg;
using btr.distrib.PurchaseContext.InvoiceHarianDetilRpt;
using btr.distrib.PurchaseContext.InvoiceInfo;
using btr.distrib.PurchaseContext.PostingStokAgg;
using btr.distrib.PurchaseContext.ReturBeliFeature;
using btr.distrib.PurchaseContext.ReturBeliInfo;
using btr.distrib.PurchaseContext.SupplierAgg;
using btr.distrib.ReportingContext;
using btr.distrib.SalesContext.AlokasiFpAgg;
using btr.distrib.SalesContext.CustomerAgg;
using btr.distrib.SalesContext.DriverFakturRpt;
using btr.distrib.SalesContext.FakturAgg;
using btr.distrib.SalesContext.FakturCashRpt;
using btr.distrib.SalesContext.FakturControlAgg;
using btr.distrib.SalesContext.FakturInfoRpt;
using btr.distrib.SalesContext.FakturPajakRpt;
using btr.distrib.SalesContext.FakturPerCustomerRpt;
using btr.distrib.SalesContext.FakturPerSupplierRpt;
using btr.distrib.SalesContext.LocationFeature;
using btr.distrib.SalesContext.OrderFeature;
using btr.distrib.SalesContext.SalesPersonAgg;
using btr.distrib.SalesContext.SalesReplacementFeat;
using btr.distrib.SalesContext.WilayahAgg;
using btr.domain.SupportContext.RoleFeature;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        private readonly IUserDal _userDal;
        private readonly IRoleRepo _roleRepo;

        public ServiceProvider ThisServicesProvider { get; private set; }
        public IUserKey UserId { get; private set; }

        public MainForm(IServiceCollection servicesCollection)
        {
            InitializeComponent();
            var mdi = Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdi != null)
                mdi.BackColor = Color.White;
            
            ThisServicesProvider = servicesCollection.BuildServiceProvider();
            _userDal = ThisServicesProvider.GetRequiredService<IUserDal>();
            _roleRepo = ThisServicesProvider.GetRequiredService<IRoleRepo>();
        }

        public void ShowCurrentUserInStatusStrip(string user)
        {
            var dbOpt =  ThisServicesProvider.GetRequiredService<IOptions<DatabaseOptions>>();
            var server = ConnStringHelper.Server;
            var db = ConnStringHelper.Database;
            LoginStatus.Text = $@"User ID: {user}";
            ServerDbStatus.Text = $@"Connected Database: {db}@{server}";
            UserId = new UserModel(user);
            if (dbOpt.Value.ServerName == "JUDE7")
                BackgroundImage = null;

            SetupUserMenu();
        }

        private void SetupUserMenu()
        {
            if (UserId.UserId.ToLower() == "yudis")
                return;
            var user = _userDal.GetData(UserId);
            if (user is null)
                return;
            var role = _roleRepo.LoadEntity(user).GetValueOrThrow("Akses not found");
            if (role.RoleId == "SYSAD")
                return;

            var allMenu = GetAllRibbonButtons();
            allMenu.Select(x => x.Enabled = false).ToList();

            // allowed names from role.ListMenu (MenuName)
            var allowed = new HashSet<string>(
                (role.ListMenu ?? Enumerable.Empty<MenuType>())
                    .Select(m => m.MenuName?.Trim()),
                StringComparer.OrdinalIgnoreCase);

            // enable buttons when either designer Name or visible Text matches the MenuName
            foreach (var btn in allMenu)
            {
                if (allowed.Contains(btn.Name) || allowed.Contains(btn.Text))
                {
                    btn.Enabled = true;
                    btn.Visible = true; // optional: make visible when allowed
                }
            }
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

        #region SALES
        //      transaction
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
        private void ST3SalesReplacementMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesReplacementForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesReplacementForm>();
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
        //      master data
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
        private void SM4RuteButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesRuteForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesRuteForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        //      sales reporting
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
        private void SF7OmzetSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<OmzetSupplierInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<OmzetSupplierInfoForm>();
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
        //      order reporting
        private void RO1CheckInListMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<CheckInInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<CheckInInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void RO2SalesOmzetMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SalesOmzetInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SalesOmzetInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void RO3EffectiveCallMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<EffectiveCallInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<EffectiveCallInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void RO4CoordinateCoverageMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<LocationCoverageInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<LocationCoverageInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        #endregion

        #region PURCHASE
        //      transaction
        private void PT1InvoiceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceForm>();
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
        private void PT3ReturBeliMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturBeliForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturBeliForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        //      reporting
        private void PF1InvoiceInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceInfoForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void PF2InvoiceBrgInfoButton_Click(object sender, EventArgs e)
        {

        }
        private void PF3InvoiceHarianButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<InvoiceHarianDetilForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<InvoiceHarianDetilForm>();
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
        //      master data
        private void PM1SupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<SupplierForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<SupplierForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        #endregion

        #region INVENTORY
        //      transaction
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
        private void IT4MutasiButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<MutasiForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<MutasiForm>();
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
        //      reporting
        private void IF1StokBalanceButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBalanceInfo2Form>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokBalanceInfo2Form>();
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
        private void IF4StokPerSupplierButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<StokBrgSupplierForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<StokBrgSupplierForm>();
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
        private void IF6StokMutasiButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<MutasiInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<MutasiInfoForm>();
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
        private void IF8KartuStokSummary_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<KartuStokSummaryForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<KartuStokSummaryForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        //      master data
        private void IM1BrgButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<BrgForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<BrgForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void IM2WarehouseButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<WarehouseForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<WarehouseForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void IM3KategoriButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<KategoriForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<KategoriForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private void IM4DriverButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DriverForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<DriverForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        #endregion

        #region FINANCE
        //      transcation
        private void FT1LunasPiutangButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<LunasPiutang2Form>())
                return;
            var form = ThisServicesProvider.GetRequiredService<LunasPiutang2Form>();
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
        private void FT3FpKeluaranButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FpKeluaranForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FpKeluaranForm>();
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
        //      reporting
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
        private void FF3FpKeluaranButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<FpKeluaranInfoForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<FpKeluaranInfoForm>();
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
        #endregion

        #region RETUR JUAL
        //      transaction
        private void RT1ReturJualButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualForm>();
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
        //      reporting
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
        private void RF3ReturJualPerSupplierInfoButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<ReturJualPerSupplierReportForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<ReturJualPerSupplierReportForm>();
            form.MdiParent = this;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }
        #endregion

        #region OTHERS
        private void UserButton_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<UserForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<UserForm>();
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
        private void AboutButton_Click(object sender, EventArgs e)
        {
            var menuStr = string.Empty;
            foreach (var item in this.ribbon1.Controls)
            {
                if (item is RibbonButton ribBut)
                    menuStr += $"\n{ribBut.Name}";
            }
            MessageBox.Show(menuStr);
            var form = ThisServicesProvider.GetRequiredService<AboutForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
        private void ImportExcelOpnameButton_Click(object sender, EventArgs e)
        {
            DumpRibbonMenuNames();
            //if (BringMdiChildToFrontIfLoaded<ImportOpnameForm>())
            //    return;
            //var form = ThisServicesProvider.GetRequiredService<ImportOpnameForm>();
            //form.StartPosition = FormStartPosition.CenterScreen;
            //form.MdiParent = this;
            //form.Show();
        }
        private void XX4RoleMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<RoleForm>())
                return;
            var form = ThisServicesProvider.GetRequiredService<RoleForm>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        #endregion

        #region HELPER-LIST-ALL-MENU
        private IEnumerable<System.Windows.Forms.RibbonButton> GetAllRibbonButtons()
        {
            var result = new List<System.Windows.Forms.RibbonButton>();
            foreach (System.Windows.Forms.RibbonTab tab in this.ribbon1.Tabs)
            {
                foreach (System.Windows.Forms.RibbonPanel panel in tab.Panels)
                {
                    foreach (var item in panel.Items)
                        CollectRibbonButtons(item, result);
                }
            }
            return result;
        }

        private void CollectRibbonButtons(object item, List<System.Windows.Forms.RibbonButton> acc)
        {
            if (item is System.Windows.Forms.RibbonButton btn)
            {
                if (btn.Style == RibbonButtonStyle.Normal)
                    acc.Add(btn);

                if (btn.DropDownItems != null)
                {
                    foreach (var di in btn.DropDownItems)
                        CollectRibbonButtons(di, acc);
                }
            }
            else if (item is System.Windows.Forms.RibbonPanel panel)
            {
                foreach (var sub in panel.Items)
                    CollectRibbonButtons(sub, acc);
            }
            // ignore separators or other item types
        }

        // example usage: call after InitializeComponent()
        private void DumpRibbonMenuNames()
        {
            var buttons = GetAllRibbonButtons();
            var lines = buttons.Select(b => $"{b.Name,-31} | {b.Text}"); // | Visible={b.Visible} | Enabled={b.Enabled}");
            Debug.WriteLine(string.Join("\n", lines));
            //MessageBox.Show(string.Join("\n", lines));
        }
        #endregion
    }
}
