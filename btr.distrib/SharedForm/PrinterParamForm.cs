using btr.application.SupportContext.ParamUserAgg;
using btr.domain.SupportContext.ParamUser;
using btr.domain.SupportContext.UserAgg;
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
    public partial class PrinterParamForm : Form
    {
        private readonly IParamUserDal _paramUserDal;
        private Dictionary<string, string> _paramDefaultValues;
        private BindingList<PrinterParamDto> _bindingList;

        public PrinterParamForm(IParamUserDal paramUserDal)
        {
            InitializeComponent();
            _paramUserDal = paramUserDal;
        }

        private void InitParamDefaultValues()
        {
            _paramDefaultValues = new Dictionary<string, string>();
            _paramDefaultValues.Add("FontName", "Lucida Console");
            _paramDefaultValues.Add("FontSize", "8.45");
            _paramDefaultValues.Add("MarginTop", "25");
            _paramDefaultValues.Add("MarginBottom", "25");
            _paramDefaultValues.Add("MarginLeft", "0");
            _paramDefaultValues.Add("MarginRight", "0");
        }

        private void ListParamUser(IUserKey userKey)
        {
            var listParamUser = _paramUserDal.ListData(userKey)?.ToList() ?? new List<ParamUserModel> ();
            _bindingList = new BindingList<PrinterParamDto>();
            foreach(var item in _paramDefaultValues)
            {
                var userValue = listParamUser.FirstOrDefault(x => x.Val)
                var newItem = new PrinterParamDto
                {
                    ParamName = item.Key,
                    Value = 
                }
            }
        }
    }

    public class PrinterParamDto
    {
        public string ParamName { get; set; }
        public string Value { get; set; }

    }
}
