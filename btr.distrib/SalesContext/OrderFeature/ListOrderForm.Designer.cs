namespace btr.distrib.SalesContext.OrderFeature
{
    partial class ListOrderForm
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
            this.OrderGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PeriodeEndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.PeriodeStartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ShowAllCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.OrderGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OrderGrid
            // 
            this.OrderGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OrderGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.OrderGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrderGrid.Location = new System.Drawing.Point(6, 62);
            this.OrderGrid.Name = "OrderGrid";
            this.OrderGrid.Size = new System.Drawing.Size(972, 376);
            this.OrderGrid.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ShowAllCheckBox);
            this.panel1.Controls.Add(this.PeriodeEndDatePicker);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.SearchTextBox);
            this.panel1.Controls.Add(this.PeriodeStartDatePicker);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 50);
            this.panel1.TabIndex = 1;
            // 
            // PeriodeEndDatePicker
            // 
            this.PeriodeEndDatePicker.CustomFormat = "ddd, dd-MM-yyyy";
            this.PeriodeEndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PeriodeEndDatePicker.Location = new System.Drawing.Point(151, 14);
            this.PeriodeEndDatePicker.Name = "PeriodeEndDatePicker";
            this.PeriodeEndDatePicker.Size = new System.Drawing.Size(129, 22);
            this.PeriodeEndDatePicker.TabIndex = 3;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Location = new System.Drawing.Point(598, 13);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchTextBox.Location = new System.Drawing.Point(287, 14);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(305, 22);
            this.SearchTextBox.TabIndex = 1;
            // 
            // PeriodeStartDatePicker
            // 
            this.PeriodeStartDatePicker.CustomFormat = "ddd, dd-MM-yyyy";
            this.PeriodeStartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PeriodeStartDatePicker.Location = new System.Drawing.Point(17, 14);
            this.PeriodeStartDatePicker.Name = "PeriodeStartDatePicker";
            this.PeriodeStartDatePicker.Size = new System.Drawing.Size(129, 22);
            this.PeriodeStartDatePicker.TabIndex = 0;
            // 
            // ShowAllCheckBox
            // 
            this.ShowAllCheckBox.AutoSize = true;
            this.ShowAllCheckBox.Location = new System.Drawing.Point(679, 17);
            this.ShowAllCheckBox.Name = "ShowAllCheckBox";
            this.ShowAllCheckBox.Size = new System.Drawing.Size(71, 17);
            this.ShowAllCheckBox.TabIndex = 4;
            this.ShowAllCheckBox.Text = "Show All";
            this.ShowAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // ListOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(133)))), ((int)(((byte)(162)))));
            this.ClientSize = new System.Drawing.Size(984, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OrderGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ListOrderForm";
            this.Text = "List Order";
            ((System.ComponentModel.ISupportInitialize)(this.OrderGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView OrderGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.DateTimePicker PeriodeStartDatePicker;
        private System.Windows.Forms.DateTimePicker PeriodeEndDatePicker;
        private System.Windows.Forms.CheckBox ShowAllCheckBox;
    }
}