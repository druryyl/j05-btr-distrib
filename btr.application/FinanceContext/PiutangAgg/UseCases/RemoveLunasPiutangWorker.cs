using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Application;

namespace btr.application.FinanceContext.PiutangAgg.UseCases
{
    //public class RemoveLunasPiutangRequest
    //{
    //    public string PiutangId { get; set; }
    //    public int NoUrut { get; set; }
    //}
    //public interface IRemoveLunasPiutangWorker : INunaServiceVoid<RemoveLunasPiutangRequest>
    //{
    //}
    //public class RemoveLunasPiutangWorker : IRemoveLunasPiutangWorker
    //{
    //    private readonly IPiutangBuilder _piutangBuilder;
    //    private readonly IPiutangWriter _piutangWriter;

    //    public RemoveLunasPiutangWorker(IPiutangBuilder piutangBuilder, IPiutangWriter piutangWriter)
    //    {
    //        _piutangBuilder = piutangBuilder;
    //        _piutangWriter = piutangWriter;
    //    }

    //    public void Execute(RemoveLunasPiutangRequest req)
    //    {
    //        var piutang = _piutangBuilder
    //            .Load(new PiutangModel(req.PiutangId))
    //            .RemoveLunas(req.NoUrut)
    //            .Build();
    //        _piutangWriter.Save(ref piutang);
            
    //    }
    //}
}