using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturInfoAgg;
using btr.domain.SalesContext.InfoFakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.InfoFakturAgg
{
    public partial class InfoFakturForm : Form
    {
        private readonly IFakturInfoDal _fakturInfoDal;

        public InfoFakturForm(IFakturInfoDal fakturInfoDal)
        {
            InitializeComponent();
            _fakturInfoDal = fakturInfoDal;
        }

        private void Proses()
        {
            var tgl1 = Tgl1Date.Value;
            var tgl2 = Tgl2Date.Value;
            var periode = new Periode(tgl1, tgl2);
            var listFaktur = _fakturInfoDal.ListData(periode);


            //var result = 
        }
    }




}
