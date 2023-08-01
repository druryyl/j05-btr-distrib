using btr.distrib.SalesContext.FakturAgg;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class MainForm : Form
    {
        private readonly FakturForm _fakturForm;

        public MainForm(FakturForm fakturForm)
        {
            InitializeComponent();
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;

            _fakturForm = fakturForm;
        }

        private void FakturButton_Click(object sender, EventArgs e)
        {
            _fakturForm.StartPosition = FormStartPosition.CenterScreen;
            _fakturForm.MdiParent = this;
            _fakturForm.Show();
        }
    }
}
