namespace btr.distrib.InventoryContext.MutasiRpt
{
    partial class MutasiInfoForm
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
            this.BrgText = new System.Windows.Forms.TextBox();
            this.Tgl2Date = new System.Windows.Forms.DateTimePicker();
            this.Tgl1Date = new System.Windows.Forms.DateTimePicker();
            this.InfoGrid = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.ProsesButton);
            this.panel1.Controls.Add(this.BrgText);
            this.panel1.Controls.Add(this.Tgl2Date);
            this.panel1.Controls.Add(this.Tgl1Date);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 34);
            this.panel1.TabIndex = 6;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Location = new System.Drawing.Point(617, 6);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelButton.TabIndex = 4;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = true;
            // 
            // ProsesButton
            // 
            this.ProsesButton.Location = new System.Drawing.Point(492, 6);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(119, 23);
            this.ProsesButton.TabIndex = 2;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = true;
            // 
            // BrgText
            // 
            this.BrgText.Location = new System.Drawing.Point(330, 6);
            this.BrgText.Name = "BrgText";
            this.BrgText.Size = new System.Drawing.Size(156, 22);
            this.BrgText.TabIndex = 3;
            // 
            // Tgl2Date
            // 
            this.Tgl2Date.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Tgl2Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl2Date.Location = new System.Drawing.Point(168, 6);
            this.Tgl2Date.Name = "Tgl2Date";
            this.Tgl2Date.Size = new System.Drawing.Size(156, 22);
            this.Tgl2Date.TabIndex = 1;
            // 
            // Tgl1Date
            // 
            this.Tgl1Date.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Tgl1Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl1Date.Location = new System.Drawing.Point(6, 6);
            this.Tgl1Date.Name = "Tgl1Date";
            this.Tgl1Date.Size = new System.Drawing.Size(156, 22);
            this.Tgl1Date.TabIndex = 0;
            // 
            // InfoGrid
            // 
            this.InfoGrid.AlphaBlendSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.InfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGrid.BackColor = System.Drawing.SystemColors.Window;
            this.InfoGrid.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoGrid.Location = new System.Drawing.Point(7, 47);
            this.InfoGrid.Name = "InfoGrid";
            this.InfoGrid.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
            this.InfoGrid.Size = new System.Drawing.Size(787, 397);
            this.InfoGrid.TabIndex = 7;
            this.InfoGrid.Text = "gridGroupingControl1";
            this.InfoGrid.UseRightToLeftCompatibleTextBox = true;
            this.InfoGrid.VersionInfo = "22.1460.34";
            // 
            // MutasiInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InfoGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MutasiInfoForm";
            this.Text = "MutasiInfoForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.Button ProsesButton;
        private System.Windows.Forms.TextBox BrgText;
        private System.Windows.Forms.DateTimePicker Tgl2Date;
        private System.Windows.Forms.DateTimePicker Tgl1Date;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGrid;
    }
}