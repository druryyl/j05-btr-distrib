using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace btr.distrib.SharedForm
{
    public enum ChartTheme
    {
        Default,
        Dark,
        Pastel,
        Monochrome,
        Vibrant
    }

    /// <summary>
    /// A generic Chart Form that displays data from an IEnumerable[T] collection.
    /// </summary>
    /// <typeparam name="T">The type of data elements in the collection.</typeparam>
    public partial class ChartForm<T> : Form
    {
        private readonly IEnumerable<T> _data;
        private readonly Func<T, string> _xSelector;
        private readonly Func<T, double> _ySelector;
        private readonly string _chartTitle;
        private SeriesChartType _chartType;
        private readonly ChartTheme _theme;

        // Chart control field
        private Chart chart1;

        /// <summary>
        /// Initializes a new instance of the ChartForm[T] class.
        /// </summary>
        /// <param name="data">The data collection to display.</param>
        /// <param name="xSelector">Function to extract X-axis value from each data element.</param>
        /// <param name="ySelector">Function to extract Y-axis value from each data element.</param>
        /// <param name="chartTitle">Optional title for the chart. Default is "Data Chart".</param>
        /// <param name="chartType">Optional chart type. Default is Column.</param>
        /// <param name="theme">Optional theme enum (Default, Dark, Pastel, Monochrome, Vibrant).</param>
        public ChartForm(
            IEnumerable<T> data,
            Func<T, string> xSelector,
            Func<T, double> ySelector,
            string chartTitle,
            SeriesChartType chartType,
            ChartTheme theme)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (xSelector == null)
                throw new ArgumentNullException(nameof(xSelector));
            if (ySelector == null)
                throw new ArgumentNullException(nameof(ySelector));

            _data = data;
            _xSelector = xSelector;
            _ySelector = ySelector;
            _chartTitle = chartTitle;
            _chartType = chartType;
            _theme = theme;

            InitializeComponent();
            ConfigureChart();
            LoadData();

            // apply theme provided by constructor
            ApplyTheme(_theme);
        }

        private void InitializeComponent()
        {
            chart1 = new Chart();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();

            //
            // chart1
            //
            chart1.Dock = DockStyle.Fill;
            chart1.Name = "chart1";
            chart1.TabIndex = 0;
            chart1.MouseClick += new MouseEventHandler(Chart1_MouseClick);

            //
            // ChartForm
            //
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);

            // Add chart to fill the form
            Controls.Add(chart1);

            Name = "ChartForm";
            Text = _chartTitle;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(600, 400);

            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
        }

        private void ConfigureChart()
        {
            // Enable anti-aliasing for better visual quality
            chart1.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart1.AntiAliasing = AntiAliasingStyles.All;

            // Set the chart title
            chart1.Titles.Add(_chartTitle);
            chart1.Titles[0].Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
            chart1.Titles[0].Docking = Docking.Top;

            // Configure the chart area
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.BackColor = Color.White;
            chartArea.BorderColor = Color.FromArgb(200, 200, 200);
            chartArea.BorderWidth = 1;

            // Configure axis
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;

            // Configure grid lines
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart1.ChartAreas.Add(chartArea);

            // Configure legend
            chart1.Legends.Add(new Legend("Legend"));
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;
            chart1.Legends[0].Font = new Font("Segoe UI", 9F);
        }

        private void LoadData()
        {
            // Check for null or empty data
            if (_data == null || !_data.Any())
            {
                ShowNoDataMessage();
                return;
            }

            // Suspend updates for better performance with large datasets
            chart1.SuspendLayout();

            try
            {
                // Create a new series
                Series series = new Series("DataSeries")
                {
                    ChartType = _chartType,
                    BorderWidth = 2,
                    ShadowOffset = 1
                };

                // Set a professional color
                series.Color = Color.FromArgb(70, 130, 180); // Steel Blue
                series.BackSecondaryColor = Color.FromArgb(100, 150, 200);
                series.BorderColor = Color.FromArgb(50, 100, 150);

                // Add data points
                foreach (var item in _data)
                {
                    try
                    {
                        string xValue = _xSelector(item);
                        double yValue = _ySelector(item);

                        if (!string.IsNullOrEmpty(xValue))
                        {
                            int pointIndex = series.Points.AddXY(xValue, yValue);

                            // Set tooltip
                            series.Points[pointIndex].ToolTip = $"{xValue}: {yValue:N2}";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Skip problematic data points
                        System.Diagnostics.Debug.WriteLine($"Error processing data point: {ex.Message}");
                    }
                }

                // Configure series based on chart type
                ApplySeriesStyle(series, _chartType);

                chart1.Series.Add(series);

                // Configure tooltips
                chart1.Series["DataSeries"].ToolTip = "#AXISLABEL: #VALY{N2}";
            }
            finally
            {
                chart1.ResumeLayout();
            }
        }

        private void ApplySeriesStyle(Series series, SeriesChartType chartType)
        {
            if (series == null) return;

            if (chartType == SeriesChartType.Line || chartType == SeriesChartType.Spline)
            {
                series.BorderWidth = 3;
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.MarkerColor = series.Color;
            }
            else
            {
                series.MarkerStyle = MarkerStyle.None;
                series.MarkerSize = 0;
                series.BorderWidth = 2;
            }

            if (chartType == SeriesChartType.Pie || chartType == SeriesChartType.Doughnut)
            {
                series.Palette = ChartColorPalette.BrightPastel;
            }
        }

        private void ShowNoDataMessage()
        {
            // Add a text annotation to show when there's no data
            TextAnnotation annotation = new TextAnnotation
            {
                Text = "No Data Available",
                Font = new Font("Segoe UI", 16F, FontStyle.Regular),
                ForeColor = Color.Gray,
                Alignment = ContentAlignment.MiddleCenter
            };

            // Set the annotation to anchor to the chart area by specifying its coordinates
            annotation.AnchorX = 50;
            annotation.AnchorY = 50;
            annotation.AnchorAlignment = ContentAlignment.MiddleCenter;
            annotation.AxisX = chart1.ChartAreas[0].AxisX;
            annotation.AxisY = chart1.ChartAreas[0].AxisY;

            chart1.Annotations.Add(annotation);
        }

        private void Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            // Handle mouse click events on the chart
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint point = result.Series.Points[result.PointIndex];

                // Display detailed information
                string message = $"X: {point.AxisLabel}\nY: {point.YValues[0]:N2}";
                MessageBox.Show(message, "Data Point Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ApplyChartType(SeriesChartType selectedType)
        {
            _chartType = selectedType;

            // Update existing series
            foreach (var s in chart1.Series)
            {
                s.ChartType = selectedType;
                ApplySeriesStyle(s, selectedType);
            }

            // Special handling for pie/doughnut: ensure palette
            if (selectedType == SeriesChartType.Pie || selectedType == SeriesChartType.Doughnut)
            {
                foreach (var s in chart1.Series)
                    s.Palette = ChartColorPalette.BrightPastel;
            }

            chart1.Invalidate();
        }

        private void ApplyTheme(ChartTheme theme)
        {
            // default values
            Color areaBg = Color.White;
            Color titleColor = Color.Black;
            Color labelColor = Color.Black;
            Color gridColor = Color.FromArgb(230, 230, 230);
            Color[] palette = new[] { Color.FromArgb(70, 130, 180) };

            switch (theme)
            {
                case ChartTheme.Dark:
                    areaBg = Color.FromArgb(34, 34, 34);
                    titleColor = Color.White;
                    labelColor = Color.WhiteSmoke;
                    gridColor = Color.FromArgb(70, 70, 70);
                    palette = new[] { Color.FromArgb(0, 122, 204), Color.FromArgb(255, 99, 71), Color.FromArgb(46, 204, 113), Color.FromArgb(255, 206, 84), Color.FromArgb(155, 89, 182) };
                    break;
                case ChartTheme.Pastel:
                    areaBg = Color.WhiteSmoke;
                    titleColor = Color.FromArgb(60, 60, 60);
                    labelColor = Color.FromArgb(60, 60, 60);
                    gridColor = Color.FromArgb(240, 240, 240);
                    palette = new[] { Color.FromArgb(179, 205, 227), Color.FromArgb(251, 180, 174), Color.FromArgb(204, 235, 197), Color.FromArgb(222, 203, 228), Color.FromArgb(255, 237, 160) };
                    break;
                case ChartTheme.Monochrome:
                    areaBg = Color.White;
                    titleColor = Color.Black;
                    labelColor = Color.Black;
                    gridColor = Color.FromArgb(240, 240, 240);
                    palette = new[] { Color.FromArgb(50, 50, 50), Color.FromArgb(90, 90, 90), Color.FromArgb(130, 130, 130), Color.FromArgb(170, 170, 170), Color.FromArgb(210, 210, 210) };
                    break;
                case ChartTheme.Vibrant:
                    areaBg = Color.White;
                    titleColor = Color.FromArgb(30, 30, 30);
                    labelColor = Color.FromArgb(30, 30, 30);
                    gridColor = Color.FromArgb(230, 230, 230);
                    palette = new[] { Color.FromArgb(231, 76, 60), Color.FromArgb(241, 196, 15), Color.FromArgb(46, 204, 113), Color.FromArgb(52, 152, 219), Color.FromArgb(155, 89, 182) };
                    break;
                default: // Default
                    areaBg = Color.White;
                    titleColor = Color.FromArgb(30, 30, 30);
                    labelColor = Color.FromArgb(30, 30, 30);
                    gridColor = Color.FromArgb(230, 230, 230);
                    palette = new[] { Color.FromArgb(70, 130, 180), Color.FromArgb(100, 150, 200), Color.FromArgb(200, 120, 120), Color.FromArgb(120, 200, 160), Color.FromArgb(200, 160, 220) };
                    break;
            }

            // apply palette
            chart1.Palette = ChartColorPalette.None;
            chart1.PaletteCustomColors = palette;

            // apply chart area styles
            foreach (ChartArea ca in chart1.ChartAreas)
            {
                ca.BackColor = areaBg;
                ca.AxisX.LabelStyle.ForeColor = labelColor;
                ca.AxisY.LabelStyle.ForeColor = labelColor;
                ca.AxisX.MajorGrid.LineColor = gridColor;
                ca.AxisY.MajorGrid.LineColor = gridColor;
            }

            // title/legend coloring
            foreach (var t in chart1.Titles)
                t.ForeColor = titleColor;

            foreach (var lg in chart1.Legends)
                lg.ForeColor = labelColor;

            // apply to existing series points (for pie charts ensure all points get colors)
            foreach (var s in chart1.Series)
            {
                s.Palette = ChartColorPalette.None;
                // if the series has points, assign colors from palette in round-robin
                if (s.Points != null && s.Points.Count > 0)
                {
                    for (int i = 0; i < s.Points.Count; i++)
                    {
                        s.Points[i].Color = palette[i % palette.Length];
                    }
                }
                else
                {
                    // set default series color
                    s.Color = palette.First();
                }
            }

            chart1.Invalidate();
        }

        /// <summary>
        /// Gets or sets the chart type for the form.
        /// </summary>
        public SeriesChartType ChartType
        {
            get => _chartType;
        }

        /// <summary>
        /// Gets the chart control for additional customization.
        /// </summary>
        public Chart Chart => chart1;
    }
}
