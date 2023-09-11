using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.FakturControlAgg
{
    public interface IFakturControlBuilder : INunaBuilder<FakturControlModel>
    {
        IFakturControlBuilder LoadOrCreate(IFakturKey fakturKey);
        IFakturControlBuilder Attach(FakturControlModel faktur);
        IFakturControlBuilder Posted(IUserKey user);
        IFakturControlBuilder Unpost(IUserKey user);

        IFakturControlBuilder Kirim(IUserKey user);
        IFakturControlBuilder KembaliFaktur(IUserKey user);
        IFakturControlBuilder FakturPajak(IUserKey user);
        IFakturControlBuilder Reset(StatusFakturEnum status);
    }
    public class FakturControlBuilder : IFakturControlBuilder
    {
        private FakturControlModel _aggregate;
        private readonly IFakturControlStatusDal _fakturControlDal;
        private readonly IFakturDal _fakturDal;
        private readonly ITglJamDal _dateTime;

        public FakturControlBuilder(IFakturControlStatusDal fakturControlDal, IFakturDal fakturDal, ITglJamDal tglJamDal)
        {
            _fakturControlDal = fakturControlDal;
            _fakturDal = fakturDal;
            _dateTime = tglJamDal;
        }

        public IFakturControlBuilder LoadOrCreate(IFakturKey fakturKey)
        {
            var faktur = _fakturDal.GetData(fakturKey)
                ?? throw new KeyNotFoundException("Faktur not found");
            faktur.RemoveNull();

            _aggregate = new FakturControlModel
            {
                FakturId = faktur.FakturId,
                FakturCode = faktur.FakturCode,
                FakturDate = faktur.FakturDate,
                CustomerCode = faktur.CustomerCode,
                CustomerName = faktur.CustomerName,
                Npwp = faktur.Npwp,
                SalesPersonId = faktur.SalesPersonId,
                SalesPersonName = faktur.SalesPersonName,
                Total = faktur.GrandTotal,
                Bayar = faktur.GrandTotal - faktur.KurangBayar,
                Sisa = faktur.KurangBayar,
                NoFakturPajak = faktur.NoFakturPajak,
                UserId = faktur.UserId,
                ListStatus = _fakturControlDal.ListData((IFakturKey)faktur)?.ToList()
                ?? new List<FakturControlStatusModel>()
            };
            return this;
        }

        public IFakturControlBuilder Attach(FakturControlModel faktur)
        {
            _aggregate = faktur;
            return this;
        }

        public IFakturControlBuilder Posted(IUserKey user)
        {
            SetStatus(StatusFakturEnum.Posted, user);
            return this;
        }

        public IFakturControlBuilder Kirim(IUserKey user)
        {
            SetStatus(StatusFakturEnum.Kirim, user);
            return this;
        }

        public IFakturControlBuilder KembaliFaktur(IUserKey user)
        {
            SetStatus(StatusFakturEnum.KembaliFaktur, user);
            return this;
        }

        public IFakturControlBuilder FakturPajak(IUserKey user)
        {
            SetStatus(StatusFakturEnum.Pajak, user);
            return this;
        }

        private void SetStatus(StatusFakturEnum status, IUserKey user)
        {
            var item = _aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == status)
                ?? new FakturControlStatusModel
                {
                    FakturId = _aggregate.FakturId,
                    FakturDate = _aggregate.FakturDate,
                    StatusFaktur = status,
                    StatusDate = _dateTime.Now(),
                    Keterangan = string.Empty,
                    UserId = user.UserId,
                };
            _aggregate.ListStatus.RemoveAll(x => x.StatusFaktur == status);
            _aggregate.ListStatus.Add(item);
        }

        public FakturControlModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IFakturControlBuilder Reset(StatusFakturEnum status)
        {
            _aggregate.ListStatus.RemoveAll(x => x.StatusFaktur == status);
            return this;
        }

        public IFakturControlBuilder Unpost(IUserKey user)
        {
            //  jika sudah kirim, maka tidak bisa unpost
            //var kirim = _aggregate.ListStatus
            throw new NotImplementedException();
        }
    }
}
