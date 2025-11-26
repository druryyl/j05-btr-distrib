using btr.application.SalesContext.CheckInFeature;
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
using Syncfusion.Drawing;
using Syncfusion.Grouping;


namespace btr.distrib.SalesContext.LocationFeature
{
    public partial class EffectiveCallInfoForm : Form
    {
        private readonly IEffectiveCallDal _effectiveCallDal;
        private List<EffectiveCallView> _dataSource;

        public EffectiveCallInfoForm(IEffectiveCallDal effectiveCallDal)
        {
            InitializeComponent();
            _effectiveCallDal = effectiveCallDal;
            InfoGrid.QueryCellStyleInfo += InfoGrid_QueryCellStyleInfo;
            ProsesButton.Click += ProsesButton_Click;
            ExcelButton.Click += ExcelButton_Click;
            InitGrid();
            _dataSource = new List<EffectiveCallView>();
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
                saveFileDialog.FileName = $"effective-call-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            var filtered = this.InfoGrid.Table.FilteredRecords;
            var listToExcel = new List<EffectiveCallView>();
            foreach (var item in filtered)
            {
                listToExcel.Add(item.GetData() as EffectiveCallView);
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("effective-call");

                // Create header row
                ws.Cell("A1").Value = "No";
                ws.Cell("B1").Value = "CheckIn Date";
                ws.Cell("C1").Value = "User Email";
                ws.Cell("D1").Value = "Customer Code";
                ws.Cell("E1").Value = "Customer Name";
                ws.Cell("F1").Value = "Order Count";
                ws.Cell("G1").Value = "CheckIn ID";

                // Fill data rows
                for (var i = 0; i < listToExcel.Count; i++)
                {
                    var effectiveCall = listToExcel[i];
                    var row = i + 2;

                    ws.Cell($"A{row}").Value = i + 1;
                    ws.Cell($"B{row}").Value = effectiveCall.CheckInDate;
                    ws.Cell($"C{row}").Value = effectiveCall.UserEmail;
                    ws.Cell($"D{row}").Value = effectiveCall.CustomerCode;
                    ws.Cell($"E{row}").Value = effectiveCall.CustomerName;
                    ws.Cell($"F{row}").Value = effectiveCall.OrderCount;
                    ws.Cell($"G{row}").Value = effectiveCall.CheckInId;

                    // Apply color coding based on order count
                    if (effectiveCall.OrderCount > 0)
                    {
                        var rowRange = ws.Range(ws.Cell($"A{row}"), ws.Cell($"G{row}"));
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    }
                }

                // Apply styling to the entire data range
                var dataRange = ws.Range(ws.Cell($"A1"), ws.Cell($"G{listToExcel.Count + 1}"));
                dataRange.Style
                    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                    .Font.SetFontName("Lucida Console")
                    .Font.SetFontSize(9);

                // Format header row
                var headerRange = ws.Range(ws.Cell($"A1"), ws.Cell($"G1"));
                headerRange.Style
                    .Font.Bold = true;

                // Format order count column
                var orderCountRange = ws.Range(ws.Cell($"F2"), ws.Cell($"F{listToExcel.Count + 1}"));
                orderCountRange.Style.NumberFormat.Format = "0";

                // Auto-fit all columns
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
        }

        private void InitGrid()
        {
            InfoGrid.DataSource = new List<EffectiveCallView>();

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
            InfoGrid.TableDescriptor.Columns["OrderCount"].Appearance.AnyRecordFieldCell.Format = "0";
            InfoGrid.TableDescriptor.Columns["OrderCount"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;

            // Set column widths for better readability
            InfoGrid.TableDescriptor.Columns["CheckInDate"].Width = 100;
            InfoGrid.TableDescriptor.Columns["UserEmail"].Width = 180;
            InfoGrid.TableDescriptor.Columns["CustomerCode"].Width = 80;
            InfoGrid.TableDescriptor.Columns["CustomerName"].Width = 200;
            InfoGrid.TableDescriptor.Columns["OrderCount"].Width = 120;
            InfoGrid.TableDescriptor.Columns["CheckInId"].Width = 150;

            // Hide CheckInId and CustomerId columns (optional)
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CheckInId");
            InfoGrid.TableDescriptor.VisibleColumns.Remove("CustomerId");

            // Summary rows - OPTION 1: Two separate summary rows
            var checkInCount = new GridSummaryColumnDescriptor("CheckIn", SummaryType.Count, "OrderCount", "CheckIn:{Count}");
            checkInCount.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightYellow);
            checkInCount.Appearance.AnySummaryCell.Format = "0";
            checkInCount.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Left;

            var orderCount = new GridSummaryColumnDescriptor("Order", SummaryType.Int32Aggregate, "IsEffectiveCall", "Order: {Sum}");
            orderCount.Appearance.AnySummaryCell.Interior = new BrushInfo(Color.LightGreen);
            orderCount.Appearance.AnySummaryCell.HorizontalAlignment = GridHorizontalAlignment.Left;

            var sumRowDescriptor1 = new GridSummaryRowDescriptor();
            sumRowDescriptor1.SummaryColumns.Add(checkInCount);
            sumRowDescriptor1.SummaryColumns.Add(orderCount);
            InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor1);

            //var sumRowDescriptor2 = new GridSummaryRowDescriptor();
            //sumRowDescriptor2.SummaryColumns.Add(orderCount);
            //InfoGrid.TableDescriptor.SummaryRows.Add(sumRowDescriptor2);

            // For all records field cells
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.AutoSize = true;
            InfoGrid.TableDescriptor.Appearance.AnyRecordFieldCell.WrapText = false;

            // Set text alignment
            InfoGrid.TableDescriptor.Columns["CheckInDate"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;
            InfoGrid.TableDescriptor.Columns["CustomerCode"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;
            InfoGrid.TableDescriptor.Columns["OrderCount"].Appearance.AnyRecordFieldCell.HorizontalAlignment = GridHorizontalAlignment.Center;

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

            var listEffectiveCall = _effectiveCallDal.ListData(periode)?.ToList() ?? new List<EffectiveCallView>();
            _dataSource = Filter(listEffectiveCall, SearchText.Text);
            InfoGrid.DataSource = _dataSource;
        }

        private List<EffectiveCallView> Filter(List<EffectiveCallView> source, string keyword)
        {
            if (keyword.Trim().Length == 0)
                return source;

            var keywordLower = keyword.ToLower();
            var listFilteredCustomer = source.Where(x =>
                x.CustomerName.ToLower().ContainMultiWord(keywordLower) ||
                x.CustomerCode.ToLower().Contains(keywordLower)).ToList();

            var listFilteredUser = source.Where(x =>
                x.UserEmail.ToLower().Contains(keywordLower)).ToList();

            var listFilteredCheckIn = source.Where(x =>
                x.CheckInId.ToLower().Contains(keywordLower)).ToList();

            var result = listFilteredCustomer
                .Union(listFilteredUser)
                .Union(listFilteredCheckIn);

            return result.ToList();
        }
    }


}
