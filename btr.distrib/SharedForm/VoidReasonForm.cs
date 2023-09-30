using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class VoidReasonForm : Form
    {
        public string Alasan { get => AlasanText.Text; }

        public VoidReasonForm()
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
            BatalButton.Click += CancelButton_Click;
        }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            if (AlasanText.Text.Length == 0)
            {
                AlasanLabel.Text = "Alasan void tidak boleh kosong";
                AlasanLabel.ForeColor = Color.DarkRed;
                SystemSounds.Beep.Play();
                return;
            }
            DialogResult = DialogResult.OK;
        }
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}