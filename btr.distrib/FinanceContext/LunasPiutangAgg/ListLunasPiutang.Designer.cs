namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    partial class ListLunasPiutang
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
            this.ListFakturLayout = new System.Windows.Forms.TableLayoutPanel();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.Tgl2Text = new System.Windows.Forms.DateTimePicker();
            this.Tgl1Text = new System.Windows.Forms.DateTimePicker();
            this.ListFakturLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListFakturLayout
            // 
            this.ListFakturLayout.ColumnCount = 1;
            this.ListFakturLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ListFakturLayout.Controls.Add(this.FakturGrid, 0, 1);
            this.ListFakturLayout.Controls.Add(this.panel1, 0, 0);
            this.ListFakturLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListFakturLayout.Location = new System.Drawing.Point(0, 0);
            this.ListFakturLayout.Name = "ListFakturLayout";
            this.ListFakturLayout.RowCount = 2;
            this.ListFakturLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.ListFakturLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ListFakturLayout.Size = new System.Drawing.Size(800, 450);
            this.ListFakturLayout.TabIndex = 2;
            // 
            // FakturGrid
            // 
            this.FakturGrid.AllowUserToAddRows = false;
            this.FakturGrid.AllowUserToDeleteRows = false;
            this.FakturGrid.AllowUserToOrderColumns = true;
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FakturGrid.Location = new System.Drawing.Point(3, 48);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(794, 399);
            this.FakturGrid.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.SearchText);
            this.panel1.Controls.Add(this.Tgl2Text);
            this.panel1.Controls.Add(this.Tgl1Text);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 39);
            this.panel1.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(560, 7);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(281, 9);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(273, 22);
            this.SearchText.TabIndex = 2;
            // 
            // Tgl2Text
            // 
            this.Tgl2Text.CustomFormat = "ddd dd MMM yyyy";
            this.Tgl2Text.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl2Text.Location = new System.Drawing.Point(145, 9);
            this.Tgl2Text.Name = "Tgl2Text";
            this.Tgl2Text.Size = new System.Drawing.Size(130, 22);
            this.Tgl2Text.TabIndex = 1;
            // 
            // Tgl1Text
            // 
            this.Tgl1Text.CustomFormat = "ddd dd MMM yyyy";
            this.Tgl1Text.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Tgl1Text.Location = new System.Drawing.Point(9, 9);
            this.Tgl1Text.Name = "Tgl1Text";
            this.Tgl1Text.Size = new System.Drawing.Size(130, 22);
            this.Tgl1Text.TabIndex = 0;
            // 
            // ListLunasPiutang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ListFakturLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ListLunasPiutang";
            this.Text = "ListLunasPiutang";
            this.ListFakturLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ListFakturLayout;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.DateTimePicker Tgl2Text;
        private System.Windows.Forms.DateTimePicker Tgl1Text;
    }
}