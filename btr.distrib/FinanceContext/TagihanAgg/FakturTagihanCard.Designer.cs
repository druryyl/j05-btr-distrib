namespace btr.distrib.FinanceContext.TagihanAgg
{
    partial class FakturTagihanCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FakturCodeLabel = new System.Windows.Forms.Label();
            this.FakturDate = new System.Windows.Forms.Label();
            this.CustomerLabel = new System.Windows.Forms.Label();
            this.AlamatLabel = new System.Windows.Forms.Label();
            this.NilaiFakturLabel = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FakturCodeLabel
            // 
            this.FakturCodeLabel.AutoSize = true;
            this.FakturCodeLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FakturCodeLabel.Location = new System.Drawing.Point(3, 4);
            this.FakturCodeLabel.Name = "FakturCodeLabel";
            this.FakturCodeLabel.Size = new System.Drawing.Size(61, 11);
            this.FakturCodeLabel.TabIndex = 0;
            this.FakturCodeLabel.Text = "G0009589";
            // 
            // FakturDate
            // 
            this.FakturDate.AutoSize = true;
            this.FakturDate.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FakturDate.Location = new System.Drawing.Point(141, 4);
            this.FakturDate.Name = "FakturDate";
            this.FakturDate.Size = new System.Drawing.Size(82, 11);
            this.FakturDate.TabIndex = 1;
            this.FakturDate.Text = "28 Aug 2025";
            // 
            // CustomerLabel
            // 
            this.CustomerLabel.AutoSize = true;
            this.CustomerLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomerLabel.Location = new System.Drawing.Point(3, 21);
            this.CustomerLabel.Name = "CustomerLabel";
            this.CustomerLabel.Size = new System.Drawing.Size(149, 17);
            this.CustomerLabel.TabIndex = 2;
            this.CustomerLabel.Text = "Ramai Mall Yogyakarta";
            // 
            // AlamatLabel
            // 
            this.AlamatLabel.AutoSize = true;
            this.AlamatLabel.Location = new System.Drawing.Point(3, 38);
            this.AlamatLabel.Name = "AlamatLabel";
            this.AlamatLabel.Size = new System.Drawing.Size(167, 13);
            this.AlamatLabel.TabIndex = 3;
            this.AlamatLabel.Text = "Jln. Jend. A.Yani 98 - Yogyakarta";
            // 
            // NilaiFakturLabel
            // 
            this.NilaiFakturLabel.AutoSize = true;
            this.NilaiFakturLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NilaiFakturLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.NilaiFakturLabel.Location = new System.Drawing.Point(3, 60);
            this.NilaiFakturLabel.Name = "NilaiFakturLabel";
            this.NilaiFakturLabel.Size = new System.Drawing.Size(75, 15);
            this.NilaiFakturLabel.TabIndex = 4;
            this.NilaiFakturLabel.Text = "Rp295.000,-";
            // 
            // AddButton
            // 
            this.AddButton.BackColor = System.Drawing.Color.DarkRed;
            this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddButton.ForeColor = System.Drawing.Color.White;
            this.AddButton.Location = new System.Drawing.Point(180, 54);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(38, 23);
            this.AddButton.TabIndex = 5;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = false;
            // 
            // FakturTagihanCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.NilaiFakturLabel);
            this.Controls.Add(this.AlamatLabel);
            this.Controls.Add(this.CustomerLabel);
            this.Controls.Add(this.FakturDate);
            this.Controls.Add(this.FakturCodeLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FakturTagihanCard";
            this.Size = new System.Drawing.Size(221, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FakturCodeLabel;
        private System.Windows.Forms.Label FakturDate;
        private System.Windows.Forms.Label CustomerLabel;
        private System.Windows.Forms.Label AlamatLabel;
        private System.Windows.Forms.Label NilaiFakturLabel;
        private System.Windows.Forms.Button AddButton;
    }
}
