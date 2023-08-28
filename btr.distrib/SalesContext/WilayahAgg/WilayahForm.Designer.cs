namespace btr.distrib.SalesContext.WilayahAgg
{
    partial class WilayahForm
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
            this.NewButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BrgCodeText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.WilayahIdText = new System.Windows.Forms.TextBox();
            this.WilayahNameText = new System.Windows.Forms.TextBox();
            this.WilayahButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListGrid = new System.Windows.Forms.DataGridView();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(318, 311);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 53;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(471, 311);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 52;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(237, 310);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 50;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(13, 311);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(219, 20);
            this.SearchText.TabIndex = 49;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Code";
            // 
            // BrgCodeText
            // 
            this.BrgCodeText.Location = new System.Drawing.Point(12, 107);
            this.BrgCodeText.Name = "BrgCodeText";
            this.BrgCodeText.Size = new System.Drawing.Size(205, 20);
            this.BrgCodeText.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Wilayah Name";
            // 
            // WilayahIdText
            // 
            this.WilayahIdText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WilayahIdText.Location = new System.Drawing.Point(12, 25);
            this.WilayahIdText.Name = "WilayahIdText";
            this.WilayahIdText.Size = new System.Drawing.Size(177, 22);
            this.WilayahIdText.TabIndex = 6;
            // 
            // WilayahNameText
            // 
            this.WilayahNameText.Location = new System.Drawing.Point(12, 66);
            this.WilayahNameText.Name = "WilayahNameText";
            this.WilayahNameText.Size = new System.Drawing.Size(205, 20);
            this.WilayahNameText.TabIndex = 8;
            // 
            // WilayahButton
            // 
            this.WilayahButton.Location = new System.Drawing.Point(195, 25);
            this.WilayahButton.Name = "WilayahButton";
            this.WilayahButton.Size = new System.Drawing.Size(28, 23);
            this.WilayahButton.TabIndex = 7;
            this.WilayahButton.Text = "...";
            this.WilayahButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Wilayah ID";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Honeydew;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.BrgCodeText);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.WilayahIdText);
            this.panel2.Controls.Add(this.WilayahNameText);
            this.panel2.Controls.Add(this.WilayahButton);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(318, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 292);
            this.panel2.TabIndex = 54;
            // 
            // ListGrid
            // 
            this.ListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListGrid.Location = new System.Drawing.Point(12, 12);
            this.ListGrid.Name = "ListGrid";
            this.ListGrid.Size = new System.Drawing.Size(300, 292);
            this.ListGrid.TabIndex = 51;
            // 
            // WilayahForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(557, 341);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ListGrid);
            this.Name = "WilayahForm";
            this.Text = "Wilayah";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BrgCodeText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WilayahIdText;
        private System.Windows.Forms.TextBox WilayahNameText;
        private System.Windows.Forms.Button WilayahButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView ListGrid;
    }
}