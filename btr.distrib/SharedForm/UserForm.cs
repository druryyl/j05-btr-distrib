using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.SupportContext.UserAgg;
using btr.distrib.Helpers;
using btr.domain.SupportContext.UserAgg;
using Polly;

namespace btr.distrib.SharedForm
{
    public partial class UserForm : Form
    {
        private readonly IUserDal _userDal;


        private readonly IUserBuilder _userBuilder;
        private readonly IUserWriter _userWriter;

        private IEnumerable<UserFormGridDto> _listUser;

        public UserForm(IUserDal userDal,
            IUserBuilder userBuilder,
            IUserWriter userWriter
            )
        {
            InitializeComponent();

            _userDal = userDal;

            _userBuilder = userBuilder;
            _userWriter = userWriter;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            UserIdText.Validated += UserIdText_Validated;

            SaveButton.Click += SaveButton_Click;
            ListGrid.CellDoubleClick += ListGrid_CellDoubleClick;

            NewButton.Click += NewButton_Click;
        }

        #region GRID-CUSTOMER
        private void ListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var grid = (DataGridView)sender;
            var userId = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            var user = _userBuilder.Load(new UserModel(userId)).Build();
            ShowData(user);
        }

        private void InitGrid()
        {
            var listUser = _userDal.ListData()?.ToList()
                ?? new List<UserModel>();

            _listUser = listUser
                .Select(x => new UserFormGridDto(x.UserId,
                    x.UserName)).ToList();
            ListGrid.DataSource = _listUser;

            ListGrid.Columns.SetDefaultCellStyle(Color.MintCream);
            ListGrid.Columns.GetCol("Id").Width = 50;
            ListGrid.Columns.GetCol("Name").Width = 150;
        }
        #endregion

        #region USER-ID
        private void UserIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;
            var fallback = Policy<UserModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new UserModel { UserId = UserIdText.Text});
            var user = fallback.Execute(() => _userBuilder
                .Load(new UserModel(textbox.Text))
                .Build());

            ShowData(user);
        }

        private void ShowData(UserModel user)
        {
            UserIdText.Text = user.UserId;
            UserNameText.Text = user.UserName;
        }

        private void ClearForm()
        {
            UserIdText.Clear();
            UserNameText.Clear();
        }
        #endregion

        #region NEW
        private void NewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region SAVE
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (PasswordText.Text != Password2Text.Text)
            {
                MessageBox.Show("Password is not the same");
                return;
            }

            var user = new UserModel(UserIdText.Text);
            user = _userBuilder
                .LoadOrCreate(user)
                .UserName(UserNameText.Text)
                .Password(PasswordText.Text)
                .Prefix(PrefixText.Text)
                .Build();

            _userWriter.Save(ref user);
            ClearForm();
            InitGrid();
        }
        #endregion
    }
    public class UserFormGridDto
    {
        public UserFormGridDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; }
        public string Name { get; }
    }
}
