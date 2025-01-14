using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranWriter : INunaWriter2<FpKeluaranModel>
    {
    }

    public class FpKeluaranWriter : IFpKeluaranWriter
    {
        private readonly IFpKeluaranDal _fpKeluaranDal;
        private readonly IFpKeluaranFakturDal _fpKeluaranFakturDal;
        private readonly IFpKeluaranBrgDal _fpKeluaranBrgDal;
        private readonly INunaCounterBL _counter;

        public FpKeluaranWriter(IFpKeluaranDal fpKeluaranDal,
            IFpKeluaranFakturDal fpKeluaranFakturDal,
            IFpKeluaranBrgDal fpKeluaranBrgDal,
            INunaCounterBL counter)
        {
            _fpKeluaranDal = fpKeluaranDal;
            _fpKeluaranFakturDal = fpKeluaranFakturDal;
            _fpKeluaranBrgDal = fpKeluaranBrgDal;
            _counter = counter;
        }

        public FpKeluaranModel Save(FpKeluaranModel model)
        {
            if (model.FpKeluaranId == string.Empty)
                model.FpKeluaranId = _counter.Generate("FPKL", IDFormatEnum.PREFYYMnnnnnC);

            int baris = 0;
            foreach(var faktur in model.ListFaktur)
            {
                baris++;
                faktur.Baris = baris;
                faktur.FpKeluaranFakturId = $"{model.FpKeluaranId}-{faktur.Baris:D3}";
                faktur.FpKeluaranId = model.FpKeluaranId;

                var noUrutBrg = 0;
                foreach (var brg in faktur.ListBrg)
                {
                    noUrutBrg++;
                    brg.Baris = baris;
                    brg.FpKeluaranFakturId = faktur.FpKeluaranFakturId;
                    brg.FpKeluaranId = model.FpKeluaranId;
                    brg.FpKeluaranBrgId = $"{faktur.FpKeluaranFakturId}-{noUrutBrg:D3}";
                }
            }

            using (var trans = TransHelper.NewScope())
            {
                var fp = _fpKeluaranDal.GetData(model);
                if (fp is null)
                    _fpKeluaranDal.Insert(model);
                else
                    _fpKeluaranDal.Update(model);

                _fpKeluaranFakturDal.Delete(model);
                _fpKeluaranFakturDal.Insert(model.ListFaktur);

                var listBrg = model.ListFaktur.SelectMany(x => x.ListBrg).ToList();
                _fpKeluaranBrgDal.Delete(model);
                _fpKeluaranBrgDal.Insert(listBrg);

                trans.Complete();
            }
            return model;
        }
    }
}
