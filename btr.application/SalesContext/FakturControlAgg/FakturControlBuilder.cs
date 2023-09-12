using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
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
        IFakturControlBuilder CancelPost(IUserKey user);

        IFakturControlBuilder Kirim(IUserKey user);
        IFakturControlBuilder CancelKirim();
        IFakturControlBuilder KembaliFaktur(IUserKey user);
        IFakturControlBuilder CancelKembaliFaktur();

        IFakturControlBuilder Lunas(IUserKey user);
        IFakturControlBuilder CancelLunas(IUserKey user);

        IFakturControlBuilder FakturPajak(IUserKey user);
    }

    public class FakturControlBuilder : IFakturControlBuilder
    {
        private FakturControlModel _aggregate;
        private readonly IFakturControlStatusDal _fakturControlDal;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturDal _fakturDal;
        private readonly ITglJamDal _dateTime;

        public FakturControlBuilder(IFakturControlStatusDal fakturControlDal, 
            IFakturDal fakturDal, 
            ITglJamDal tglJamDal, 
            IFakturBuilder fakturBuilder)
        {
            _fakturControlDal = fakturControlDal;
            _fakturDal = fakturDal;
            _dateTime = tglJamDal;
            _fakturBuilder = fakturBuilder;
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
                GrandTotal = faktur.GrandTotal,
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
        public IFakturControlBuilder CancelPost(IUserKey user)
        {
            //  jika sudah KIRIM, maka tidak bisa CANCEL-POST
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Kirim) != null)
                throw new ArgumentException("Status KIRIM masih aktif, Cancel POSTED gagal");
            CancelStatus(StatusFakturEnum.Posted);
            return this;
        }

        public IFakturControlBuilder Kirim(IUserKey user)
        {
            //  jika belum POSTED, maka tidak bisa KIRIM
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Posted) == null)
                throw new ArgumentException("Faktur belum POSTED, Status KIRIM gagal");
            SetStatus(StatusFakturEnum.Kirim, user);
            return this;
        }
        public IFakturControlBuilder CancelKirim()
        {
            //  jika sudah KEMBALI, maka tidak bisa CANCEL-KIRIM
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.KembaliFaktur) != null)
                throw new ArgumentException("Status KEMBALI masih aktif, Cancel KIRIM gagal");
            CancelStatus(StatusFakturEnum.Kirim);
            return this;
        }

        public IFakturControlBuilder KembaliFaktur(IUserKey user)
        {
            //  jika belum KIRIM, maka tidak bisa KEMBALI
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Kirim) == null)
                throw new ArgumentException("Faktur belum KIRIM, Status KEMBALI gagal");
            SetStatus(StatusFakturEnum.KembaliFaktur, user);
            return this;
        }
        public IFakturControlBuilder CancelKembaliFaktur()
        {
            //  jika sudah FAKTUR PAJAK, maka tidak bisa CANCEL-KEMBALI
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Pajak) != null)
                throw new ArgumentException("Status PAJAK masih aktif, Cancel KEMBALI gagal");
            CancelStatus(StatusFakturEnum.KembaliFaktur);
            return this;
        }

        public IFakturControlBuilder Lunas(IUserKey user)
        {
            //  jika sudah FAKTUR-PAJAK, maka tidak bisa ubah ke LUNAS/CASH
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Pajak) != null)
                throw new ArgumentException("Faktur sudah dibuatkan FAKTUR-PAJAK, Status LUNAS gagal");
            SetStatus(StatusFakturEnum.Lunas, user);
            return this;
        }

        public IFakturControlBuilder CancelLunas(IUserKey user)
        {
            //  jika sudah FAKTUR-PAJAK, maka tidak bisa ubah ke LUNAS/CASH
            if (_aggregate.ListStatus.FirstOrDefault(x => x.StatusFaktur == StatusFakturEnum.Pajak) != null)
                throw new ArgumentException("Faktur sudah dibuatkan FAKTUR-PAJAK, Status CANCEL LUNAS gagal");
            SetStatus(StatusFakturEnum.Lunas, user);
            return this;
        }

        public IFakturControlBuilder FakturPajak(IUserKey user)
        {
            SetStatus(StatusFakturEnum.Pajak, user);
            return this;
        }

        public FakturControlModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }


        #region PRIVATE-HELPERS
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
        private void CancelStatus(StatusFakturEnum status)
        {
            _aggregate.ListStatus.RemoveAll(x => x.StatusFaktur == status);
        }

        #endregion

    }
}
