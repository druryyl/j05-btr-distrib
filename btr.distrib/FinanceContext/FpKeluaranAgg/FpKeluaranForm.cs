using btr.application.SalesContext.FakturAgg.Contracts;
using btr.distrib.Helpers;
using btr.domain.SalesContext.FakturAgg;
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

namespace btr.distrib.FinanceContext.FpKeluaranAgg
{
    public partial class FpKeluaranForm : Form
    {
        private readonly IFakturDal _fakturDal;

        private readonly BindingList<FpKeluaranFakturDto> _listFaktur;
        private readonly BindingList<FpKeluaranFakturDto> _listFakturPilih;
        private readonly BindingSource _fakturBindingSource;

        public FpKeluaranForm(IFakturDal fakturDal)
        {
            InitializeComponent();

            _fakturDal = fakturDal;
            _listFaktur = new BindingList<FpKeluaranFakturDto>();
            _listFakturPilih = new BindingList<FpKeluaranFakturDto>();
            _fakturBindingSource = new BindingSource(_listFaktur, null);

            RegisterEventHandler();
            InitGrid();
            InitCalender();
        }

        private void RegisterEventHandler()
        {
            SearchButton.Click += SearchButton_Click;
            FakturGrid.RowPostPaint += DataGridViewExtensions.DataGridView_RowPostPaint;
        }

        private void InitCalender()
        {
            PeriodeCalender.MaxSelectionCount = 31;
            PeriodeCalender.SelectionStart = DateTime.Now;
            PeriodeCalender.SelectionEnd = DateTime.Now;
        }

        #region GRID
        private void InitGrid()
        {
            #region TESTING UX
            //_listFaktur.Clear();
            //foreach(var item in ListFakturFaker())
            //{
            //    _listFaktur.Add(item);
            //}
            #endregion

            FakturGrid.DataSource = _fakturBindingSource;
            FakturGrid.DefaultCellStyle.Font = new Font("Lucida Console", 8);

            var grid = FakturGrid.Columns;
            grid["FakturId"].Width = 100;
            grid["FakturCode"].Width = 70;
            grid["FakturDate"].Width = 80;
            grid["CustomerName"].Width = 130;
            grid["Npwp"].Width = 125;
            grid["Address"].Width = 220;
            grid["GrandTotal"].Width = 80;
            grid["IsPilih"].Width = 30;

            grid["FakturId"].HeaderText = "ID";
            grid["FakturCode"].HeaderText = "Kode";
            grid["FakturDate"].HeaderText = "Tanggal";
            grid["CustomerName"].HeaderText = "Customer";
            grid["Npwp"].HeaderText = "NPWP";
            grid["Address"].HeaderText = "Alamat";
            grid["GrandTotal"].HeaderText = "Total";
            grid["IsPilih"].HeaderText = "Pilih";

            grid["FakturDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
            grid["GrandTotal"].DefaultCellStyle.Format = "N0";
            grid["GrandTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid["CustomerName"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
            grid["Address"].DefaultCellStyle.Font = new Font("Segoe UI", 8);
        }
        private IEnumerable<FpKeluaranFakturDto> ListFakturFaker()
        {
            return new List<FpKeluaranFakturDto>
            {
                new FpKeluaranFakturDto("FKTR24C001EA0", "B0000848", DateTime.Now, "APOTEK WIJAYA KUSUMA", "0000000000000000", "JL.TAMBAKAN NO 16A, TEGAL SLEREM", 96327.71M, false),
                new FpKeluaranFakturDto("FKTR251000005", "B0000849", DateTime.Now, "TOKO BAROKAH ABADI", "3308065808780001", "JL MAGELANG PUCANG PANCORAN MAS", 409176.06M, false),
                new FpKeluaranFakturDto("FKTR251000017", "B0000850", DateTime.Now, "APOTEK KRANGGAN", "3308095304750002", "JL.RAYA KRANGGAN - TEMANGGUNG", 87523.68M, true),
                new FpKeluaranFakturDto("FKTR251000023", "B0000851", DateTime.Now, "NAKITA BABY SHOP", "3308075705950001", "DONGKELAN UTARA, JAMPIROSO", 61272.00M, true),
                new FpKeluaranFakturDto("FKTR251000030", "B0000852", DateTime.Now, "TK ENGGAL", "3308152704700001", "JL TEMANGGUNG-SECANG BADRAN KRANGGAN", 3280127.70M, false),

                new FpKeluaranFakturDto("FKTR24C001EA0", "B0000848", DateTime.Now, "APOTEK WIJAYA KUSUMA", "0000000000000000", "JL.TAMBAKAN NO 16A, TEGAL SLEREM", 96327.71M, false),
                new FpKeluaranFakturDto("FKTR251000005", "B0000849", DateTime.Now, "TOKO BAROKAH ABADI", "3308065808780001", "JL MAGELANG PUCANG PANCORAN MAS", 409176.06M, false),
                new FpKeluaranFakturDto("FKTR251000017", "B0000850", DateTime.Now, "APOTEK KRANGGAN", "3308095304750002", "JL.RAYA KRANGGAN - TEMANGGUNG", 87523.68M, true),
                new FpKeluaranFakturDto("FKTR251000023", "B0000851", DateTime.Now, "NAKITA BABY SHOP", "3308075705950001", "DONGKELAN UTARA, JAMPIROSO", 61272.00M, true),
                new FpKeluaranFakturDto("FKTR251000030", "B0000852", DateTime.Now, "TK ENGGAL", "3308152704700001", "JL TEMANGGUNG-SECANG BADRAN KRANGGAN", 3280127.70M, false),
            };
        }
        #endregion


        #region SEARCH
        private void SearchButton_Click(object sender, EventArgs e)
        {
            PreserveTerpilih();
            SearchFaktur();
            RestoreTerpilih();
        }

        private void PreserveTerpilih()
        {
            var listTerpilih = _listFaktur.Where(x => x.IsPilih).ToList();
            _listFakturPilih.Clear();
            foreach (var item in listTerpilih)
                _listFakturPilih.Add(item);
        }

        private void RestoreTerpilih()
        {
            //  remove _listFaktur which is already in _listFakturPilih
            foreach (var item in _listFakturPilih)
            {
                var faktur = _listFaktur.FirstOrDefault(x => x.FakturId == item.FakturId);
                if (faktur != null)
                    _listFaktur.Remove(faktur);
            }
            int i = 0;
            foreach (var item in _listFakturPilih)
            {
                _listFaktur.Insert(i, item);
                i++;
            }
        }

        public void SearchFaktur()
        {
            var tgl1 = PeriodeCalender.SelectionStart;
            var tgl2 = PeriodeCalender.SelectionEnd;
            var periode = new Periode(tgl1, tgl2);
            var listFaktur = _fakturDal.ListData(periode)?.ToList() ?? new List<FakturModel>();
            listFaktur.RemoveAll(x => x.FpKeluaranId != string.Empty);

            var searchText = SearchText.Text.ToUpper();
            if (searchText != string.Empty)
                listFaktur = listFaktur.Where(x => x.FakturCode.ToUpper().Contains(searchText) || x.CustomerName.ToUpper().Contains(searchText)).ToList();

            _listFaktur.Clear();
            foreach(var item in listFaktur)
            {
                var newItem = new FpKeluaranFakturDto(item.FakturId, item.FakturCode, item.FakturDate, item.CustomerName, item.Npwp, item.Address, item.GrandTotal, false);
                _listFaktur.Add(newItem);
            }
        }
        #endregion
    }
}
