using j07_btrade_sync.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace j07_btrade_sync
{
    public partial class KonfigurasiForm : Form
    {
        private readonly RegistryHelper _registryHelper;
        public KonfigurasiForm()
        {
            InitializeComponent();
            _registryHelper = new RegistryHelper();

            OKButton.Click += OKButton_Click;
            CancelButton.Click += (s, e) => this.Close();

            ServerTargetIDText.Text = _registryHelper.ReadString("ServerTargetID", "JOG");
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                _registryHelper.WriteString("ServerTargetID", ServerTargetIDText.Text);
                MessageBox.Show("Configuration saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
