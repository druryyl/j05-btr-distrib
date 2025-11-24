using btr.domain.SalesContext.CheckInFeature;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.PurchaseContext.InvoiceInfo;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using btr.application.SalesContext.CheckInFeature;

namespace btr.distrib.SalesContext.LocationFeature
{
    public partial class CheckInInfoForm : Form
    {
        private readonly ICheckInDal _checkInModelDal;
        private List<CheckInModel> _dataSource;

        public CheckInInfoForm(ICheckInDal checkInModelDal)
        {
            InitializeComponent();
            _checkInModelDal = checkInModelDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<CheckInModel>();
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            // Export _dataSource to excel
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"checkin-info-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<CheckInModel>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as CheckInModel);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("checkin-info")
                    .Cell($"B1")
                    .InsertTable(listToExcel, false);
                var ws = wb.Worksheets.First();

                // Set border and font
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"N{listToExcel.Count + 1}")).Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);
                ws.Range(ws.Cell($"A{1}"), ws.Cell($"N{listToExcel.Count + 1}")).Style
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                // Format date and time columns
                ws.Range(ws.Cell($"C{2}"), ws.Cell($"D{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "@"; // Text format for date/time strings

                // Format numeric columns
                ws.Range(ws.Cell($"F{2}"), ws.Cell($"G{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.000000";
                ws.Range(ws.Cell($"H{2}"), ws.Cell($"H{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.00";
                ws.Range(ws.Cell($"L{2}"), ws.Cell($"M{listToExcel.Count + 1}"))
                    .Style.NumberFormat.Format = "#,##0.000000";

                // Add row numbering
                ws.Cell($"A1").Value = "No";
                for (var i = 0; i < listToExcel.Count; i++)
                    ws.Cell($"A{i + 2}").Value = i + 1;

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            System.Diagnostics.Process.Start(filePath);
        }

        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            if (e.TableCellIdentity.TableCellType == GridTableCellType.GroupCaptionCell)
            {
                e.Style.Themed = false;
                e.Style.BackColor = Color.PowderBlue;
            }

            //// Optional: Color code based on StatusSync
            //if (e.TableCellIdentity.TableCellType == GridTableCellType.RecordCell)
            //{
            //    var checkIn = e.TableCellIdentity.Record.GetData() as CheckInModel;
            //    if (checkIn != null && e.TableCellIdentity.Column.Name == "StatusSync")
            //    {
            //        switch (checkIn.StatusSync?.ToUpper())
            //        {
            //            case "DRAFT":
            //                e.Style.Themed = false;
            //                e.Style.BackColor = Color.LightYellow;
            //                break;
            //            case "SYNCED":
            //                e.Style.Themed = false;
            //                e.Style.BackColor = Color.LightGreen;
            //                break;
            //            case "ERROR":
            //                e.Style.Themed = false;
            //                e.Style.BackColor = Color.LightCoral;
            //                break;
            //        }
            //    }
            //}
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<CheckInModel>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            // Configure column appearances
            InfoGrid.TableDescriptor.Columns["CheckInLatitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["CheckInLongitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["CustomerLatitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["CustomerLongitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["Accuracy"].Appearance.AnyRecordFieldCell.Format = "N2";

            // Optional: Add summary for accuracy if needed
            var sumColAccuracy = new GridSummaryColumnDescriptor("Accuracy", SummaryType.DoubleAggregate, "Accuracy", "{Average}");
            sumColAccuracy.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColAccuracy.Appearance.AnySummaryCell.Format = "N2";
            sumColAccuracy.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColAccuracy });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            InfoGrid.Refresh();
            Proses();
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

            var listCheckIn = _checkInModelDal.ListData(periode)?.ToList() ?? new List<CheckInModel>();
            _dataSource = Filter(listCheckIn, SearchText.Text);
            InfoGrid.DataSource = _dataSource;
        }

        private List<CheckInModel> Filter(List<CheckInModel> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var keywordLower = keyword.ToLower();
            var listFilteredCustomer = source.Where(x =>
                x.CustomerName.ToLower().ContainMultiWord(keywordLower) ||
                x.CustomerCode.ToLower().Contains(keywordLower)).ToList();

            var listFilteredUser = source.Where(x =>
                x.UserEmail.ToLower().Contains(keywordLower)).ToList();

            var listFilteredCheckInId = source.Where(x =>
                x.CheckInId.ToLower().Contains(keywordLower)).ToList();

            var result = listFilteredCustomer
                .Union(listFilteredUser)
                .Union(listFilteredCheckInId);

            return result.ToList();
        }

        // Helper method to calculate distance between coordinates (optional)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in kilometers
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
