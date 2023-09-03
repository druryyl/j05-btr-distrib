namespace btr.distrib.SharedForm
{
    partial class Browser2Form<T>
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
            this.SearchButton = new System.Windows.Forms.Button();
            this.FilterDate2TextBox = new System.Windows.Forms.DateTimePicker();
            this.FilterDate1TextBox = new System.Windows.Forms.DateTimePicker();
            this.FilterTextBox = new System.Windows.Forms.TextBox();
            this.BrowserGrid = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrowserGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.YellowGreen;
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.FilterDate2TextBox);
            this.panel1.Controls.Add(this.FilterDate1TextBox);
            this.panel1.Controls.Add(this.FilterTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 64);
            this.panel1.TabIndex = 9;
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(257, 5);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(70, 23);
            this.SearchButton.TabIndex = 6;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // FilterDate2TextBox
            // 
            this.FilterDate2TextBox.CustomFormat = "ddd, dd-MM-yyyy";
            this.FilterDate2TextBox.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FilterDate2TextBox.Location = new System.Drawing.Point(132, 34);
            this.FilterDate2TextBox.Name = "FilterDate2TextBox";
            this.FilterDate2TextBox.Size = new System.Drawing.Size(123, 22);
            this.FilterDate2TextBox.TabIndex = 5;
            // 
            // FilterDate1TextBox
            // 
            this.FilterDate1TextBox.CustomFormat = "ddd, dd-MM-yyyy";
            this.FilterDate1TextBox.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FilterDate1TextBox.Location = new System.Drawing.Point(3, 34);
            this.FilterDate1TextBox.Name = "FilterDate1TextBox";
            this.FilterDate1TextBox.Size = new System.Drawing.Size(123, 22);
            this.FilterDate1TextBox.TabIndex = 4;
            // 
            // FilterTextBox
            // 
            this.FilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterTextBox.Location = new System.Drawing.Point(3, 6);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.Size = new System.Drawing.Size(252, 22);
            this.FilterTextBox.TabIndex = 1;
            // 
            // BrowserGrid
            // 
            this.BrowserGrid.AllowUserToAddRows = false;
            this.BrowserGrid.AllowUserToDeleteRows = false;
            this.BrowserGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowserGrid.BackgroundColor = System.Drawing.Color.DarkOliveGreen;
            this.BrowserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrowserGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.BrowserGrid.Location = new System.Drawing.Point(0, 63);
            this.BrowserGrid.Name = "BrowserGrid";
            this.BrowserGrid.Size = new System.Drawing.Size(331, 388);
            this.BrowserGrid.TabIndex = 8;
            // 
            // Browser2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BrowserGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "Browser2Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Browser";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrowserGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.DateTimePicker FilterDate2TextBox;
        private System.Windows.Forms.DateTimePicker FilterDate1TextBox;
        private System.Windows.Forms.TextBox FilterTextBox;
        private System.Windows.Forms.DataGridView BrowserGrid;
    }
}