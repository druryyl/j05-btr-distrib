namespace btr.distrib.FinanceContext.PiutangSalesWilayahRpt
{
    partial class PiutangSalesWilayahForm
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
            this.Faktur2Date = new System.Windows.Forms.DateTimePicker();
            this.Faktur1Date = new System.Windows.Forms.DateTimePicker();
            this.ExcelButton = new System.Windows.Forms.Button();
            this.ProsesButton = new System.Windows.Forms.Button();
            this.InfoGrid = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.ExcelFlatButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.Controls.Add(this.ExcelFlatButton);
            this.panel1.Controls.Add(this.Faktur2Date);
            this.panel1.Controls.Add(this.Faktur1Date);
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.ProsesButton);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 34);
            this.panel1.TabIndex = 4;
            // 
            // Faktur2Date
            // 
            this.Faktur2Date.CustomFormat = "ddd, dd MMM yyyy";
            this.Faktur2Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Faktur2Date.Location = new System.Drawing.Point(144, 6);
            this.Faktur2Date.Name = "Faktur2Date";
            this.Faktur2Date.Size = new System.Drawing.Size(133, 22);
            this.Faktur2Date.TabIndex = 6;
            // 
            // Faktur1Date
            // 
            this.Faktur1Date.CustomFormat = "ddd, dd MMM yyyy";
            this.Faktur1Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Faktur1Date.Location = new System.Drawing.Point(5, 6);
            this.Faktur1Date.Name = "Faktur1Date";
            this.Faktur1Date.Size = new System.Drawing.Size(133, 22);
            this.Faktur1Date.TabIndex = 5;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Location = new System.Drawing.Point(408, 5);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelButton.TabIndex = 4;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = true;
            // 
            // ProsesButton
            // 
            this.ProsesButton.Location = new System.Drawing.Point(283, 5);
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
            // ExcelFlatButton
            // 
            this.ExcelFlatButton.Location = new System.Drawing.Point(533, 6);
            this.ExcelFlatButton.Name = "ExcelFlatButton";
            this.ExcelFlatButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelFlatButton.TabIndex = 7;
            this.ExcelFlatButton.Text = "Excel Flat";
            this.ExcelFlatButton.UseVisualStyleBackColor = true;
            // 
            // PiutangSalesWilayahForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InfoGrid);
            this.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PiutangSalesWilayahForm";
            this.Text = "PiutangSalesWilayahForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.Button ProsesButton;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGrid;
        private System.Windows.Forms.DateTimePicker Faktur2Date;
        private System.Windows.Forms.DateTimePicker Faktur1Date;
        private System.Windows.Forms.Button ExcelFlatButton;
    }
}