namespace btr.distrib.InventoryContext.DriverAgg
{
    partial class DriverForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.DriverIdText = new System.Windows.Forms.TextBox();
            this.DriverNameText = new System.Windows.Forms.TextBox();
            this.DriverButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListGrid = new System.Windows.Forms.DataGridView();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(318, 311);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(70, 23);
            this.NewButton.TabIndex = 65;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(471, 311);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(70, 23);
            this.SaveButton.TabIndex = 64;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(237, 310);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 62;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(13, 311);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(219, 22);
            this.SearchText.TabIndex = 61;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Driver Name";
            // 
            // DriverIdText
            // 
            this.DriverIdText.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriverIdText.Location = new System.Drawing.Point(12, 25);
            this.DriverIdText.Name = "DriverIdText";
            this.DriverIdText.Size = new System.Drawing.Size(177, 22);
            this.DriverIdText.TabIndex = 6;
            // 
            // DriverNameText
            // 
            this.DriverNameText.Location = new System.Drawing.Point(12, 66);
            this.DriverNameText.Name = "DriverNameText";
            this.DriverNameText.Size = new System.Drawing.Size(205, 22);
            this.DriverNameText.TabIndex = 8;
            // 
            // DriverButton
            // 
            this.DriverButton.Location = new System.Drawing.Point(195, 25);
            this.DriverButton.Name = "DriverButton";
            this.DriverButton.Size = new System.Drawing.Size(28, 23);
            this.DriverButton.TabIndex = 7;
            this.DriverButton.Text = "...";
            this.DriverButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Driver ID";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.DriverIdText);
            this.panel2.Controls.Add(this.DriverNameText);
            this.panel2.Controls.Add(this.DriverButton);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(318, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 292);
            this.panel2.TabIndex = 66;
            // 
            // ListGrid
            // 
            this.ListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListGrid.Location = new System.Drawing.Point(12, 12);
            this.ListGrid.Name = "ListGrid";
            this.ListGrid.Size = new System.Drawing.Size(300, 292);
            this.ListGrid.TabIndex = 63;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(394, 311);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(70, 23);
            this.DeleteButton.TabIndex = 67;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // DriverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(555, 343);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ListGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DriverForm";
            this.Text = "DriverForm";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DriverIdText;
        private System.Windows.Forms.TextBox DriverNameText;
        private System.Windows.Forms.Button DriverButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView ListGrid;
        private System.Windows.Forms.Button DeleteButton;
    }
}