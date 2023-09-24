using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using btr.application.SalesContext.FakturPajakVoidAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.SalesContext.FakturPajakVoidAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.AlokasiFpAgg
{
    public interface IAlokasiFpBuilder : INunaBuilder<AlokasiFpModel>
    {
        IAlokasiFpBuilder Create();
        IAlokasiFpBuilder Load(IAlokasiFpKey nsfpKey);
        IAlokasiFpBuilder Attach(AlokasiFpModel nsfp);
        IAlokasiFpBuilder NomorSeri(string noAwal, string noAkhir);
        IAlokasiFpBuilder SetFaktur<T>(string nomorSeri, T faktur)
            where T: IFakturKey, IFakturCode;
        IAlokasiFpBuilder SetFaktur<T>(T faktur)
            where T : IFakturKey, IFakturCode;
        IAlokasiFpBuilder UnSetFaktur(IFakturKey faktur);

        IAlokasiFpBuilder ClearListNomor();

    }
    public class AlokasiFpBuilder : IAlokasiFpBuilder
    {
        private AlokasiFpModel _aggregate;
        private readonly IAlokasiFpDal _alokasiFpDal;
        private readonly IAlokasiFpItemDal _alokasiFpItemDal;
        private readonly ITglJamDal _dateTime;
        private readonly IFakturPajakVoidDal _fakturPajakVoiDal;

        public AlokasiFpBuilder(IAlokasiFpDal alokasiFpDal,
            IAlokasiFpItemDal alokasiFpItemDal,
            ITglJamDal dateTime,
            IFakturPajakVoidDal fakturPajakVoiDal)
        {
            _alokasiFpDal = alokasiFpDal;
            _alokasiFpItemDal = alokasiFpItemDal;
            _dateTime = dateTime;
            _fakturPajakVoiDal = fakturPajakVoiDal;
        }

        public IAlokasiFpBuilder Create()
        {
            _aggregate = new AlokasiFpModel();
            _aggregate.AlokasiFpDate = _dateTime.Now;
            _aggregate.ListItem = new List<AlokasiFpItemModel>();
            return this;
        }

        public IAlokasiFpBuilder Load(IAlokasiFpKey nsfpKey)
        {
            _aggregate = _alokasiFpDal.GetData(nsfpKey)
                ?? throw new KeyNotFoundException("Nomor Seri Faktur Pajak not found");
            _aggregate.ListItem = _alokasiFpItemDal.ListData(nsfpKey)?.ToList()
                ?? new List<AlokasiFpItemModel>();
            return this;
        }

        public IAlokasiFpBuilder NomorSeri(string noAwal, string noAkhir)
        {
            const string pattern = @"(^\d{3}\.\d{3}-\d{2}\.)(\d{8})$";
            //  sample string  "010.000-10.23456789";

            var match1 = Regex.Match(noAwal, pattern);
            if (!match1.Success)
                throw new ArgumentException("Format Nomor Seri Awal invalid");

            var match2 = Regex.Match(noAkhir, pattern);
            if (!match2.Success)
                throw new ArgumentException("Format Nomor Seri Akhir invalid");
            
            var prefix1 = match1.Groups[1].Value;
            var prefix2 = match2.Groups[1].Value;
            if (prefix1 != prefix2)
                throw new ArgumentException("Awalan Nomor Seri Pajak tidak sama");


            var noAwalN = long.Parse(match1.Groups[2].Value);
            var noAkhirN = long.Parse(match2.Groups[2].Value);
            if (noAkhirN - noAwalN > 50000)
                throw new ArgumentException("Alokasi Nomor Seri FP terlalu besar. Maximum 50.000");

            _aggregate.NoAwal = noAwal;
            _aggregate.NoAkhir = noAkhir;

            var listItem = new List<AlokasiFpItemModel>();
            for(long i = noAwalN; i <= noAkhirN; i++)
            {
                var newItem = new AlokasiFpItemModel
                {
                    NoUrut = i,
                    NoFakturPajak = $"{prefix1}{i:D8}"
                };
                newItem.RemoveNull();
                listItem.Add(newItem);
            }

            //      jika sudah pernah di-void, maka hapus dari list
            var listVoid = _fakturPajakVoiDal.ListData(new Periode(_dateTime.Now.AddYears(-1), _dateTime.Now))
                ?.ToList() ?? new List<FakturPajakVoidModel>();
            var listVoidCopy = listVoid.ToList();
            foreach(var item in listVoidCopy)
                listItem.RemoveAll(x => x.NoFakturPajak == item.NoFakturPajak);

            //      selesai; assign to aggregate
            _aggregate.ListItem = listItem;
            _aggregate.Sisa = _aggregate.ListItem.Count(x => x.FakturId.Length == 0);
            _aggregate.Kapasitas = _aggregate.ListItem.Count();
            return this;
        }

        public AlokasiFpModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IAlokasiFpBuilder Attach(AlokasiFpModel nsfp)
        {
            _aggregate = nsfp;
            return this;
        }

        public IAlokasiFpBuilder SetFaktur<T>(string nomorSeri, T faktur) where T : IFakturKey, IFakturCode
        {
            var item = _aggregate.ListItem.FirstOrDefault(x => x.NoFakturPajak == nomorSeri)
                ?? throw new ArgumentException($"Nomor Seri {nomorSeri} not found");
            item.FakturId = faktur.FakturId;
            item.FakturCode = faktur.FakturCode;
            _aggregate.Sisa = _aggregate.ListItem.Count(x => x.FakturId.Length == 0);
            return this;
        }
        
        public IAlokasiFpBuilder SetFaktur<T>(T faktur) where T : IFakturKey, IFakturCode
        {
            var available = _aggregate.ListItem
                .OrderBy(x => x.NoFakturPajak)
                .FirstOrDefault(x => x.FakturId.Length == 0)
                ?? throw new ArgumentException("Alokasi Faktur Pajak sudah terpakai semua");

            available.FakturId = faktur.FakturId;
            available.FakturCode = faktur.FakturCode;
            _aggregate.Sisa = _aggregate.ListItem.Count(x => x.FakturId.Length == 0);
            return this;
        }

        public IAlokasiFpBuilder UnSetFaktur(IFakturKey faktur)
        {
            var available = _aggregate.ListItem
                .OrderBy(x => x.NoFakturPajak)
                .FirstOrDefault(x => x.FakturId == faktur.FakturId)
                ?? throw new ArgumentException("Faktur tidak ditemukan dalam alokasi");

            available.FakturId = string.Empty;
            available.FakturCode = string.Empty;
            _aggregate.Sisa = _aggregate.ListItem.Count(x => x.FakturId.Length == 0);
            return this;
        }

        public IAlokasiFpBuilder ClearListNomor()
        {
            _aggregate.ListItem.Clear();
            _aggregate.Sisa = _aggregate.Kapasitas;
            return this;
        }
    }
}