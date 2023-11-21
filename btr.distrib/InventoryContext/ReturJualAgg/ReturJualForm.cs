using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.distrib.Browsers;
using btr.domain.SalesContext.SalesPersonAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.ReturJualAgg
{
    public partial class ReturJualForm : Form
    {
        private readonly IBrowser<SalesPersonBrowserView> _salesBrowser;
        private readonly IBrowser<Faktur2BrowserView> _fakturBrowser;
        private readonly ISalesPersonDal _salesPersonDal;

        public ReturJualForm(IBrowser<SalesPersonBrowserView> salesBrowser,
            IBrowser<Faktur2BrowserView> fakturBrowser,
            ISalesPersonDal salesPersonDal)
        {
            InitializeComponent();
            _salesBrowser = salesBrowser;
            _fakturBrowser = fakturBrowser;

            RegisterEventHandler();
            _salesPersonDal = salesPersonDal;
        }

        private void RegisterEventHandler()
        {
            SalesPersonButton.Click += SalesPersonButton_Click;
            SalesPersonIdText.Validated += SalesPersonIdText_Validated;
            SalesPersonIdText.KeyDown += SalesPersonIdText_KeyDown;
        }

        private void SalesPersonIdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                _salesBrowser.Filter.UserKeyword = SalesPersonIdText.Text;
                SalesPersonIdText.Text = _salesBrowser.Browse(SalesPersonIdText.Text);
                SalesPersonIdText_Validated(sender, null);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void SalesPersonButton_Click(object sender, EventArgs e)
        {
            _salesBrowser.Filter.UserKeyword = SalesPersonIdText.Text;
            SalesPersonIdText.Text = _salesBrowser.Browse(SalesPersonIdText.Text);
        }

        private void SalesPersonIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var sales = _salesPersonDal.GetData(new SalesPersonModel(textbox.Text));
            SalesPersonNameText.Text = sales?.SalesPersonName ?? string.Empty;
        }
    }
}
