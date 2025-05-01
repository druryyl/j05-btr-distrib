namespace btr.distrib.SalesContext.SalesReplacementFeat
{
    partial class SalesReplacementForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.LoadFakturButton = new System.Windows.Forms.Button();
            this.Periode2DatePicker = new System.Windows.Forms.DateTimePicker();
            this.Periode1DatePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SalesAsalLabel = new System.Windows.Forms.Label();
            this.SalesPenggantiCombo = new System.Windows.Forms.ComboBox();
            this.SalesAsalCombo = new System.Windows.Forms.ComboBox();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.ReplaceButton = new System.Windows.Forms.Button();
            this.SearchFakturButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LoadFakturButton);
            this.panel1.Controls.Add(this.Periode2DatePicker);
            this.panel1.Controls.Add(this.Periode1DatePicker);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SalesAsalLabel);
            this.panel1.Controls.Add(this.SalesPenggantiCombo);
            this.panel1.Controls.Add(this.SalesAsalCombo);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 65);
            this.panel1.TabIndex = 0;
            // 
            // SearchText
            // 
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchText.Location = new System.Drawing.Point(6, 75);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(153, 22);
            this.SearchText.TabIndex = 2;
            // 
            // LoadFakturButton
            // 
            this.LoadFakturButton.BackColor = System.Drawing.Color.PowderBlue;
            this.LoadFakturButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadFakturButton.Location = new System.Drawing.Point(552, 28);
            this.LoadFakturButton.Name = "LoadFakturButton";
            this.LoadFakturButton.Size = new System.Drawing.Size(99, 23);
            this.LoadFakturButton.TabIndex = 7;
            this.LoadFakturButton.Text = "Load Faktur";
            this.LoadFakturButton.UseVisualStyleBackColor = false;
            // 
            // Periode2DatePicker
            // 
            this.Periode2DatePicker.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Periode2DatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode2DatePicker.Location = new System.Drawing.Point(413, 28);
            this.Periode2DatePicker.Name = "Periode2DatePicker";
            this.Periode2DatePicker.Size = new System.Drawing.Size(133, 22);
            this.Periode2DatePicker.TabIndex = 6;
            // 
            // Periode1DatePicker
            // 
            this.Periode1DatePicker.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Periode1DatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode1DatePicker.Location = new System.Drawing.Point(274, 28);
            this.Periode1DatePicker.Name = "Periode1DatePicker";
            this.Periode1DatePicker.Size = new System.Drawing.Size(133, 22);
            this.Periode1DatePicker.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Periode Faktur";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(144, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sales Pengganti";
            // 
            // SalesAsalLabel
            // 
            this.SalesAsalLabel.AutoSize = true;
            this.SalesAsalLabel.Location = new System.Drawing.Point(14, 12);
            this.SalesAsalLabel.Name = "SalesAsalLabel";
            this.SalesAsalLabel.Size = new System.Drawing.Size(57, 13);
            this.SalesAsalLabel.TabIndex = 2;
            this.SalesAsalLabel.Text = "Sales Asal";
            // 
            // SalesPenggantiCombo
            // 
            this.SalesPenggantiCombo.FormattingEnabled = true;
            this.SalesPenggantiCombo.Location = new System.Drawing.Point(147, 28);
            this.SalesPenggantiCombo.Name = "SalesPenggantiCombo";
            this.SalesPenggantiCombo.Size = new System.Drawing.Size(121, 21);
            this.SalesPenggantiCombo.TabIndex = 1;
            // 
            // SalesAsalCombo
            // 
            this.SalesAsalCombo.FormattingEnabled = true;
            this.SalesAsalCombo.Location = new System.Drawing.Point(17, 28);
            this.SalesAsalCombo.Name = "SalesAsalCombo";
            this.SalesAsalCombo.Size = new System.Drawing.Size(121, 21);
            this.SalesAsalCombo.TabIndex = 0;
            // 
            // FakturGrid
            // 
            this.FakturGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Location = new System.Drawing.Point(6, 103);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(787, 313);
            this.FakturGrid.TabIndex = 1;
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceButton.BackColor = System.Drawing.Color.PowderBlue;
            this.ReplaceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReplaceButton.Location = new System.Drawing.Point(690, 422);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(103, 23);
            this.ReplaceButton.TabIndex = 8;
            this.ReplaceButton.Text = "Replace Sales";
            this.ReplaceButton.UseVisualStyleBackColor = false;
            // 
            // SearchFakturButton
            // 
            this.SearchFakturButton.BackColor = System.Drawing.Color.PowderBlue;
            this.SearchFakturButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchFakturButton.Location = new System.Drawing.Point(165, 74);
            this.SearchFakturButton.Name = "SearchFakturButton";
            this.SearchFakturButton.Size = new System.Drawing.Size(75, 23);
            this.SearchFakturButton.TabIndex = 9;
            this.SearchFakturButton.Text = "Search";
            this.SearchFakturButton.UseVisualStyleBackColor = false;
            // 
            // SalesReplacementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 454);
            this.Controls.Add(this.SearchFakturButton);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.ReplaceButton);
            this.Controls.Add(this.FakturGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SalesReplacementForm";
            this.Text = "SalesReplacementForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SalesAsalLabel;
        private System.Windows.Forms.ComboBox SalesPenggantiCombo;
        private System.Windows.Forms.ComboBox SalesAsalCombo;
        private System.Windows.Forms.DateTimePicker Periode2DatePicker;
        private System.Windows.Forms.DateTimePicker Periode1DatePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Button LoadFakturButton;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.Button SearchFakturButton;
    }
}