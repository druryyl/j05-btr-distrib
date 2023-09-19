namespace btr.distrib.PrintDocs
{
    partial class PrintManagerForm
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
            this.components = new System.ComponentModel.Container();
            this.RefreshPrgBar = new System.Windows.Forms.ProgressBar();
            this.WarehouseCombo = new System.Windows.Forms.ComboBox();
            this.PrintTimer = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.Tgl2Text = new System.Windows.Forms.DateTimePicker();
            this.Tgl1Text = new System.Windows.Forms.DateTimePicker();
            this.PrinterLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.GridAtas = new System.Windows.Forms.DataGridView();
            this.GridBawah = new System.Windows.Forms.DataGridView();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridAtas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBawah)).BeginInit();
            this.SuspendLayout();
            // 
            // RefreshPrgBar
            // 
            this.RefreshPrgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshPrgBar.Location = new System.Drawing.Point(1, 439);
            this.RefreshPrgBar.Name = "RefreshPrgBar";
            this.RefreshPrgBar.Size = new System.Drawing.Size(582, 10);
            this.RefreshPrgBar.TabIndex = 5;
            // 
            // WarehouseCombo
            // 
            this.WarehouseCombo.FormattingEnabled = true;
            this.WarehouseCombo.Location = new System.Drawing.Point(288, 8);
            this.WarehouseCombo.Name = "WarehouseCombo";
            this.WarehouseCombo.Size = new System.Drawing.Size(186, 21);
            this.WarehouseCombo.TabIndex = 0;
            // 
            // PrintTimer
            // 
            this.PrintTimer.Enabled = true;
            this.PrintTimer.Interval = 1000;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.panel2.Controls.Add(this.Tgl2Text);
            this.panel2.Controls.Add(this.WarehouseCombo);
            this.panel2.Controls.Add(this.Tgl1Text);
            this.panel2.Controls.Add(this.PrinterLabel);
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(572, 64);
            this.panel2.TabIndex = 4;
            // 
            // Tgl2Text
            // 
            this.Tgl2Text.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Tgl2Text.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl2Text.Location = new System.Drawing.Point(147, 7);
            this.Tgl2Text.Name = "Tgl2Text";
            this.Tgl2Text.Size = new System.Drawing.Size(135, 22);
            this.Tgl2Text.TabIndex = 2;
            // 
            // Tgl1Text
            // 
            this.Tgl1Text.CustomFormat = "ddd, dd-MMM-yyyy";
            this.Tgl1Text.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl1Text.Location = new System.Drawing.Point(6, 7);
            this.Tgl1Text.Name = "Tgl1Text";
            this.Tgl1Text.Size = new System.Drawing.Size(135, 22);
            this.Tgl1Text.TabIndex = 1;
            // 
            // PrinterLabel
            // 
            this.PrinterLabel.AutoSize = true;
            this.PrinterLabel.Location = new System.Drawing.Point(6, 41);
            this.PrinterLabel.Name = "PrinterLabel";
            this.PrinterLabel.Size = new System.Drawing.Size(79, 13);
            this.PrinterLabel.TabIndex = 0;
            this.PrinterLabel.Text = "[Printer Name]";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 79);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.GridAtas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GridBawah);
            this.splitContainer1.Size = new System.Drawing.Size(572, 352);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 5;
            // 
            // GridAtas
            // 
            this.GridAtas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridAtas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridAtas.Location = new System.Drawing.Point(0, 0);
            this.GridAtas.Name = "GridAtas";
            this.GridAtas.Size = new System.Drawing.Size(572, 176);
            this.GridAtas.TabIndex = 1;
            // 
            // GridBawah
            // 
            this.GridBawah.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridBawah.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridBawah.Location = new System.Drawing.Point(0, 0);
            this.GridBawah.Name = "GridBawah";
            this.GridBawah.Size = new System.Drawing.Size(572, 172);
            this.GridBawah.TabIndex = 4;
            // 
            // PrintManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(585, 450);
            this.Controls.Add(this.RefreshPrgBar);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintManagerForm";
            this.Text = "Print Manager";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridAtas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBawah)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox WarehouseCombo;
        private System.Windows.Forms.Timer PrintTimer;
        private System.Windows.Forms.ProgressBar RefreshPrgBar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label PrinterLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView GridAtas;
        private System.Windows.Forms.DataGridView GridBawah;
        private System.Windows.Forms.DateTimePicker Tgl1Text;
        private System.Windows.Forms.DateTimePicker Tgl2Text;
    }
}