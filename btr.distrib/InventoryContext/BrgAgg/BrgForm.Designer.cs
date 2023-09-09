namespace btr.distrib.InventoryContext.BrgAgg
{
    partial class BrgForm
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
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.BrgGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.HppTimestampText = new System.Windows.Forms.DateTimePicker();
            this.HppText = new System.Windows.Forms.NumericUpDown();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BrgCodeText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrgIdText = new System.Windows.Forms.TextBox();
            this.BrgNameText = new System.Windows.Forms.TextBox();
            this.BrgButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.KategoriIdText = new System.Windows.Forms.TextBox();
            this.KategoriNameText = new System.Windows.Forms.TextBox();
            this.KategoriButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.JenisBrgCombo = new System.Windows.Forms.ComboBox();
            this.SupplierIdText = new System.Windows.Forms.TextBox();
            this.SupplierNameText = new System.Windows.Forms.TextBox();
            this.SupplierButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SatuanTab = new System.Windows.Forms.TabPage();
            this.SatuanGrid = new System.Windows.Forms.DataGridView();
            this.HargaTab = new System.Windows.Forms.TabPage();
            this.HargaGrid = new System.Windows.Forms.DataGridView();
            this.StokTab = new System.Windows.Forms.TabPage();
            this.StokGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HppText)).BeginInit();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SatuanTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SatuanGrid)).BeginInit();
            this.HargaTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HargaGrid)).BeginInit();
            this.StokTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StokGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchText
            // 
            this.SearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchText.Location = new System.Drawing.Point(7, 395);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(349, 22);
            this.SearchText.TabIndex = 0;
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(361, 394);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // BrgGrid
            // 
            this.BrgGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGrid.Location = new System.Drawing.Point(6, 8);
            this.BrgGrid.Name = "BrgGrid";
            this.BrgGrid.Size = new System.Drawing.Size(430, 380);
            this.BrgGrid.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.HppTimestampText);
            this.panel2.Controls.Add(this.HppText);
            this.panel2.Controls.Add(this.TotalLabel);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.BrgCodeText);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.BrgIdText);
            this.panel2.Controls.Add(this.BrgNameText);
            this.panel2.Controls.Add(this.BrgButton);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(442, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 211);
            this.panel2.TabIndex = 47;
            // 
            // HppTimestampText
            // 
            this.HppTimestampText.CalendarMonthBackground = System.Drawing.SystemColors.InactiveCaption;
            this.HppTimestampText.CustomFormat = "ddd, dd-MMM-yyyy";
            this.HppTimestampText.Enabled = false;
            this.HppTimestampText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.HppTimestampText.Location = new System.Drawing.Point(12, 174);
            this.HppTimestampText.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.HppTimestampText.Name = "HppTimestampText";
            this.HppTimestampText.Size = new System.Drawing.Size(205, 22);
            this.HppTimestampText.TabIndex = 21;
            // 
            // HppText
            // 
            this.HppText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HppText.Location = new System.Drawing.Point(12, 148);
            this.HppText.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.HppText.Name = "HppText";
            this.HppText.ReadOnly = true;
            this.HppText.Size = new System.Drawing.Size(205, 20);
            this.HppText.TabIndex = 20;
            this.HppText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.HppText.ThousandsSeparator = true;
            // 
            // TotalLabel
            // 
            this.TotalLabel.AutoSize = true;
            this.TotalLabel.Location = new System.Drawing.Point(9, 132);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(27, 13);
            this.TotalLabel.TabIndex = 19;
            this.TotalLabel.Text = "HPP";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(173, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Aktif";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Code";
            // 
            // BrgCodeText
            // 
            this.BrgCodeText.Location = new System.Drawing.Point(12, 107);
            this.BrgCodeText.Name = "BrgCodeText";
            this.BrgCodeText.Size = new System.Drawing.Size(205, 22);
            this.BrgCodeText.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Brg Name";
            // 
            // BrgIdText
            // 
            this.BrgIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrgIdText.Location = new System.Drawing.Point(12, 25);
            this.BrgIdText.Name = "BrgIdText";
            this.BrgIdText.Size = new System.Drawing.Size(177, 22);
            this.BrgIdText.TabIndex = 6;
            // 
            // BrgNameText
            // 
            this.BrgNameText.Location = new System.Drawing.Point(12, 66);
            this.BrgNameText.Name = "BrgNameText";
            this.BrgNameText.Size = new System.Drawing.Size(205, 22);
            this.BrgNameText.TabIndex = 8;
            // 
            // BrgButton
            // 
            this.BrgButton.Location = new System.Drawing.Point(195, 25);
            this.BrgButton.Name = "BrgButton";
            this.BrgButton.Size = new System.Drawing.Size(28, 23);
            this.BrgButton.TabIndex = 7;
            this.BrgButton.Text = "...";
            this.BrgButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Brg ID";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Cornsilk;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.KategoriIdText);
            this.panel3.Controls.Add(this.KategoriNameText);
            this.panel3.Controls.Add(this.KategoriButton);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.JenisBrgCombo);
            this.panel3.Controls.Add(this.SupplierIdText);
            this.panel3.Controls.Add(this.SupplierNameText);
            this.panel3.Controls.Add(this.SupplierButton);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(676, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 211);
            this.panel3.TabIndex = 49;
            // 
            // KategoriIdText
            // 
            this.KategoriIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KategoriIdText.Location = new System.Drawing.Point(12, 134);
            this.KategoriIdText.Name = "KategoriIdText";
            this.KategoriIdText.Size = new System.Drawing.Size(169, 22);
            this.KategoriIdText.TabIndex = 23;
            // 
            // KategoriNameText
            // 
            this.KategoriNameText.Location = new System.Drawing.Point(11, 162);
            this.KategoriNameText.Name = "KategoriNameText";
            this.KategoriNameText.ReadOnly = true;
            this.KategoriNameText.Size = new System.Drawing.Size(205, 22);
            this.KategoriNameText.TabIndex = 25;
            // 
            // KategoriButton
            // 
            this.KategoriButton.Location = new System.Drawing.Point(187, 134);
            this.KategoriButton.Name = "KategoriButton";
            this.KategoriButton.Size = new System.Drawing.Size(28, 23);
            this.KategoriButton.TabIndex = 24;
            this.KategoriButton.Text = "...";
            this.KategoriButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Kategori";
            // 
            // JenisBrgCombo
            // 
            this.JenisBrgCombo.FormattingEnabled = true;
            this.JenisBrgCombo.Location = new System.Drawing.Point(12, 94);
            this.JenisBrgCombo.Name = "JenisBrgCombo";
            this.JenisBrgCombo.Size = new System.Drawing.Size(203, 21);
            this.JenisBrgCombo.TabIndex = 21;
            // 
            // SupplierIdText
            // 
            this.SupplierIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupplierIdText.Location = new System.Drawing.Point(12, 25);
            this.SupplierIdText.Name = "SupplierIdText";
            this.SupplierIdText.Size = new System.Drawing.Size(169, 22);
            this.SupplierIdText.TabIndex = 18;
            // 
            // SupplierNameText
            // 
            this.SupplierNameText.Location = new System.Drawing.Point(11, 53);
            this.SupplierNameText.Name = "SupplierNameText";
            this.SupplierNameText.ReadOnly = true;
            this.SupplierNameText.Size = new System.Drawing.Size(205, 22);
            this.SupplierNameText.TabIndex = 20;
            // 
            // SupplierButton
            // 
            this.SupplierButton.Location = new System.Drawing.Point(187, 25);
            this.SupplierButton.Name = "SupplierButton";
            this.SupplierButton.Size = new System.Drawing.Size(28, 23);
            this.SupplierButton.TabIndex = 19;
            this.SupplierButton.Text = "...";
            this.SupplierButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Supplier";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Jenis";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.HargaTab);
            this.tabControl1.Controls.Add(this.SatuanTab);
            this.tabControl1.Controls.Add(this.StokTab);
            this.tabControl1.Location = new System.Drawing.Point(442, 226);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(462, 191);
            this.tabControl1.TabIndex = 50;
            // 
            // SatuanTab
            // 
            this.SatuanTab.BackColor = System.Drawing.Color.Cornsilk;
            this.SatuanTab.Controls.Add(this.SatuanGrid);
            this.SatuanTab.Location = new System.Drawing.Point(4, 22);
            this.SatuanTab.Name = "SatuanTab";
            this.SatuanTab.Padding = new System.Windows.Forms.Padding(3);
            this.SatuanTab.Size = new System.Drawing.Size(454, 165);
            this.SatuanTab.TabIndex = 0;
            this.SatuanTab.Text = "Satuan";
            // 
            // SatuanGrid
            // 
            this.SatuanGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SatuanGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SatuanGrid.Location = new System.Drawing.Point(3, 3);
            this.SatuanGrid.Name = "SatuanGrid";
            this.SatuanGrid.Size = new System.Drawing.Size(448, 159);
            this.SatuanGrid.TabIndex = 0;
            // 
            // HargaTab
            // 
            this.HargaTab.BackColor = System.Drawing.Color.Cornsilk;
            this.HargaTab.Controls.Add(this.HargaGrid);
            this.HargaTab.Location = new System.Drawing.Point(4, 22);
            this.HargaTab.Name = "HargaTab";
            this.HargaTab.Padding = new System.Windows.Forms.Padding(3);
            this.HargaTab.Size = new System.Drawing.Size(454, 165);
            this.HargaTab.TabIndex = 1;
            this.HargaTab.Text = "Harga";
            // 
            // HargaGrid
            // 
            this.HargaGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HargaGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HargaGrid.Location = new System.Drawing.Point(3, 3);
            this.HargaGrid.Name = "HargaGrid";
            this.HargaGrid.Size = new System.Drawing.Size(448, 159);
            this.HargaGrid.TabIndex = 1;
            // 
            // StokTab
            // 
            this.StokTab.BackColor = System.Drawing.Color.Cornsilk;
            this.StokTab.Controls.Add(this.StokGrid);
            this.StokTab.Location = new System.Drawing.Point(4, 22);
            this.StokTab.Name = "StokTab";
            this.StokTab.Padding = new System.Windows.Forms.Padding(3);
            this.StokTab.Size = new System.Drawing.Size(454, 165);
            this.StokTab.TabIndex = 2;
            this.StokTab.Text = "Stok";
            // 
            // StokGrid
            // 
            this.StokGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StokGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StokGrid.Location = new System.Drawing.Point(3, 3);
            this.StokGrid.Name = "StokGrid";
            this.StokGrid.Size = new System.Drawing.Size(448, 159);
            this.StokGrid.TabIndex = 1;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(829, 423);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 51;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewButton.Location = new System.Drawing.Point(442, 423);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 52;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // BrgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(912, 450);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.BrgGrid);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchText);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BrgForm";
            this.Text = "Barang";
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HppText)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.SatuanTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SatuanGrid)).EndInit();
            this.HargaTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HargaGrid)).EndInit();
            this.StokTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StokGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.DataGridView BrgGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox BrgIdText;
        private System.Windows.Forms.TextBox BrgNameText;
        private System.Windows.Forms.Button BrgButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BrgCodeText;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox SupplierIdText;
        private System.Windows.Forms.TextBox SupplierNameText;
        private System.Windows.Forms.Button SupplierButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SatuanTab;
        private System.Windows.Forms.TabPage HargaTab;
        private System.Windows.Forms.TabPage StokTab;
        private System.Windows.Forms.DataGridView SatuanGrid;
        private System.Windows.Forms.DataGridView HargaGrid;
        private System.Windows.Forms.DataGridView StokGrid;
        private System.Windows.Forms.ComboBox JenisBrgCombo;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox KategoriIdText;
        private System.Windows.Forms.TextBox KategoriNameText;
        private System.Windows.Forms.Button KategoriButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown HppText;
        private System.Windows.Forms.Label TotalLabel;
        private System.Windows.Forms.DateTimePicker HppTimestampText;
        private System.Windows.Forms.Button NewButton;
    }
}