namespace btr.distrib.SalesContext.FakturControlAgg
{
    partial class FakturControlForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.PrintFakturJualButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.Tgl2Text = new System.Windows.Forms.DateTimePicker();
            this.Tgl1Text = new System.Windows.Forms.DateTimePicker();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.PrintFakturKlaimButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.MediumTurquoise;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FakturGrid, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1081, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.PrintFakturKlaimButton);
            this.Panel1.Controls.Add(this.PrintFakturJualButton);
            this.Panel1.Controls.Add(this.ClearButton);
            this.Panel1.Controls.Add(this.SearchButton);
            this.Panel1.Controls.Add(this.SearchText);
            this.Panel1.Controls.Add(this.Tgl2Text);
            this.Panel1.Controls.Add(this.Tgl1Text);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(3, 3);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1075, 44);
            this.Panel1.TabIndex = 0;
            // 
            // PrintFakturJualButton
            // 
            this.PrintFakturJualButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PrintFakturJualButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintFakturJualButton.ForeColor = System.Drawing.Color.Black;
            this.PrintFakturJualButton.Location = new System.Drawing.Point(845, 7);
            this.PrintFakturJualButton.Name = "PrintFakturJualButton";
            this.PrintFakturJualButton.Size = new System.Drawing.Size(108, 23);
            this.PrintFakturJualButton.TabIndex = 5;
            this.PrintFakturJualButton.Text = "Print Faktur Jual";
            this.PrintFakturJualButton.UseVisualStyleBackColor = false;
            // 
            // ClearButton
            // 
            this.ClearButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearButton.Location = new System.Drawing.Point(641, 7);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 4;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = false;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Location = new System.Drawing.Point(560, 7);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            // 
            // SearchText
            // 
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            // FakturGrid
            // 
            this.FakturGrid.AllowUserToAddRows = false;
            this.FakturGrid.AllowUserToDeleteRows = false;
            this.FakturGrid.AllowUserToOrderColumns = true;
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FakturGrid.Location = new System.Drawing.Point(3, 53);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(1075, 394);
            this.FakturGrid.TabIndex = 1;
            // 
            // PrintFakturKlaimButton
            // 
            this.PrintFakturKlaimButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PrintFakturKlaimButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintFakturKlaimButton.ForeColor = System.Drawing.Color.Black;
            this.PrintFakturKlaimButton.Location = new System.Drawing.Point(959, 7);
            this.PrintFakturKlaimButton.Name = "PrintFakturKlaimButton";
            this.PrintFakturKlaimButton.Size = new System.Drawing.Size(108, 23);
            this.PrintFakturKlaimButton.TabIndex = 6;
            this.PrintFakturKlaimButton.Text = "Print Faktur Klaim";
            this.PrintFakturKlaimButton.UseVisualStyleBackColor = false;
            // 
            // FakturControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FakturControlForm";
            this.Text = "FakturControlForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button ClearButton;

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.DateTimePicker Tgl1Text;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.DateTimePicker Tgl2Text;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button PrintFakturJualButton;
        private System.Windows.Forms.Button PrintFakturKlaimButton;
    }
}