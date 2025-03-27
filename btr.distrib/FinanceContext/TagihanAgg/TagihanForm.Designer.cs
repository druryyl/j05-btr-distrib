namespace btr.distrib.FinanceContext.TagihanAgg
{
    partial class TagihanForm
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
            this.LastIdLabel = new System.Windows.Forms.Label();
            this.TglTagihText = new System.Windows.Forms.DateTimePicker();
            this.TagihanButton = new System.Windows.Forms.Button();
            this.TagihanIdLabel = new System.Windows.Forms.Label();
            this.TagihanIdText = new System.Windows.Forms.TextBox();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListFakturButton = new System.Windows.Forms.Button();
            this.SalesRuteCombo = new System.Windows.Forms.ComboBox();
            this.SalesCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TotalTagihanLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.JumlahFakturLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.TglTagihText);
            this.panel1.Controls.Add(this.TagihanButton);
            this.panel1.Controls.Add(this.TagihanIdLabel);
            this.panel1.Controls.Add(this.TagihanIdText);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 86);
            this.panel1.TabIndex = 0;
            // 
            // LastIdLabel
            // 
            this.LastIdLabel.AutoSize = true;
            this.LastIdLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastIdLabel.Location = new System.Drawing.Point(15, 52);
            this.LastIdLabel.Name = "LastIdLabel";
            this.LastIdLabel.Size = new System.Drawing.Size(0, 13);
            this.LastIdLabel.TabIndex = 10;
            this.LastIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TglTagihText
            // 
            this.TglTagihText.CustomFormat = "ddd, dd MMM yyyy";
            this.TglTagihText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.TglTagihText.Location = new System.Drawing.Point(9, 50);
            this.TglTagihText.Name = "TglTagihText";
            this.TglTagihText.Size = new System.Drawing.Size(136, 22);
            this.TglTagihText.TabIndex = 7;
            // 
            // TagihanButton
            // 
            this.TagihanButton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.TagihanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TagihanButton.Location = new System.Drawing.Point(119, 22);
            this.TagihanButton.Name = "TagihanButton";
            this.TagihanButton.Size = new System.Drawing.Size(26, 22);
            this.TagihanButton.TabIndex = 6;
            this.TagihanButton.Text = "...";
            this.TagihanButton.UseVisualStyleBackColor = false;
            // 
            // TagihanIdLabel
            // 
            this.TagihanIdLabel.AutoSize = true;
            this.TagihanIdLabel.Location = new System.Drawing.Point(6, 6);
            this.TagihanIdLabel.Name = "TagihanIdLabel";
            this.TagihanIdLabel.Size = new System.Drawing.Size(61, 13);
            this.TagihanIdLabel.TabIndex = 1;
            this.TagihanIdLabel.Text = "Tagihan ID";
            // 
            // TagihanIdText
            // 
            this.TagihanIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TagihanIdText.Location = new System.Drawing.Point(9, 22);
            this.TagihanIdText.Name = "TagihanIdText";
            this.TagihanIdText.Size = new System.Drawing.Size(107, 22);
            this.TagihanIdText.TabIndex = 0;
            // 
            // FakturGrid
            // 
            this.FakturGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FakturGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(56)))));
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Location = new System.Drawing.Point(6, 96);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(927, 337);
            this.FakturGrid.TabIndex = 1;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(858, 438);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 13;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ListFakturButton);
            this.panel2.Controls.Add(this.SalesRuteCombo);
            this.panel2.Controls.Add(this.SalesCombo);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(184, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(427, 86);
            this.panel2.TabIndex = 14;
            // 
            // ListFakturButton
            // 
            this.ListFakturButton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ListFakturButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListFakturButton.Location = new System.Drawing.Point(141, 46);
            this.ListFakturButton.Name = "ListFakturButton";
            this.ListFakturButton.Size = new System.Drawing.Size(110, 23);
            this.ListFakturButton.TabIndex = 20;
            this.ListFakturButton.Text = "List Faktur";
            this.ListFakturButton.UseVisualStyleBackColor = false;
            // 
            // SalesRuteCombo
            // 
            this.SalesRuteCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SalesRuteCombo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SalesRuteCombo.FormattingEnabled = true;
            this.SalesRuteCombo.Location = new System.Drawing.Point(7, 48);
            this.SalesRuteCombo.Name = "SalesRuteCombo";
            this.SalesRuteCombo.Size = new System.Drawing.Size(124, 21);
            this.SalesRuteCombo.TabIndex = 19;
            // 
            // SalesCombo
            // 
            this.SalesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SalesCombo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SalesCombo.FormattingEnabled = true;
            this.SalesCombo.Location = new System.Drawing.Point(7, 21);
            this.SalesCombo.Name = "SalesCombo";
            this.SalesCombo.Size = new System.Drawing.Size(124, 21);
            this.SalesCombo.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sales / Rute";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.TotalTagihanLabel);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(615, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 86);
            this.panel3.TabIndex = 15;
            // 
            // TotalTagihanLabel
            // 
            this.TotalTagihanLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalTagihanLabel.Location = new System.Drawing.Point(-1, 27);
            this.TotalTagihanLabel.Name = "TotalTagihanLabel";
            this.TotalTagihanLabel.Size = new System.Drawing.Size(162, 42);
            this.TotalTagihanLabel.TabIndex = 21;
            this.TotalTagihanLabel.Text = "Rp. 0,-";
            this.TotalTagihanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Total Tagihan";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(230)))), ((int)(((byte)(242)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.JumlahFakturLabel);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(781, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(152, 86);
            this.panel4.TabIndex = 16;
            // 
            // JumlahFakturLabel
            // 
            this.JumlahFakturLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JumlahFakturLabel.Location = new System.Drawing.Point(-1, 27);
            this.JumlahFakturLabel.Name = "JumlahFakturLabel";
            this.JumlahFakturLabel.Size = new System.Drawing.Size(152, 42);
            this.JumlahFakturLabel.TabIndex = 21;
            this.JumlahFakturLabel.Text = "99";
            this.JumlahFakturLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(-1, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Jumlah Faktur";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TagihanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(140)))), ((int)(((byte)(175)))));
            this.ClientSize = new System.Drawing.Size(940, 467);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FakturGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TagihanForm";
            this.Text = "TagihanForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label LastIdLabel;

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Label TagihanIdLabel;
        private System.Windows.Forms.TextBox TagihanIdText;
        private System.Windows.Forms.Button TagihanButton;
        private System.Windows.Forms.DateTimePicker TglTagihText;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ListFakturButton;
        private System.Windows.Forms.ComboBox SalesRuteCombo;
        private System.Windows.Forms.ComboBox SalesCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TotalTagihanLabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label JumlahFakturLabel;
        private System.Windows.Forms.Label label5;
    }
}