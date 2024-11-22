using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.UseCases
{
    public interface ICreatePiutangWorker : INunaServiceVoid<IFakturKey>
    {
    }
    public class CreatePiutangWorker : ICreatePiutangWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IPiutangBuilder _piutangBuilder;
        private readonly IPiutangWriter _piutangWriter;
        private readonly ICancelPiutangWorker _cancelPiutangWorker;

        public CreatePiutangWorker(IFakturBuilder fakturBuilder,
            IPiutangBuilder piutangBuilder,
            IPiutangWriter piutangWriter,
            ICancelPiutangWorker cancelPiutangWorker)
        {
            _fakturBuilder = fakturBuilder;
            _piutangBuilder = piutangBuilder;
            _piutangWriter = piutangWriter;
            _cancelPiutangWorker = cancelPiutangWorker;
        }

        public void Execute(IFakturKey req)
        {
            _cancelPiutangWorker.Execute(new PiutangModel(req.FakturId));

            var faktur = _fakturBuilder.Load(req).Build();
            var piutang = _piutangBuilder
                .Create(req)
                .Customer(faktur)
                .PiutangDate(faktur.FakturDate)
                .DueDate(faktur.DueDate)
                .NilaiPiutang(faktur.GrandTotal)
                .Build();

            _piutangWriter.Save(ref piutang);
        }
    }
}
