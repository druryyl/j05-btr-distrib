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
            this.panel1 = new System.Windows.Forms.Panel();
            this.PeriodeCalender = new System.Windows.Forms.MonthCalendar();
            this.SearchBrgText = new System.Windows.Forms.TextBox();
            this.ListBarangButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.BrgGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PencatatanRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.PengakuanRadioButton = new System.Windows.Forms.RadioButton();
            this.KartuStokGrid = new System.Windows.Forms.DataGridView();
            this.ExcelButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.PeriodeCalender);
            this.panel1.Controls.Add(this.SearchBrgText);
            this.panel1.Controls.Add(this.ListBarangButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 238);
            this.panel1.TabIndex = 2;
            // 
            // PeriodeCalender
            // 
            this.PeriodeCalender.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PeriodeCalender.Location = new System.Drawing.Point(10, 9);
            this.PeriodeCalender.MaxSelectionCount = 31;
            this.PeriodeCalender.Name = "PeriodeCalender";
            this.PeriodeCalender.TabIndex = 5;
            // 
            // SearchBrgText
            // 
            this.SearchBrgText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchBrgText.Location = new System.Drawing.Point(10, 175);
            this.SearchBrgText.Name = "SearchBrgText";
            this.SearchBrgText.Size = new System.Drawing.Size(227, 22);
            this.SearchBrgText.TabIndex = 4;
            // 
            // ListBarangButton
            // 
            this.ListBarangButton.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ListBarangButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListBarangButton.Location = new System.Drawing.Point(10, 203);
            this.ListBarangButton.Name = "ListBarangButton";
            this.ListBarangButton.Size = new System.Drawing.Size(77, 26);
            this.ListBarangButton.TabIndex = 3;
            this.ListBarangButton.Text = "List Barang";
            this.ListBarangButton.UseVisualStyleBackColor = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(6, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.BrgGrid);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.KartuStokGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1017, 498);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 5;
            // 
            // BrgGrid
            // 
            this.BrgGrid.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.BrgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrgGrid.Location = new System.Drawing.Point(0, 238);
            this.BrgGrid.Name = "BrgGrid";
            this.BrgGrid.Size = new System.Drawing.Size(254, 258);
            this.BrgGrid.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel2.Controls.Add(this.ExcelButton);
            this.panel2.Controls.Add(this.PencatatanRadioButton);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.PengakuanRadioButton);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(755, 43);
            this.panel2.TabIndex = 1;
            // 
            // PencatatanRadioButton
            // 
            this.PencatatanRadioButton.AutoSize = true;
            this.PencatatanRadioButton.Location = new System.Drawing.Point(189, 11);
            this.PencatatanRadioButton.Name = "PencatatanRadioButton";
            this.PencatatanRadioButton.Size = new System.Drawing.Size(114, 17);
            this.PencatatanRadioButton.TabIndex = 2;
            this.PencatatanRadioButton.Text = "By Tgl Pencatatan";
            this.PencatatanRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sorting:";
            // 
            // PengakuanRadioButton
            // 
            this.PengakuanRadioButton.AutoSize = true;
            this.PengakuanRadioButton.Checked = true;
            this.PengakuanRadioButton.Location = new System.Drawing.Point(69, 11);
            this.PengakuanRadioButton.Name = "PengakuanRadioButton";
            this.PengakuanRadioButton.Size = new System.Drawing.Size(115, 17);
            this.PengakuanRadioButton.TabIndex = 0;
            this.PengakuanRadioButton.TabStop = true;
            this.PengakuanRadioButton.Text = "By Tgl Pengakuan";
            this.PengakuanRadioButton.UseVisualStyleBackColor = true;
            // 
            // KartuStokGrid
            // 
            this.KartuStokGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KartuStokGrid.BackgroundColor = System.Drawing.Color.DarkSlateGray;
            this.KartuStokGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KartuStokGrid.Location = new System.Drawing.Point(0, 43);
            this.KartuStokGrid.Name = "KartuStokGrid";
            this.KartuStokGrid.Size = new System.Drawing.Size(756, 453);
            this.KartuStokGrid.TabIndex = 0;
            // 
            // ExcelButton
            // 
            this.ExcelButton.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ExcelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExcelButton.Location = new System.Drawing.Point(663, 9);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(77, 26);
            this.ExcelButton.TabIndex = 6;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = false;
            // 
            // KartuStokInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(1028, 511);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "KartuStokInfoForm";
            this.Text = "Kartu Stok";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ListBarangButton;
        private System.Windows.Forms.TextBox SearchBrgText;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MonthCalendar PeriodeCalender;
        private System.Windows.Forms.DataGridView KartuStokGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton PengakuanRadioButton;
        private System.Windows.Forms.RadioButton PencatatanRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView BrgGrid;
        private System.Windows.Forms.Button ExcelButton;
    }
}