namespace btr.distrib.InventoryContext.PackingAgg
{
    partial class PackingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.KiriSplit = new System.Windows.Forms.SplitContainer();
            this.KananSplit = new System.Windows.Forms.SplitContainer();
            this.KiriBawahSplit = new System.Windows.Forms.SplitContainer();
            this.KananBawahSplit = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.FakturKiriGrid = new System.Windows.Forms.DataGridView();
            this.BrgKiriGrid = new System.Windows.Forms.DataGridView();
            this.FakturGridKanan = new System.Windows.Forms.DataGridView();
            this.BrgGridKiri = new System.Windows.Forms.DataGridView();
            this.PackingGrid = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel1.SuspendLayout();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KiriSplit)).BeginInit();
            this.KiriSplit.Panel1.SuspendLayout();
            this.KiriSplit.Panel2.SuspendLayout();
            this.KiriSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KananSplit)).BeginInit();
            this.KananSplit.Panel1.SuspendLayout();
            this.KananSplit.Panel2.SuspendLayout();
            this.KananSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KiriBawahSplit)).BeginInit();
            this.KiriBawahSplit.Panel1.SuspendLayout();
            this.KiriBawahSplit.Panel2.SuspendLayout();
            this.KiriBawahSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KananBawahSplit)).BeginInit();
            this.KananBawahSplit.Panel1.SuspendLayout();
            this.KananBawahSplit.Panel2.SuspendLayout();
            this.KananBawahSplit.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturKiriGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrgKiriGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGridKanan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGridKiri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PackingGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // MainSplit
            // 
            this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplit.Location = new System.Drawing.Point(0, 0);
            this.MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            this.MainSplit.Panel1.Controls.Add(this.KiriSplit);
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.Controls.Add(this.KananSplit);
            this.MainSplit.Size = new System.Drawing.Size(1017, 469);
            this.MainSplit.SplitterDistance = 484;
            this.MainSplit.TabIndex = 0;
            // 
            // KiriSplit
            // 
            this.KiriSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KiriSplit.Location = new System.Drawing.Point(0, 0);
            this.KiriSplit.Name = "KiriSplit";
            this.KiriSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // KiriSplit.Panel1
            // 
            this.KiriSplit.Panel1.Controls.Add(this.panel1);
            // 
            // KiriSplit.Panel2
            // 
            this.KiriSplit.Panel2.Controls.Add(this.KiriBawahSplit);
            this.KiriSplit.Size = new System.Drawing.Size(484, 469);
            this.KiriSplit.SplitterDistance = 70;
            this.KiriSplit.TabIndex = 0;
            // 
            // KananSplit
            // 
            this.KananSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KananSplit.Location = new System.Drawing.Point(0, 0);
            this.KananSplit.Name = "KananSplit";
            this.KananSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // KananSplit.Panel1
            // 
            this.KananSplit.Panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.KananSplit.Panel1.Controls.Add(this.button2);
            this.KananSplit.Panel1.Controls.Add(this.button1);
            this.KananSplit.Panel1.Controls.Add(this.PackingGrid);
            // 
            // KananSplit.Panel2
            // 
            this.KananSplit.Panel2.Controls.Add(this.KananBawahSplit);
            this.KananSplit.Size = new System.Drawing.Size(529, 469);
            this.KananSplit.SplitterDistance = 70;
            this.KananSplit.TabIndex = 1;
            // 
            // KiriBawahSplit
            // 
            this.KiriBawahSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KiriBawahSplit.Location = new System.Drawing.Point(0, 0);
            this.KiriBawahSplit.Name = "KiriBawahSplit";
            this.KiriBawahSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // KiriBawahSplit.Panel1
            // 
            this.KiriBawahSplit.Panel1.Controls.Add(this.FakturKiriGrid);
            // 
            // KiriBawahSplit.Panel2
            // 
            this.KiriBawahSplit.Panel2.Controls.Add(this.BrgKiriGrid);
            this.KiriBawahSplit.Size = new System.Drawing.Size(484, 395);
            this.KiriBawahSplit.SplitterDistance = 111;
            this.KiriBawahSplit.TabIndex = 1;
            // 
            // KananBawahSplit
            // 
            this.KananBawahSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KananBawahSplit.Location = new System.Drawing.Point(0, 0);
            this.KananBawahSplit.Name = "KananBawahSplit";
            this.KananBawahSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // KananBawahSplit.Panel1
            // 
            this.KananBawahSplit.Panel1.Controls.Add(this.FakturGridKanan);
            // 
            // KananBawahSplit.Panel2
            // 
            this.KananBawahSplit.Panel2.Controls.Add(this.BrgGridKiri);
            this.KananBawahSplit.Size = new System.Drawing.Size(529, 395);
            this.KananBawahSplit.SplitterDistance = 111;
            this.KananBawahSplit.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 70);
            this.panel1.TabIndex = 0;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "ddd, dd MMM yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(8, 8);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(116, 22);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "ddd, dd MMM yyyy";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(130, 8);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(116, 22);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 22);
            this.textBox1.TabIndex = 2;
            // 
            // FakturKiriGrid
            // 
            this.FakturKiriGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturKiriGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FakturKiriGrid.Location = new System.Drawing.Point(0, 0);
            this.FakturKiriGrid.Name = "FakturKiriGrid";
            this.FakturKiriGrid.Size = new System.Drawing.Size(484, 111);
            this.FakturKiriGrid.TabIndex = 0;
            // 
            // BrgKiriGrid
            // 
            this.BrgKiriGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgKiriGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrgKiriGrid.Location = new System.Drawing.Point(0, 0);
            this.BrgKiriGrid.Name = "BrgKiriGrid";
            this.BrgKiriGrid.Size = new System.Drawing.Size(484, 280);
            this.BrgKiriGrid.TabIndex = 1;
            // 
            // FakturGridKanan
            // 
            this.FakturGridKanan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FakturGridKanan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FakturGridKanan.Location = new System.Drawing.Point(0, 0);
            this.FakturGridKanan.Name = "FakturGridKanan";
            this.FakturGridKanan.Size = new System.Drawing.Size(529, 111);
            this.FakturGridKanan.TabIndex = 1;
            // 
            // BrgGridKiri
            // 
            this.BrgGridKiri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BrgGridKiri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrgGridKiri.Location = new System.Drawing.Point(0, 0);
            this.BrgGridKiri.Name = "BrgGridKiri";
            this.BrgGridKiri.Size = new System.Drawing.Size(529, 280);
            this.BrgGridKiri.TabIndex = 2;
            // 
            // PackingGrid
            // 
            this.PackingGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PackingGrid.Location = new System.Drawing.Point(3, 3);
            this.PackingGrid.Name = "PackingGrid";
            this.PackingGrid.Size = new System.Drawing.Size(400, 64);
            this.PackingGrid.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(409, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Preview Faktur";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(409, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Preview Barang";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // PackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(1017, 469);
            this.Controls.Add(this.MainSplit);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PackingForm";
            this.Text = "Packing List";
            this.MainSplit.Panel1.ResumeLayout(false);
            this.MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.KiriSplit.Panel1.ResumeLayout(false);
            this.KiriSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KiriSplit)).EndInit();
            this.KiriSplit.ResumeLayout(false);
            this.KananSplit.Panel1.ResumeLayout(false);
            this.KananSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KananSplit)).EndInit();
            this.KananSplit.ResumeLayout(false);
            this.KiriBawahSplit.Panel1.ResumeLayout(false);
            this.KiriBawahSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KiriBawahSplit)).EndInit();
            this.KiriBawahSplit.ResumeLayout(false);
            this.KananBawahSplit.Panel1.ResumeLayout(false);
            this.KananBawahSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KananBawahSplit)).EndInit();
            this.KananBawahSplit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FakturKiriGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrgKiriGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FakturGridKanan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrgGridKiri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PackingGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplit;
        private System.Windows.Forms.SplitContainer KiriSplit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.SplitContainer KiriBawahSplit;
        private System.Windows.Forms.SplitContainer KananSplit;
        private System.Windows.Forms.SplitContainer KananBawahSplit;
        private System.Windows.Forms.DataGridView FakturKiriGrid;
        private System.Windows.Forms.DataGridView BrgKiriGrid;
        private System.Windows.Forms.DataGridView PackingGrid;
        private System.Windows.Forms.DataGridView FakturGridKanan;
        private System.Windows.Forms.DataGridView BrgGridKiri;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}