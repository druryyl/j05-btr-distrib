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
            this.GridAtas = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RefreshPrgBar = new System.Windows.Forms.ProgressBar();
            this.RefreshNowButton = new System.Windows.Forms.Button();
            this.WarehouseCombo = new System.Windows.Forms.ComboBox();
            this.PrintTimer = new System.Windows.Forms.Timer(this.components);
            this.GridBawah = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.GridAtas)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridBawah)).BeginInit();
            this.SuspendLayout();
            // 
            // GridAtas
            // 
            this.GridAtas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridAtas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridAtas.Location = new System.Drawing.Point(12, 63);
            this.GridAtas.Name = "GridAtas";
            this.GridAtas.Size = new System.Drawing.Size(561, 130);
            this.GridAtas.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RefreshPrgBar);
            this.panel1.Controls.Add(this.RefreshNowButton);
            this.panel1.Controls.Add(this.WarehouseCombo);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 45);
            this.panel1.TabIndex = 1;
            // 
            // RefreshPrgBar
            // 
            this.RefreshPrgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshPrgBar.Location = new System.Drawing.Point(3, 30);
            this.RefreshPrgBar.Name = "RefreshPrgBar";
            this.RefreshPrgBar.Size = new System.Drawing.Size(551, 10);
            this.RefreshPrgBar.TabIndex = 5;
            // 
            // RefreshNowButton
            // 
            this.RefreshNowButton.Location = new System.Drawing.Point(195, 3);
            this.RefreshNowButton.Name = "RefreshNowButton";
            this.RefreshNowButton.Size = new System.Drawing.Size(119, 23);
            this.RefreshNowButton.TabIndex = 2;
            this.RefreshNowButton.Text = "Refresh Now";
            this.RefreshNowButton.UseVisualStyleBackColor = true;
            // 
            // WarehouseCombo
            // 
            this.WarehouseCombo.FormattingEnabled = true;
            this.WarehouseCombo.Location = new System.Drawing.Point(3, 3);
            this.WarehouseCombo.Name = "WarehouseCombo";
            this.WarehouseCombo.Size = new System.Drawing.Size(186, 21);
            this.WarehouseCombo.TabIndex = 0;
            // 
            // PrintTimer
            // 
            this.PrintTimer.Enabled = true;
            this.PrintTimer.Interval = 1000;
            // 
            // GridBawah
            // 
            this.GridBawah.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridBawah.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridBawah.Location = new System.Drawing.Point(12, 199);
            this.GridBawah.Name = "GridBawah";
            this.GridBawah.Size = new System.Drawing.Size(561, 239);
            this.GridBawah.TabIndex = 3;
            // 
            // PrintManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(585, 450);
            this.Controls.Add(this.GridBawah);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.GridAtas);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintManagerForm";
            this.Text = "Print Manager";
            ((System.ComponentModel.ISupportInitialize)(this.GridAtas)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridBawah)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridAtas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox WarehouseCombo;
        private System.Windows.Forms.Button RefreshNowButton;
        private System.Windows.Forms.Timer PrintTimer;
        private System.Windows.Forms.DataGridView GridBawah;
        private System.Windows.Forms.ProgressBar RefreshPrgBar;
    }
}