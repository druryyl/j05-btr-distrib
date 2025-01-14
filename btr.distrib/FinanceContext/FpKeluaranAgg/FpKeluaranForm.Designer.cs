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
            this.FpKeluaranDateText = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.FPKeluaranBroweButton = new System.Windows.Forms.Button();
            this.FpKeluaranText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.AllOutstandingCheckBox = new System.Windows.Forms.CheckBox();
            this.PeriodeCalender = new System.Windows.Forms.MonthCalendar();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.LastIdLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.FpKeluaranDateText);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.FPKeluaranBroweButton);
            this.panel1.Controls.Add(this.FpKeluaranText);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 157);
            this.panel1.TabIndex = 0;
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
            // FPKeluaranBroweButton
            // 
            this.FPKeluaranBroweButton.BackColor = System.Drawing.Color.Khaki;
            this.FPKeluaranBroweButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FPKeluaranBroweButton.Location = new System.Drawing.Point(167, 25);
            this.FPKeluaranBroweButton.Name = "FPKeluaranBroweButton";
            this.FPKeluaranBroweButton.Size = new System.Drawing.Size(26, 22);
            this.FPKeluaranBroweButton.TabIndex = 2;
            this.FPKeluaranBroweButton.Text = "...";
            this.FPKeluaranBroweButton.UseVisualStyleBackColor = false;
            // 
            // FpKeluaranText
            // 
            this.FpKeluaranText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FpKeluaranText.Location = new System.Drawing.Point(20, 25);
            this.FpKeluaranText.Name = "FpKeluaranText";
            this.FpKeluaranText.Size = new System.Drawing.Size(144, 22);
            this.FpKeluaranText.TabIndex = 1;
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
            this.FakturGrid.Size = new System.Drawing.Size(795, 457);
            this.FakturGrid.TabIndex = 4;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.Cornsilk;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(950, 469);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(82, 27);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.BackColor = System.Drawing.Color.Cornsilk;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(6, 469);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 27);
            this.button3.TabIndex = 6;
            this.button3.Text = "New";
            this.button3.UseVisualStyleBackColor = false;
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
            // FpKeluaranForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(1037, 502);
            this.Controls.Add(this.PeriodeCalender);
            this.Controls.Add(this.button3);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker FpKeluaranDateText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button FPKeluaranBroweButton;
        private System.Windows.Forms.TextBox FpKeluaranText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.CheckBox AllOutstandingCheckBox;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.MonthCalendar PeriodeCalender;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LastIdLabel;
    }
}