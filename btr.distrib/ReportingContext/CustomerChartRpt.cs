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
            CustomerChart.Series.Add("SeriesName"); // Add a new series
            CustomerChart.Series["SeriesName"].XValueMember = "Kota"; // Set X-axis data column
            CustomerChart.Series["SeriesName"].YValueMembers = "JumCustomer"; // Set Y-axis data column
            CustomerChart.ChartAreas[0].AxisX.Title = "Kota";
            CustomerChart.ChartAreas[0].AxisY.Title = "Jumlah Customer";
        }
    }
}
