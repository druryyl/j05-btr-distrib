using btr.application.BrgContext.JenisBrgAgg.Contracts;
using btr.application.BrgContext.KategoriAgg.Contracts;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.distrib.Browsers;
using btr.distrib.Helpers;
using btr.distrib.PurchaseContext.PurchaseOrderAgg;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.JenisBrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.InventoryContext.BrgAgg
{
    public partial class BrgForm : Form
    {
        private readonly IBrowser<SupplierBrowserView> _supplierBrowser;
        private readonly IBrowser<KategoriBrowserView> _kategoriBrowser;

        private readonly ISupplierDal _supplierDal;
        private readonly IJenisBrgDal _jenisBrgDal;
        private readonly IKategoriDal _kategoriDal;

        private readonly BindingList<BrgFormSatuanDto> _listSatuan = new BindingList<BrgFormSatuanDto>();
        private readonly BindingList<BrgFormHargaDto> _listHarga = new BindingList<BrgFormHargaDto>();


        public BrgForm(
            IBrowser<SupplierBrowserView> supplierBrowser,
            IBrowser<KategoriBrowserView> kategoriBrowser,
            ISupplierDal supplierDal,
            IJenisBrgDal jenisBrgDal,
            IKategoriDal kategoriDal)
        {
            InitializeComponent();
            RegisterEventHandler();

            _supplierBrowser = supplierBrowser;
            _kategoriBrowser = kategoriBrowser;

            _supplierDal = supplierDal;
            _jenisBrgDal = jenisBrgDal;
            _kategoriDal = kategoriDal;

            InitJenisBrg();
            InitGridSatuan();
            InitGridHarga();
        }

        private void RegisterEventHandler()
        {
            SupplierButton.Click += SupplierButton_Click;
            SupplierIdText.Validated += SupplierIdText_Validated;

            KategoriButton.Click += KategoriButton_Click;
            KategoriIdText.Validated += KategoriIdText_Validated;
        }

        #region SUPPLIER
        private void SupplierButton_Click(object sender, EventArgs e)
        {
            SupplierIdText.Text = _supplierBrowser.Browse(SupplierIdText.Text);
            SupplierIdText_Validated(SupplierIdText, null);
        }

        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var supplier = _supplierDal.GetData(new SupplierModel(textbox.Text));
            SupplierNameText.Text = supplier?.SupplierName ?? string.Empty;
        }

        #endregion

        #region JENIS-BRG
        private void InitJenisBrg()
        {
            var listJenisBrg = _jenisBrgDal.ListData()?.ToList() ?? new List<JenisBrgModel>();
            JenisBrgCombo.DataSource = listJenisBrg;
            JenisBrgCombo.DisplayMember = "JenisBrgName";
            JenisBrgCombo.ValueMember = "JenisBrgId";
        }
        #endregion

        #region KATEGORI-BRG
        private void KategoriButton_Click(object sender, EventArgs e)
        {
            KategoriIdText.Text = _kategoriBrowser.Browse(KategoriIdText.Text);
            KategoriIdText_Validated(KategoriIdText, null);
        }
        private void KategoriIdText_Validated(object sender, EventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;

            var kategori = _kategoriDal.GetData(new KategoriModel(textbox.Text));
            KategoriNameText.Text = kategori?.KategoriName ?? string.Empty;
        }



        #endregion

        #region GRID-SATUAN
        private void InitGridSatuan()
        {
            var binding = new BindingSource();
            binding.DataSource = _listSatuan;
            SatuanGrid.DataSource = binding;
            SatuanGrid.Refresh();
            SatuanGrid.Columns.SetDefaultCellStyle();
            SatuanGrid.Columns.GetCol("Satuan").Width = 80;
            SatuanGrid.Columns.GetCol("Conversion").Width = 80;
        }
        #endregion

        #region GRID-SATUAN
        private void InitGridHarga()
        {
            var binding = new BindingSource();
            binding.DataSource = _listHarga;
            HargaGrid.DataSource = binding;
            HargaGrid.Refresh();
            HargaGrid.Columns.SetDefaultCellStyle();
            HargaGrid.Columns.GetCol("TypeId").Width = 50;
            HargaGrid.Columns.GetCol("Name").Width = 80;
            HargaGrid.Columns.GetCol("Hpp").Width = 60;
            HargaGrid.Columns.GetCol("Margin").Width = 60;
            HargaGrid.Columns.GetCol("Harga").Width = 60;
            HargaGrid.Columns.GetCol("Keterangan").Width = 120;
        }
        #endregion

    }
}
    