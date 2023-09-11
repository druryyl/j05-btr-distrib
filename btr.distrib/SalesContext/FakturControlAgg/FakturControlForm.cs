using btr.application.SalesContext.FakturAgg.UseCases;
using btr.application.SalesContext.FakturControlAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.FakturControlAgg
{
    public partial class FakturControlForm : Form
    {
        private readonly IListFaktorControlWorker _listFaktorControlWorker;
        private readonly IFakturControlStatusDal _fakturControlStatusDal;
        private readonly IFakturControlBuilder _builder;
        private readonly IFakturControlWriter _writer;
        private readonly IVoidFakturWorker _voidFakturWorker;

        private BindingList<FakturControlView> _listItem = new BindingList<FakturControlView>();
        public FakturControlForm(IListFaktorControlWorker listFaktorControlWorker,
            IFakturControlBuilder builder,
            IFakturControlWriter writer,
            IFakturControlStatusDal fakturControlStatusDal,
            IVoidFakturWorker voidFakturWorker)
        {
            _listFaktorControlWorker = listFaktorControlWorker;
            _fakturControlStatusDal = fakturControlStatusDal;
            _builder = builder;
            _writer = writer;

            InitializeComponent();
            InitGrid();
            RefreshGrid();
            RegisterEventHandler();
            _voidFakturWorker = voidFakturWorker;
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            FakturGrid.CellContentClick += FakturGrid_CellContentClick;
        }

        private void FakturGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                return;

            FakturGrid.EndEdit();
            StatusFakturEnum statusFaktur = StatusFakturEnum.Unknown;
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "Posted": statusFaktur = StatusFakturEnum.Posted; break;
                case "Kirim": statusFaktur = StatusFakturEnum.Kirim; break;
                case "Kembali": statusFaktur = StatusFakturEnum.KembaliFaktur; break;
                case "Lunas": statusFaktur = StatusFakturEnum.Lunas; break;
                case "Pajak": statusFaktur = StatusFakturEnum.Pajak; break;
            }

            var isChecked = (bool)grid.CurrentCell.Value;
            var mainMenu = this.Parent.Parent;
            var user = ((MainForm)mainMenu).UserId;
            var faktur = new FakturModel(_listItem[e.RowIndex].FakturId);
            if (isChecked)
                FakturProses(faktur, statusFaktur, user);
            else
                FakturRollback(faktur, statusFaktur, user);
        }

        private void FakturProses(IFakturKey fakturKey, StatusFakturEnum statusFaktur, IUserKey userKey)
        {
            switch (statusFaktur)
            {
                case StatusFakturEnum.Posted:

                    break;
                
                case StatusFakturEnum.Kirim:
                    var faktur = _builder
                        .LoadOrCreate(fakturKey)
                        .Kirim(userKey)
                        .Build();
                    _writer.Save(faktur);

                    break;
                case StatusFakturEnum.KembaliFaktur:
                    faktur = _builder
                        .LoadOrCreate(fakturKey)
                        .KembaliFaktur(userKey)
                        .Build();
                    _writer.Save(faktur);
                    break;
                case StatusFakturEnum.Lunas:
                    break;
                case StatusFakturEnum.Pajak:
                    break;
                default:
                    break;
            }

        }

        private void FakturRollback(IFakturKey faktur, StatusFakturEnum statusFaktur, IUserKey userKey)
        {
            switch (statusFaktur)
            {
                //  void faktur
                case StatusFakturEnum.Posted:
                    var voidFakturRequest = new VoidFakturRequest(faktur.FakturId, userKey.UserId);
                    _voidFakturWorker.Execute(voidFakturRequest);
                    break;

                case StatusFakturEnum.Kirim:
                    break;
                case StatusFakturEnum.KembaliFaktur:
                    break;
                case StatusFakturEnum.Lunas:
                    break;
                case StatusFakturEnum.Pajak:
                    break;
                default:
                    break;
            }
        }

        private void SetStatus(string fakturId, bool value, StatusFakturEnum status)
        {

            //FakturControlModel faktur = null;
            //if (value is true)
            //{
            //    switch (status)
            //    {
            //        case StatusFakturEnum.Posted:
            //            break;
            //        case StatusFakturEnum.Kirim:
            //            faktur = _builder
            //                .LoadOrCreate(new FakturModel(fakturId))
            //                .Kirim(user)
            //                .Build();
            //            break;
            //        case StatusFakturEnum.KembaliFaktur:
            //            faktur = _builder
            //                .LoadOrCreate(new FakturModel(fakturId))
            //                .KembaliFaktur(user)
            //                .Build();
            //            break;
            //        case StatusFakturEnum.Lunas:
            //            break;
            //        case StatusFakturEnum.Pajak:
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //    faktur = _builder
            //        .LoadOrCreate(new FakturModel(fakturId))
            //        .Reset(status)
            //        .Build();

            //if(faktur != null)
            //    _ = _writer.Save(faktur);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void InitGrid()
        {
            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
            FakturGrid.Columns.SetDefaultCellStyle(Color.Beige);
            FakturGrid.Columns.GetCol("FakturId").Visible = false;
            FakturGrid.Columns.GetCol("FakturDate").DefaultCellStyle.Format = "ddd dd MMM yyyy";

            FakturGrid.Columns.GetCol("FakturId").Width = 80;
            FakturGrid.Columns.GetCol("FakturCode").Width = 80;
            FakturGrid.Columns.GetCol("FakturDate").Width = 100;
            FakturGrid.Columns.GetCol("CustomerName").Width = 150;
            FakturGrid.Columns.GetCol("Npwp").Width = 100;
            FakturGrid.Columns.GetCol("SalesPersonName").Width = 80;

            FakturGrid.Columns.GetCol("Total").Width = 80;
            FakturGrid.Columns.GetCol("Bayar").Width = 80;
            FakturGrid.Columns.GetCol("Sisa").Width = 80;

            FakturGrid.Columns.GetCol("Posted").Width = 50;
            FakturGrid.Columns.GetCol("Kirim").Width = 50;
            FakturGrid.Columns.GetCol("Kembali").Width = 50;
            FakturGrid.Columns.GetCol("Lunas").Width = 50;
            FakturGrid.Columns.GetCol("Pajak").Width = 50;
            FakturGrid.Columns.GetCol("NoFakturPajak").Width = 80;
            FakturGrid.Columns.GetCol("UserId").Width = 80;
            FakturGrid.RowHeadersWidth = 55;

            FakturGrid.Columns.GetCol("Total").DefaultCellStyle.BackColor = Color.PaleTurquoise;
            FakturGrid.Columns.GetCol("Bayar").DefaultCellStyle.BackColor = Color.Pink;
            FakturGrid.Columns.GetCol("Sisa").DefaultCellStyle.BackColor = Color.PaleTurquoise;

            FakturGrid.Columns.GetCol("FakturDate").HeaderText = "Tgl";
            FakturGrid.Columns.GetCol("CustomerName").HeaderText = "Customer";
            FakturGrid.Columns.GetCol("Npwp").HeaderText = "NPWP";
            FakturGrid.Columns.GetCol("SalesPersonName").HeaderText = "Sales";
            FakturGrid.Columns.GetCol("NoFakturPajak").HeaderText = "Faktur Pajak";
            FakturGrid.Columns.GetCol("UserId").HeaderText = "Admin";
        }

        private void RefreshGrid()
        {
            var periode = new Periode(Tgl1Text.Value, Tgl2Text.Value);
            var diffDate = periode.Tgl2 - periode.Tgl1;
            if (diffDate.Days > 31)
            {
                MessageBox.Show("Periode max 31 hari");
                return;
            }    

            var listData = _listFaktorControlWorker.Execute(periode);
            if (SearchText.Text.Length > 0)
                listData = FilterFaktur(listData, SearchText.Text);

            var _listTemp = listData.Select(x => x.Adapt<FakturControlView>()).ToList();
            _listItem = new BindingList<FakturControlView>(_listTemp);

            var listStatus = _fakturControlStatusDal.ListData(periode)?.ToList() ?? new List<FakturControlStatusModel>();
            foreach(var item in _listItem)
            {
                item.Kirim = listStatus
                    .Where(x => x.FakturId == item.FakturId)
                    .FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Kirim) != null ? true : false;
                item.Kembali = listStatus
                    .Where(x => x.FakturId == item.FakturId)
                    .FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.KembaliFaktur) != null ? true : false;
            }

            var binding = new BindingSource();
            binding.DataSource = _listItem;
            FakturGrid.DataSource = binding;
            FakturGrid.Refresh();
            foreach (DataGridViewRow row in FakturGrid.Rows)
                row.HeaderCell.Value = $"{(row.Index + 1):N0}";
            
        }

        private List<FakturControlModel> FilterFaktur(IEnumerable<FakturControlModel> listData, string keywork)
        {
            var listCustomerName = listData.Where(x => x.CustomerName.ToLower().ContainMultiWord(SearchText.Text.ToLower()))?.ToList();
            var listCustomerId = listData.Where(x => x.CustomerCode.ToLower().StartsWith(SearchText.Text.ToLower())).ToList();
            var listFakturCode = listData.Where(x => x.FakturCode.ToLower().StartsWith(SearchText.Text.ToLower())).ToList();
            var listSales = listData.Where(x => x.SalesPersonName.ToLower().StartsWith(SearchText.Text.ToLower())).ToList();
            var listAdmin = listData.Where(x => x.UserId.ToLower() == SearchText.Text.ToLower());
            listData = listCustomerName
                .Concat(listCustomerId)
                .Concat(listFakturCode)
                .Concat(listSales)
                .Concat(listAdmin)
                .OrderBy(x => x.FakturDate);

            return listData.ToList();
        }
    }

    public class FakturControlView
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string Npwp { get; private set; }
        public string SalesPersonName { get; private set; }
        public Decimal Total { get; private set; }
        public decimal Bayar { get; private set; }
        public decimal Sisa { get; private set; }

        public bool Posted { get; set; }
        public bool Kirim { get; set; }
        public bool Kembali { get; set; }
        public bool Lunas { get; private set; }
        public bool Pajak { get; private set; }
        public string NoFakturPajak { get; private set; }
        public string UserId { get; private set; }

        public void SetLunas(bool val) => Lunas = val;
        public void SetPajak(bool val) => Pajak = val;


    }
}
