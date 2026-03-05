namespace btr.distrib.InventoryContext.OpnameAgg
{
    partial class StokOpForm
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
            this.BrgGrid = new System.Windows.Forms.DataGridView();
            this.PeriodeOpText = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExcelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.KategoriCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WarehouseCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ListBrgButton = new System.Windows.Forms.Button();
            this.TotalAdjustText = new System.Windows.Forms.TextBox();
            this.TotalOpnameText = new System.Windows.Forms.TextBox();
            this.TotalAwalText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrgGrid
            // 
            this.BrgGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrgGrid.BackgroundColor = System.Drawing.Color.SlateGray;
            this.BrgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGrid.Location = new System.Drawing.Point(6, 53);
            this.BrgGrid.Name = "BrgGrid";
            this.BrgGrid.Size = new System.Drawing.Size(867, 364);
            this.BrgGrid.TabIndex = 4;
            // 
            // PeriodeOpText
            // 
            this.PeriodeOpText.CustomFormat = "ddd, dd MMM yyyy";
            this.PeriodeOpText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PeriodeOpText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PeriodeOpText.Location = new System.Drawing.Point(9, 19);
            this.PeriodeOpText.Name = "PeriodeOpText";
            this.PeriodeOpText.Size = new System.Drawing.Size(141, 20);
            this.PeriodeOpText.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.KategoriCombo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.WarehouseCombo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ListBrgButton);
            this.panel1.Controls.Add(this.PeriodeOpText);
            this.panel1.Location = new System.Drawing.Point(6, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(867, 47);
            this.panel1.TabIndex = 3;
            // 
            // ExcelButton
            // 
            this.ExcelButton.BackColor = System.Drawing.Color.PowderBlue;
            this.ExcelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExcelButton.Location = new System.Drawing.Point(652, 16);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(100, 23);
            this.ExcelButton.TabIndex = 9;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Kategori Barang";
            // 
            // KategoriCombo
            // 
            this.KategoriCombo.FormattingEnabled = true;
            this.KategoriCombo.Location = new System.Drawing.Point(351, 18);
            this.KategoriCombo.Name = "KategoriCombo";
            this.KategoriCombo.Size = new System.Drawing.Size(189, 21);
            this.KategoriCombo.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lokasi / Gudang";
            // 
            // WarehouseCombo
            // 
            this.WarehouseCombo.FormattingEnabled = true;
            this.WarehouseCombo.Location = new System.Drawing.Point(156, 18);
            this.WarehouseCombo.Name = "WarehouseCombo";
            this.WarehouseCombo.Size = new System.Drawing.Size(189, 21);
            this.WarehouseCombo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tgl Opname";
            // 
            // ListBrgButton
            // 
            this.ListBrgButton.BackColor = System.Drawing.Color.PowderBlue;
            this.ListBrgButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListBrgButton.Location = new System.Drawing.Point(546, 16);
            this.ListBrgButton.Name = "ListBrgButton";
            this.ListBrgButton.Size = new System.Drawing.Size(100, 23);
            this.ListBrgButton.TabIndex = 1;
            this.ListBrgButton.Text = "List Stok";
            this.ListBrgButton.UseVisualStyleBackColor = false;
            // 
            // TotalAdjustText
            // 
            this.TotalAdjustText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalAdjustText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAdjustText.Location = new System.Drawing.Point(773, 436);
            this.TotalAdjustText.Name = "TotalAdjustText";
            this.TotalAdjustText.ReadOnly = true;
            this.TotalAdjustText.Size = new System.Drawing.Size(100, 20);
            this.TotalAdjustText.TabIndex = 5;
            this.TotalAdjustText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TotalOpnameText
            // 
            this.TotalOpnameText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalOpnameText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalOpnameText.Location = new System.Drawing.Point(667, 436);
            this.TotalOpnameText.Name = "TotalOpnameText";
            this.TotalOpnameText.ReadOnly = true;
            this.TotalOpnameText.Size = new System.Drawing.Size(100, 20);
            this.TotalOpnameText.TabIndex = 6;
            this.TotalOpnameText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TotalAwalText
            // 
            this.TotalAwalText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalAwalText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAwalText.Location = new System.Drawing.Point(561, 436);
            this.TotalAwalText.Name = "TotalAwalText";
            this.TotalAwalText.ReadOnly = true;
            this.TotalAwalText.Size = new System.Drawing.Size(100, 20);
            this.TotalAwalText.TabIndex = 7;
            this.TotalAwalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(558, 420);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Total Awal";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Location = new System.Drawing.Point(664, 420);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Total Opname";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Location = new System.Drawing.Point(770, 420);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Total Adjust";
            // 
            // StokOpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(879, 470);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TotalAwalText);
            this.Controls.Add(this.TotalOpnameText);
            this.Controls.Add(this.TotalAdjustText);
            this.Controls.Add(this.BrgGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StokOpForm";
            this.Text = "Stok Opname";
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView BrgGrid;
        private System.Windows.Forms.DateTimePicker PeriodeOpText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox KategoriCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox WarehouseCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ListBrgButton;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.TextBox TotalAdjustText;
        private System.Windows.Forms.TextBox TotalOpnameText;
        private System.Windows.Forms.TextBox TotalAwalText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}