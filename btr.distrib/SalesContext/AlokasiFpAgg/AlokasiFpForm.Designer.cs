namespace btr.distrib.SalesContext.AlokasiFpAgg
{
    partial class AlokasiFpForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.AlokasiButton = new System.Windows.Forms.Button();
            this.NoAwalText = new System.Windows.Forms.MaskedTextBox();
            this.NoAkhirText = new System.Windows.Forms.MaskedTextBox();
            this.AlokasiGrid = new System.Windows.Forms.DataGridView();
            this.ExportExcelButton = new System.Windows.Forms.Button();
            this.ExportEFakturButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SisaFpLabel = new System.Windows.Forms.Label();
            this.NoAkhirLabel = new System.Windows.Forms.Label();
            this.NoAwalLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AllOutstandingCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ListButton = new System.Windows.Forms.Button();
            this.Periode2Date = new System.Windows.Forms.DateTimePicker();
            this.Periode1Date = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlokasiGrid)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.AlokasiGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.ExportExcelButton);
            this.splitContainer1.Panel2.Controls.Add(this.ExportEFakturButton);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.FakturGrid);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1025, 451);
            this.splitContainer1.SplitterDistance = 237;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.AlokasiButton);
            this.panel2.Controls.Add(this.NoAwalText);
            this.panel2.Controls.Add(this.NoAkhirText);
            this.panel2.Location = new System.Drawing.Point(6, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(229, 84);
            this.panel2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.DarkKhaki;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "Alokasi Nomor Seri Faktur Pajak";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlokasiButton
            // 
            this.AlokasiButton.Location = new System.Drawing.Point(131, 54);
            this.AlokasiButton.Name = "AlokasiButton";
            this.AlokasiButton.Size = new System.Drawing.Size(90, 22);
            this.AlokasiButton.TabIndex = 11;
            this.AlokasiButton.Text = "Create Alokasi";
            this.AlokasiButton.UseVisualStyleBackColor = true;
            // 
            // NoAwalText
            // 
            this.NoAwalText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAwalText.Location = new System.Drawing.Point(5, 28);
            this.NoAwalText.Mask = "000-000-00-00000000";
            this.NoAwalText.Name = "NoAwalText";
            this.NoAwalText.Size = new System.Drawing.Size(120, 20);
            this.NoAwalText.TabIndex = 9;
            // 
            // NoAkhirText
            // 
            this.NoAkhirText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAkhirText.Location = new System.Drawing.Point(5, 54);
            this.NoAkhirText.Mask = "000-000-00-00000000";
            this.NoAkhirText.Name = "NoAkhirText";
            this.NoAkhirText.Size = new System.Drawing.Size(120, 20);
            this.NoAkhirText.TabIndex = 10;
            // 
            // AlokasiGrid
            // 
            this.AlokasiGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AlokasiGrid.BackgroundColor = System.Drawing.Color.DarkKhaki;
            this.AlokasiGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AlokasiGrid.Location = new System.Drawing.Point(6, 97);
            this.AlokasiGrid.Name = "AlokasiGrid";
            this.AlokasiGrid.Size = new System.Drawing.Size(229, 347);
            this.AlokasiGrid.TabIndex = 4;
            // 
            // ExportExcelButton
            // 
            this.ExportExcelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportExcelButton.BackColor = System.Drawing.Color.Linen;
            this.ExportExcelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportExcelButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExportExcelButton.Location = new System.Drawing.Point(650, 66);
            this.ExportExcelButton.Name = "ExportExcelButton";
            this.ExportExcelButton.Size = new System.Drawing.Size(126, 24);
            this.ExportExcelButton.TabIndex = 10;
            this.ExportExcelButton.Text = "Export Excel";
            this.ExportExcelButton.UseVisualStyleBackColor = false;
            // 
            // ExportEFakturButton
            // 
            this.ExportEFakturButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportEFakturButton.BackColor = System.Drawing.Color.Linen;
            this.ExportEFakturButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportEFakturButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExportEFakturButton.Location = new System.Drawing.Point(650, 36);
            this.ExportEFakturButton.Name = "ExportEFakturButton";
            this.ExportEFakturButton.Size = new System.Drawing.Size(126, 24);
            this.ExportEFakturButton.TabIndex = 9;
            this.ExportEFakturButton.Text = "Export CSV";
            this.ExportEFakturButton.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Cornsilk;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.SisaFpLabel);
            this.panel3.Controls.Add(this.NoAkhirLabel);
            this.panel3.Controls.Add(this.NoAwalLabel);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(272, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(372, 84);
            this.panel3.TabIndex = 4;
            // 
            // SisaFpLabel
            // 
            this.SisaFpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SisaFpLabel.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SisaFpLabel.Location = new System.Drawing.Point(280, 25);
            this.SisaFpLabel.Name = "SisaFpLabel";
            this.SisaFpLabel.Size = new System.Drawing.Size(91, 38);
            this.SisaFpLabel.TabIndex = 12;
            this.SisaFpLabel.Text = "0";
            this.SisaFpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NoAkhirLabel
            // 
            this.NoAkhirLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoAkhirLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAkhirLabel.Location = new System.Drawing.Point(0, 44);
            this.NoAkhirLabel.Name = "NoAkhirLabel";
            this.NoAkhirLabel.Size = new System.Drawing.Size(284, 19);
            this.NoAkhirLabel.TabIndex = 11;
            this.NoAkhirLabel.Text = "xxx.xxx-xx.xxxxxxxx";
            this.NoAkhirLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NoAwalLabel
            // 
            this.NoAwalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoAwalLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAwalLabel.Location = new System.Drawing.Point(0, 25);
            this.NoAwalLabel.Name = "NoAwalLabel";
            this.NoAwalLabel.Size = new System.Drawing.Size(284, 19);
            this.NoAwalLabel.TabIndex = 10;
            this.NoAwalLabel.Text = "xxx.xxx-xx.xxxxxxxx";
            this.NoAwalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.DarkKhaki;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Alokasi Terpilih";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FakturGrid
            // 
            this.FakturGrid.AllowUserToAddRows = false;
            this.FakturGrid.AllowUserToDeleteRows = false;
            this.FakturGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FakturGrid.BackgroundColor = System.Drawing.Color.DarkKhaki;
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Location = new System.Drawing.Point(3, 125);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(773, 319);
            this.FakturGrid.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.AllOutstandingCheckBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ListButton);
            this.panel1.Controls.Add(this.Periode2Date);
            this.panel1.Controls.Add(this.Periode1Date);
            this.panel1.Location = new System.Drawing.Point(3, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 84);
            this.panel1.TabIndex = 2;
            // 
            // AllOutstandingCheckBox
            // 
            this.AllOutstandingCheckBox.AutoSize = true;
            this.AllOutstandingCheckBox.Location = new System.Drawing.Point(6, 58);
            this.AllOutstandingCheckBox.Name = "AllOutstandingCheckBox";
            this.AllOutstandingCheckBox.Size = new System.Drawing.Size(108, 17);
            this.AllOutstandingCheckBox.TabIndex = 14;
            this.AllOutstandingCheckBox.Text = "All Outstanding";
            this.AllOutstandingCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.DarkKhaki;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(263, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "Periode Faktur";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(160, 54);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(92, 23);
            this.ListButton.TabIndex = 2;
            this.ListButton.Text = "List Faktur";
            this.ListButton.UseVisualStyleBackColor = true;
            // 
            // Periode2Date
            // 
            this.Periode2Date.CalendarFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Periode2Date.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Periode2Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode2Date.Location = new System.Drawing.Point(132, 28);
            this.Periode2Date.Name = "Periode2Date";
            this.Periode2Date.Size = new System.Drawing.Size(120, 22);
            this.Periode2Date.TabIndex = 1;
            // 
            // Periode1Date
            // 
            this.Periode1Date.CalendarFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Periode1Date.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Periode1Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode1Date.Location = new System.Drawing.Point(6, 28);
            this.Periode1Date.Name = "Periode1Date";
            this.Periode1Date.Size = new System.Drawing.Size(120, 22);
            this.Periode1Date.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(68, 97);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "No.Faktur";
            // 
            // AlokasiFpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(1025, 451);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AlokasiFpForm";
            this.Text = "Nomor Seri Faktur Pajak";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlokasiGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button ExportExcelButton;

        private System.Windows.Forms.Button ExportEFakturButton;

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView AlokasiGrid;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MaskedTextBox NoAkhirText;
        private System.Windows.Forms.MaskedTextBox NoAwalText;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.DateTimePicker Periode2Date;
        private System.Windows.Forms.DateTimePicker Periode1Date;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AlokasiButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label NoAwalLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NoAkhirLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SisaFpLabel;
        private System.Windows.Forms.CheckBox AllOutstandingCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
    }
}