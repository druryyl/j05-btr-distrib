using System.ComponentModel;

namespace btr.distrib.InventoryContext.KartuStokRpt
{
    partial class KartuStokInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.Periode1Date = new System.Windows.Forms.DateTimePicker();
            this.Periode2Date = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SearchBrgText = new System.Windows.Forms.TextBox();
            this.BrgGrid = new System.Windows.Forms.DataGridView();
            this.ListBarangButton = new System.Windows.Forms.Button();
            this.KartuStokGrid = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DetilGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetilGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Periode1Date
            // 
            this.Periode1Date.CustomFormat = "ddd, dd MMM yyyy";
            this.Periode1Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode1Date.Location = new System.Drawing.Point(6, 6);
            this.Periode1Date.Name = "Periode1Date";
            this.Periode1Date.Size = new System.Drawing.Size(131, 22);
            this.Periode1Date.TabIndex = 0;
            // 
            // Periode2Date
            // 
            this.Periode2Date.CustomFormat = "ddd, dd MMM yyyy";
            this.Periode2Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Periode2Date.Location = new System.Drawing.Point(143, 6);
            this.Periode2Date.Name = "Periode2Date";
            this.Periode2Date.Size = new System.Drawing.Size(131, 22);
            this.Periode2Date.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SearchBrgText);
            this.panel1.Controls.Add(this.BrgGrid);
            this.panel1.Controls.Add(this.ListBarangButton);
            this.panel1.Controls.Add(this.Periode1Date);
            this.panel1.Controls.Add(this.Periode2Date);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 442);
            this.panel1.TabIndex = 2;
            // 
            // SearchBrgText
            // 
            this.SearchBrgText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBrgText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchBrgText.Location = new System.Drawing.Point(7, 34);
            this.SearchBrgText.Name = "SearchBrgText";
            this.SearchBrgText.Size = new System.Drawing.Size(294, 22);
            this.SearchBrgText.TabIndex = 4;
            // 
            // BrgGrid
            // 
            this.BrgGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrgGrid.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.BrgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGrid.Location = new System.Drawing.Point(7, 61);
            this.BrgGrid.Name = "BrgGrid";
            this.BrgGrid.Size = new System.Drawing.Size(355, 372);
            this.BrgGrid.TabIndex = 3;
            // 
            // ListBarangButton
            // 
            this.ListBarangButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBarangButton.BackColor = System.Drawing.Color.PowderBlue;
            this.ListBarangButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListBarangButton.Location = new System.Drawing.Point(307, 6);
            this.ListBarangButton.Name = "ListBarangButton";
            this.ListBarangButton.Size = new System.Drawing.Size(55, 50);
            this.ListBarangButton.TabIndex = 3;
            this.ListBarangButton.Text = "List Barang";
            this.ListBarangButton.UseVisualStyleBackColor = false;
            // 
            // KartuStokGrid
            // 
            this.KartuStokGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KartuStokGrid.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.KartuStokGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KartuStokGrid.Location = new System.Drawing.Point(3, 3);
            this.KartuStokGrid.Name = "KartuStokGrid";
            this.KartuStokGrid.Size = new System.Drawing.Size(616, 442);
            this.KartuStokGrid.TabIndex = 4;
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
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.DetilGrid);
            this.splitContainer1.Panel2.Controls.Add(this.KartuStokGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1007, 450);
            this.splitContainer1.SplitterDistance = 379;
            this.splitContainer1.TabIndex = 5;
            // 
            // DetilGrid
            // 
            this.DetilGrid.AccessibleName = "Table";
            this.DetilGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetilGrid.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.DetilGrid.Location = new System.Drawing.Point(3, 2);
            this.DetilGrid.Name = "DetilGrid";
            this.DetilGrid.Size = new System.Drawing.Size(618, 443);
            this.DetilGrid.TabIndex = 6;
            this.DetilGrid.Text = "sfDataGrid1";
            this.DetilGrid.Visible = false;
            // 
            // KartuStokInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(1007, 450);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "KartuStokInfoForm";
            this.Text = "KartuStokInfo";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetilGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker Periode1Date;
        private System.Windows.Forms.DateTimePicker Periode2Date;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView BrgGrid;
        private System.Windows.Forms.DataGridView KartuStokGrid;
        private System.Windows.Forms.Button ListBarangButton;
        private System.Windows.Forms.TextBox SearchBrgText;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Syncfusion.WinForms.DataGrid.SfDataGrid DetilGrid;
    }
}