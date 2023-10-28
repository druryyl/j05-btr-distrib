using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturInfoAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SupportContext.UserAgg;
using btr.domain.SalesContext.InfoFakturAgg;
using btr.nuna.Domain;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.InfoFakturAgg
{
    public partial class InfoFakturForm : Form
    {
        private readonly IFakturInfoDal _fakturInfoDal;
        private readonly ISalesPersonDal _salesDal;
        private readonly IUserDal _userDal;
        private readonly IWarehouseDal _warehouseDal;

        public InfoFakturForm(IFakturInfoDal fakturInfoDal, 
            ISalesPersonDal salesDal, 
            IUserDal userDal, 
            IWarehouseDal warehouseDal)
        {
            InitializeComponent();
            _fakturInfoDal = fakturInfoDal;
            _salesDal = salesDal;
            _userDal = userDal;
            _warehouseDal = warehouseDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            InitComboValue();
            InitGrid();
        }

        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = Color.PowderBlue;
            }
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<FakturInfoDto>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;
            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in this.InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }
            InfoGrid.TableDescriptor.Columns["Total"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Diskon"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tax"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["GrandTotal"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Tgl"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy";
            InfoGrid.Refresh();
        }

        private void InitComboValue()
        {
            var listSales = _salesDal.ListData()?.Select(x => x.SalesPersonName).ToList() ?? new List<string>();
            listSales.Add(string.Empty);
            SalesCombo.DataSource = listSales;
            SalesCombo.SelectedIndex = -1;

            var listAdmin = _userDal.ListData()?.Select(x => x.UserName).ToList() ?? new List<string>();
            listAdmin.Add(string.Empty);
            AdminCombo.DataSource = listAdmin;
            AdminCombo.SelectedIndex = -1;
            AdminCombo.SelectedIndex = -1;

            var listWarehouse = _warehouseDal.ListData()?.Select(x => x.WarehouseName).ToList() ?? new List<string>();
            listWarehouse.Add(string.Empty);
            WarehouseCombo.DataSource = listWarehouse;
            WarehouseCombo.SelectedIndex = -1;
            WarehouseCombo.SelectedIndex = -1;
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var tgl1 = Tgl1Date.Value;
            var tgl2 = Tgl2Date.Value;
            var periode = new Periode(tgl1, tgl2);
            var timeSpan = tgl2 - tgl1;
            var dayCount = timeSpan.Days;
            if (dayCount > 122)
            {
                MessageBox.Show("Periode informasi maximal 3 bulan");
                return;
            }
            var listFaktur = _fakturInfoDal.ListData(periode)?.ToList() ?? new List<FakturInfoDto>();
            var result = FilterCustomer(listFaktur, CustomerText.Text);
            result = FilterAdmin(result, AdminCombo.SelectedItem?.ToString()??string.Empty);
            result = FilterSales(result, SalesCombo.SelectedItem?.ToString() ?? string.Empty);
            result = FilterWarehouse(result, WarehouseCombo.SelectedItem?.ToString() ?? string.Empty);
            InfoGrid.DataSource = result;
        }

        private List<FakturInfoDto> FilterCustomer(List<FakturInfoDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var customerCodeResult = source.Where(x => x.CustomerCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var customerNameResult = source.Where(x => x.Customer.ToLower().ContainMultiWord(keyword)).ToList();
            var result = customerCodeResult.Union(customerNameResult);
            return result.ToList();
        }
        private List<FakturInfoDto> FilterAdmin(List<FakturInfoDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var result = source.Where(x => x.Admin.ToLower().ContainMultiWord(keyword)).ToList();
            return result;
        }
        private List<FakturInfoDto> FilterSales(List<FakturInfoDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var result = source.Where(x => x.SalesPersonName.ToLower().ContainMultiWord(keyword)).ToList();
            return result;
        }
        private List<FakturInfoDto> FilterWarehouse(List<FakturInfoDto> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

             var result = source.Where(x => x.WarehouseName.ToLower().ContainMultiWord(keyword)).ToList();
            return result;
        }

    }




}
