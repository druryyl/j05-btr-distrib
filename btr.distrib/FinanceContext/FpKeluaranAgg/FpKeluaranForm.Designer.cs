namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    partial class FpKeluaranForm
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
            this.LastIdLabel = new System.Windows.Forms.Label();
            this.FpKeluaranDateText = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.FpKeluaranIdButton = new System.Windows.Forms.Button();
            this.FpKeluaranIdText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.AllOutstandingCheckBox = new System.Windows.Forms.CheckBox();
            this.PeriodeCalender = new System.Windows.Forms.MonthCalendar();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TotalPpnText = new System.Windows.Forms.NumericUpDown();
            this.TotalFakturText = new System.Windows.Forms.NumericUpDown();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.FakturTerlipihLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalPpnText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalFakturText)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.FpKeluaranDateText);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.FpKeluaranIdButton);
            this.panel1.Controls.Add(this.FpKeluaranIdText);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 157);
            this.panel1.TabIndex = 0;
            // 
            // LastIdLabel
            // 
            this.LastIdLabel.AutoSize = true;
            this.LastIdLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastIdLabel.ForeColor = System.Drawing.Color.DimGray;
            this.LastIdLabel.Location = new System.Drawing.Point(17, 50);
            this.LastIdLabel.Name = "LastIdLabel";
            this.LastIdLabel.Size = new System.Drawing.Size(46, 13);
            this.LastIdLabel.TabIndex = 5;
            this.LastIdLabel.Text = "[Last ID]";
            // 
            // FpKeluaranDateText
            // 
            this.FpKeluaranDateText.CustomFormat = "ddd, dd MMM yyyy";
            this.FpKeluaranDateText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FpKeluaranDateText.Location = new System.Drawing.Point(20, 91);
            this.FpKeluaranDateText.Name = "FpKeluaranDateText";
            this.FpKeluaranDateText.Size = new System.Drawing.Size(173, 22);
            this.FpKeluaranDateText.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tanggal Pembuatan";
            // 
            // FpKeluaranIdButton
            // 
            this.FpKeluaranIdButton.BackColor = System.Drawing.Color.Khaki;
            this.FpKeluaranIdButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FpKeluaranIdButton.Location = new System.Drawing.Point(167, 25);
            this.FpKeluaranIdButton.Name = "FpKeluaranIdButton";
            this.FpKeluaranIdButton.Size = new System.Drawing.Size(26, 22);
            this.FpKeluaranIdButton.TabIndex = 2;
            this.FpKeluaranIdButton.Text = "...";
            this.FpKeluaranIdButton.UseVisualStyleBackColor = false;
            // 
            // FpKeluaranIdText
            // 
            this.FpKeluaranIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FpKeluaranIdText.Location = new System.Drawing.Point(20, 25);
            this.FpKeluaranIdText.Name = "FpKeluaranIdText";
            this.FpKeluaranIdText.Size = new System.Drawing.Size(144, 22);
            this.FpKeluaranIdText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FP Keluaran ID";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.SearchText);
            this.panel2.Controls.Add(this.SearchButton);
            this.panel2.Controls.Add(this.AllOutstandingCheckBox);
            this.panel2.Location = new System.Drawing.Point(6, 330);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 133);
            this.panel2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Search";
            // 
            // SearchText
            // 
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchText.Location = new System.Drawing.Point(9, 22);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(210, 22);
            this.SearchText.TabIndex = 17;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.Khaki;
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Location = new System.Drawing.Point(134, 50);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(85, 28);
            this.SearchButton.TabIndex = 16;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            // 
            // AllOutstandingCheckBox
            // 
            this.AllOutstandingCheckBox.AutoSize = true;
            this.AllOutstandingCheckBox.Location = new System.Drawing.Point(9, 50);
            this.AllOutstandingCheckBox.Name = "AllOutstandingCheckBox";
            this.AllOutstandingCheckBox.Size = new System.Drawing.Size(108, 17);
            this.AllOutstandingCheckBox.TabIndex = 14;
            this.AllOutstandingCheckBox.Text = "All Outstanding";
            this.AllOutstandingCheckBox.UseVisualStyleBackColor = true;
            // 
            // PeriodeCalender
            // 
            this.PeriodeCalender.Location = new System.Drawing.Point(6, 166);
            this.PeriodeCalender.Name = "PeriodeCalender";
            this.PeriodeCalender.TabIndex = 19;
            // 
            // FakturGrid
            // 
            this.FakturGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FakturGrid.BackgroundColor = System.Drawing.Color.Cornsilk;
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Location = new System.Drawing.Point(237, 6);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(824, 411);
            this.FakturGrid.TabIndex = 4;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.Cornsilk;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(979, 469);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(82, 27);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewButton.BackColor = System.Drawing.Color.Cornsilk;
            this.NewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewButton.Location = new System.Drawing.Point(6, 469);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(82, 27);
            this.NewButton.TabIndex = 6;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Cornsilk;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.FakturTerlipihLabel);
            this.panel3.Controls.Add(this.SelectAllButton);
            this.panel3.Controls.Add(this.TotalPpnText);
            this.panel3.Controls.Add(this.TotalFakturText);
            this.panel3.Location = new System.Drawing.Point(237, 422);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(823, 41);
            this.panel3.TabIndex = 20;
            // 
            // TotalPpnText
            // 
            this.TotalPpnText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalPpnText.Location = new System.Drawing.Point(678, 11);
            this.TotalPpnText.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.TotalPpnText.Name = "TotalPpnText";
            this.TotalPpnText.ReadOnly = true;
            this.TotalPpnText.Size = new System.Drawing.Size(120, 18);
            this.TotalPpnText.TabIndex = 21;
            this.TotalPpnText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TotalPpnText.ThousandsSeparator = true;
            // 
            // TotalFakturText
            // 
            this.TotalFakturText.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalFakturText.Location = new System.Drawing.Point(552, 11);
            this.TotalFakturText.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.TotalFakturText.Name = "TotalFakturText";
            this.TotalFakturText.ReadOnly = true;
            this.TotalFakturText.Size = new System.Drawing.Size(120, 18);
            this.TotalFakturText.TabIndex = 20;
            this.TotalFakturText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TotalFakturText.ThousandsSeparator = true;
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectAllButton.BackColor = System.Drawing.Color.Khaki;
            this.SelectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectAllButton.Location = new System.Drawing.Point(4, 6);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(82, 27);
            this.SelectAllButton.TabIndex = 22;
            this.SelectAllButton.Text = "Select All";
            this.SelectAllButton.UseVisualStyleBackColor = false;
            // 
            // FakturTerlipihLabel
            // 
            this.FakturTerlipihLabel.BackColor = System.Drawing.Color.Wheat;
            this.FakturTerlipihLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FakturTerlipihLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FakturTerlipihLabel.Location = new System.Drawing.Point(92, 6);
            this.FakturTerlipihLabel.Name = "FakturTerlipihLabel";
            this.FakturTerlipihLabel.Size = new System.Drawing.Size(234, 27);
            this.FakturTerlipihLabel.TabIndex = 23;
            this.FakturTerlipihLabel.Text = " Total Faktur = 0 | Terpilih = 0";
            this.FakturTerlipihLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FpKeluaranForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(1066, 502);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.PeriodeCalender);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FakturGrid);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FpKeluaranForm";
            this.Text = "FpKeluaranForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TotalPpnText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalFakturText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker FpKeluaranDateText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button FpKeluaranIdButton;
        private System.Windows.Forms.TextBox FpKeluaranIdText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.CheckBox AllOutstandingCheckBox;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.MonthCalendar PeriodeCalender;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LastIdLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown TotalPpnText;
        private System.Windows.Forms.NumericUpDown TotalFakturText;
        private System.Windows.Forms.Button SelectAllButton;
        private System.Windows.Forms.Label FakturTerlipihLabel;
    }
}