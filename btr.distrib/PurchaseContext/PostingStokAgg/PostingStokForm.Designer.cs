namespace btr.distrib.PurchaseContext.PostingStokAgg
{
    partial class PostingStokForm
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
            this.InvoiceGrid = new System.Windows.Forms.DataGridView();
            this.SearchButton = new System.Windows.Forms.Button();
            this.InvoiceCalender = new System.Windows.Forms.MonthCalendar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AllOutStanding = new System.Windows.Forms.CheckBox();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GrandTotalText = new System.Windows.Forms.TextBox();
            this.SupplierNameText = new System.Windows.Forms.TextBox();
            this.InvoiceDateText = new System.Windows.Forms.TextBox();
            this.InvoiceIdText = new System.Windows.Forms.TextBox();
            this.PostingButton = new System.Windows.Forms.Button();
            this.InvoiceCodeText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // InvoiceGrid
            // 
            this.InvoiceGrid.AllowUserToAddRows = false;
            this.InvoiceGrid.AllowUserToDeleteRows = false;
            this.InvoiceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvoiceGrid.BackgroundColor = System.Drawing.Color.DarkSlateGray;
            this.InvoiceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InvoiceGrid.Location = new System.Drawing.Point(239, 6);
            this.InvoiceGrid.Name = "InvoiceGrid";
            this.InvoiceGrid.Size = new System.Drawing.Size(630, 509);
            this.InvoiceGrid.TabIndex = 0;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Location = new System.Drawing.Point(138, 37);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(81, 23);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            // 
            // InvoiceCalender
            // 
            this.InvoiceCalender.Location = new System.Drawing.Point(6, 6);
            this.InvoiceCalender.MaxSelectionCount = 31;
            this.InvoiceCalender.Name = "InvoiceCalender";
            this.InvoiceCalender.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.AllOutStanding);
            this.panel1.Controls.Add(this.SearchText);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Location = new System.Drawing.Point(6, 170);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 112);
            this.panel1.TabIndex = 3;
            // 
            // AllOutStanding
            // 
            this.AllOutStanding.AutoSize = true;
            this.AllOutStanding.Location = new System.Drawing.Point(6, 37);
            this.AllOutStanding.Name = "AllOutStanding";
            this.AllOutStanding.Size = new System.Drawing.Size(108, 17);
            this.AllOutStanding.TabIndex = 3;
            this.AllOutStanding.Text = "All Outstanding";
            this.AllOutStanding.UseVisualStyleBackColor = true;
            // 
            // SearchText
            // 
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchText.Location = new System.Drawing.Point(6, 9);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(213, 22);
            this.SearchText.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.InvoiceCodeText);
            this.panel2.Controls.Add(this.GrandTotalText);
            this.panel2.Controls.Add(this.SupplierNameText);
            this.panel2.Controls.Add(this.InvoiceDateText);
            this.panel2.Controls.Add(this.InvoiceIdText);
            this.panel2.Controls.Add(this.PostingButton);
            this.panel2.Location = new System.Drawing.Point(6, 285);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 230);
            this.panel2.TabIndex = 4;
            // 
            // GrandTotalText
            // 
            this.GrandTotalText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GrandTotalText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrandTotalText.Location = new System.Drawing.Point(6, 121);
            this.GrandTotalText.Name = "GrandTotalText";
            this.GrandTotalText.ReadOnly = true;
            this.GrandTotalText.Size = new System.Drawing.Size(213, 20);
            this.GrandTotalText.TabIndex = 6;
            this.GrandTotalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SupplierNameText
            // 
            this.SupplierNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SupplierNameText.Location = new System.Drawing.Point(6, 93);
            this.SupplierNameText.Name = "SupplierNameText";
            this.SupplierNameText.ReadOnly = true;
            this.SupplierNameText.Size = new System.Drawing.Size(213, 22);
            this.SupplierNameText.TabIndex = 5;
            // 
            // InvoiceDateText
            // 
            this.InvoiceDateText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InvoiceDateText.Location = new System.Drawing.Point(6, 65);
            this.InvoiceDateText.Name = "InvoiceDateText";
            this.InvoiceDateText.ReadOnly = true;
            this.InvoiceDateText.Size = new System.Drawing.Size(213, 22);
            this.InvoiceDateText.TabIndex = 4;
            // 
            // InvoiceIdText
            // 
            this.InvoiceIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InvoiceIdText.Location = new System.Drawing.Point(6, 9);
            this.InvoiceIdText.Name = "InvoiceIdText";
            this.InvoiceIdText.ReadOnly = true;
            this.InvoiceIdText.Size = new System.Drawing.Size(213, 22);
            this.InvoiceIdText.TabIndex = 3;
            // 
            // PostingButton
            // 
            this.PostingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PostingButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PostingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PostingButton.Location = new System.Drawing.Point(138, 147);
            this.PostingButton.Name = "PostingButton";
            this.PostingButton.Size = new System.Drawing.Size(81, 23);
            this.PostingButton.TabIndex = 1;
            this.PostingButton.Text = "Posting";
            this.PostingButton.UseVisualStyleBackColor = false;
            // 
            // InvoiceCodeText
            // 
            this.InvoiceCodeText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InvoiceCodeText.Location = new System.Drawing.Point(6, 37);
            this.InvoiceCodeText.Name = "InvoiceCodeText";
            this.InvoiceCodeText.ReadOnly = true;
            this.InvoiceCodeText.Size = new System.Drawing.Size(213, 22);
            this.InvoiceCodeText.TabIndex = 7;
            // 
            // PostingStokForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(876, 524);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InvoiceCalender);
            this.Controls.Add(this.InvoiceGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "PostingStokForm";
            this.Text = "Posting Stok";
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView InvoiceGrid;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.MonthCalendar InvoiceCalender;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox AllOutStanding;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button PostingButton;
        private System.Windows.Forms.TextBox GrandTotalText;
        private System.Windows.Forms.TextBox SupplierNameText;
        private System.Windows.Forms.TextBox InvoiceDateText;
        private System.Windows.Forms.TextBox InvoiceIdText;
        private System.Windows.Forms.TextBox InvoiceCodeText;
    }
}