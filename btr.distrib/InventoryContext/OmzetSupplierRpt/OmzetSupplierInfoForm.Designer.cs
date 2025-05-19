namespace btr.distrib.InventoryContext.OmzetSupplierRpt
{
    partial class OmzetSupplierInfoForm
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
            this.ExcelButton = new System.Windows.Forms.Button();
            this.ProsesButton = new System.Windows.Forms.Button();
            this.InfoGrid = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.HarianRadio = new System.Windows.Forms.RadioButton();
            this.BulananRadio = new System.Windows.Forms.RadioButton();
            this.HarianText = new System.Windows.Forms.MaskedTextBox();
            this.BulananText = new System.Windows.Forms.MaskedTextBox();
            this.InfoGridBulanan = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGridBulanan)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.Controls.Add(this.BulananText);
            this.panel1.Controls.Add(this.HarianText);
            this.panel1.Controls.Add(this.BulananRadio);
            this.panel1.Controls.Add(this.HarianRadio);
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.ProsesButton);
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 34);
            this.panel1.TabIndex = 4;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Location = new System.Drawing.Point(662, 5);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelButton.TabIndex = 4;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = true;
            // 
            // ProsesButton
            // 
            this.ProsesButton.Location = new System.Drawing.Point(538, 4);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(119, 23);
            this.ProsesButton.TabIndex = 2;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = true;
            // 
            // InfoGrid
            // 
            this.InfoGrid.AlphaBlendSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.InfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGrid.BackColor = System.Drawing.SystemColors.Window;
            this.InfoGrid.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoGrid.Location = new System.Drawing.Point(7, 47);
            this.InfoGrid.Name = "InfoGrid";
            this.InfoGrid.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
            this.InfoGrid.Size = new System.Drawing.Size(787, 397);
            this.InfoGrid.TabIndex = 5;
            this.InfoGrid.Text = "gridGroupingControl1";
            this.InfoGrid.UseRightToLeftCompatibleTextBox = true;
            this.InfoGrid.VersionInfo = "22.1460.34";
            // 
            // HarianRadio
            // 
            this.HarianRadio.AutoSize = true;
            this.HarianRadio.Checked = true;
            this.HarianRadio.Location = new System.Drawing.Point(21, 7);
            this.HarianRadio.Name = "HarianRadio";
            this.HarianRadio.Size = new System.Drawing.Size(59, 17);
            this.HarianRadio.TabIndex = 5;
            this.HarianRadio.TabStop = true;
            this.HarianRadio.Text = "Harian";
            this.HarianRadio.UseVisualStyleBackColor = true;
            // 
            // BulananRadio
            // 
            this.BulananRadio.AutoSize = true;
            this.BulananRadio.Location = new System.Drawing.Point(175, 6);
            this.BulananRadio.Name = "BulananRadio";
            this.BulananRadio.Size = new System.Drawing.Size(67, 17);
            this.BulananRadio.TabIndex = 6;
            this.BulananRadio.Text = "Bulanan";
            this.BulananRadio.UseVisualStyleBackColor = true;
            this.BulananRadio.CheckedChanged += new System.EventHandler(this.BulananRadio_CheckedChanged);
            // 
            // HarianText
            // 
            this.HarianText.Location = new System.Drawing.Point(86, 5);
            this.HarianText.Name = "HarianText";
            this.HarianText.Size = new System.Drawing.Size(56, 22);
            this.HarianText.TabIndex = 7;
            // 
            // BulananText
            // 
            this.BulananText.Location = new System.Drawing.Point(248, 5);
            this.BulananText.Name = "BulananText";
            this.BulananText.Size = new System.Drawing.Size(59, 22);
            this.BulananText.TabIndex = 8;
            // 
            // InfoGridBulanan
            // 
            this.InfoGridBulanan.AlphaBlendSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.InfoGridBulanan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGridBulanan.BackColor = System.Drawing.SystemColors.Window;
            this.InfoGridBulanan.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoGridBulanan.Location = new System.Drawing.Point(7, 47);
            this.InfoGridBulanan.Name = "InfoGridBulanan";
            this.InfoGridBulanan.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
            this.InfoGridBulanan.Size = new System.Drawing.Size(787, 397);
            this.InfoGridBulanan.TabIndex = 6;
            this.InfoGridBulanan.Text = "gridGroupingControl1";
            this.InfoGridBulanan.UseRightToLeftCompatibleTextBox = true;
            this.InfoGridBulanan.VersionInfo = "22.1460.34";
            this.InfoGridBulanan.Visible = false;
            // 
            // OmzetSupplierInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InfoGridBulanan);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InfoGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OmzetSupplierInfoForm";
            this.Text = "OmzetSupplierInfoForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGridBulanan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.Button ProsesButton;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGrid;
        private System.Windows.Forms.RadioButton HarianRadio;
        private System.Windows.Forms.RadioButton BulananRadio;
        private System.Windows.Forms.MaskedTextBox BulananText;
        private System.Windows.Forms.MaskedTextBox HarianText;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGridBulanan;
    }
}