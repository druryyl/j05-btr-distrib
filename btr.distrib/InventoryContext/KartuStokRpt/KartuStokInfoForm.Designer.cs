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
            this.ListBarangButton = new System.Windows.Forms.Button();
            this.BrgGrid = new System.Windows.Forms.DataGridView();
            this.KartuStokGrid = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).BeginInit();
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
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SearchBrgText);
            this.panel1.Controls.Add(this.ListBarangButton);
            this.panel1.Controls.Add(this.Periode1Date);
            this.panel1.Controls.Add(this.Periode2Date);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 70);
            this.panel1.TabIndex = 2;
            // 
            // SearchBrgText
            // 
            this.SearchBrgText.Location = new System.Drawing.Point(6, 34);
            this.SearchBrgText.Name = "SearchBrgText";
            this.SearchBrgText.Size = new System.Drawing.Size(268, 22);
            this.SearchBrgText.TabIndex = 4;
            // 
            // ListBarangButton
            // 
            this.ListBarangButton.Location = new System.Drawing.Point(280, 6);
            this.ListBarangButton.Name = "ListBarangButton";
            this.ListBarangButton.Size = new System.Drawing.Size(55, 49);
            this.ListBarangButton.TabIndex = 3;
            this.ListBarangButton.Text = "List Barang";
            this.ListBarangButton.UseVisualStyleBackColor = true;
            // 
            // BrgGrid
            // 
            this.BrgGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.BrgGrid.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.BrgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGrid.Location = new System.Drawing.Point(6, 82);
            this.BrgGrid.Name = "BrgGrid";
            this.BrgGrid.Size = new System.Drawing.Size(340, 359);
            this.BrgGrid.TabIndex = 3;
            // 
            // KartuStokGrid
            // 
            this.KartuStokGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.KartuStokGrid.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.KartuStokGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KartuStokGrid.Location = new System.Drawing.Point(352, 6);
            this.KartuStokGrid.Name = "KartuStokGrid";
            this.KartuStokGrid.Size = new System.Drawing.Size(480, 435);
            this.KartuStokGrid.TabIndex = 4;
            // 
            // KartuStokInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(841, 450);
            this.Controls.Add(this.KartuStokGrid);
            this.Controls.Add(this.BrgGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "KartuStokInfoForm";
            this.Text = "KartuStokInfo";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KartuStokGrid)).EndInit();
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
    }
}