using btr.application.SupportContext.TglJamAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.domain.SalesContext.FakturPajakVoidAgg;
using btr.nuna.Domain;

namespace btr.application.SalesContext.FakturPajakVoidAgg
{
    public interface IFakturPajakVoidBuilder : INunaBuilder<FakturPajakVoidModel>
    {
        IFakturPajakVoidBuilder LoadOrCreate(INoFakturPajak key);
        IFakturPajakVoidBuilder Alasan(string alasan);
        IFakturPajakVoidBuilder User(IUserKey user);
    }

    public class FakturPajakVoidBuilder : IFakturPajakVoidBuilder
    {
        private FakturPajakVoidModel _aggregate;
        private readonly IFakturPajakVoidDal _fpvDal;
        private readonly ITglJamDal _dateTime;

        public FakturPajakVoidBuilder(IFakturPajakVoidDal fpvDal, ITglJamDal dateTime)
        {
            _fpvDal = fpvDal;
            _dateTime = dateTime;
        }

        public IFakturPajakVoidBuilder LoadOrCreate(INoFakturPajak key)
        {
            _aggregate = _fpvDal.GetData(key) ??
                new FakturPajakVoidModel 
                { 
                    NoFakturPajak = key.NoFakturPajak,
                    VoidDate = _dateTime.Now
                };
            return this;
        }

        public IFakturPajakVoidBuilder Alasan(string alasan)
        {
            _aggregate.AlasanVoid = alasan;
            return this;
        }

        public IFakturPajakVoidBuilder User(IUserKey user)
        {
            _aggregate.UserId = user.UserId;
            return this;

        }

        public FakturPajakVoidModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }
    }
}
