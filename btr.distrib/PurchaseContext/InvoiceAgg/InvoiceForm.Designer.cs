namespace btr.distrib.PurchaseContext.InvoiceAgg
{
    partial class InvoiceForm
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
            this.TermOfPaymentCombo = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.NoFakturPajakText = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SupplierIdText = new System.Windows.Forms.TextBox();
            this.SupplierNameText = new System.Windows.Forms.TextBox();
            this.SupplierButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.InvoiceIdText = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.WarehouseIdText = new System.Windows.Forms.TextBox();
            this.WarehouseNameText = new System.Windows.Forms.TextBox();
            this.WarehouseButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DueDateText = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PanelAtas4 = new System.Windows.Forms.Panel();
            this.VoidPanel = new System.Windows.Forms.Panel();
            this.CancelLabel = new System.Windows.Forms.Label();
            this.NoteTextBox = new System.Windows.Forms.TextBox();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.InvoiceItemGrid = new System.Windows.Forms.DataGridView();
            this.PanelTengah = new System.Windows.Forms.Panel();
            this.TaxText = new System.Windows.Forms.NumericUpDown();
            this.BiayaLainLabel = new System.Windows.Forms.Label();
            this.SisaText = new System.Windows.Forms.NumericUpDown();
            this.SisaLabel = new System.Windows.Forms.Label();
            this.UangMukaText = new System.Windows.Forms.NumericUpDown();
            this.UangMukaLabel = new System.Windows.Forms.Label();
            this.GrandTotalText = new System.Windows.Forms.NumericUpDown();
            this.GrandTotalLabel = new System.Windows.Forms.Label();
            this.DiscountText = new System.Windows.Forms.NumericUpDown();
            this.DisconutLainLabel = new System.Windows.Forms.Label();
            this.TotalText = new System.Windows.Forms.NumericUpDown();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LastIdLabel = new System.Windows.Forms.Label();
            this.InvoiceCodeText = new System.Windows.Forms.TextBox();
            this.InvoiceDateText = new System.Windows.Forms.DateTimePicker();
            this.InvoiceCodeLabel = new System.Windows.Forms.Label();
            this.InvoiceDateLabel = new System.Windows.Forms.Label();
            this.InvoiceButton = new System.Windows.Forms.Button();
            this.InvoiceLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.PrintButton = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.PanelAtas4.SuspendLayout();
            this.VoidPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceItemGrid)).BeginInit();
            this.PanelTengah.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TaxText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SisaText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UangMukaText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrandTotalText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscountText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalText)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TermOfPaymentCombo
            // 
            this.TermOfPaymentCombo.FormattingEnabled = true;
            this.TermOfPaymentCombo.Items.AddRange(new object[] { "Credit", "Cash" });
            this.TermOfPaymentCombo.Location = new System.Drawing.Point(6, 24);
            this.TermOfPaymentCombo.Name = "TermOfPaymentCombo";
            this.TermOfPaymentCombo.Size = new System.Drawing.Size(175, 21);
            this.TermOfPaymentCombo.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Honeydew;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.NoFakturPajakText);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.SupplierIdText);
            this.panel2.Controls.Add(this.SupplierNameText);
            this.panel2.Controls.Add(this.SupplierButton);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(212, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 186);
            this.panel2.TabIndex = 50;
            // 
            // NoFakturPajakText
            // 
            this.NoFakturPajakText.BeepOnError = true;
            this.NoFakturPajakText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NoFakturPajakText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoFakturPajakText.Location = new System.Drawing.Point(11, 91);
            this.NoFakturPajakText.Mask = "000-000-00-00000000";
            this.NoFakturPajakText.Name = "NoFakturPajakText";
            this.NoFakturPajakText.Size = new System.Drawing.Size(176, 20);
            this.NoFakturPajakText.TabIndex = 24;
            this.NoFakturPajakText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "No. Faktur Pajak";
            // 
            // SupplierIdText
            // 
            this.SupplierIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SupplierIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupplierIdText.Location = new System.Drawing.Point(11, 22);
            this.SupplierIdText.Name = "SupplierIdText";
            this.SupplierIdText.Size = new System.Drawing.Size(147, 22);
            this.SupplierIdText.TabIndex = 19;
            // 
            // SupplierNameText
            // 
            this.SupplierNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SupplierNameText.Location = new System.Drawing.Point(11, 50);
            this.SupplierNameText.Name = "SupplierNameText";
            this.SupplierNameText.ReadOnly = true;
            this.SupplierNameText.Size = new System.Drawing.Size(176, 22);
            this.SupplierNameText.TabIndex = 21;
            // 
            // SupplierButton
            // 
            this.SupplierButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.SupplierButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SupplierButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupplierButton.Location = new System.Drawing.Point(161, 22);
            this.SupplierButton.Name = "SupplierButton";
            this.SupplierButton.Size = new System.Drawing.Size(26, 22);
            this.SupplierButton.TabIndex = 20;
            this.SupplierButton.Text = "...";
            this.SupplierButton.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Supplier ID";
            // 
            // InvoiceIdText
            // 
            this.InvoiceIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InvoiceIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvoiceIdText.Location = new System.Drawing.Point(11, 23);
            this.InvoiceIdText.Name = "InvoiceIdText";
            this.InvoiceIdText.ReadOnly = true;
            this.InvoiceIdText.Size = new System.Drawing.Size(147, 22);
            this.InvoiceIdText.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Honeydew;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.WarehouseIdText);
            this.panel3.Controls.Add(this.WarehouseNameText);
            this.panel3.Controls.Add(this.WarehouseButton);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(418, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 186);
            this.panel3.TabIndex = 51;
            // 
            // WarehouseIdText
            // 
            this.WarehouseIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WarehouseIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarehouseIdText.Location = new System.Drawing.Point(11, 22);
            this.WarehouseIdText.Name = "WarehouseIdText";
            this.WarehouseIdText.Size = new System.Drawing.Size(147, 22);
            this.WarehouseIdText.TabIndex = 11;
            // 
            // WarehouseNameText
            // 
            this.WarehouseNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WarehouseNameText.Location = new System.Drawing.Point(11, 50);
            this.WarehouseNameText.Name = "WarehouseNameText";
            this.WarehouseNameText.ReadOnly = true;
            this.WarehouseNameText.Size = new System.Drawing.Size(176, 22);
            this.WarehouseNameText.TabIndex = 13;
            // 
            // WarehouseButton
            // 
            this.WarehouseButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.WarehouseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WarehouseButton.Location = new System.Drawing.Point(161, 22);
            this.WarehouseButton.Name = "WarehouseButton";
            this.WarehouseButton.Size = new System.Drawing.Size(26, 22);
            this.WarehouseButton.TabIndex = 12;
            this.WarehouseButton.Text = "...";
            this.WarehouseButton.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Warehouse";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Honeydew;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.DueDateText);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.TermOfPaymentCombo);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(624, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 186);
            this.panel4.TabIndex = 54;
            // 
            // DueDateText
            // 
            this.DueDateText.CustomFormat = "ddd dd-MM-yyyy";
            this.DueDateText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DueDateText.Location = new System.Drawing.Point(6, 64);
            this.DueDateText.Name = "DueDateText";
            this.DueDateText.Size = new System.Drawing.Size(176, 22);
            this.DueDateText.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Jatuh Tempo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Term Of Payment";
            // 
            // PanelAtas4
            // 
            this.PanelAtas4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelAtas4.BackColor = System.Drawing.Color.Cornsilk;
            this.PanelAtas4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelAtas4.Controls.Add(this.VoidPanel);
            this.PanelAtas4.Controls.Add(this.NoteTextBox);
            this.PanelAtas4.Controls.Add(this.NoteLabel);
            this.PanelAtas4.Location = new System.Drawing.Point(830, 9);
            this.PanelAtas4.Name = "PanelAtas4";
            this.PanelAtas4.Size = new System.Drawing.Size(270, 186);
            this.PanelAtas4.TabIndex = 52;
            // 
            // VoidPanel
            // 
            this.VoidPanel.Controls.Add(this.CancelLabel);
            this.VoidPanel.Location = new System.Drawing.Point(-1, -1);
            this.VoidPanel.Name = "VoidPanel";
            this.VoidPanel.Size = new System.Drawing.Size(270, 186);
            this.VoidPanel.TabIndex = 46;
            this.VoidPanel.Visible = false;
            // 
            // CancelLabel
            // 
            this.CancelLabel.BackColor = System.Drawing.Color.Honeydew;
            this.CancelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelLabel.ForeColor = System.Drawing.Color.Red;
            this.CancelLabel.Location = new System.Drawing.Point(0, 0);
            this.CancelLabel.Name = "CancelLabel";
            this.CancelLabel.Size = new System.Drawing.Size(270, 186);
            this.CancelLabel.TabIndex = 23;
            this.CancelLabel.Text = "CANCELLED";
            this.CancelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NoteTextBox
            // 
            this.NoteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.NoteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NoteTextBox.Location = new System.Drawing.Point(11, 24);
            this.NoteTextBox.Multiline = true;
            this.NoteTextBox.Name = "NoteTextBox";
            this.NoteTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NoteTextBox.Size = new System.Drawing.Size(247, 144);
            this.NoteTextBox.TabIndex = 17;
            // 
            // NoteLabel
            // 
            this.NoteLabel.AutoSize = true;
            this.NoteLabel.Location = new System.Drawing.Point(8, 8);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(32, 13);
            this.NoteLabel.TabIndex = 0;
            this.NoteLabel.Text = "Note";
            // 
            // InvoiceItemGrid
            // 
            this.InvoiceItemGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.InvoiceItemGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InvoiceItemGrid.Location = new System.Drawing.Point(6, 201);
            this.InvoiceItemGrid.Name = "InvoiceItemGrid";
            this.InvoiceItemGrid.Size = new System.Drawing.Size(950, 284);
            this.InvoiceItemGrid.TabIndex = 46;
            // 
            // PanelTengah
            // 
            this.PanelTengah.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelTengah.BackColor = System.Drawing.Color.Honeydew;
            this.PanelTengah.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTengah.Controls.Add(this.TaxText);
            this.PanelTengah.Controls.Add(this.BiayaLainLabel);
            this.PanelTengah.Controls.Add(this.SisaText);
            this.PanelTengah.Controls.Add(this.SisaLabel);
            this.PanelTengah.Controls.Add(this.UangMukaText);
            this.PanelTengah.Controls.Add(this.UangMukaLabel);
            this.PanelTengah.Controls.Add(this.GrandTotalText);
            this.PanelTengah.Controls.Add(this.GrandTotalLabel);
            this.PanelTengah.Controls.Add(this.DiscountText);
            this.PanelTengah.Controls.Add(this.DisconutLainLabel);
            this.PanelTengah.Controls.Add(this.TotalText);
            this.PanelTengah.Controls.Add(this.TotalLabel);
            this.PanelTengah.Location = new System.Drawing.Point(962, 201);
            this.PanelTengah.Name = "PanelTengah";
            this.PanelTengah.Size = new System.Drawing.Size(138, 284);
            this.PanelTengah.TabIndex = 53;
            // 
            // TaxText
            // 
            this.TaxText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaxText.Location = new System.Drawing.Point(11, 102);
            this.TaxText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.TaxText.Name = "TaxText";
            this.TaxText.ReadOnly = true;
            this.TaxText.Size = new System.Drawing.Size(117, 20);
            this.TaxText.TabIndex = 21;
            this.TaxText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TaxText.ThousandsSeparator = true;
            // 
            // BiayaLainLabel
            // 
            this.BiayaLainLabel.AutoSize = true;
            this.BiayaLainLabel.Location = new System.Drawing.Point(8, 86);
            this.BiayaLainLabel.Name = "BiayaLainLabel";
            this.BiayaLainLabel.Size = new System.Drawing.Size(23, 13);
            this.BiayaLainLabel.TabIndex = 14;
            this.BiayaLainLabel.Text = "Tax";
            // 
            // SisaText
            // 
            this.SisaText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SisaText.Location = new System.Drawing.Point(11, 248);
            this.SisaText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.SisaText.Name = "SisaText";
            this.SisaText.ReadOnly = true;
            this.SisaText.Size = new System.Drawing.Size(117, 20);
            this.SisaText.TabIndex = 24;
            this.SisaText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SisaText.ThousandsSeparator = true;
            // 
            // SisaLabel
            // 
            this.SisaLabel.AutoSize = true;
            this.SisaLabel.Location = new System.Drawing.Point(8, 232);
            this.SisaLabel.Name = "SisaLabel";
            this.SisaLabel.Size = new System.Drawing.Size(27, 13);
            this.SisaLabel.TabIndex = 12;
            this.SisaLabel.Text = "Sisa";
            // 
            // UangMukaText
            // 
            this.UangMukaText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UangMukaText.Location = new System.Drawing.Point(11, 209);
            this.UangMukaText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.UangMukaText.Name = "UangMukaText";
            this.UangMukaText.Size = new System.Drawing.Size(117, 20);
            this.UangMukaText.TabIndex = 23;
            this.UangMukaText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.UangMukaText.ThousandsSeparator = true;
            // 
            // UangMukaLabel
            // 
            this.UangMukaLabel.AutoSize = true;
            this.UangMukaLabel.Location = new System.Drawing.Point(8, 193);
            this.UangMukaLabel.Name = "UangMukaLabel";
            this.UangMukaLabel.Size = new System.Drawing.Size(67, 13);
            this.UangMukaLabel.TabIndex = 10;
            this.UangMukaLabel.Text = "Uang Muka";
            // 
            // GrandTotalText
            // 
            this.GrandTotalText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrandTotalText.Location = new System.Drawing.Point(9, 141);
            this.GrandTotalText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.GrandTotalText.Name = "GrandTotalText";
            this.GrandTotalText.ReadOnly = true;
            this.GrandTotalText.Size = new System.Drawing.Size(117, 20);
            this.GrandTotalText.TabIndex = 22;
            this.GrandTotalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.GrandTotalText.ThousandsSeparator = true;
            // 
            // GrandTotalLabel
            // 
            this.GrandTotalLabel.AutoSize = true;
            this.GrandTotalLabel.Location = new System.Drawing.Point(8, 125);
            this.GrandTotalLabel.Name = "GrandTotalLabel";
            this.GrandTotalLabel.Size = new System.Drawing.Size(67, 13);
            this.GrandTotalLabel.TabIndex = 8;
            this.GrandTotalLabel.Text = "Grand Total";
            // 
            // DiscountText
            // 
            this.DiscountText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscountText.Location = new System.Drawing.Point(11, 63);
            this.DiscountText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.DiscountText.Name = "DiscountText";
            this.DiscountText.ReadOnly = true;
            this.DiscountText.Size = new System.Drawing.Size(117, 20);
            this.DiscountText.TabIndex = 20;
            this.DiscountText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DiscountText.ThousandsSeparator = true;
            // 
            // DisconutLainLabel
            // 
            this.DisconutLainLabel.AutoSize = true;
            this.DisconutLainLabel.Location = new System.Drawing.Point(8, 47);
            this.DisconutLainLabel.Name = "DisconutLainLabel";
            this.DisconutLainLabel.Size = new System.Drawing.Size(53, 13);
            this.DisconutLainLabel.TabIndex = 6;
            this.DisconutLainLabel.Text = "Discount";
            // 
            // TotalText
            // 
            this.TotalText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalText.Location = new System.Drawing.Point(11, 24);
            this.TotalText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.TotalText.Name = "TotalText";
            this.TotalText.ReadOnly = true;
            this.TotalText.Size = new System.Drawing.Size(117, 20);
            this.TotalText.TabIndex = 19;
            this.TotalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TotalText.ThousandsSeparator = true;
            // 
            // TotalLabel
            // 
            this.TotalLabel.AutoSize = true;
            this.TotalLabel.Location = new System.Drawing.Point(8, 8);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(32, 13);
            this.TotalLabel.TabIndex = 4;
            this.TotalLabel.Text = "Total";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Honeydew;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.InvoiceCodeText);
            this.panel1.Controls.Add(this.InvoiceIdText);
            this.panel1.Controls.Add(this.InvoiceDateText);
            this.panel1.Controls.Add(this.InvoiceCodeLabel);
            this.panel1.Controls.Add(this.InvoiceDateLabel);
            this.panel1.Controls.Add(this.InvoiceButton);
            this.panel1.Controls.Add(this.InvoiceLabel);
            this.panel1.Location = new System.Drawing.Point(6, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 186);
            this.panel1.TabIndex = 49;
            // 
            // LastIdLabel
            // 
            this.LastIdLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastIdLabel.Location = new System.Drawing.Point(11, 48);
            this.LastIdLabel.Name = "LastIdLabel";
            this.LastIdLabel.Size = new System.Drawing.Size(175, 13);
            this.LastIdLabel.TabIndex = 16;
            this.LastIdLabel.Text = "Invoice ID";
            this.LastIdLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // InvoiceCodeText
            // 
            this.InvoiceCodeText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InvoiceCodeText.Location = new System.Drawing.Point(8, 132);
            this.InvoiceCodeText.Name = "InvoiceCodeText";
            this.InvoiceCodeText.Size = new System.Drawing.Size(176, 22);
            this.InvoiceCodeText.TabIndex = 15;
            // 
            // InvoiceDateText
            // 
            this.InvoiceDateText.CustomFormat = "ddd dd-MM-yyyy HH:mm";
            this.InvoiceDateText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.InvoiceDateText.Location = new System.Drawing.Point(8, 91);
            this.InvoiceDateText.Name = "InvoiceDateText";
            this.InvoiceDateText.ShowUpDown = true;
            this.InvoiceDateText.Size = new System.Drawing.Size(176, 22);
            this.InvoiceDateText.TabIndex = 2;
            // 
            // InvoiceCodeLabel
            // 
            this.InvoiceCodeLabel.AutoSize = true;
            this.InvoiceCodeLabel.Location = new System.Drawing.Point(5, 116);
            this.InvoiceCodeLabel.Name = "InvoiceCodeLabel";
            this.InvoiceCodeLabel.Size = new System.Drawing.Size(87, 13);
            this.InvoiceCodeLabel.TabIndex = 14;
            this.InvoiceCodeLabel.Text = "Invoice Number";
            // 
            // InvoiceDateLabel
            // 
            this.InvoiceDateLabel.AutoSize = true;
            this.InvoiceDateLabel.Location = new System.Drawing.Point(5, 75);
            this.InvoiceDateLabel.Name = "InvoiceDateLabel";
            this.InvoiceDateLabel.Size = new System.Drawing.Size(70, 13);
            this.InvoiceDateLabel.TabIndex = 3;
            this.InvoiceDateLabel.Text = "Invoice Date";
            // 
            // InvoiceButton
            // 
            this.InvoiceButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.InvoiceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InvoiceButton.Location = new System.Drawing.Point(161, 23);
            this.InvoiceButton.Name = "InvoiceButton";
            this.InvoiceButton.Size = new System.Drawing.Size(26, 22);
            this.InvoiceButton.TabIndex = 1;
            this.InvoiceButton.Text = "...";
            this.InvoiceButton.UseVisualStyleBackColor = false;
            // 
            // InvoiceLabel
            // 
            this.InvoiceLabel.AutoSize = true;
            this.InvoiceLabel.Location = new System.Drawing.Point(8, 8);
            this.InvoiceLabel.Name = "InvoiceLabel";
            this.InvoiceLabel.Size = new System.Drawing.Size(57, 13);
            this.InvoiceLabel.TabIndex = 0;
            this.InvoiceLabel.Text = "Invoice ID";
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(1025, 491);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 47;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewButton.Location = new System.Drawing.Point(6, 491);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 48;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // PrintButton
            // 
            this.PrintButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintButton.Location = new System.Drawing.Point(87, 491);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(75, 23);
            this.PrintButton.TabIndex = 55;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(1106, 523);
            this.Controls.Add(this.PrintButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.PanelAtas4);
            this.Controls.Add(this.InvoiceItemGrid);
            this.Controls.Add(this.PanelTengah);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.NewButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InvoiceForm";
            this.Text = "InvoiceForm";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.PanelAtas4.ResumeLayout(false);
            this.PanelAtas4.PerformLayout();
            this.VoidPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceItemGrid)).EndInit();
            this.PanelTengah.ResumeLayout(false);
            this.PanelTengah.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TaxText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SisaText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UangMukaText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrandTotalText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscountText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalText)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button PrintButton;

        #endregion
        private System.Windows.Forms.ComboBox TermOfPaymentCombo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox InvoiceIdText;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox WarehouseIdText;
        private System.Windows.Forms.TextBox WarehouseNameText;
        private System.Windows.Forms.Button WarehouseButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DateTimePicker DueDateText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel PanelAtas4;
        private System.Windows.Forms.Panel VoidPanel;
        private System.Windows.Forms.Label CancelLabel;
        private System.Windows.Forms.TextBox NoteTextBox;
        private System.Windows.Forms.Label NoteLabel;
        private System.Windows.Forms.DataGridView InvoiceItemGrid;
        private System.Windows.Forms.Panel PanelTengah;
        private System.Windows.Forms.NumericUpDown TaxText;
        private System.Windows.Forms.Label BiayaLainLabel;
        private System.Windows.Forms.NumericUpDown SisaText;
        private System.Windows.Forms.Label SisaLabel;
        private System.Windows.Forms.NumericUpDown UangMukaText;
        private System.Windows.Forms.Label UangMukaLabel;
        private System.Windows.Forms.NumericUpDown GrandTotalText;
        private System.Windows.Forms.Label GrandTotalLabel;
        private System.Windows.Forms.NumericUpDown DiscountText;
        private System.Windows.Forms.Label DisconutLainLabel;
        private System.Windows.Forms.NumericUpDown TotalText;
        private System.Windows.Forms.Label TotalLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker InvoiceDateText;
        private System.Windows.Forms.Label InvoiceDateLabel;
        private System.Windows.Forms.Button InvoiceButton;
        private System.Windows.Forms.Label InvoiceLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.TextBox SupplierIdText;
        private System.Windows.Forms.TextBox SupplierNameText;
        private System.Windows.Forms.Button SupplierButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox InvoiceCodeText;
        private System.Windows.Forms.Label InvoiceCodeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox NoFakturPajakText;
        private System.Windows.Forms.Label LastIdLabel;
    }
}