namespace btr.distrib.InventoryContext.StokBalanceRpt
{
    partial class StokBalanceInfoForm
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
            this.KategoriButton = new System.Windows.Forms.Button();
            this.SupplierButton = new System.Windows.Forms.Button();
            this.WarehouseGrid = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.ProsesButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.KategoriText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SupplierText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrgText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.ExcelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.SkyBlue;
            this.splitContainer1.Panel1.Controls.Add(this.ExcelButton);
            this.splitContainer1.Panel1.Controls.Add(this.KategoriButton);
            this.splitContainer1.Panel1.Controls.Add(this.SupplierButton);
            this.splitContainer1.Panel1.Controls.Add(this.WarehouseGrid);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.ProsesButton);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel1.Controls.Add(this.KategoriText);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.SupplierText);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.BrgText);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ResultGrid);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.TabIndex = 0;
            // 
            // KategoriButton
            // 
            this.KategoriButton.BackColor = System.Drawing.Color.PowderBlue;
            this.KategoriButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KategoriButton.Location = new System.Drawing.Point(157, 107);
            this.KategoriButton.Name = "KategoriButton";
            this.KategoriButton.Size = new System.Drawing.Size(26, 22);
            this.KategoriButton.TabIndex = 12;
            this.KategoriButton.Text = "...";
            this.KategoriButton.UseVisualStyleBackColor = false;
            // 
            // SupplierButton
            // 
            this.SupplierButton.BackColor = System.Drawing.Color.PowderBlue;
            this.SupplierButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SupplierButton.Location = new System.Drawing.Point(157, 66);
            this.SupplierButton.Name = "SupplierButton";
            this.SupplierButton.Size = new System.Drawing.Size(26, 22);
            this.SupplierButton.TabIndex = 11;
            this.SupplierButton.Text = "...";
            this.SupplierButton.UseVisualStyleBackColor = false;
            // 
            // WarehouseGrid
            // 
            this.WarehouseGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WarehouseGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WarehouseGrid.Location = new System.Drawing.Point(12, 148);
            this.WarehouseGrid.Name = "WarehouseGrid";
            this.WarehouseGrid.RowHeadersVisible = false;
            this.WarehouseGrid.Size = new System.Drawing.Size(171, 150);
            this.WarehouseGrid.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Warehouse";
            // 
            // ProsesButton
            // 
            this.ProsesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProsesButton.Location = new System.Drawing.Point(12, 365);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(171, 23);
            this.ProsesButton.TabIndex = 7;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 424);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(171, 12);
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // KategoriText
            // 
            this.KategoriText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KategoriText.Location = new System.Drawing.Point(12, 107);
            this.KategoriText.Name = "KategoriText";
            this.KategoriText.Size = new System.Drawing.Size(142, 22);
            this.KategoriText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Kategori";
            // 
            // SupplierText
            // 
            this.SupplierText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SupplierText.Location = new System.Drawing.Point(12, 66);
            this.SupplierText.Name = "SupplierText";
            this.SupplierText.Size = new System.Drawing.Size(142, 22);
            this.SupplierText.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Supplier";
            // 
            // BrgText
            // 
            this.BrgText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrgText.Location = new System.Drawing.Point(12, 25);
            this.BrgText.Name = "BrgText";
            this.BrgText.Size = new System.Drawing.Size(171, 22);
            this.BrgText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brg Name";
            // 
            // ResultGrid
            // 
            this.ResultGrid.BackgroundColor = System.Drawing.Color.SteelBlue;
            this.ResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultGrid.Location = new System.Drawing.Point(0, 0);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.Size = new System.Drawing.Size(598, 448);
            this.ResultGrid.TabIndex = 0;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcelButton.Location = new System.Drawing.Point(12, 394);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(171, 23);
            this.ExcelButton.TabIndex = 13;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = true;
            // 
            // StokBalanceInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StokBalanceInfoForm";
            this.Text = "Stok Balance";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView ResultGrid;
        private System.Windows.Forms.TextBox BrgText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox KategoriText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SupplierText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button ProsesButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView WarehouseGrid;
        private System.Windows.Forms.Button SupplierButton;
        private System.Windows.Forms.Button KategoriButton;
        private System.Windows.Forms.Button ExcelButton;
    }
}