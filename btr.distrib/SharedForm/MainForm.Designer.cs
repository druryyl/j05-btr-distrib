namespace btr.distrib.SharedForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.SalesTab = new System.Windows.Forms.RibbonTab();
            this.FakturPanel = new System.Windows.Forms.RibbonPanel();
            this.FakturButton = new System.Windows.Forms.RibbonButton();
            this.InfoFakturJualButton = new System.Windows.Forms.RibbonButton();
            this.ribbonSeparator2 = new System.Windows.Forms.RibbonSeparator();
            this.ControlFakturButton = new System.Windows.Forms.RibbonButton();
            this.FakturPajakButton = new System.Windows.Forms.RibbonButton();
            this.MasterSalesPanel = new System.Windows.Forms.RibbonPanel();
            this.OutletButton = new System.Windows.Forms.RibbonButton();
            this.KategoriButton = new System.Windows.Forms.RibbonButton();
            this.ribbonSeparator1 = new System.Windows.Forms.RibbonSeparator();
            this.SalesPersonButton = new System.Windows.Forms.RibbonButton();
            this.WilayahButton = new System.Windows.Forms.RibbonButton();
            this.PurchaseTab = new System.Windows.Forms.RibbonTab();
            this.PurchaseOrderRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.PoButton = new System.Windows.Forms.RibbonButton();
            this.InvoiceButton = new System.Windows.Forms.RibbonButton();
            this.InvoiceRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.InvoiceReportButton = new System.Windows.Forms.RibbonButton();
            this.MasterPurchaseRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.SupplierButton = new System.Windows.Forms.RibbonButton();
            this.InventoryTab = new System.Windows.Forms.RibbonTab();
            this.InventoryTrsPanel = new System.Windows.Forms.RibbonPanel();
            this.OpnameButton = new System.Windows.Forms.RibbonButton();
            this.ImportExcelOpnameButton = new System.Windows.Forms.RibbonButton();
            this.PrintFakturButton = new System.Windows.Forms.RibbonButton();
            this.PackingButton = new System.Windows.Forms.RibbonButton();
            this.MutasiButton = new System.Windows.Forms.RibbonButton();
            this.ReportingPanel = new System.Windows.Forms.RibbonPanel();
            this.StokBalanceButton = new System.Windows.Forms.RibbonButton();
            this.BukuStokButton = new System.Windows.Forms.RibbonButton();
            this.InventoryMasterPanel = new System.Windows.Forms.RibbonPanel();
            this.BrgButton = new System.Windows.Forms.RibbonButton();
            this.WarehouseButton = new System.Windows.Forms.RibbonButton();
            this.FinanceTab = new System.Windows.Forms.RibbonTab();
            this.SettingTab = new System.Windows.Forms.RibbonTab();
            this.DeliveryPanel = new System.Windows.Forms.RibbonPanel();
            this.UserButton = new System.Windows.Forms.RibbonButton();
            this.TestingButton = new System.Windows.Forms.RibbonButton();
            this.ReceivingPanel = new System.Windows.Forms.RibbonPanel();
            this.AboutButton = new System.Windows.Forms.RibbonButton();
            this.AppStatus = new System.Windows.Forms.StatusStrip();
            this.LoginStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ServerDbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.AppStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon1
            // 
            this.ribbon1.CaptionBarVisible = false;
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 72);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbon1.Size = new System.Drawing.Size(876, 120);
            this.ribbon1.TabIndex = 2;
            this.ribbon1.Tabs.Add(this.SalesTab);
            this.ribbon1.Tabs.Add(this.PurchaseTab);
            this.ribbon1.Tabs.Add(this.InventoryTab);
            this.ribbon1.Tabs.Add(this.FinanceTab);
            this.ribbon1.Tabs.Add(this.SettingTab);
            this.ribbon1.Text = "ribbon1";
            this.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Blue_2010;
            // 
            // SalesTab
            // 
            this.SalesTab.Name = "SalesTab";
            this.SalesTab.Panels.Add(this.FakturPanel);
            this.SalesTab.Panels.Add(this.MasterSalesPanel);
            this.SalesTab.Text = "Sales";
            // 
            // FakturPanel
            // 
            this.FakturPanel.ButtonMoreEnabled = false;
            this.FakturPanel.ButtonMoreVisible = false;
            this.FakturPanel.Items.Add(this.FakturButton);
            this.FakturPanel.Items.Add(this.InfoFakturJualButton);
            this.FakturPanel.Items.Add(this.ribbonSeparator2);
            this.FakturPanel.Items.Add(this.ControlFakturButton);
            this.FakturPanel.Items.Add(this.FakturPajakButton);
            this.FakturPanel.Name = "FakturPanel";
            this.FakturPanel.Text = "";
            // 
            // FakturButton
            // 
            this.FakturButton.Image = global::btr.distrib.Properties.Resources.icons8_day_view;
            this.FakturButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_day_view;
            this.FakturButton.Name = "FakturButton";
            this.FakturButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("FakturButton.SmallImage")));
            this.FakturButton.Text = "Faktur";
            this.FakturButton.Click += new System.EventHandler(this.FakturButton_Click);
            // 
            // InfoFakturJualButton
            // 
            this.InfoFakturJualButton.Image = global::btr.distrib.Properties.Resources.icons8_documents_32;
            this.InfoFakturJualButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_documents_32;
            this.InfoFakturJualButton.Name = "InfoFakturJualButton";
            this.InfoFakturJualButton.SmallImage = global::btr.distrib.Properties.Resources.icons8_documents_32;
            this.InfoFakturJualButton.Text = "Info Faktur";
            this.InfoFakturJualButton.Click += new System.EventHandler(this.InfoFakturJualButton_Click);
            // 
            // ribbonSeparator2
            // 
            this.ribbonSeparator2.Name = "ribbonSeparator2";
            this.ribbonSeparator2.Text = "";
            // 
            // ControlFakturButton
            // 
            this.ControlFakturButton.Image = global::btr.distrib.Properties.Resources.icons8_to_do_32;
            this.ControlFakturButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_to_do_32;
            this.ControlFakturButton.Name = "ControlFakturButton";
            this.ControlFakturButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("ControlFakturButton.SmallImage")));
            this.ControlFakturButton.Text = "Control";
            this.ControlFakturButton.Click += new System.EventHandler(this.ControlFakturButton_Click);
            // 
            // FakturPajakButton
            // 
            this.FakturPajakButton.Image = global::btr.distrib.Properties.Resources.tax2;
            this.FakturPajakButton.LargeImage = global::btr.distrib.Properties.Resources.tax2;
            this.FakturPajakButton.Name = "FakturPajakButton";
            this.FakturPajakButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("FakturPajakButton.SmallImage")));
            this.FakturPajakButton.Text = "Faktur-Pajak";
            this.FakturPajakButton.Click += new System.EventHandler(this.FakturPajakButton_Click);
            // 
            // MasterSalesPanel
            // 
            this.MasterSalesPanel.ButtonMoreEnabled = false;
            this.MasterSalesPanel.ButtonMoreVisible = false;
            this.MasterSalesPanel.Items.Add(this.OutletButton);
            this.MasterSalesPanel.Items.Add(this.KategoriButton);
            this.MasterSalesPanel.Items.Add(this.ribbonSeparator1);
            this.MasterSalesPanel.Items.Add(this.SalesPersonButton);
            this.MasterSalesPanel.Items.Add(this.WilayahButton);
            this.MasterSalesPanel.Name = "MasterSalesPanel";
            this.MasterSalesPanel.Text = "";
            // 
            // OutletButton
            // 
            this.OutletButton.Image = global::btr.distrib.Properties.Resources.icons8_3d_farm_32;
            this.OutletButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_3d_farm_32;
            this.OutletButton.Name = "OutletButton";
            this.OutletButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("OutletButton.SmallImage")));
            this.OutletButton.Text = "Outlet";
            this.OutletButton.Click += new System.EventHandler(this.OutletButton_Click);
            // 
            // KategoriButton
            // 
            this.KategoriButton.Image = global::btr.distrib.Properties.Resources.icons8_real_estate;
            this.KategoriButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_real_estate;
            this.KategoriButton.Name = "KategoriButton";
            this.KategoriButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("KategoriButton.SmallImage")));
            this.KategoriButton.Text = "Kategori";
            this.KategoriButton.Click += new System.EventHandler(this.KategoriButton_Click);
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // SalesPersonButton
            // 
            this.SalesPersonButton.Image = global::btr.distrib.Properties.Resources.icons8_caretaker;
            this.SalesPersonButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_caretaker;
            this.SalesPersonButton.Name = "SalesPersonButton";
            this.SalesPersonButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("SalesPersonButton.SmallImage")));
            this.SalesPersonButton.Text = "Sales";
            this.SalesPersonButton.Click += new System.EventHandler(this.SalesPersonButton_Click);
            // 
            // WilayahButton
            // 
            this.WilayahButton.Image = global::btr.distrib.Properties.Resources.icons8_map_marker;
            this.WilayahButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_map_marker;
            this.WilayahButton.Name = "WilayahButton";
            this.WilayahButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("WilayahButton.SmallImage")));
            this.WilayahButton.Text = "Wilayah";
            this.WilayahButton.Click += new System.EventHandler(this.WilayahButton_Click);
            // 
            // PurchaseTab
            // 
            this.PurchaseTab.Name = "PurchaseTab";
            this.PurchaseTab.Panels.Add(this.PurchaseOrderRibbonPanel);
            this.PurchaseTab.Panels.Add(this.InvoiceRibbonPanel);
            this.PurchaseTab.Panels.Add(this.MasterPurchaseRibbonPanel);
            this.PurchaseTab.Text = "Purchase";
            // 
            // PurchaseOrderRibbonPanel
            // 
            this.PurchaseOrderRibbonPanel.Items.Add(this.PoButton);
            this.PurchaseOrderRibbonPanel.Items.Add(this.InvoiceButton);
            this.PurchaseOrderRibbonPanel.Name = "PurchaseOrderRibbonPanel";
            this.PurchaseOrderRibbonPanel.Text = "Transaction";
            // 
            // PoButton
            // 
            this.PoButton.Image = global::btr.distrib.Properties.Resources.icons8_purchase_order;
            this.PoButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_purchase_order;
            this.PoButton.Name = "PoButton";
            this.PoButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("PoButton.SmallImage")));
            this.PoButton.Text = "PO";
            this.PoButton.Click += new System.EventHandler(this.PoButton_Click);
            // 
            // InvoiceButton
            // 
            this.InvoiceButton.Image = global::btr.distrib.Properties.Resources.icons8_day_view;
            this.InvoiceButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_day_view;
            this.InvoiceButton.Name = "InvoiceButton";
            this.InvoiceButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("InvoiceButton.SmallImage")));
            this.InvoiceButton.Text = "Invoice";
            this.InvoiceButton.Click += new System.EventHandler(this.InvoiceButton_Click);
            // 
            // InvoiceRibbonPanel
            // 
            this.InvoiceRibbonPanel.Items.Add(this.InvoiceReportButton);
            this.InvoiceRibbonPanel.Name = "InvoiceRibbonPanel";
            this.InvoiceRibbonPanel.Text = "";
            // 
            // InvoiceReportButton
            // 
            this.InvoiceReportButton.Image = global::btr.distrib.Properties.Resources.icons8_graph_32;
            this.InvoiceReportButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_graph_32;
            this.InvoiceReportButton.Name = "InvoiceReportButton";
            this.InvoiceReportButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("InvoiceReportButton.SmallImage")));
            this.InvoiceReportButton.Text = "Reporting";
            this.InvoiceReportButton.Click += new System.EventHandler(this.InvoiceReportButton_Click);
            // 
            // MasterPurchaseRibbonPanel
            // 
            this.MasterPurchaseRibbonPanel.Items.Add(this.SupplierButton);
            this.MasterPurchaseRibbonPanel.Name = "MasterPurchaseRibbonPanel";
            this.MasterPurchaseRibbonPanel.Text = "";
            // 
            // SupplierButton
            // 
            this.SupplierButton.Image = global::btr.distrib.Properties.Resources.icons8_factory;
            this.SupplierButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_factory;
            this.SupplierButton.Name = "SupplierButton";
            this.SupplierButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("SupplierButton.SmallImage")));
            this.SupplierButton.Text = "Supplier";
            this.SupplierButton.Click += new System.EventHandler(this.SupplierButton_Click);
            // 
            // InventoryTab
            // 
            this.InventoryTab.Name = "InventoryTab";
            this.InventoryTab.Panels.Add(this.InventoryTrsPanel);
            this.InventoryTab.Panels.Add(this.ReportingPanel);
            this.InventoryTab.Panels.Add(this.InventoryMasterPanel);
            this.InventoryTab.Text = "Inventory";
            // 
            // InventoryTrsPanel
            // 
            this.InventoryTrsPanel.Items.Add(this.OpnameButton);
            this.InventoryTrsPanel.Items.Add(this.ImportExcelOpnameButton);
            this.InventoryTrsPanel.Items.Add(this.PrintFakturButton);
            this.InventoryTrsPanel.Items.Add(this.PackingButton);
            this.InventoryTrsPanel.Items.Add(this.MutasiButton);
            this.InventoryTrsPanel.Name = "InventoryTrsPanel";
            this.InventoryTrsPanel.Text = "Transaction";
            // 
            // OpnameButton
            // 
            this.OpnameButton.Image = global::btr.distrib.Properties.Resources.icons8_hashtag_activity_feed_32;
            this.OpnameButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_hashtag_activity_feed_32;
            this.OpnameButton.Name = "OpnameButton";
            this.OpnameButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("OpnameButton.SmallImage")));
            this.OpnameButton.Text = "Opname";
            this.OpnameButton.Click += new System.EventHandler(this.OpnameButton_Click);
            // 
            // ImportExcelOpnameButton
            // 
            this.ImportExcelOpnameButton.Image = global::btr.distrib.Properties.Resources.icons8_microsoft_excel_32;
            this.ImportExcelOpnameButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_microsoft_excel_32;
            this.ImportExcelOpnameButton.Name = "ImportExcelOpnameButton";
            this.ImportExcelOpnameButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("ImportExcelOpnameButton.SmallImage")));
            this.ImportExcelOpnameButton.Text = "Import Excel";
            this.ImportExcelOpnameButton.Click += new System.EventHandler(this.ImportExcelOpnameButton_Click);
            // 
            // PrintFakturButton
            // 
            this.PrintFakturButton.Image = global::btr.distrib.Properties.Resources.icons8_print;
            this.PrintFakturButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_print;
            this.PrintFakturButton.Name = "PrintFakturButton";
            this.PrintFakturButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("PrintFakturButton.SmallImage")));
            this.PrintFakturButton.Text = "Print Faktur";
            this.PrintFakturButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // PackingButton
            // 
            this.PackingButton.Image = global::btr.distrib.Properties.Resources.icons8_winrar_32;
            this.PackingButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_winrar_32;
            this.PackingButton.Name = "PackingButton";
            this.PackingButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("PackingButton.SmallImage")));
            this.PackingButton.Text = "Packing";
            this.PackingButton.Click += new System.EventHandler(this.PackingButton_Click);
            // 
            // MutasiButton
            // 
            this.MutasiButton.Image = global::btr.distrib.Properties.Resources.icons8_python_32;
            this.MutasiButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_python_32;
            this.MutasiButton.Name = "MutasiButton";
            this.MutasiButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("MutasiButton.SmallImage")));
            this.MutasiButton.Text = "Mutasi";
            this.MutasiButton.Click += new System.EventHandler(this.MutasiButton_Click);
            // 
            // ReportingPanel
            // 
            this.ReportingPanel.Items.Add(this.StokBalanceButton);
            this.ReportingPanel.Items.Add(this.BukuStokButton);
            this.ReportingPanel.Name = "ReportingPanel";
            this.ReportingPanel.Text = "Reporting";
            // 
            // StokBalanceButton
            // 
            this.StokBalanceButton.Image = global::btr.distrib.Properties.Resources.icons8_vue_js_32;
            this.StokBalanceButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_vue_js_32;
            this.StokBalanceButton.Name = "StokBalanceButton";
            this.StokBalanceButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("StokBalanceButton.SmallImage")));
            this.StokBalanceButton.Text = "Stok Balance";
            this.StokBalanceButton.Click += new System.EventHandler(this.StokBalanceButton_Click);
            // 
            // BukuStokButton
            // 
            this.BukuStokButton.Image = global::btr.distrib.Properties.Resources.icons8_stack_overflow_32;
            this.BukuStokButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_stack_overflow_32;
            this.BukuStokButton.Name = "BukuStokButton";
            this.BukuStokButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("BukuStokButton.SmallImage")));
            this.BukuStokButton.Text = "Kartu Stok";
            // 
            // InventoryMasterPanel
            // 
            this.InventoryMasterPanel.Items.Add(this.BrgButton);
            this.InventoryMasterPanel.Items.Add(this.WarehouseButton);
            this.InventoryMasterPanel.Name = "InventoryMasterPanel";
            this.InventoryMasterPanel.Text = "Master Data";
            // 
            // BrgButton
            // 
            this.BrgButton.Image = global::btr.distrib.Properties.Resources.icons8_ingredients;
            this.BrgButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_ingredients;
            this.BrgButton.Name = "BrgButton";
            this.BrgButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("BrgButton.SmallImage")));
            this.BrgButton.Text = "Barang";
            this.BrgButton.Click += new System.EventHandler(this.BrgButton_Click);
            // 
            // WarehouseButton
            // 
            this.WarehouseButton.Image = global::btr.distrib.Properties.Resources.icons8_warehouse;
            this.WarehouseButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_warehouse;
            this.WarehouseButton.Name = "WarehouseButton";
            this.WarehouseButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("WarehouseButton.SmallImage")));
            this.WarehouseButton.Text = "Warehouse";
            this.WarehouseButton.Click += new System.EventHandler(this.WarehouseButton_Click);
            // 
            // FinanceTab
            // 
            this.FinanceTab.Name = "FinanceTab";
            this.FinanceTab.Text = "Finance";
            // 
            // SettingTab
            // 
            this.SettingTab.Name = "SettingTab";
            this.SettingTab.Panels.Add(this.DeliveryPanel);
            this.SettingTab.Panels.Add(this.ReceivingPanel);
            this.SettingTab.Text = "Setting";
            // 
            // DeliveryPanel
            // 
            this.DeliveryPanel.Items.Add(this.UserButton);
            this.DeliveryPanel.Items.Add(this.TestingButton);
            this.DeliveryPanel.Name = "DeliveryPanel";
            this.DeliveryPanel.Text = "";
            // 
            // UserButton
            // 
            this.UserButton.Image = global::btr.distrib.Properties.Resources.icons8_user_account_32;
            this.UserButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_user_account_32;
            this.UserButton.Name = "UserButton";
            this.UserButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("UserButton.SmallImage")));
            this.UserButton.Text = "User";
            this.UserButton.Click += new System.EventHandler(this.UserButton_Click);
            // 
            // TestingButton
            // 
            this.TestingButton.Image = global::btr.distrib.Properties.Resources.icons8_microscope_32;
            this.TestingButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_microscope_32;
            this.TestingButton.Name = "TestingButton";
            this.TestingButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("TestingButton.SmallImage")));
            this.TestingButton.Text = "Testing";
            // 
            // ReceivingPanel
            // 
            this.ReceivingPanel.Items.Add(this.AboutButton);
            this.ReceivingPanel.Name = "ReceivingPanel";
            this.ReceivingPanel.Text = "";
            // 
            // AboutButton
            // 
            this.AboutButton.Image = global::btr.distrib.Properties.Resources.icons8_about_32;
            this.AboutButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_about_32;
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("AboutButton.SmallImage")));
            this.AboutButton.Text = "About";
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // AppStatus
            // 
            this.AppStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginStatus,
            this.ServerDbStatus});
            this.AppStatus.Location = new System.Drawing.Point(0, 428);
            this.AppStatus.Name = "AppStatus";
            this.AppStatus.Size = new System.Drawing.Size(876, 22);
            this.AppStatus.TabIndex = 4;
            this.AppStatus.Text = "statusStrip1";
            // 
            // LoginStatus
            // 
            this.LoginStatus.Name = "LoginStatus";
            this.LoginStatus.Size = new System.Drawing.Size(37, 17);
            this.LoginStatus.Text = "Login";
            // 
            // ServerDbStatus
            // 
            this.ServerDbStatus.Name = "ServerDbStatus";
            this.ServerDbStatus.Size = new System.Drawing.Size(54, 17);
            this.ServerDbStatus.Text = "ServerDb";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::btr.distrib.Properties.Resources.app_wallpaper_5;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(876, 450);
            this.Controls.Add(this.AppStatus);
            this.Controls.Add(this.ribbon1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "BTR-App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.AppStatus.ResumeLayout(false);
            this.AppStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.RibbonButton TestingButton;

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab PurchaseTab;
        private System.Windows.Forms.RibbonPanel PurchaseOrderRibbonPanel;
        private System.Windows.Forms.RibbonPanel InvoiceRibbonPanel;
        private System.Windows.Forms.RibbonPanel MasterPurchaseRibbonPanel;
        private System.Windows.Forms.RibbonTab SalesTab;
        private System.Windows.Forms.RibbonPanel FakturPanel;
        private System.Windows.Forms.RibbonTab SettingTab;
        private System.Windows.Forms.RibbonTab InventoryTab;
        private System.Windows.Forms.RibbonTab FinanceTab;
        private System.Windows.Forms.RibbonPanel MasterSalesPanel;
        private System.Windows.Forms.RibbonPanel ReceivingPanel;
        private System.Windows.Forms.RibbonPanel DeliveryPanel;
        private System.Windows.Forms.RibbonPanel ReportingPanel;
        private System.Windows.Forms.RibbonPanel InventoryMasterPanel;
        private System.Windows.Forms.RibbonButton FakturButton;
        private System.Windows.Forms.RibbonButton FakturPajakButton;
        private System.Windows.Forms.RibbonButton ControlFakturButton;
        private System.Windows.Forms.RibbonButton OutletButton;
        private System.Windows.Forms.RibbonButton KategoriButton;
        private System.Windows.Forms.RibbonSeparator ribbonSeparator1;
        private System.Windows.Forms.RibbonButton SalesPersonButton;
        private System.Windows.Forms.RibbonButton WilayahButton;
        private System.Windows.Forms.RibbonButton PoButton;
        private System.Windows.Forms.RibbonButton InvoiceReportButton;
        private System.Windows.Forms.RibbonButton SupplierButton;
        private System.Windows.Forms.RibbonButton BrgButton;
        private System.Windows.Forms.RibbonButton WarehouseButton;
        private System.Windows.Forms.RibbonButton StokBalanceButton;
        private System.Windows.Forms.RibbonButton AboutButton;
        private System.Windows.Forms.RibbonButton UserButton;
        private System.Windows.Forms.StatusStrip AppStatus;
        private System.Windows.Forms.ToolStripStatusLabel LoginStatus;
        private System.Windows.Forms.ToolStripStatusLabel ServerDbStatus;
        private System.Windows.Forms.RibbonPanel InventoryTrsPanel;
        private System.Windows.Forms.RibbonButton OpnameButton;
        private System.Windows.Forms.RibbonButton PrintFakturButton;
        private System.Windows.Forms.RibbonButton PackingButton;
        private System.Windows.Forms.RibbonButton BukuStokButton;
        private System.Windows.Forms.RibbonButton InvoiceButton;
        private System.Windows.Forms.RibbonButton MutasiButton;
        private System.Windows.Forms.RibbonButton ImportExcelOpnameButton;
        private System.Windows.Forms.RibbonButton InfoFakturJualButton;
        private System.Windows.Forms.RibbonSeparator ribbonSeparator2;
    }
}