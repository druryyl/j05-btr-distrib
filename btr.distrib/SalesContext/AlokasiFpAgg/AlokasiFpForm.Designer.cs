namespace btr.distrib.SalesContext.NomorSeriFpAgg
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
            this.FakturKiriGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NoAkhirText = new System.Windows.Forms.MaskedTextBox();
            this.NoAwalText = new System.Windows.Forms.MaskedTextBox();
            this.ListButton = new System.Windows.Forms.Button();
            this.Periode2Date = new System.Windows.Forms.DateTimePicker();
            this.Periode1Date = new System.Windows.Forms.DateTimePicker();
            this.AlokasiGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AlokasiButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ProsesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturKiriGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlokasiGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.FakturKiriGrid);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.ProsesButton);
            this.splitContainer1.Size = new System.Drawing.Size(907, 501);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.TabIndex = 0;
            // 
            // FakturKiriGrid
            // 
            this.FakturKiriGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturKiriGrid.Location = new System.Drawing.Point(3, 97);
            this.FakturKiriGrid.Name = "FakturKiriGrid";
            this.FakturKiriGrid.Size = new System.Drawing.Size(622, 397);
            this.FakturKiriGrid.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MistyRose;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ListButton);
            this.panel1.Controls.Add(this.Periode2Date);
            this.panel1.Controls.Add(this.Periode1Date);
            this.panel1.Location = new System.Drawing.Point(3, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 84);
            this.panel1.TabIndex = 2;
            // 
            // NoAkhirText
            // 
            this.NoAkhirText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAkhirText.Location = new System.Drawing.Point(131, 28);
            this.NoAkhirText.Mask = "000.000-00.00000000";
            this.NoAkhirText.Name = "NoAkhirText";
            this.NoAkhirText.Size = new System.Drawing.Size(120, 20);
            this.NoAkhirText.TabIndex = 10;
            // 
            // NoAwalText
            // 
            this.NoAwalText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoAwalText.Location = new System.Drawing.Point(5, 28);
            this.NoAwalText.Mask = "000.000-00.00000000";
            this.NoAwalText.Name = "NoAwalText";
            this.NoAwalText.Size = new System.Drawing.Size(120, 20);
            this.NoAwalText.TabIndex = 9;
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(172, 54);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(80, 23);
            this.ListButton.TabIndex = 2;
            this.ListButton.Text = "List";
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
            // AlokasiGrid
            // 
            this.AlokasiGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AlokasiGrid.Location = new System.Drawing.Point(6, 97);
            this.AlokasiGrid.Name = "AlokasiGrid";
            this.AlokasiGrid.Size = new System.Drawing.Size(262, 397);
            this.AlokasiGrid.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MistyRose;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.AlokasiButton);
            this.panel2.Controls.Add(this.NoAwalText);
            this.panel2.Controls.Add(this.NoAkhirText);
            this.panel2.Location = new System.Drawing.Point(6, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(262, 84);
            this.panel2.TabIndex = 5;
            // 
            // AlokasiButton
            // 
            this.AlokasiButton.Location = new System.Drawing.Point(131, 54);
            this.AlokasiButton.Name = "AlokasiButton";
            this.AlokasiButton.Size = new System.Drawing.Size(120, 22);
            this.AlokasiButton.TabIndex = 11;
            this.AlokasiButton.Text = "Create Alokasi";
            this.AlokasiButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MistyRose;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(272, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(267, 84);
            this.panel3.TabIndex = 4;
            // 
            // ProsesButton
            // 
            this.ProsesButton.Location = new System.Drawing.Point(545, 69);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(80, 22);
            this.ProsesButton.TabIndex = 8;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.SaddleBrown;
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Alokasi Terpilih";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(265, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "xxx.xxx-xx.xxxxxxxx";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(265, 19);
            this.label5.TabIndex = 11;
            this.label5.Text = "xxx.xxx-xx.xxxxxxxx";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.SaddleBrown;
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "Alokasi Nomor Seri Faktur Pajak";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.SaddleBrown;
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(263, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "Periode Faktur";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NomorSeriFpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSalmon;
            this.ClientSize = new System.Drawing.Size(907, 501);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NomorSeriFpForm";
            this.Text = "Nomor Seri Faktur Pajak";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FakturKiriGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AlokasiGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView AlokasiGrid;
        private System.Windows.Forms.DataGridView FakturKiriGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MaskedTextBox NoAkhirText;
        private System.Windows.Forms.MaskedTextBox NoAwalText;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.DateTimePicker Periode2Date;
        private System.Windows.Forms.DateTimePicker Periode1Date;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AlokasiButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ProsesButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}