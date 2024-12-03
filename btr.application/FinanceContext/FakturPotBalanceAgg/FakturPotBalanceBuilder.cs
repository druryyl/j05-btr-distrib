using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.application.FinanceContext.PiutangAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.FinanceContext.PiutangAgg;
using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FakturPotBalanceAgg
{
    public interface IFakturPotBalanceBuilder : INunaBuilder<FakturPotBalanceModel>
    {
        IFakturPotBalanceBuilder LoadOrCreate(FakturModel faktur);
        IFakturPotBalanceBuilder AddPost(ReturJualModel retur, string userId, decimal nilai);
        IFakturPotBalanceBuilder RemovePost(ReturJualModel retur);
    }
    public class FakturPotBalanceBuilder : IFakturPotBalanceBuilder
    {
        private FakturPotBalanceModel _agg;
        private readonly IFakturPotBalanceDal _fakturPotBalanceDal;
        private readonly IFakturPotBalancePostDal _fakturPotBalancePostDal;
        private readonly IPiutangDal _piutangDal;
        private readonly ITglJamDal _dateTime;

        public FakturPotBalanceBuilder(IFakturPotBalanceDal fakturPotBalanceDal, 
            IFakturPotBalancePostDal fakturPotBalancePostDal, 
            IPiutangDal piutangDal, 
            ITglJamDal dateTime)
        {
            _fakturPotBalanceDal = fakturPotBalanceDal;
            _fakturPotBalancePostDal = fakturPotBalancePostDal;
            _piutangDal = piutangDal;
            _dateTime = dateTime;
        }

        public FakturPotBalanceModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IFakturPotBalanceBuilder LoadOrCreate(FakturModel faktur)
        {
            var piutang = _piutangDal.GetData(new PiutangModel(faktur.FakturId))
                ?? new PiutangModel { Potongan = 0 };

            _agg = _fakturPotBalanceDal.GetData(faktur) ??
                new FakturPotBalanceModel
                {
                    FakturId = faktur.FakturId,
                    FakturDate = faktur.FakturDate,
                    CustomerId = faktur.CustomerId,
                    CustomerName = faktur.CustomerName,
                    NilaiFaktur = faktur.GrandTotal,
                    NilaiPotong = piutang.Potongan,
                    ListPost = new List<FakturPotBalancePostModel>()
                };

            var listPost = _fakturPotBalancePostDal.ListData(faktur)?.ToList()
                ?? new List<FakturPotBalancePostModel>();
            _agg.ListPost = listPost;
            return this;
        }

        public IFakturPotBalanceBuilder AddPost(ReturJualModel retur, string userId, decimal nilaiPost)
        {
            _agg.ListPost.RemoveAll(x => x.ReturJualId == retur.ReturJualId);
            var newPost = new FakturPotBalancePostModel
            {
                FakturId = _agg.FakturId,
                NoUrut = _agg.ListPost.Max(x => x.NoUrut),
                PostDate = _dateTime.Now,
                UserId = userId,
                ReturJualId = retur.ReturJualId,
                NilaiRetur = retur.GrandTotal,
                NilaiPost = nilaiPost,
            };
            _agg.ListPost.Add(newPost);
            return this;
        }

        public IFakturPotBalanceBuilder RemovePost(ReturJualModel retur)
        {
            _agg.ListPost.RemoveAll(x => x.ReturJualId == retur.ReturJualId);
            return this;
        }

    }
}
