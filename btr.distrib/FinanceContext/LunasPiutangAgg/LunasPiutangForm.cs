using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.FinanceContext.LunasPiutangAgg
{
    public partial class LunasPiutangForm : Form
    {
        public LunasPiutangForm()
        {
            InitializeComponent();
            InitEventHandler();
            InputPanel.Dock = DockStyle.Fill;
        }

        private void InitEventHandler()
        {
            CancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            InputPanel.Visible = false;
        }
    }
}
