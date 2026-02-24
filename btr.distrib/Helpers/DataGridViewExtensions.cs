using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace btr.distrib.Helpers
{
    public static class DataGridViewExtensions
    {
        public static DataGridViewColumn GetCol(this DataGridViewColumnCollection cols, string caption)
        {
            foreach (DataGridViewColumn item in cols)
                if (item.Name == caption)
                    return item;
            throw new KeyNotFoundException($"Column not found ({caption})");
        }

        public static void SetDefaultCellStyle(this DataGridViewColumnCollection cols,
            Color readOnlyColor)
        {
            foreach (DataGridViewColumn col in cols)
            {
                col.DefaultCellStyle.Font = new Font("Lucida Console", 8.25f);
                if (col.ReadOnly)
                    col.DefaultCellStyle.BackColor = readOnlyColor;


                if (col.ValueType.Name == "String")
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }

                if (col.ValueType.Name == "Int32")
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    col.DefaultCellStyle.Format = "N0";
                }

                if (col.ValueType.Name == "Decimal")
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    col.DefaultCellStyle.Format = "#,##";
                }
                if (col.ValueType.Name == "Double")
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    col.DefaultCellStyle.Format = "#,##0.00";
                }
                Debug.Print(col.ValueType.Name);    
            }
        }

        public static void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, new Font("Lucida Console", 8.25f), SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        public static void SetAlternatingRowColors(
    this DataGridView dataGridView,
    Color? evenRowColor = null,
    Color? oddRowColor = null,
    Color? textColor = null,
    Color? selectionBackColor = null,
    Color? selectionTextColor = null)
        {
            // Set default colors if not provided
            Color evenColor = evenRowColor ?? Color.White;
            Color oddColor = oddRowColor ?? Color.LightGray;
            Color text = textColor ?? Color.Black;
            Color selBackColor = selectionBackColor ?? Color.LightBlue;
            Color selTextColor = selectionTextColor ?? Color.Black;

            // Configure even rows (default style)
            dataGridView.RowsDefaultCellStyle.BackColor = evenColor;
            dataGridView.RowsDefaultCellStyle.ForeColor = text;
            dataGridView.RowsDefaultCellStyle.SelectionBackColor = selBackColor;
            dataGridView.RowsDefaultCellStyle.SelectionForeColor = selTextColor;

            // Configure odd rows (alternating style)
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = oddColor;
            dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = text;
            dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor = selBackColor;
            dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = selTextColor;
        }

        /// <summary>
        /// Sets alternating row colors with custom colors for each state
        /// </summary>
        public static void SetAlternatingRowColors(
            this DataGridView dataGridView,
            Color evenRowColor,
            Color oddRowColor,
            Color evenRowSelectionColor,
            Color oddRowSelectionColor,
            Color textColor = default,
            Color selectionTextColor = default)
        {
            // Use default colors if not specified
            if (textColor == default) textColor = Color.Black;
            if (selectionTextColor == default) selectionTextColor = Color.Black;

            // Even rows
            dataGridView.RowsDefaultCellStyle.BackColor = evenRowColor;
            dataGridView.RowsDefaultCellStyle.ForeColor = textColor;
            dataGridView.RowsDefaultCellStyle.SelectionBackColor = evenRowSelectionColor;
            dataGridView.RowsDefaultCellStyle.SelectionForeColor = selectionTextColor;

            // Odd rows
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = oddRowColor;
            dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = textColor;
            dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor = oddRowSelectionColor;
            dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = selectionTextColor;
        }

        /// <summary>
        /// Resets alternating row colors to default system colors
        /// </summary>
        public static void ResetAlternatingRowColors(this DataGridView dataGridView)
        {
            dataGridView.RowsDefaultCellStyle = new DataGridViewCellStyle();
            dataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle();
        }
    }
}