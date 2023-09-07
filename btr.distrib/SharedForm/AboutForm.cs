using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class AboutForm : Form
    {
        private readonly DatabaseOptions _opt;
        public AboutForm(IOptions<DatabaseOptions> opt)
        {
            InitializeComponent();
            _opt = opt.Value;
            label1.Text = GetDbConn();
        }

        private string GetDbConn()
        {
            var sb = new StringBuilder();
            sb.Append($"Version {Application.ProductVersion}\n");
            sb.Append($"Server {_opt.ServerName}\n");
            sb.Append($"Database {_opt.DbName}\n");
            return sb.ToString();
        }
    }
}
