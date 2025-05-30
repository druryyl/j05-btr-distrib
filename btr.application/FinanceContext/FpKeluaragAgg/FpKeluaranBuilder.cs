﻿using btr.application.SupportContext.TglJamAgg;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FpKeluaragAgg
{
    public interface IFpKeluaranBuilder : INunaBuilder<FpKeluaranModel>
    {
        IFpKeluaranBuilder Load(IFpKeluaranKey fpKeluaranKey);
        IFpKeluaranBuilder Create();
        IFpKeluaranBuilder Attach(FpKeluaranModel fpKeluaran);
        IFpKeluaranBuilder UserDate(DateTime tanggal);
        IFpKeluaranBuilder User(IUserKey userKey);

    }
    public class FpKeluaranBuilder : IFpKeluaranBuilder
    {
        private FpKeluaranModel _agg;
        private readonly IFpKeluaranDal _fpKeluaranDal;
        private readonly IFpKeluaranFakturDal _fpKeluaranFakturDal;
        private readonly IFpKeluaranBrgDal _fpKeluaranBrgDal;
        private readonly ITglJamDal _dateTime;

        public FpKeluaranBuilder(IFpKeluaranDal fpKeluaranDal,
            IFpKeluaranFakturDal fpKeluranFakturDal,
            IFpKeluaranBrgDal fpKeluaranBrgDal,
            ITglJamDal dateTime)
        {
            _fpKeluaranDal = fpKeluaranDal;
            _fpKeluaranFakturDal = fpKeluranFakturDal;
            _fpKeluaranBrgDal = fpKeluaranBrgDal;
            _agg = new FpKeluaranModel();
            _dateTime = dateTime;
        }


        public IFpKeluaranBuilder Load(IFpKeluaranKey fpKeluaranKey)
        {
            _agg = _fpKeluaranDal.GetData(fpKeluaranKey)
                ?? throw new KeyNotFoundException("Faktur Pajak Keluaran not found");
            _agg.ListFaktur = _fpKeluaranFakturDal.ListData(_agg)?.ToList()
                ?? new List<FpKeluaranFakturModel>();
            var listBrg = _fpKeluaranBrgDal.ListData(_agg)?.ToList() ?? new List<FpKeluaranBrgModel>();
            foreach(var faktur in _agg.ListFaktur)
            {
                faktur.ListBrg = listBrg.Where(x => x.FpKeluaranFakturId == faktur.FpKeluaranFakturId).ToList();
            }
            return this;
        }
        public IFpKeluaranBuilder Create()
        {
            _agg = new FpKeluaranModel
            {
                ListFaktur = new List<FpKeluaranFakturModel>(),
                FpKeluaranDate = _dateTime.Now
            };
            return this;
        }

        public FpKeluaranModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IFpKeluaranBuilder Attach(FpKeluaranModel fpKeluaran)
        {
            _agg = fpKeluaran;
            return this;
        }

        public IFpKeluaranBuilder UserDate(DateTime tanggal)
        {
            _agg.UserDate = tanggal;
            return this;
        }

        public IFpKeluaranBuilder User(IUserKey userKey)
        {
            _agg.UserId = userKey.UserId;
            return this;
        }

    }
}
