namespace btr.distrib.SalesContext.RuteAgg
{
    partial class RuteForm
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
            this.RuteItemGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AddNewRuteButton = new System.Windows.Forms.Button();
            this.RuteIdButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.RuteNameText = new System.Windows.Forms.TextBox();
            this.RuteCodeText = new System.Windows.Forms.TextBox();
            this.RuteIdText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.CustomerGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.RuteItemGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // RuteItemGrid
            // 
            this.RuteItemGrid.AllowUserToDeleteRows = false;
            this.RuteItemGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RuteItemGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RuteItemGrid.Location = new System.Drawing.Point(6, 100);
            this.RuteItemGrid.Name = "RuteItemGrid";
            this.RuteItemGrid.Size = new System.Drawing.Size(421, 357);
            this.RuteItemGrid.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Controls.Add(this.AddNewRuteButton);
            this.panel1.Controls.Add(this.RuteIdButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.RuteNameText);
            this.panel1.Controls.Add(this.RuteCodeText);
            this.panel1.Controls.Add(this.RuteIdText);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 88);
            this.panel1.TabIndex = 2;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(321, 52);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(95, 23);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            // 
            // AddNewRuteButton
            // 
            this.AddNewRuteButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.AddNewRuteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddNewRuteButton.Location = new System.Drawing.Point(6, 52);
            this.AddNewRuteButton.Name = "AddNewRuteButton";
            this.AddNewRuteButton.Size = new System.Drawing.Size(104, 23);
            this.AddNewRuteButton.TabIndex = 7;
            this.AddNewRuteButton.Text = "New";
            this.AddNewRuteButton.UseVisualStyleBackColor = false;
            // 
            // RuteIdButton
            // 
            this.RuteIdButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.RuteIdButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RuteIdButton.Location = new System.Drawing.Point(81, 24);
            this.RuteIdButton.Name = "RuteIdButton";
            this.RuteIdButton.Size = new System.Drawing.Size(26, 22);
            this.RuteIdButton.TabIndex = 6;
            this.RuteIdButton.Text = "...";
            this.RuteIdButton.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(217, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Rute Name";
            // 
            // RuteNameText
            // 
            this.RuteNameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RuteNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RuteNameText.Location = new System.Drawing.Point(220, 24);
            this.RuteNameText.Name = "RuteNameText";
            this.RuteNameText.Size = new System.Drawing.Size(197, 22);
            this.RuteNameText.TabIndex = 4;
            // 
            // RuteCodeText
            // 
            this.RuteCodeText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RuteCodeText.Location = new System.Drawing.Point(111, 24);
            this.RuteCodeText.Name = "RuteCodeText";
            this.RuteCodeText.Size = new System.Drawing.Size(100, 22);
            this.RuteCodeText.TabIndex = 3;
            // 
            // RuteIdText
            // 
            this.RuteIdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RuteIdText.Location = new System.Drawing.Point(6, 24);
            this.RuteIdText.Name = "RuteIdText";
            this.RuteIdText.ReadOnly = true;
            this.RuteIdText.Size = new System.Drawing.Size(72, 22);
            this.RuteIdText.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rute ID";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.DarkKhaki;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Khaki;
            this.splitContainer1.Panel1.Controls.Add(this.SearchText);
            this.splitContainer1.Panel1.Controls.Add(this.CustomerGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Khaki;
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.RuteItemGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1053, 466);
            this.splitContainer1.SplitterDistance = 613;
            this.splitContainer1.TabIndex = 3;
            // 
            // SearchText
            // 
            this.SearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchText.Location = new System.Drawing.Point(6, 6);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(600, 22);
            this.SearchText.TabIndex = 2;
            // 
            // CustomerGrid
            // 
            this.CustomerGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomerGrid.Location = new System.Drawing.Point(6, 34);
            this.CustomerGrid.Name = "CustomerGrid";
            this.CustomerGrid.Size = new System.Drawing.Size(599, 423);
            this.CustomerGrid.TabIndex = 1;
            // 
            // RuteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.ClientSize = new System.Drawing.Size(1053, 466);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(6, 6);
            this.Name = "RuteForm";
            this.Text = "RuteForm";
            ((System.ComponentModel.ISupportInitialize)(this.RuteItemGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView RuteItemGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView CustomerGrid;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.Button RuteIdButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RuteNameText;
        private System.Windows.Forms.TextBox RuteCodeText;
        private System.Windows.Forms.TextBox RuteIdText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button AddNewRuteButton;
    }
}