namespace btr.distrib.ReportingContext
{
    partial class CustomerChartRpt
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MenuCombo = new System.Windows.Forms.ComboBox();
            this.CustomerChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartTypeCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerChart)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.splitContainer1.Panel1.Controls.Add(this.ChartTypeCombo);
            this.splitContainer1.Panel1.Controls.Add(this.MenuCombo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CustomerChart);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // MenuCombo
            // 
            this.MenuCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuCombo.FormattingEnabled = true;
            this.MenuCombo.Items.AddRange(new object[] {
            "1. Customer Per-Wilayah",
            "2. Customer Per-Jenis"});
            this.MenuCombo.Location = new System.Drawing.Point(12, 12);
            this.MenuCombo.Name = "MenuCombo";
            this.MenuCombo.Size = new System.Drawing.Size(247, 21);
            this.MenuCombo.TabIndex = 0;
            // 
            // CustomerChart
            // 
            this.CustomerChart.BackColor = System.Drawing.Color.PowderBlue;
            chartArea5.Name = "ChartArea1";
            this.CustomerChart.ChartAreas.Add(chartArea5);
            this.CustomerChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.CustomerChart.Legends.Add(legend5);
            this.CustomerChart.Location = new System.Drawing.Point(0, 0);
            this.CustomerChart.Name = "CustomerChart";
            this.CustomerChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.CustomerChart.Series.Add(series5);
            this.CustomerChart.Size = new System.Drawing.Size(530, 450);
            this.CustomerChart.TabIndex = 0;
            this.CustomerChart.Text = "chart1";
            // 
            // ChartTypeCombo
            // 
            this.ChartTypeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartTypeCombo.FormattingEnabled = true;
            this.ChartTypeCombo.Items.AddRange(new object[] {
            "Bar",
            "Pie",
            "Area",
            "Line"});
            this.ChartTypeCombo.Location = new System.Drawing.Point(12, 52);
            this.ChartTypeCombo.Name = "ChartTypeCombo";
            this.ChartTypeCombo.Size = new System.Drawing.Size(246, 21);
            this.ChartTypeCombo.TabIndex = 1;
            this.ChartTypeCombo.SelectedIndexChanged += new System.EventHandler(this.ChartTypeCombo_SelectedIndexChanged);
            // 
            // CustomerChartRpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CustomerChartRpt";
            this.Text = "CustomerChartRpt";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart CustomerChart;
        private System.Windows.Forms.ComboBox MenuCombo;
        private System.Windows.Forms.ComboBox ChartTypeCombo;
    }
}