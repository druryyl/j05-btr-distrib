﻿using btr.infrastructure.SalesContext.FakturPajakVoidAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturPajakVoidAgg
{
    public interface IFakturPajakVoidWriter : INunaWriter2<FakturPajakVoidModel>
    {
    }

    public class FaktuPajakVoidWriter : IFakturPajakVoidWriter
    {
        private readonly IFakturPajakVoidDal _fakturPajakVoidDal;

        public FaktuPajakVoidWriter(IFakturPajakVoidDal fakturPajakVoidDal)
        {
            _fakturPajakVoidDal = fakturPajakVoidDal;
        }

        public FakturPajakVoidModel Save(FakturPajakVoidModel model)
        {
            var db = _fakturPajakVoidDal.GetData(model);
            if (db is null)
                _fakturPajakVoidDal.Insert(model);
            else
                _fakturPajakVoidDal.Update(model);

            return model;
        }
    }
}