using btr.application.SupportContext.RoleFeature;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.domain.SupportContext.RoleFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SharedForm
{
    public partial class RoleForm : Form
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IMenuRepo _menuRepo;
        private readonly BindingList<MenuView> _listMenu;
        private readonly BindingSource _menuBindingSource;
        private readonly IBrowser<RoleBrowserView> _roleBrowser;
        private Dictionary<int, Color> _colorMap = new Dictionary<int, Color>
        {
            { 0, Color.White },
            { 1, Color.FromArgb(210, 230, 255) },
            { 2, Color.FromArgb(210, 255, 225) },
            { 3, Color.FromArgb(225, 210, 255) },
            { 4, Color.FromArgb(255, 225, 210) },
            { 5, Color.FromArgb(200, 255, 230) },
            { 6, Color.FromArgb(200, 255, 230) },
            { 7, Color.LightGray }
        };

        public RoleForm(IRoleRepo roleRepo,
            IMenuRepo menuRepo,
            IBrowser<RoleBrowserView> roleBrowser)
        {
            InitializeComponent();
            _roleRepo = roleRepo;
            _menuRepo = menuRepo;
            _listMenu = new BindingList<MenuView>();
            _menuBindingSource = new BindingSource(_listMenu, null);
            _roleBrowser = roleBrowser;

            RegisterEventHandler();
            InitGrid();
        }

        private void RegisterEventHandler()
        {
            RoleListButton.Click += RoleListButton_Click;
            RoleIdTextBox.Validated += RoleIdTextBox_Validated;
            SaveButton.Click += (s, e) => SaveRole();
        }

        private void SaveRole()
        {
            var key = RoleType.Key(RoleIdTextBox.Text);
            RoleType role = null;
            var roleMaybe = _roleRepo.LoadEntity(key);
            if (roleMaybe.HasValue)
                role = roleMaybe.Value;
            else
                role = RoleType.Create(RoleIdTextBox.Text, RoleNameTextBox.Text);

            role.ClearListMenu();
            foreach (var item in _listMenu.Where(x => x.Pilih))
                role.AddMenu(item.ToModel());

            _roleRepo.SaveChanges(role);
            ClearScreen();
            RoleIdTextBox.Text = string.Empty;
        }

        private void RoleIdTextBox_Validated(object sender, EventArgs e)
        {
            var key = RoleType.Key(RoleIdTextBox.Text);
            var roleMaybe = _roleRepo.LoadEntity(key);
            if (roleMaybe.HasValue)
                LoadRole(roleMaybe.Value);
            else
                ClearScreen();
        }

        private void LoadRole(RoleType role)
        {
            ClearScreen();
            RoleNameTextBox.Text = role.RoleName; 
            foreach(var item in role.ListMenu)
            {
                var menu = _listMenu.FirstOrDefault(x => x.MenuId == item.MenuId);
                if (menu is null)
                    continue;
                menu.Pilih = true;
            }
        }

        private void ClearScreen()
        {
            RoleNameTextBox.Clear();
            foreach (var item in _listMenu)
                item.Pilih = false;
            MenuGrid.Refresh();
        }
        private void RoleListButton_Click(object sender, EventArgs e)
        {
            RoleIdTextBox.Text = _roleBrowser.Browse(RoleIdTextBox.Text);
            RoleIdTextBox_Validated(RoleIdTextBox, null);
        }

        private void InitGrid()
        {
            MenuGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
            MenuGrid.CellFormatting += MenuGrid_CellFormatting;

            _listMenu.RaiseListChangedEvents = true;
            _listMenu.AllowNew = true;
            _listMenu.AllowEdit = true;
            _listMenu.AllowRemove = true;
            
            MenuGrid.DataSource = _menuBindingSource;
            var col = MenuGrid.Columns;
            col.SetDefaultCellStyle(Color.Beige);
            
            col["MenuId"].Width = 50;
            col["MenuId"].HeaderText = "ID";

            col["MenuName"].Width = 50;
            col["MenuName"].Visible = false;

            col["GroupOrder"].Visible = false;
            col["FormType"].Width = 80;


            col["Caption"].Width = 200;
            col["Caption"].HeaderText = "Menu Name";

            col["Pilih"].Width = 50;
            col["Pilih"].HeaderText = "Pilih";

            SeedingMenuGrid();
        }

        private void SeedingMenuGrid()
        {
            var listData = _menuRepo.ListData()?.ToList() ?? new List<MenuType>();
            _listMenu.Clear();
            foreach (var item in listData.OrderBy(x => x.GroupOrder).ThenBy(x => x.MenuId))
                _listMenu.Add(new MenuView(item));
        }

        private void MenuGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = (DataGridView)sender;
            var row = grid.Rows[e.RowIndex];

            if (grid.Columns["GroupOrder"] != null)
            {
                var groupOrder = Convert.ToInt32(row.Cells["GroupOrder"].Value ?? 0);
                groupOrder = groupOrder / 10;
                if (_colorMap.ContainsKey(groupOrder))
                    row.DefaultCellStyle.BackColor = _colorMap[groupOrder];
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            }
        }
        private void MenuGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            if (grid.CurrentCell.ColumnIndex != grid.Columns["Pilih"].Index)
                return;
            grid.EndEdit();
        }
    }

    public class MenuView : INotifyPropertyChanged
    {
        private bool _pilih;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MenuView(string menuId, 
            int groupOrder, string formType,
            string menuName, string caption, bool pilih)
        {
            MenuId = menuId;
            GroupOrder = groupOrder;
            FormType = formType;
            MenuName = menuName;
            Caption = caption;
            _pilih = pilih;
        }

        public MenuView(MenuType model)
        {
            MenuId = model.MenuId;
            GroupOrder = model.GroupOrder;
            FormType = model.FormType;
            MenuName = model.MenuName;
            Caption = model.Caption;
            _pilih = false;
        }

        public string MenuId { get; private set; }
        public int GroupOrder { get; private set; }
        public string FormType { get; private set; }
        public string MenuName { get; private set; }
        public string Caption { get; private set; }

        public bool Pilih
        {
            get => _pilih;
            set
            {
                if (_pilih != value)
                {
                    _pilih = value;
                    OnPropertyChanged(nameof(Pilih));
                }
            }
        }

        public MenuType ToModel()
        {
            var result = new MenuType(MenuId, GroupOrder, FormType, MenuName, Caption);
            return result;
        }
    }
}
