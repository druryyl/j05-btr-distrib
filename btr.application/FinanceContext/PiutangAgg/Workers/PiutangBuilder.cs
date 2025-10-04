using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.FinanceContext.PiutangAgg.Workers
{
    public interface IPiutangBuilder : INunaBuilder<PiutangModel>
    {
        IPiutangBuilder Create(IFakturKey fakturKey);
        IPiutangBuilder Load(IPiutangKey piutangKey);
        IPiutangBuilder Attach(PiutangModel piutangModel);
        IPiutangBuilder Customer(ICustomerKey customerKey);
        IPiutangBuilder PiutangDate(DateTime dateTime);
        IPiutangBuilder DueDate(DateTime dateTime);
        IPiutangBuilder AddPlusElement(PiutangElementEnum elementTag, decimal value, DateTime elementDate);
        IPiutangBuilder AddMinusElement(PiutangElementEnum elementTag, decimal value, DateTime elementDate);
        IPiutangBuilder ClearElement();
        IPiutangBuilder AddLunasCash(decimal value, DateTime lunasDate, string tagihanId);
        IPiutangBuilder AddLunasBg(decimal value, DateTime lunasDate, string tagihanId, DateTime jatuhTempo, string namaBank, string noRek, string atasNama);
        IPiutangBuilder SetUangMuka(decimal value);
        IPiutangBuilder RemoveLunas(string tagihanId);
        IPiutangBuilder StatusPiutang(StatusPiutangEnum status);
        IPiutangBuilder NilaiPiutang(decimal nilai);
    }
    public class PiutangBuilder : IPiutangBuilder
    {
        private PiutangModel _aggregate = new PiutangModel();
        private readonly IPiutangDal _piutangDal;
        private readonly IPiutangElementDal _piutangElementDal;
        private readonly IPiutangLunasDal _piutangLunasDal;
        private readonly ICustomerDal _customerDal;

        public PiutangBuilder(IPiutangDal piutangDal,
            IPiutangElementDal piutangElementDal,
            IPiutangLunasDal piutangLunasDal,
            ICustomerDal customerDal)
        {
            _piutangDal = piutangDal;
            _piutangElementDal = piutangElementDal;
            _piutangLunasDal = piutangLunasDal;
            _customerDal = customerDal;
        }

        public PiutangModel Build()
        {
            _aggregate.RemoveNull();
            ReCalc();
            return _aggregate;
        }

        public IPiutangBuilder Create(IFakturKey fakturKey)
        {
            _aggregate = _piutangDal.GetData(new PiutangModel(fakturKey.FakturId));
            if (_aggregate != null)
                throw new ArgumentException("Piutang sudah tercatat. Create Piutang gagal");

            _aggregate = new PiutangModel
            {
                PiutangId = fakturKey.FakturId,
                ListElement = new List<PiutangElementModel>(),
                ListLunas = new List<PiutangLunasModel>()
            };
            return this;
        }


        public IPiutangBuilder Load(IPiutangKey piutangKey)
        {
            _aggregate = _piutangDal.GetData(piutangKey)
                ?? throw new KeyNotFoundException("Piutang not found");
            _aggregate.ListElement = _piutangElementDal.ListData(piutangKey)?.ToList()
                ?? new List<PiutangElementModel>();
            _aggregate.ListLunas = _piutangLunasDal.ListData(piutangKey)?.ToList()
                ?? new List<PiutangLunasModel>();
            return this;
        }

        public IPiutangBuilder Attach(PiutangModel piutangModel)
        {
            _aggregate = piutangModel;
            return this;
        }

        public IPiutangBuilder Customer(ICustomerKey customerKey)
        {
            var customer = _customerDal.GetData(customerKey)
                ?? throw new KeyNotFoundException("Customer not found");
            _aggregate.CustomerName = customer.CustomerName;
            _aggregate.CustomerId = customer.CustomerId;
            return this;
        }

        public IPiutangBuilder PiutangDate(DateTime dateTime)
        {
            _aggregate.PiutangDate = dateTime;
            return this;
        }

        public IPiutangBuilder DueDate(DateTime dateTime)
        {
            _aggregate.DueDate = dateTime;
            return this;
        }

        public IPiutangBuilder AddPlusElement(PiutangElementEnum elementTag, decimal value, DateTime elementDate)
        {
            if (value == 0)
                return this;

            var noUrut = _aggregate.ListElement
                .DefaultIfEmpty(new PiutangElementModel { NoUrut = 0 })
                .Max(x => x.NoUrut);
            noUrut++;
            var newElement = new PiutangElementModel
            {
                NoUrut = noUrut,
                ElementTag = elementTag,
                ElementName = elementTag.ToString(),
                NilaiPlus = value,
                ElementDate = elementDate,
            };
            _aggregate.ListElement.Add(newElement);
            ReCalc();
            return this;
        }

        public IPiutangBuilder AddMinusElement(PiutangElementEnum elementTag, decimal value, DateTime elementDate)
        {
            if (value == 0)
                return this;
            var noUrut = _aggregate.ListElement
                .DefaultIfEmpty(new PiutangElementModel { NoUrut = 0 })
                .Max(x => x.NoUrut);
            noUrut++;
            var newElement = new PiutangElementModel
            {
                NoUrut = noUrut,
                ElementTag = elementTag,
                ElementName = elementTag.ToString(),
                NilaiMinus = value,
                ElementDate = elementDate,
            };
            _aggregate.ListElement.Add(newElement);
            return this;
        }

        private void ReCalc()
        {
            var totalElement = _aggregate.ListElement.Sum(x => x.NilaiPlus - x.NilaiMinus);

            _aggregate.Potongan = totalElement;

            _aggregate.Terbayar = _aggregate.ListLunas.Sum(x => x.Nilai);
            _aggregate.Sisa = _aggregate.Total + _aggregate.Potongan - _aggregate.Terbayar;
        }

        public IPiutangBuilder AddLunasCash(decimal value, DateTime lunasDate, string tagihanId)
        {
            var lunas = new PiutangLunasModel
            {
                LunasDate = lunasDate,
                TagihanId = tagihanId,
                Nilai = value,
                JenisLunas = JenisLunasEnum.Cash,
                JatuhTempoBg = new DateTime(3000,1,1),
            };
            lunas.RemoveNull();
            AddLunas(lunas);
            return this;
        }

        private void AddLunas(PiutangLunasModel lunas)
        {
            var noUrut = _aggregate.ListLunas
                .DefaultIfEmpty(new PiutangLunasModel { NoUrut = 0 })
                .Max(x => x.NoUrut);
            noUrut++;
            lunas.NoUrut = noUrut;
            lunas.PelunasanId = $"{_aggregate.PiutangId}PL{noUrut:D2}";
            _aggregate.ListLunas.Add(lunas);
            _aggregate.Terbayar = _aggregate.ListLunas.Sum(x => x.Nilai);
            _aggregate.Sisa = _aggregate.Total - _aggregate.Terbayar;
        }

        public IPiutangBuilder AddLunasBg(decimal value, DateTime lunasDate, string tagihanId, DateTime jatuhTempo, string namaBank, string noRek, string atasNama)
        {
            if (value == 0)
                return this;
            var lunas = new PiutangLunasModel
            {
                LunasDate = lunasDate,
                TagihanId = tagihanId,
                Nilai = value,
                JenisLunas = JenisLunasEnum.CekBg,
                JatuhTempoBg = jatuhTempo,
                NamaBank = namaBank,
                NoRekBg= noRek,
                AtasNamaBank = atasNama,
            };
            lunas.RemoveNull();
            AddLunas(lunas);
            return this;
        }
        public IPiutangBuilder SetUangMuka(decimal value)
        {
            if (value == 0)
                return this;
            var lunas = new PiutangLunasModel
            {
                LunasDate = _aggregate.PiutangDate,
                TagihanId = _aggregate.PiutangId,
                Nilai = value,
                JenisLunas = JenisLunasEnum.UangMuka,
                JatuhTempoBg = new DateTime(3000,1,1),
            };
            lunas.RemoveNull();
            _aggregate.ListLunas.RemoveAll(x => x.JenisLunas == JenisLunasEnum.UangMuka);
            AddLunas(lunas);
            return this;
        }

        public IPiutangBuilder RemoveLunas(string tagihanId)
        {
            _aggregate.ListLunas.RemoveAll(x => x.TagihanId== tagihanId);
            ReCalc();
            return this;
        }

        public IPiutangBuilder ClearElement()
        {
            _aggregate.ListElement.Where(x => x.NoUrut > 1).ToList().ForEach(x => _aggregate.ListElement.Remove(x));
            ReCalc();
            return this;
        }

        public IPiutangBuilder NilaiPiutang(decimal nilai)
        {
            _aggregate.Total = nilai;
            ReCalc();
            return this;
        }

        public IPiutangBuilder StatusPiutang(StatusPiutangEnum status)
        {
            _aggregate.StatusPiutang = status;
            return this;
        }
    }
}