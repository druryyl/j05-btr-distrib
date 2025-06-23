namespace btr.distrib.InventoryContext.StokBrgSupplierRpt
{
    partial class StokBrgSupplierForm
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
            this.SearchText = new System.Windows.Forms.TextBox();
            this.InfoGrid = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.ShowInTransitCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ShowInTransitCheckBox);
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.ProsesButton);
            this.panel1.Controls.Add(this.SearchText);
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 34);
            this.panel1.TabIndex = 4;
            // 
            // ExcelButton
            // 
            this.ExcelButton.BackColor = System.Drawing.Color.GhostWhite;
            this.ExcelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExcelButton.Location = new System.Drawing.Point(339, 6);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(119, 23);
            this.ExcelButton.TabIndex = 4;
            this.ExcelButton.Text = "Excel";
            this.ExcelButton.UseVisualStyleBackColor = false;
            // 
            // ProsesButton
            // 
            this.ProsesButton.BackColor = System.Drawing.Color.GhostWhite;
            this.ProsesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProsesButton.Location = new System.Drawing.Point(214, 6);
            this.ProsesButton.Name = "ProsesButton";
            this.ProsesButton.Size = new System.Drawing.Size(119, 23);
            this.ProsesButton.TabIndex = 2;
            this.ProsesButton.Text = "Proses";
            this.ProsesButton.UseVisualStyleBackColor = false;
            // 
            // SearchText
            // 
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchText.Location = new System.Drawing.Point(5, 8);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(203, 18);
            this.SearchText.TabIndex = 3;
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
            // ShowInTransitCheckBox
            // 
            this.ShowInTransitCheckBox.AutoSize = true;
            this.ShowInTransitCheckBox.Location = new System.Drawing.Point(464, 11);
            this.ShowInTransitCheckBox.Name = "ShowInTransitCheckBox";
            this.ShowInTransitCheckBox.Size = new System.Drawing.Size(164, 15);
            this.ShowInTransitCheckBox.TabIndex = 6;
            this.ShowInTransitCheckBox.Text = "Tampilkan In-Transit";
            this.ShowInTransitCheckBox.UseVisualStyleBackColor = true;
            // 
            // StokBrgSupplierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InfoGrid);
            this.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StokBrgSupplierForm";
            this.Text = "StokBrgSupplierForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.Button ProsesButton;
        private System.Windows.Forms.TextBox SearchText;
        private Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl InfoGrid;
        private System.Windows.Forms.CheckBox ShowInTransitCheckBox;
    }
}