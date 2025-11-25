using btr.application.SalesContext.CheckInFeature;
using btr.domain.SalesContext.CheckInFeature;
using btr.nuna.Domain;
using ClosedXML.Excel;
using Syncfusion.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace btr.distrib.SalesContext.LocationFeature
{
    public partial class CheckInInfoForm : Form
    {
        private readonly ICheckInDal _checkInModelDal;
        private List<CheckInView> _dataSource;

        public CheckInInfoForm(ICheckInDal checkInModelDal)
        {
            InitializeComponent();
            _checkInModelDal = checkInModelDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<CheckInView>();
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
            var listToExcel = new List<CheckInView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as CheckInView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("checkin-info");

                // Create header row
                ws.Cell("A1").Value = "No";
                ws.Cell("B1").Value = "CheckIn ID";
                ws.Cell("C1").Value = "Date";
                ws.Cell("D1").Value = "Time";
                ws.Cell("E1").Value = "User Email";
                ws.Cell("F1").Value = "CheckIn Location";
                ws.Cell("G1").Value = "Customer Code";
                ws.Cell("H1").Value = "Customer Name";
                ws.Cell("I1").Value = "Customer Address";
                ws.Cell("J1").Value = "Customer Location";
                ws.Cell("K1").Value = "Distance (m)";

                // Fill data rows
                for (var i = 0; i < listToExcel.Count; i++)
                {
                    var checkIn = listToExcel[i];
                    var row = i + 2; // Start from row 2 (after header)

                    ws.Cell($"A{row}").Value = i + 1;
                    ws.Cell($"B{row}").Value = checkIn.CheckInId;
                    ws.Cell($"C{row}").Value = checkIn.CheckInDate;
                    ws.Cell($"D{row}").Value = checkIn.CheckInTime;
                    ws.Cell($"E{row}").Value = checkIn.UserEmail;
                    ws.Cell($"F{row}").Value = checkIn.CheckInLoc;
                    ws.Cell($"G{row}").Value = checkIn.CustomerCode;
                    ws.Cell($"H{row}").Value = checkIn.CustomerName;
                    ws.Cell($"I{row}").Value = checkIn.CustomerAddress;
                    ws.Cell($"J{row}").Value = checkIn.CustomerLoc;
                    ws.Cell($"K{row}").Value = Math.Round(checkIn.Distance, 2);
                }

                // Apply styling to the entire data range
                var dataRange = ws.Range(ws.Cell($"A1"), ws.Cell($"K{listToExcel.Count + 1}"));
                dataRange.Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                // Format header row
                var headerRange = ws.Range(ws.Cell($"A1"), ws.Cell($"K1"));
                headerRange.Style
                    .Font.Bold = true;

                // Format distance column
                var distanceRange = ws.Range(ws.Cell($"K2"), ws.Cell($"K{listToExcel.Count + 1}"));
                distanceRange.Style.NumberFormat.Format = "#,##0.00";

                // Apply color coding based on distance (optional)
                for (var i = 0; i < listToExcel.Count; i++)
                {
                    var checkIn = listToExcel[i];
                    var row = i + 2;
                    var distance = checkIn.Distance;

                    if (distance <= 50)
                    {
                        ws.Cell($"K{row}").Style.Fill.BackgroundColor = XLColor.LightGreen;
                    }
                    else if (distance <= 100)
                    {
                        ws.Cell($"K{row}").Style.Fill.BackgroundColor = XLColor.LightYellow;
                    }
                    else
                    {
                        ws.Cell($"K{row}").Style.Fill.BackgroundColor = XLColor.LightCoral;
                    }
                }

                // Auto-fit all columns
                ws.Columns().AdjustToContents();

                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(filePath);
        }

        private void InfoGrid_QueryCellStyleInfo(object sender, GridTableCellStyleInfoEventArgs e)
        {
            // Check if this is a record field cell (data row)
            if (e.TableCellIdentity.TableCellType == GridTableCellType.RecordFieldCell ||
                e.TableCellIdentity.TableCellType == GridTableCellType.AlternateRecordFieldCell)
            {
                // Get the record associated with this row
                var record = e.TableCellIdentity.DisplayElement.ParentRecord;

                if (record != null )
                {
                    // Get the underlying data object
                    var checkInView = record.GetData() as CheckInView;

                    if (checkInView != null)
                    {
                        double distance = checkInView.Distance;

                        // Apply conditional formatting based on distance
                        if (distance < 50)
                        {
                            e.Style.BackColor = Color.White;
                        }
                        else if (distance >= 50 && distance <= 100)
                        {
                            e.Style.BackColor = Color.Yellow;
                        }
                        else // distance > 100
                        {
                            e.Style.BackColor = Color.Red;
                            e.Style.TextColor = Color.White; // Optional: white text for better readability
                        }
                    }
                }
            }
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<CheckInView>();

            InfoGrid.TableDescriptor.AllowEdit = false;
            InfoGrid.TableDescriptor.AllowNew = false;
            InfoGrid.TableDescriptor.AllowRemove = false;
            InfoGrid.ShowGroupDropArea = true;

            InfoGrid.TopLevelGroupOptions.ShowFilterBar = true;
            foreach (GridColumnDescriptor column in InfoGrid.TableDescriptor.Columns)
            {
                column.AllowFilter = true;
            }

            // Remove the manual distance column since it's now a computed property in CheckInView
            // The Distance property will be automatically detected as a column

            // Configure column appearances for numeric properties
            InfoGrid.TableDescriptor.Columns["Distance"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["Distance"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;
            InfoGrid.TableDescriptor.Columns["Distance"].Width = 100;

            // Format the computed location columns if needed
            InfoGrid.TableDescriptor.Columns["CheckInLoc"].Width = 180;
            InfoGrid.TableDescriptor.Columns["CustomerLoc"].Width = 180;

            // Hide individual coordinate columns since we now have computed location strings
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CheckInLatitude");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CheckInLongitude");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CustomerLatitude");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CustomerLongitude");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("Accuracy");

            // Reorder columns for better readability (optional)
            var visibleColumns = InfoGrid.TableDescriptor.VisibleColumns;
            var desiredOrder = new List<string>
            {
                "CheckInId", "CheckInDate", "CheckInTime", "UserEmail", "CheckInLoc",
                "CustomerCode", "CustomerName", "CustomerAddress", "CustomerLoc", "Distance"
            };

            // Reorder columns according to desired order
            foreach (var columnName in desiredOrder)
            {
                var column = visibleColumns[columnName];
                if (column != null)
                {
                    visibleColumns.Remove(column);
                    visibleColumns.Add(column);
                }
            }

            // Summary for distance (now using the computed property)
            var sumColDistance = new GridSummaryColumnDescriptor("Distance", SummaryType.DoubleAggregate, "Distance", "{Average}");
            sumColDistance.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            sumColDistance.Appearance.AnySummaryCell.Format = "N2";
            sumColDistance.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Right;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] { sumColDistance });
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            // For all records field cells
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.AutoSize = true;
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.WrapText = false;

            // Set specific column text alignment
            InfoGrid.TableDescriptor.Columns["CheckInDate"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;
            InfoGrid.TableDescriptor.Columns["CheckInTime"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;
            InfoGrid.TableDescriptor.Columns["CustomerCode"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;

            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;

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

            var listCheckInModel = _checkInModelDal.ListData(periode)?.ToList() ?? new List<CheckInModel>();
            var listCheckIn = listCheckInModel.Select(x => new CheckInView(x))?.ToList() ?? new List<CheckInView>();
            _dataSource = Filter(listCheckIn, SearchText.Text);
            InfoGrid.DataSource = _dataSource;
        }

        private List<CheckInView> Filter(List<CheckInView> source, string keyword)
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
    }

    public class CheckInView
    {
        public CheckInView(CheckInModel model)
        {
            CheckInId = model.CheckInId;
            CheckInId = model.CheckInId;
            CheckInDate = model.CheckInDate;
            CheckInTime = model.CheckInTime;
            UserEmail = model.UserEmail;
            CheckInLatitude = model.CheckInLatitude;
            CheckInLongitude = model.CheckInLongitude;
            Accuracy = model.Accuracy;
            CustomerId = model.CustomerId;
            CustomerCode = model.CustomerCode;
            CustomerName = model.CustomerName;
            CustomerAddress = model.CustomerAddress;
            CustomerLatitude = model.CustomerLatitude;
            CustomerLongitude = model.CustomerLongitude;
        }
        public string CheckInId { get; set; }
        public string CheckInDate { get; set; }        // yyyy-MM-dd
        public string CheckInTime { get; set; }        // HH:mm:ss
        public string UserEmail { get; set; }
        public string CheckInLoc => $"{CheckInLatitude}, {CheckInLongitude}";
        public double CheckInLatitude { get; set; }
        public double CheckInLongitude { get; set; }
        public float Accuracy { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double CustomerLatitude { get; set; }
        public double CustomerLongitude { get; set; }
        public string CustomerLoc => $"{CustomerLatitude},{CustomerLongitude}";
        public double Distance => CalculateDistance();
        
        private double CalculateDistance()
        {
            const double R = 6371000; // Earth radius in meters

            var dLat = (CustomerLatitude - CheckInLatitude) * Math.PI / 180;
            var dLon = (CustomerLongitude - CheckInLongitude) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(CheckInLatitude * Math.PI / 180) * Math.Cos(CustomerLatitude * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return distance;
        }
    }
}
