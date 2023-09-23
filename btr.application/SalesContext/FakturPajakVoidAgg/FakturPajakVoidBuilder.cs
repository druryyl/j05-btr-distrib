using btr.application.SupportContext.TglJamAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.SalesContext.FakturPajakVoidAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public IFakturPajakVoidBuilder User(IUserKey user)
        {
            throw new NotImplementedException();
        }

        public FakturPajakVoidModel Build()
        {
            throw new NotImplementedException();
        }
    }
}
