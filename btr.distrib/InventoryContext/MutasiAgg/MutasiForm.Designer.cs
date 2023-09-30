using System.Windows.Forms;

namespace btr.distrib.InventoryContext.MutasiAgg
{
    partial class MutasiForm
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
            this.MutasiDateText = new System.Windows.Forms.DateTimePicker();
            this.MutasiDateLabel = new System.Windows.Forms.Label();
            this.LastIdLabel = new System.Windows.Forms.Label();
            this.MutasiButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MutasiIdText = new System.Windows.Forms.TextBox();
            this.MutasiItemGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.WarehouseNameText = new System.Windows.Forms.TextBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.WarehouseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.WarehouseIdText = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.KeteranganText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.TotalText = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MutasiItemGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalText)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.MutasiDateText);
            this.panel1.Controls.Add(this.MutasiDateLabel);
            this.panel1.Controls.Add(this.LastIdLabel);
            this.panel1.Controls.Add(this.MutasiButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.MutasiIdText);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 117);
            this.panel1.TabIndex = 0;
            // 
            // MutasiDateText
            // 
            this.MutasiDateText.CustomFormat = "ddd dd-MM-yyyy HH:mm";
            this.MutasiDateText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.MutasiDateText.Location = new System.Drawing.Point(9, 83);
            this.MutasiDateText.Name = "MutasiDateText";
            this.MutasiDateText.ShowUpDown = true;
            this.MutasiDateText.Size = new System.Drawing.Size(176, 22);
            this.MutasiDateText.TabIndex = 6;
            // 
            // MutasiDateLabel
            // 
            this.MutasiDateLabel.AutoSize = true;
            this.MutasiDateLabel.Location = new System.Drawing.Point(6, 67);
            this.MutasiDateLabel.Name = "MutasiDateLabel";
            this.MutasiDateLabel.Size = new System.Drawing.Size(72, 13);
            this.MutasiDateLabel.TabIndex = 7;
            this.MutasiDateLabel.Text = "Mutasi  Date";
            // 
            // LastIdLabel
            // 
            this.LastIdLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastIdLabel.Location = new System.Drawing.Point(6, 47);
            this.LastIdLabel.Name = "LastIdLabel";
            this.LastIdLabel.Size = new System.Drawing.Size(153, 20);
            this.LastIdLabel.TabIndex = 5;
            this.LastIdLabel.Text = "[Last ID]";
            this.LastIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MutasiButton
            // 
            this.MutasiButton.BackColor = System.Drawing.Color.CadetBlue;
            this.MutasiButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MutasiButton.Location = new System.Drawing.Point(165, 22);
            this.MutasiButton.Name = "MutasiButton";
            this.MutasiButton.Size = new System.Drawing.Size(26, 22);
            this.MutasiButton.TabIndex = 2;
            this.MutasiButton.Text = "...";
            this.MutasiButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mutasi ID";
            // 
            // MutasiIdText
            // 
            this.MutasiIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MutasiIdText.Location = new System.Drawing.Point(9, 22);
            this.MutasiIdText.Name = "MutasiIdText";
            this.MutasiIdText.ReadOnly = true;
            this.MutasiIdText.Size = new System.Drawing.Size(150, 22);
            this.MutasiIdText.TabIndex = 0;
            // 
            // MutasiItemGrid
            // 
            this.MutasiItemGrid.BackgroundColor = System.Drawing.Color.DarkSlateGray;
            this.MutasiItemGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MutasiItemGrid.Location = new System.Drawing.Point(6, 129);
            this.MutasiItemGrid.Name = "MutasiItemGrid";
            this.MutasiItemGrid.Size = new System.Drawing.Size(788, 246);
            this.MutasiItemGrid.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PowderBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.WarehouseNameText);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.WarehouseButton);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.WarehouseIdText);
            this.panel2.Location = new System.Drawing.Point(212, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 117);
            this.panel2.TabIndex = 2;
            // 
            // WarehouseNameText
            // 
            this.WarehouseNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WarehouseNameText.Location = new System.Drawing.Point(9, 50);
            this.WarehouseNameText.Name = "WarehouseNameText";
            this.WarehouseNameText.ReadOnly = true;
            this.WarehouseNameText.Size = new System.Drawing.Size(182, 22);
            this.WarehouseNameText.TabIndex = 5;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(100, 82);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(59, 17);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.Text = "Masuk";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(9, 82);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(57, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Keluar";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // WarehouseButton
            // 
            this.WarehouseButton.BackColor = System.Drawing.Color.CadetBlue;
            this.WarehouseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WarehouseButton.Location = new System.Drawing.Point(165, 22);
            this.WarehouseButton.Name = "WarehouseButton";
            this.WarehouseButton.Size = new System.Drawing.Size(26, 22);
            this.WarehouseButton.TabIndex = 2;
            this.WarehouseButton.Text = "...";
            this.WarehouseButton.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Warehouse ID";
            // 
            // WarehouseIdText
            // 
            this.WarehouseIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WarehouseIdText.Location = new System.Drawing.Point(9, 22);
            this.WarehouseIdText.Name = "WarehouseIdText";
            this.WarehouseIdText.Size = new System.Drawing.Size(150, 22);
            this.WarehouseIdText.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.PowderBlue;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.KeteranganText);
            this.panel3.Location = new System.Drawing.Point(418, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(376, 117);
            this.panel3.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Keterangan";
            // 
            // KeteranganText
            // 
            this.KeteranganText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KeteranganText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.KeteranganText.Location = new System.Drawing.Point(9, 22);
            this.KeteranganText.Multiline = true;
            this.KeteranganText.Name = "KeteranganText";
            this.KeteranganText.Size = new System.Drawing.Size(350, 77);
            this.KeteranganText.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(636, 383);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Total";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(719, 410);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(6, 410);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 7;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // TotalText
            // 
            this.TotalText.InterceptArrowKeys = false;
            this.TotalText.Location = new System.Drawing.Point(674, 381);
            this.TotalText.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.TotalText.Name = "TotalText";
            this.TotalText.ReadOnly = true;
            this.TotalText.Size = new System.Drawing.Size(120, 22);
            this.TotalText.TabIndex = 8;
            this.TotalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TotalText.ThousandsSeparator = true;
            // 
            // MutasiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(800, 441);
            this.Controls.Add(this.TotalText);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.MutasiItemGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MutasiForm";
            this.Text = "Mutasi Barang";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MutasiItemGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TotalText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MutasiIdText;
        private System.Windows.Forms.DataGridView MutasiItemGrid;
        private System.Windows.Forms.Button MutasiButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button WarehouseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WarehouseIdText;
        private System.Windows.Forms.Label LastIdLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox KeteranganText;
        private System.Windows.Forms.TextBox WarehouseNameText;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewButton;
        private DateTimePicker MutasiDateText;
        private Label MutasiDateLabel;
        private NumericUpDown TotalText;
    }
}