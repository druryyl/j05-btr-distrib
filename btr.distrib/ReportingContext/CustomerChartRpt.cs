using btr.application.SalesContext.CustomerAgg.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace btr.distrib.ReportingContext
{
    public partial class CustomerChartRpt : Form
    {
        private readonly ICustomerDal _customerDal;
        public CustomerChartRpt(ICustomerDal customerDal)
        {
            InitializeComponent();

            _customerDal = customerDal;

            MenuCombo.SelectedIndexChanged += MenuCombo_SelectedIndexChanged;
        }

        private void MenuCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Proses1CustomerPerWilayah();
        }

        private void Proses1CustomerPerWilayah()
        {
            var listData = _customerDal.ListData();
            var chartData = (
                from c in listData
                group c by c.Kota into g
                orderby g.Count() descending
                select new
                {
                    Kota = g.Key,
                    JumCustomer = g.Count()
                }).Take(10).ToList();
            CustomerChart.DataSource = chartData;
            CustomerChart.DataBind();
            CustomerChart.Invalidate();

            CustomerChart.Series.Clear(); // Clear any existing series
            CustomerChart.Series.Add("Customer1"); // Add a new series
            CustomerChart.Series[0].XValueMember = "Kota"; // Set X-axis data column
            CustomerChart.Series[0].YValueMembers = "JumCustomer"; // Set Y-axis data column
            CustomerChart.Series[0].ChartType = SeriesChartType.Bar;

            CustomerChart.ChartAreas[0].AxisX.Title = "Kota";
            CustomerChart.ChartAreas[0].AxisY.Title = "Jumlah Customer";

            CustomerChart.Titles.Add("Jumlah Customer");
            CustomerChart.Titles.Add("Per-Kota");

        }

        private void ChartTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ChartTypeCombo.SelectedIndex)
            {
                case 0:
                    CustomerChart.Series["Customer1"].ChartType = SeriesChartType.Bar;
                    break;
                case 1:
                    CustomerChart.Series["Customer1"].ChartType = SeriesChartType.Pie;
                    break;
                case 2:
                    CustomerChart.Series["Customer1"].ChartType = SeriesChartType.Area;
                    break;
                case 3:
                    CustomerChart.Series["Customer1"].ChartType = SeriesChartType.Line;
                    break;
            }
        }
    }
}
