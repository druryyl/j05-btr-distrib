using System.ComponentModel;

namespace btr.distrib.SharedForm
{
    partial class TestPlayground
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.AddStokButton = new System.Windows.Forms.Button();
            this.PrgBar = new System.Windows.Forms.ProgressBar();
            this.RemoveFifoButton = new System.Windows.Forms.Button();
            this.RollBackButton = new System.Windows.Forms.Button();
            this.RemovePriorityButton = new System.Windows.Forms.Button();
            this.ImportOpnameButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AddStokButton
            // 
            this.AddStokButton.Location = new System.Drawing.Point(12, 12);
            this.AddStokButton.Name = "AddStokButton";
            this.AddStokButton.Size = new System.Drawing.Size(75, 23);
            this.AddStokButton.TabIndex = 0;
            this.AddStokButton.Text = "AddStok";
            this.AddStokButton.UseVisualStyleBackColor = true;
            // 
            // PrgBar
            // 
            this.PrgBar.Location = new System.Drawing.Point(12, 124);
            this.PrgBar.Name = "PrgBar";
            this.PrgBar.Size = new System.Drawing.Size(279, 15);
            this.PrgBar.TabIndex = 1;
            // 
            // RemoveFifoButton
            // 
            this.RemoveFifoButton.Location = new System.Drawing.Point(93, 12);
            this.RemoveFifoButton.Name = "RemoveFifoButton";
            this.RemoveFifoButton.Size = new System.Drawing.Size(96, 23);
            this.RemoveFifoButton.TabIndex = 2;
            this.RemoveFifoButton.Text = "Remove FIFO";
            this.RemoveFifoButton.UseVisualStyleBackColor = true;
            // 
            // RollBackButton
            // 
            this.RollBackButton.Location = new System.Drawing.Point(195, 12);
            this.RollBackButton.Name = "RollBackButton";
            this.RollBackButton.Size = new System.Drawing.Size(96, 23);
            this.RollBackButton.TabIndex = 3;
            this.RollBackButton.Text = "RollBack";
            this.RollBackButton.UseVisualStyleBackColor = true;
            // 
            // RemovePriorityButton
            // 
            this.RemovePriorityButton.Location = new System.Drawing.Point(12, 41);
            this.RemovePriorityButton.Name = "RemovePriorityButton";
            this.RemovePriorityButton.Size = new System.Drawing.Size(96, 23);
            this.RemovePriorityButton.TabIndex = 4;
            this.RemovePriorityButton.Text = "Remove Priority";
            this.RemovePriorityButton.UseVisualStyleBackColor = true;
            // 
            // ImportOpnameButton
            // 
            this.ImportOpnameButton.Location = new System.Drawing.Point(191, 95);
            this.ImportOpnameButton.Name = "ImportOpnameButton";
            this.ImportOpnameButton.Size = new System.Drawing.Size(96, 23);
            this.ImportOpnameButton.TabIndex = 5;
            this.ImportOpnameButton.Text = "Import Opname";
            this.ImportOpnameButton.UseVisualStyleBackColor = true;
            this.ImportOpnameButton.Click += new System.EventHandler(this.ImportOpnameButton_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Excel Document";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 98);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(173, 20);
            this.textBox1.TabIndex = 7;
            // 
            // TestPlayground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 151);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ImportOpnameButton);
            this.Controls.Add(this.RemovePriorityButton);
            this.Controls.Add(this.RollBackButton);
            this.Controls.Add(this.RemoveFifoButton);
            this.Controls.Add(this.PrgBar);
            this.Controls.Add(this.AddStokButton);
            this.Name = "TestPlayground";
            this.Text = "TestPlayground";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button RemovePriorityButton;

        #endregion

        private System.Windows.Forms.Button AddStokButton;
        private System.Windows.Forms.ProgressBar PrgBar;
        private System.Windows.Forms.Button RemoveFifoButton;
        private System.Windows.Forms.Button RollBackButton;
        private System.Windows.Forms.Button ImportOpnameButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
    }
}