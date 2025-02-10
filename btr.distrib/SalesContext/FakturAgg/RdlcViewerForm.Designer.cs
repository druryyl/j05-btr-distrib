namespace btr.distrib.SalesContext.FakturAgg
{
    partial class RdlcViewerForm
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
            this.TheViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // TheViewer
            // 
            this.TheViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TheViewer.Location = new System.Drawing.Point(0, 0);
            this.TheViewer.Name = "TheViewer";
            this.TheViewer.ServerReport.BearerToken = null;
            this.TheViewer.Size = new System.Drawing.Size(800, 450);
            this.TheViewer.TabIndex = 0;
            // 
            // RdlcViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TheViewer);
            this.Name = "RdlcViewerForm";
            this.Text = "RdlcViewerForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer TheViewer;
    }
}