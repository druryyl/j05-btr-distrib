using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.StokBalanceRpt;
using btr.domain.InventoryContext.StokBalanceRpt;
using btr.nuna.Domain;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;

namespace btr.distrib.InventoryContext.StokBalanceRpt
{
    public partial class StokBalanceInfo2Form : Form
    {
        private readonly IStokBalanceReportDal _stokBalanceReportDal;

        public StokBalanceInfo2Form(IStokBalanceReportDal stokBalanceReportDal)
        {
            InitializeComponent();
            _stokBalanceReportDal = stokBalanceReportDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
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
            Proses();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            var sumColNilaiSediaan = new GridSummaryColumnDescriptor("NilaiSediaan", SummaryType.DoubleAggregate, "NilaiSediaan", "{Sum}");
            sumColNilaiSediaan.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.Yellow);
            sumColNilaiSediaan.Appearance.AnySummaryCell.Format = "N0";
            sumColNilaiSediaan.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColNilaiSediaan });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);


            InfoGrid.TableDescriptor.Columns["QtyBesar"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["QtyKecil"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["InPcs"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["Hpp"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.TableDescriptor.Columns["NilaiSediaan"].Appearance.AnyRecordFieldCell.Format = "N0";
            InfoGrid.Refresh();
            Proses();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            var listFaktur = _stokBalanceReportDal.ListData()?.ToList() ?? new List<StokBalanceView>();
            listFaktur.ForEach(x => x.NilaiSediaan = x.Hpp * x.Qty);
            var filtered = Filter(listFaktur, SearchText.Text);
            var projection =
                from c in filtered
                select new
                {
                    Supplier = c.SupplierName,
                    Kategori = c.KategoriName,
                    BrgId = c.BrgId,
                    BrgCode = c.BrgCode,
                    BrgName = c.BrgName,
                    Warehouse = c.WarehouseName,
                    QtyBesar = c.QtyBesar,
                    SatBesar = c.SatBesar,
                    Conversion = c.Conversion,
                    QtyKecil = c.QtyKecil,
                    SatKecil = c.SatKecil,
                    InPcs = c.Qty,
                    Hpp = c.Hpp,
                    NilaiSediaan = c.NilaiSediaan,
                };
            InfoGrid.DataSource = projection.ToList();
        }

        private List<StokBalanceView> Filter(List<StokBalanceView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;
            var listFilteredBrgName = source.Where(x => x.BrgName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredBrgCode = source.Where(x => x.BrgCode.ToLower().StartsWith(keyword.ToLower())).ToList();
            var listFilteredKategori = source.Where(x => x.KategoriName.ToLower().ContainMultiWord(keyword)).ToList();
            var listFilteredSupplier = source.Where(x => x.SupplierName.ToLower().ContainMultiWord(keyword)).ToList();

            var result = listFilteredBrgName
                .Union(listFilteredBrgCode)
                .Union(listFilteredKategori)
                .Union(listFilteredSupplier);
            return result.ToList();
        }
    }
}
