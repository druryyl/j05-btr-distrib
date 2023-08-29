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
            this.ReturJualButton = new System.Windows.Forms.RibbonButton();
            this.ReportFakturButton = new System.Windows.Forms.RibbonButton();
            this.MasterSalesPanel = new System.Windows.Forms.RibbonPanel();
            this.OutletButton = new System.Windows.Forms.RibbonButton();
            this.KategoriButton = new System.Windows.Forms.RibbonButton();
            this.ribbonSeparator1 = new System.Windows.Forms.RibbonSeparator();
            this.SalesPersonButton = new System.Windows.Forms.RibbonButton();
            this.WilayahButton = new System.Windows.Forms.RibbonButton();
            this.PurchaseTab = new System.Windows.Forms.RibbonTab();
            this.PurchaseOrderRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.PoButton = new System.Windows.Forms.RibbonButton();
            this.PoReportButton = new System.Windows.Forms.RibbonButton();
            this.InvoiceRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.InvoiceButton = new System.Windows.Forms.RibbonButton();
            this.InvoiceReportButton = new System.Windows.Forms.RibbonButton();
            this.MasterPurchaseRibbonPanel = new System.Windows.Forms.RibbonPanel();
            this.SupplierButton = new System.Windows.Forms.RibbonButton();
            this.InventoryTab = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.BrgButton = new System.Windows.Forms.RibbonButton();
            this.WarehouseButton = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.PrintButton = new System.Windows.Forms.RibbonButton();
            this.FinanceTab = new System.Windows.Forms.RibbonTab();
            this.DistributionTab = new System.Windows.Forms.RibbonTab();
            this.ReceivingPanel = new System.Windows.Forms.RibbonPanel();
            this.DeliveryPanel = new System.Windows.Forms.RibbonPanel();
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
            this.ribbon1.Size = new System.Drawing.Size(876, 106);
            this.ribbon1.TabIndex = 2;
            this.ribbon1.Tabs.Add(this.SalesTab);
            this.ribbon1.Tabs.Add(this.PurchaseTab);
            this.ribbon1.Tabs.Add(this.InventoryTab);
            this.ribbon1.Tabs.Add(this.FinanceTab);
            this.ribbon1.Tabs.Add(this.DistributionTab);
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
            this.FakturPanel.Items.Add(this.FakturButton);
            this.FakturPanel.Items.Add(this.ReturJualButton);
            this.FakturPanel.Items.Add(this.ReportFakturButton);
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
            // ReturJualButton
            // 
            this.ReturJualButton.Image = global::btr.distrib.Properties.Resources.icons8_import;
            this.ReturJualButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_import;
            this.ReturJualButton.Name = "ReturJualButton";
            this.ReturJualButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("ReturJualButton.SmallImage")));
            this.ReturJualButton.Text = "Retur";
            // 
            // ReportFakturButton
            // 
            this.ReportFakturButton.Image = global::btr.distrib.Properties.Resources.icons8_documents;
            this.ReportFakturButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_documents;
            this.ReportFakturButton.Name = "ReportFakturButton";
            this.ReportFakturButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("ReportFakturButton.SmallImage")));
            this.ReportFakturButton.Text = "Reporting";
            // 
            // MasterSalesPanel
            // 
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
            this.PurchaseOrderRibbonPanel.Items.Add(this.PoReportButton);
            this.PurchaseOrderRibbonPanel.Name = "PurchaseOrderRibbonPanel";
            this.PurchaseOrderRibbonPanel.Text = "";
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
            // PoReportButton
            // 
            this.PoReportButton.Image = global::btr.distrib.Properties.Resources.icons8_documents;
            this.PoReportButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_documents;
            this.PoReportButton.Name = "PoReportButton";
            this.PoReportButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("PoReportButton.SmallImage")));
            this.PoReportButton.Text = "Reporting";
            // 
            // InvoiceRibbonPanel
            // 
            this.InvoiceRibbonPanel.Items.Add(this.InvoiceButton);
            this.InvoiceRibbonPanel.Items.Add(this.InvoiceReportButton);
            this.InvoiceRibbonPanel.Name = "InvoiceRibbonPanel";
            this.InvoiceRibbonPanel.Text = "";
            // 
            // InvoiceButton
            // 
            this.InvoiceButton.Image = global::btr.distrib.Properties.Resources.icons8_bill;
            this.InvoiceButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_bill;
            this.InvoiceButton.Name = "InvoiceButton";
            this.InvoiceButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("InvoiceButton.SmallImage")));
            this.InvoiceButton.Text = "Invoice";
            // 
            // InvoiceReportButton
            // 
            this.InvoiceReportButton.Image = global::btr.distrib.Properties.Resources.icons8_graph_32;
            this.InvoiceReportButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_graph_32;
            this.InvoiceReportButton.Name = "InvoiceReportButton";
            this.InvoiceReportButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("InvoiceReportButton.SmallImage")));
            this.InvoiceReportButton.Text = "Reporting";
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
            this.InventoryTab.Panels.Add(this.ribbonPanel2);
            this.InventoryTab.Panels.Add(this.ribbonPanel1);
            this.InventoryTab.Text = "Inventory";
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Items.Add(this.BrgButton);
            this.ribbonPanel2.Items.Add(this.WarehouseButton);
            this.ribbonPanel2.Name = "ribbonPanel2";
            this.ribbonPanel2.Text = "";
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
            // ribbonPanel1
            // 
            this.ribbonPanel1.Items.Add(this.PrintButton);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Text = "";
            // 
            // PrintButton
            // 
            this.PrintButton.Image = global::btr.distrib.Properties.Resources.icons8_print;
            this.PrintButton.LargeImage = global::btr.distrib.Properties.Resources.icons8_print;
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("PrintButton.SmallImage")));
            this.PrintButton.Text = "Print";
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // FinanceTab
            // 
            this.FinanceTab.Name = "FinanceTab";
            this.FinanceTab.Text = "Finance";
            // 
            // DistributionTab
            // 
            this.DistributionTab.Name = "DistributionTab";
            this.DistributionTab.Panels.Add(this.ReceivingPanel);
            this.DistributionTab.Panels.Add(this.DeliveryPanel);
            this.DistributionTab.Text = "Distribusi";
            this.DistributionTab.Visible = false;
            // 
            // ReceivingPanel
            // 
            this.ReceivingPanel.Name = "ReceivingPanel";
            this.ReceivingPanel.Text = "Receiving";
            // 
            // DeliveryPanel
            // 
            this.DeliveryPanel.Name = "DeliveryPanel";
            this.DeliveryPanel.Text = "Delivery";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::btr.distrib.Properties.Resources.app_wallpaper_4;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(876, 450);
            this.Controls.Add(this.ribbon1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab PurchaseTab;
        private System.Windows.Forms.RibbonPanel PurchaseOrderRibbonPanel;
        private System.Windows.Forms.RibbonPanel InvoiceRibbonPanel;
        private System.Windows.Forms.RibbonPanel MasterPurchaseRibbonPanel;
        private System.Windows.Forms.RibbonTab SalesTab;
        private System.Windows.Forms.RibbonPanel FakturPanel;
        private System.Windows.Forms.RibbonTab DistributionTab;
        private System.Windows.Forms.RibbonTab InventoryTab;
        private System.Windows.Forms.RibbonTab FinanceTab;
        private System.Windows.Forms.RibbonPanel MasterSalesPanel;
        private System.Windows.Forms.RibbonPanel ReceivingPanel;
        private System.Windows.Forms.RibbonPanel DeliveryPanel;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonButton FakturButton;
        private System.Windows.Forms.RibbonButton ReturJualButton;
        private System.Windows.Forms.RibbonButton ReportFakturButton;
        private System.Windows.Forms.RibbonButton OutletButton;
        private System.Windows.Forms.RibbonButton KategoriButton;
        private System.Windows.Forms.RibbonSeparator ribbonSeparator1;
        private System.Windows.Forms.RibbonButton SalesPersonButton;
        private System.Windows.Forms.RibbonButton WilayahButton;
        private System.Windows.Forms.RibbonButton PoButton;
        private System.Windows.Forms.RibbonButton PoReportButton;
        private System.Windows.Forms.RibbonButton InvoiceButton;
        private System.Windows.Forms.RibbonButton InvoiceReportButton;
        private System.Windows.Forms.RibbonButton SupplierButton;
        private System.Windows.Forms.RibbonButton BrgButton;
        private System.Windows.Forms.RibbonButton WarehouseButton;
        private System.Windows.Forms.RibbonButton PrintButton;
    }
}