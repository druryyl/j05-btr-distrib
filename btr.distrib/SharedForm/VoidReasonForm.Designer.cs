using System.ComponentModel;

namespace btr.distrib.SharedForm
{
    partial class VoidReasonForm
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
            this.AlasanText = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.BatalButton = new System.Windows.Forms.Button();
            this.AlasanLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AlasanText
            // 
            this.AlasanText.Location = new System.Drawing.Point(6, 22);
            this.AlasanText.Multiline = true;
            this.AlasanText.Name = "AlasanText";
            this.AlasanText.Size = new System.Drawing.Size(254, 58);
            this.AlasanText.TabIndex = 0;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkButton.Location = new System.Drawing.Point(57, 85);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.BatalButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BatalButton.Location = new System.Drawing.Point(138, 85);
            this.BatalButton.Name = "CancelButton";
            this.BatalButton.Size = new System.Drawing.Size(75, 23);
            this.BatalButton.TabIndex = 2;
            this.BatalButton.Text = "Cancel";
            this.BatalButton.UseVisualStyleBackColor = true;
            // 
            // AlasanLabel
            // 
            this.AlasanLabel.AutoSize = true;
            this.AlasanLabel.Location = new System.Drawing.Point(6, 6);
            this.AlasanLabel.Name = "AlasanLabel";
            this.AlasanLabel.Size = new System.Drawing.Size(67, 13);
            this.AlasanLabel.TabIndex = 3;
            this.AlasanLabel.Text = "Alasan Void";
            // 
            // VoidReasonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.ClientSize = new System.Drawing.Size(266, 113);
            this.ControlBox = false;
            this.Controls.Add(this.AlasanLabel);
            this.Controls.Add(this.BatalButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.AlasanText);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VoidReasonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Konfirmasi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AlasanText;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button BatalButton;
        private System.Windows.Forms.Label AlasanLabel;
    }
}