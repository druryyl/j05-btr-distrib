﻿using btr.nuna.Domain;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using btr.distrib.Helpers;

namespace btr.distrib.SharedForm
{
    public partial class BrowserForm<T> : Form
    {
        private readonly IBrowseEngine<T> _engine;
        private readonly BindingSource _binder;
        public string Result = string.Empty;


        public BrowserForm(IBrowseEngine<T> engine)
        {
            InitializeComponent();
            _engine = engine;
            _binder = new BindingSource();

            BrowserGrid.DataBindingComplete += BrowserGrid_DataBindingComplete;
            BrowserGrid.CellDoubleClick += BrowserGrid_CellDoubleClick;
            BrowserGrid.KeyDown += BrowserGrid_KeyDown;
            FilterTextBox.KeyDown += FilterTextBox_KeyDown;
            SearchButton.Click += SearchButton_Click;
            this.KeyDown += BrowserForm_KeyDown;

            if (!_engine.Filter.IsDate)
                HideDateInput();

            _engine.Filter.RemoveNull();
            if (_engine.Filter.UserKeyword.Length > 0)
            {
                FilterTextBox.Text = _engine.Filter.UserKeyword;
            }

            RefreshGrid();
            BrowserGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            BrowserGrid.AutoResizeColumns();
        }

        private void BrowserForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
            private void BrowserGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.KeyCode == Keys.Enter)
            {
                if (grid.CurrentRow is null) return;
                ConfirmPilihan(grid.CurrentRow.Index);
            }
        }

        private void HideDateInput()
        {
            FilterDate1TextBox.Visible = false;
            FilterDate2TextBox.Visible = false;
            const int shiftUp = 30;
            panel1.Size = new Size(panel1.Size.Width, panel1.Size.Height - shiftUp);
            BrowserGrid.Location = new Point(BrowserGrid.Location.X, BrowserGrid.Location.Y - shiftUp);
            BrowserGrid.Size = new Size(BrowserGrid.Width, BrowserGrid.Height + shiftUp);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void FilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RefreshGrid();
                BrowserGrid.Focus();
            }
        }

        private void RefreshGrid()
        {
            _engine.Filter.UserKeyword = FilterTextBox.Text;
            _engine.Filter.Date = new Periode(FilterDate1TextBox.Value, FilterDate2TextBox.Value);

            if (_engine.Filter.HideAllRows)
                if (_engine.Filter.UserKeyword.Length == 0)
                {
                    _binder.DataSource = null;
                    BrowserGrid.DataSource = _binder;
                    return;
                }

            _binder.DataSource = _engine.GenDataSource();
            BrowserGrid.DataSource = _binder;
            BrowserGrid.Columns.SetDefaultCellStyle(Color.MintCream);
        }

        private void BrowserGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.SuspendLayout();
            int allColWidth = 0;
            int colIndex = 0;
            foreach (DataGridViewColumn col in BrowserGrid.Columns)
            {
                allColWidth += (int)col.FillWeight;
                colIndex++;
            }
            int allColWidth2 =
                BrowserGrid.Columns.Cast<DataGridViewColumn>()
                    .Sum(x => x.Width)
                    + (BrowserGrid.RowHeadersVisible ? BrowserGrid.RowHeadersWidth : 0);

            allColWidth2 += 20;
            var maxWidth = 1200;
            var minWidth = 300;
            var margin = 20;
            if (allColWidth2 <= maxWidth)
                this.Width = allColWidth2 + margin;

            if (allColWidth2 <= minWidth)
                this.Width = minWidth + margin;
            this.ResumeLayout();
        }

        private void BrowserGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            ConfirmPilihan(e.RowIndex);
        }

        private void ConfirmPilihan(int rowIndes)
        {
            if (rowIndes < 0)
                return;
            Result = BrowserGrid.Rows[rowIndes].Cells[0].Value.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}
