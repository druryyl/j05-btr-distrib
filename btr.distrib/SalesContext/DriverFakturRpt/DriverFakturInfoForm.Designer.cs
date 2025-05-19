namespace btr.distrib.SalesContext.DriverFakturRpt
{
    partial class DriverFakturInfoForm
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
            this.Tgl1Date = new System.Windows.Forms.DateTimePicker();
            this.DriverCombo = new System.Windows.Forms.ComboBox();
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
            this.panel1.Controls.Add(this.DriverCombo);
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.ProsesButton);
            this.panel1.Controls.Add(this.Tgl1Date);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 34);
            this.panel1.TabIndex = 3;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Location = new System.Drawing.Point(468, 4);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelButton.TabIndex = 4;
            this.ExcelButton.Text = "Print";
            this.ExcelButton.UseVisualStyleBackColor = true;
            // 
            // ProsesButton
            // 
            this.ProsesButton.Location = new System.Drawing.Point(343, 4);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(119, 23);
            this.ProsesButton.TabIndex = 2;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = true;
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
            // DriverCombo
            // 
            this.DriverCombo.FormattingEnabled = true;
            this.DriverCombo.Location = new System.Drawing.Point(168, 6);
            this.DriverCombo.Name = "DriverCombo";
            this.DriverCombo.Size = new System.Drawing.Size(169, 21);
            this.DriverCombo.TabIndex = 5;
            // 
            // InfoGrid
            // 
            this.InfoGrid.AlphaBlendSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.InfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGrid.BackColor = System.Drawing.SystemColors.Window;
            this.InfoGrid.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoGrid.Location = new System.Drawing.Point(6, 46);
            this.InfoGrid.Name = "InfoGrid";
            this.InfoGrid.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
            this.InfoGrid.Size = new System.Drawing.Size(787, 397);
            this.InfoGrid.TabIndex = 4;
            this.InfoGrid.Text = "gridGroupingControl1";
            this.InfoGrid.UseRightToLeftCompatibleTextBox = true;
            this.InfoGrid.VersionInfo = "22.1460.34";
            // 
            // DriverFakturInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InfoGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DriverFakturInfoForm";
            this.Text = "Driver Faktur Report";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.Button ProsesButton;
        private System.Windows.Forms.DateTimePicker Tgl1Date;
        private System.Windows.Forms.ComboBox DriverCombo;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGrid;
    }
}