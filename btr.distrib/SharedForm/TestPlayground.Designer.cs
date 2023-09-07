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
            this.PrgBar.Location = new System.Drawing.Point(12, 94);
            this.PrgBar.Name = "PrgBar";
            this.PrgBar.Size = new System.Drawing.Size(267, 23);
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
            // TestPlayground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 134);
            this.Controls.Add(this.RollBackButton);
            this.Controls.Add(this.RemoveFifoButton);
            this.Controls.Add(this.PrgBar);
            this.Controls.Add(this.AddStokButton);
            this.Name = "TestPlayground";
            this.Text = "TestPlayground";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddStokButton;
        private System.Windows.Forms.ProgressBar PrgBar;
        private System.Windows.Forms.Button RemoveFifoButton;
        private System.Windows.Forms.Button RollBackButton;
    }
}