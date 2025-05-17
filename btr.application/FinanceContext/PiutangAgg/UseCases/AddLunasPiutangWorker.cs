using System;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;

namespace btr.application.FinanceContext.PiutangAgg.UseCases
{
    public class AddLunasPiutangRequest : IPiutangKey
    {
        public string PiutangId { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal Materai { get; set; }
        public decimal Admin { get; set; }
        
        public decimal Nilai { get; set; }
        public DateTime LunasDate { get; set; }
        public string TagihanId { get; set; }
        public JenisLunasEnum JenisLunas { get; set; }
        public DateTime JatuhTempoBg { get; set; }
        public string NoBgRek { get; set; }
        public string NamaBank { get; set; }
        public string AtasNamaBank { get; set; }

    }
    
    public interface IAddLunasPiutangWorker : INunaServiceVoid<AddLunasPiutangRequest>
    {
    }
    
    public class AddLunasPiutangWorker : IAddLunasPiutangWorker
    {
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
        private readonly ITglJamDal _dateTime;

        public AddLunasPiutangWorker(IPiutangBuilder builder,
            IPiutangWriter piutangWriter,
            ITglJamDal dateTime)
        {
            _piutangBuilder = builder;
            _piutangWriter = piutangWriter;
            _dateTime = dateTime;
        }

        public void Execute(AddLunasPiutangRequest req)
        {
            var piutang = _piutangBuilder
                .Load(new PiutangModel(req.PiutangId))
                .Build();
            piutang.ListElement.RemoveAll(x => x.NoUrut > 1);
            piutang = _piutangBuilder
                .Attach(piutang)
                .AddMinusElement(PiutangElementEnum.Retur, req.Retur, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Potongan, req.Potongan, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Materai, req.Materai, _dateTime.Now)
                .AddMinusElement(PiutangElementEnum.Admin, req.Admin, _dateTime.Now)
                .Build();

            if (req.Nilai != 0)
                if (req.JenisLunas == JenisLunasEnum.Cash)
                    piutang = _piutangBuilder
                        .Attach(piutang)
                        .AddLunasCash(req.Nilai, req.LunasDate, req.TagihanId)
                        .Build();
                else
                    piutang = _piutangBuilder
                        .Attach(piutang)
                        .AddLunasBg(req.Nilai, req.LunasDate, req.TagihanId, req.JatuhTempoBg, req.NamaBank, req.NoBgRek, req.AtasNamaBank)
                        .Build();

            _piutangWriter.Save(ref piutang);
        }
    }
}