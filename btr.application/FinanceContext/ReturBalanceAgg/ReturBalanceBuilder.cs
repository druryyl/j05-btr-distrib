using btr.application.FinanceContext.PiutangAgg.Contracts;
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

namespace btr.application.FinanceContext.ReturBalanceAgg
{
    public interface IReturBalanceBuilder : INunaBuilder<ReturBalanceModel>
    {
        IReturBalanceBuilder LoadOrCreate(ReturJualModel retur);
        IReturBalanceBuilder AddPost(FakturModel faktur, string userId, decimal nilaiPost);
        IReturBalanceBuilder RemovePost(FakturModel faktur);
    }

    public class ReturBalanceBuilder : IReturBalanceBuilder
    {
        private ReturBalanceModel _agg;
        private readonly IReturBalanceDal _returBalanceDal;
        private readonly IReturBalancePostDal _returBalancePostDal;
        private readonly ITglJamDal _dateTime;
        private readonly IPiutangDal _piutangDal;

        public ReturBalanceBuilder(IReturBalanceDal returBalanceDal,
            IReturBalancePostDal returBalancePostDal,
            ITglJamDal dateTime,
            IPiutangDal piutangDal)
        {
            _returBalanceDal = returBalanceDal;
            _returBalancePostDal = returBalancePostDal;
            _dateTime = dateTime;
            _piutangDal = piutangDal;
        }

        public ReturBalanceModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IReturBalanceBuilder LoadOrCreate(ReturJualModel retur)
        {
            _agg = _returBalanceDal.GetData(retur) ?? 
                new ReturBalanceModel
                {
                    ReturJualId = retur.ReturJualId,
                    ReturJualDate = retur.ReturJualDate,
                    CustomerId = retur.CustomerId,
                    CustomerName = retur.CustomerName,
                    NilaiRetur = retur.GrandTotal,
                    ListPost = new List<ReturBalancePostModel>()
                };

            var listPost = _returBalancePostDal.ListData(retur)?.ToList() 
                ?? new List<ReturBalancePostModel>();
            _agg.ListPost = listPost;
            return this;
        }

        public IReturBalanceBuilder AddPost(FakturModel faktur, string userId, decimal nilaiPost)
        {
            var piutang = _piutangDal.GetData(new PiutangModel(faktur.FakturId))
                ?? new PiutangModel { Potongan = 0 };
            _agg.ListPost.RemoveAll(x => x.FakturId == faktur.FakturId);
            var newPost = new ReturBalancePostModel
                {
                    ReturJualId = _agg.ReturJualId,
                    NoUrut = _agg.ListPost.Max(x => x.NoUrut) + 1,
                    PostDate = _dateTime.Now,
                    UserId = userId,
                    FakturId = faktur.FakturId,
                    NilaiFaktur = faktur.GrandTotal,
                    NilaiPotong = piutang.Potongan,
                    NilaiPost = nilaiPost
                };
            _agg.ListPost.Add(newPost);
            return this;
        }

        public IReturBalanceBuilder RemovePost(FakturModel faktur)
        {
            _agg.ListPost.RemoveAll(x => x.FakturId == faktur.FakturId);
            return this;
        }
    }
}
