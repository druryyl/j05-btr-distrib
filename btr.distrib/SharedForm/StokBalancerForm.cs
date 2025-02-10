using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.nuna.Domain;
using Polly;

namespace btr.distrib.SharedForm
{
    public partial class StokBalancerForm : Form
    {
        private readonly IFakturDal _fakturDal;
        private readonly IGenStokFakturRegenWorker _genStokFaktur;

        public StokBalancerForm(IFakturDal fakturDal, 
            IGenStokFakturRegenWorker genStokFaktur)
        {
            _fakturDal = fakturDal;
            _genStokFaktur = genStokFaktur;
            InitializeComponent();
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (textBox1.Text != @"jude")
                return;

            GenStokFaktur();
        }

        private void GenStokFaktur()
        {
            label1.Text = @"Proses Re-generate stok...";
            var listFaktur = _fakturDal.ListData(new Periode(new DateTime(2023,10,1), new DateTime(2024,3,31)));
            
            var fallback = Policy<bool>
                .Handle<KeyNotFoundException>()
                .Or<ArgumentException>()
                .Fallback(() => false);
            foreach (var faktur in listFaktur.OrderBy(x => x.FakturId))
            {
                var isSuccess = fallback.Execute(() =>
                {
                    _genStokFaktur.Execute(new GenStokFakturRequest(faktur.FakturId));
                    return true;
                });
                var text = isSuccess
                    ? $@"Processing {faktur.FakturDate:yyyy-MM-dd} - {faktur.FakturId}"
                    : $@"Failed {faktur.FakturDate:yyyy-MM-dd} - {faktur.FakturId}";

                label1.Text = text;
                Debug.WriteLine(text);

                this.Invalidate();
            }

            label1.Text = @"Proses Re-generate stok... done";
        }

    }
}