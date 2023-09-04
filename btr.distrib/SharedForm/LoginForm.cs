using btr.application.SupportContext.UserAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class LoginForm : Form
    {
        private readonly IUserDal _userDal;
        public string UserId { get; private set; }

        public LoginForm(IUserDal userDal)
        {
            InitializeComponent();
            _userDal = userDal;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var user = _userDal.GetData(new UserModel(UserIdText.Text))
                ?? new UserModel();
            user.RemoveNull();
            var passHash = PasswrodText.Text.HashSha256();
            if (passHash == user.Password)
            {
                UserId = user.UserId;
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Cancel;
        }
    }
}
