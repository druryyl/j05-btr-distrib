using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Application;

namespace btr.application.FinanceContext.PiutangAgg.Workers
{
    public interface IPiutangBuilder : INunaBuilder<PiutangModel>
    {
        IPiutangBuilder Create(IPiutangKey piutangKey);
        
    }
    public class PiutangBuilder : IPiutangBuilder
    {
        public PiutangModel Build()
        {
            throw new System.NotImplementedException();
        }

        public IPiutangBuilder Create(IPiutangKey piutangKey)
        {
            throw new System.NotImplementedException();
        }
    }
}