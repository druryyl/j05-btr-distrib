using btr.domain.SalesContext.FakturPajak;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace btr.application.SalesContext.NomorFpAgg
{
    public interface INomorSeriFpBuilder : INunaBuilder<NomorSeriFpModel>
    {
        INomorSeriFpBuilder Create();
        INomorSeriFpBuilder Load(INomorSeriFpKey nsfpKey);
        INomorSeriFpBuilder NomorSeri(string noAwal, string noAkhir);

    }
    public class NomorSeriFpBuilder : INomorSeriFpBuilder
    {
        private NomorSeriFpModel _aggregate;
        private readonly INomorSeriFpDal _nomorSeriFpDal;
        private readonly INomorSeriFpItemDal _nomorSeriFpItemDal;

        public INomorSeriFpBuilder Create()
        {
            _aggregate = new NomorSeriFpModel();
            _aggregate.ListItem = new List<NomorSeriFpItemModel>();
        }

        public INomorSeriFpBuilder Load(INomorSeriFpKey nsfpKey)
        {
            _aggregate = _nomorSeriFpDal.GetData(nsfpKey)
                ?? throw new KeyNotFoundException("Nomor Seri Faktur Pajak not found");
            _aggregate.ListItem = _nomorSeriFpItemDal.ListData(nsfpKey)?.ToList()
                ?? new List<NomorSeriFpItemModel>();
            return this;
        }

        public INomorSeriFpBuilder NomorSeri(string noAwal, string noAkhir)
        {
            const string pattern = @"^\d{3}\.\d{3}-\d{10}";
            
            var matched = Regex.Match(noAwal, pattern);
            if (!matched.Success)
                throw new ArgumentException("Format Nomor Seri Awal invalid");

            matched = Regex.Match(noAkhir, pattern);
            if (!matched.Success)
                throw new ArgumentException("Format Nomor Seri Akhir invalid");
            
            long noAwalN = long.Parse(matched.Groups[1].Value);
            _aggregate.NoAwal = noAwal;
            _aggregate.NoAkhir = noAkhir;



        }

        public NomorSeriFpModel Build()
        {
            throw new NotImplementedException();
        }
    }
}
