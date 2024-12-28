using btr.application.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.ParamSistemAgg;
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
    public partial class ParamSistemForm : Form
    {
        private readonly IParamSistemDal _paramSistemDal;
        private readonly BindingList<ParamSistemModel> _listItem = new BindingList<ParamSistemModel>();
        private readonly BindingSource _bindingSource = new BindingSource();
        public ParamSistemForm(IParamSistemDal paramSistemDal)
        {
            InitializeComponent();

            _paramSistemDal = paramSistemDal;
            InitGrid();
            RegisterControlEvent();
        }

        private void RegisterControlEvent()
        {
            SaveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            foreach(var item in _listItem)
            {
                _paramSistemDal.Update(item);
            }
            MessageBox.Show("Data berhasil disimpan");
        }

        private void InitGrid()
        {
            _bindingSource.DataSource = _listItem;
            ParamGrid.DataSource = _bindingSource;
            var listParam = _paramSistemDal.ListData();
            _listItem.Clear();
            foreach(var item in listParam)
            {
                _listItem.Add(item);
            }
            //  auto size column width
            ParamGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ParamGrid.Columns["ParamCode"].ReadOnly = true;
            ParamGrid.AllowUserToAddRows = false;
        }
    }
}
