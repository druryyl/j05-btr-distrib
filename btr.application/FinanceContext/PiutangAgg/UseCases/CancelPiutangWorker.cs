using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Application;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.UseCases
{
    public interface ICancelPiutangWorker : INunaServiceVoid<IPiutangKey>
    {
    }

    public class CancelPiutangWorker : ICancelPiutangWorker
    {
        private readonly IPiutangBuilder _builder;
        private readonly IPiutangWriter _writer;

        public CancelPiutangWorker(IPiutangBuilder builder, 
            IPiutangWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public void Execute(IPiutangKey req)
        {
            var fallback = Policy<PiutangModel>
                .Handle<KeyNotFoundException>()
                .Fallback(() => null);
            var piutang = fallback.Execute(() => _builder.Load(req).Build());
            if (piutang is null)
                return;

            if (piutang.Terbayar > 0)
                throw new ArgumentException("Piutang sudah ada pelunasan. Cancel Piutang gagal");
            _writer.Delete(piutang);
        }
    }
}
