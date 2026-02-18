namespace btr.distrib.SharedForm
{
    partial class RoleForm
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
            this.RoleNameTextBox = new System.Windows.Forms.TextBox();
            this.RoleListButton = new System.Windows.Forms.Button();
            this.RoleIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MenuGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MenuGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RoleNameTextBox);
            this.panel1.Controls.Add(this.RoleListButton);
            this.panel1.Controls.Add(this.RoleIdTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 48);
            this.panel1.TabIndex = 0;
            // 
            // RoleNameTextBox
            // 
            this.RoleNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RoleNameTextBox.Location = new System.Drawing.Point(169, 10);
            this.RoleNameTextBox.Name = "RoleNameTextBox";
            this.RoleNameTextBox.Size = new System.Drawing.Size(264, 22);
            this.RoleNameTextBox.TabIndex = 3;
            // 
            // RoleListButton
            // 
            this.RoleListButton.BackColor = System.Drawing.Color.RosyBrown;
            this.RoleListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RoleListButton.Location = new System.Drawing.Point(137, 9);
            this.RoleListButton.Name = "RoleListButton";
            this.RoleListButton.Size = new System.Drawing.Size(26, 23);
            this.RoleListButton.TabIndex = 2;
            this.RoleListButton.Text = "...";
            this.RoleListButton.UseVisualStyleBackColor = false;
            // 
            // RoleIdTextBox
            // 
            this.RoleIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RoleIdTextBox.Location = new System.Drawing.Point(57, 11);
            this.RoleIdTextBox.Name = "RoleIdTextBox";
            this.RoleIdTextBox.Size = new System.Drawing.Size(74, 22);
            this.RoleIdTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Role";
            // 
            // MenuGrid
            // 
            this.MenuGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MenuGrid.Location = new System.Drawing.Point(12, 66);
            this.MenuGrid.Name = "MenuGrid";
            this.MenuGrid.Size = new System.Drawing.Size(611, 432);
            this.MenuGrid.TabIndex = 1;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.Gainsboro;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(415, 504);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(101, 23);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Gainsboro;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(522, 504);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // RoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.ClientSize = new System.Drawing.Size(635, 539);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.MenuGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RoleForm";
            this.Text = "RoleForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MenuGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox RoleNameTextBox;
        private System.Windows.Forms.Button RoleListButton;
        private System.Windows.Forms.TextBox RoleIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView MenuGrid;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button3;
    }
}