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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TglTagihText = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.TotalTagihanText = new System.Windows.Forms.NumericUpDown();
            this.SalesCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TagihanIdLabel = new System.Windows.Forms.Label();
            this.TagihanIdText = new System.Windows.Forms.TextBox();
            this.FakturGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalTagihanText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TglTagihText);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.TotalTagihanText);
            this.panel1.Controls.Add(this.SalesCombo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TagihanIdLabel);
            this.panel1.Controls.Add(this.TagihanIdText);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 68);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(391, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Total Tagihan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tgl Tagih";
            // 
            // TglTagihText
            // 
            this.TglTagihText.CustomFormat = "dd-MMM-yyyy";
            this.TglTagihText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.TglTagihText.Location = new System.Drawing.Point(154, 28);
            this.TglTagihText.Name = "TglTagihText";
            this.TglTagihText.Size = new System.Drawing.Size(107, 22);
            this.TglTagihText.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(121, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // TotalTagihanText
            // 
            this.TotalTagihanText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalTagihanText.Location = new System.Drawing.Point(394, 29);
            this.TotalTagihanText.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.TotalTagihanText.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147483648 });
            this.TotalTagihanText.Name = "TotalTagihanText";
            this.TotalTagihanText.ReadOnly = true;
            this.TotalTagihanText.Size = new System.Drawing.Size(138, 20);
            this.TotalTagihanText.TabIndex = 5;
            this.TotalTagihanText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TotalTagihanText.ThousandsSeparator = true;
            // 
            // SalesCombo
            // 
            this.SalesCombo.FormattingEnabled = true;
            this.SalesCombo.Location = new System.Drawing.Point(267, 29);
            this.SalesCombo.Name = "SalesCombo";
            this.SalesCombo.Size = new System.Drawing.Size(121, 21);
            this.SalesCombo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sales";
            // 
            // TagihanIdLabel
            // 
            this.TagihanIdLabel.AutoSize = true;
            this.TagihanIdLabel.Location = new System.Drawing.Point(12, 11);
            this.TagihanIdLabel.Name = "TagihanIdLabel";
            this.TagihanIdLabel.Size = new System.Drawing.Size(62, 13);
            this.TagihanIdLabel.TabIndex = 1;
            this.TagihanIdLabel.Text = "Tagihan ID";
            // 
            // TagihanIdText
            // 
            this.TagihanIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TagihanIdText.Location = new System.Drawing.Point(15, 27);
            this.TagihanIdText.Name = "TagihanIdText";
            this.TagihanIdText.Size = new System.Drawing.Size(100, 22);
            this.TagihanIdText.TabIndex = 0;
            // 
            // FakturGrid
            // 
            this.FakturGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.FakturGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGrid.Location = new System.Drawing.Point(9, 83);
            this.FakturGrid.Name = "FakturGrid";
            this.FakturGrid.Size = new System.Drawing.Size(782, 326);
            this.FakturGrid.TabIndex = 1;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(716, 415);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // TagihanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FakturGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TagihanForm";
            this.Text = "TagihanForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalTagihanText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label LastIdLabel;

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView FakturGrid;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label TagihanIdLabel;
        private System.Windows.Forms.TextBox TagihanIdText;
        private System.Windows.Forms.NumericUpDown TotalTagihanText;
        private System.Windows.Forms.ComboBox SalesCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker TglTagihText;
    }
}