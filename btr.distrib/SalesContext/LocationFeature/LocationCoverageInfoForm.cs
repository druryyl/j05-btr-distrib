using btr.application.SalesContext.CustomerAgg.Contracts;
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
using btr.application.SalesContext.CheckInFeature;
using btr.nuna.Domain;
using Syncfusion.Drawing;
using Syncfusion.Grouping;

namespace btr.distrib.SalesContext.LocationFeature
{
    public partial class LocationCoverageInfoForm : Form
    {
        private readonly ICustomerDal _customerDal;
        private List<CustomerLocationView> _dataSource;

        public LocationCoverageInfoForm(ICustomerDal customerDal)
        {
            InitializeComponent();
            _customerDal = customerDal;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<CustomerLocationView>();
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
                saveFileDialog.FileName = $"customer-location-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<CustomerLocationView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as CustomerLocationView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("customer-location");

                // Create header row
                ws.Cell("A1").Value = "No";
                ws.Cell("B1").Value = "Customer ID";
                ws.Cell("C1").Value = "Customer Name";
                ws.Cell("D1").Value = "Address";
                ws.Cell("E1").Value = "Wilayah";
                ws.Cell("F1").Value = "Klasifikasi";
                ws.Cell("G1").Value = "Latitude";
                ws.Cell("H1").Value = "Longitude";
                ws.Cell("I1").Value = "Accuracy";
                ws.Cell("J1").Value = "Coordinate Timestamp";
                ws.Cell("K1").Value = "Coordinate User";

                // Fill data rows
                for (var i = 0; i < listToExcel.Count; i++)
                {
                    var customer = listToExcel[i];
                    var row = i + 2;

                    ws.Cell($"A{row}").Value = i + 1;
                    ws.Cell($"B{row}").Value = customer.CustomerId;
                    ws.Cell($"C{row}").Value = customer.CustomerName;
                    ws.Cell($"D{row}").Value = customer.CustomerAddress;
                    ws.Cell($"E{row}").Value = customer.WilayahName;
                    ws.Cell($"F{row}").Value = customer.KlasifikasiName;
                    ws.Cell($"G{row}").Value = customer.Latitude;
                    ws.Cell($"H{row}").Value = customer.Longitude;
                    ws.Cell($"I{row}").Value = customer.Accuracy;
                    ws.Cell($"J{row}").Value = customer.CoordinateTimestamp;
                    ws.Cell($"K{row}").Value = customer.CoordinateUser;

                    // Apply color coding based on coordinate availability
                    if (customer.Latitude != 0 && customer.Longitude != 0)
                    {
                        var rowRange = ws.Range(ws.Cell($"A{row}"), ws.Cell($"K{row}"));
                        if (customer.Accuracy <= 50)
                            rowRange.Style.Fill.BackgroundColor = XLColor.LightGreen; // High accuracy
                        else if (customer.Accuracy <= 100)
                            rowRange.Style.Fill.BackgroundColor = XLColor.LightYellow; // Medium accuracy
                        else
                            rowRange.Style.Fill.BackgroundColor = XLColor.LightCoral; // Low accuracy
                    }
                    else
                    {
                        var rowRange = ws.Range(ws.Cell($"A{row}"), ws.Cell($"K{row}"));
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightGray; // No coordinates
                    }
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

                // Format numeric columns
                var latRange = ws.Range(ws.Cell($"G2"), ws.Cell($"G{listToExcel.Count + 1}"));
                var lonRange = ws.Range(ws.Cell($"H2"), ws.Cell($"H{listToExcel.Count + 1}"));
                var accuracyRange = ws.Range(ws.Cell($"I2"), ws.Cell($"I{listToExcel.Count + 1}"));
                latRange.Style.NumberFormat.Format = "#,##0.000000";
                lonRange.Style.NumberFormat.Format = "#,##0.000000";
                accuracyRange.Style.NumberFormat.Format = "#,##0.00";

                // Format date column
                var dateRange = ws.Range(ws.Cell($"J2"), ws.Cell($"J{listToExcel.Count + 1}"));
                dateRange.Style.NumberFormat.Format = "dd-MMM-yyyy HH:mm";

                // Auto-fit all columns
                ws.Columns().AdjustToContents();

                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(filePath);
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<CustomerLocationView>();

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
            InfoGrid.TableDescriptor.Columns["Latitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["Longitude"].Appearance.AnyRecordFieldCell.Format = "N6";
            InfoGrid.TableDescriptor.Columns["Accuracy"].Appearance.AnyRecordFieldCell.Format = "N2";
            InfoGrid.TableDescriptor.Columns["CoordinateTimestamp"].Appearance.AnyRecordFieldCell.Format = "dd-MMM-yyyy HH:mm";

            // Set column widths for better readability
            InfoGrid.TableDescriptor.Columns["CustomerId"].Width = 100;
            InfoGrid.TableDescriptor.Columns["CustomerName"].Width = 200;
            InfoGrid.TableDescriptor.Columns["CustomerAddress"].Width = 250;
            InfoGrid.TableDescriptor.Columns["WilayahName"].Width = 120;
            InfoGrid.TableDescriptor.Columns["KlasifikasiName"].Width = 120;
            InfoGrid.TableDescriptor.Columns["Latitude"].Width = 110;
            InfoGrid.TableDescriptor.Columns["Longitude"].Width = 110;
            InfoGrid.TableDescriptor.Columns["Accuracy"].Width = 80;
            InfoGrid.TableDescriptor.Columns["CoordinateTimestamp"].Width = 140;
            InfoGrid.TableDescriptor.Columns["CoordinateUser"].Width = 120;

            // Hide technical ID columns (optional)
            InfoGrid.TableDescriptor.VisibleColumns.Remove("WilayahId");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("KlasifikasiId");

            // Summary rows
            var countAllCustomer = new GridSummaryColumnDescriptor("AllCust", SummaryType.Count, "CoordinateTimestamp", "All-Cust: {Count}");
            countAllCustomer.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightGreen);
            countAllCustomer.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Left;

            var countWithCoord = new GridSummaryColumnDescriptor("HasCoord", SummaryType.Int32Aggregate, "HasCoordinate", "With-Coord: {Sum}");
            countWithCoord.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightGreen);
            countWithCoord.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Left;

            var sumRowDescriptor = new GridSummaryRowDescriptor();
            sumRowDescriptor.SummaryColumns.AddRange(new GridSummaryColumnDescriptor[] {
            countAllCustomer, countWithCoord});
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor);

            // For all records field cells
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.AutoSize = true;
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.WrapText = false;

            // Set text alignment
            InfoGrid.TableDescriptor.Columns["Latitude"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;
            InfoGrid.TableDescriptor.Columns["Longitude"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;
            InfoGrid.TableDescriptor.Columns["Accuracy"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Right;
            InfoGrid.TableDescriptor.Columns["CoordinateTimestamp"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;

            InfoGrid.Refresh();
            Proses();
        }

        private void ProsesButton_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Proses()
        {
            try
            {
                var listCustomerLocation = _customerDal.ListLocation()?.ToList() ?? new List<CustomerLocationView>();
                _dataSource = Filter(listCustomerLocation, SearchText.Text);
                InfoGrid.DataSource = _dataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer location data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<CustomerLocationView> Filter(List<CustomerLocationView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var keywordLower = keyword.ToLower();
            var listFilteredCustomer = source.Where(x =>
                x.CustomerName.ToLower().ContainMultiWord(keywordLower) ||
                x.CustomerId.ToLower().Contains(keywordLower)).ToList();

            var listFilteredWilayah = source.Where(x =>
                x.WilayahName?.ToLower().Contains(keywordLower) == true).ToList();

            var listFilteredKlasifikasi = source.Where(x =>
                x.KlasifikasiName?.ToLower().Contains(keywordLower) == true).ToList();

            var listFilteredAddress = source.Where(x =>
                x.CustomerAddress?.ToLower().Contains(keywordLower) == true).ToList();

            var result = listFilteredCustomer
                .Union(listFilteredWilayah)
                .Union(listFilteredKlasifikasi)
                .Union(listFilteredAddress);

            return result.ToList();
        }
    }


}
