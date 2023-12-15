using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.FinanceContext.TagihanAgg
{
    public interface ITagihanBuilder : INunaBuilder<TagihanModel>
    {
        ITagihanBuilder Create();
        ITagihanBuilder Load(ITagihanKey tagihanKey);
        ITagihanBuilder Attach(TagihanModel tagihan);
        ITagihanBuilder TglTagihan(DateTime tglTagihan);
        ITagihanBuilder Sales(ISalesPersonKey salesKey);
        ITagihanBuilder AddFaktur(IFakturKey fakturKey, decimal sisaTagihan);
       ITagihanBuilder RemoveFaktur(IFakturKey fakturKey); 
    }
    public class TagihanBuilder : ITagihanBuilder
    {
        private TagihanModel _aggregate;
        private readonly ITagihanDal _tagihanDal;
        private readonly ITagihanFakturDal _tagihanFakturDal;
        private readonly ISalesPersonDal _salesDal;
        private readonly IFakturDal _fakturDal;

        public TagihanBuilder(ITagihanDal tagihalDal, 
            ITagihanFakturDal tagihanFakturDal, 
            ISalesPersonDal salesDal, 
            IFakturDal fakturDal)
        {
            _tagihanDal = tagihalDal;
            _tagihanFakturDal = tagihanFakturDal;
            _salesDal = salesDal;
            _fakturDal = fakturDal;
        }

        public TagihanModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public ITagihanBuilder Create()
        {
            _aggregate = new TagihanModel
            {
                ListFaktur = new List<TagihanFakturModel>()
            };
            return this;
        }

        public ITagihanBuilder Load(ITagihanKey tagihanKey)
        {
            _aggregate = _tagihanDal.GetData(tagihanKey)
                ?? throw new KeyNotFoundException("Tagihan not found");
            _aggregate.ListFaktur = _tagihanFakturDal.ListData(tagihanKey)?.ToList()
                ?? new List<TagihanFakturModel>();
            return this;
        }

        public ITagihanBuilder Attach(TagihanModel tagihan)
        {
            _aggregate = tagihan;
            return this;
        }

        public ITagihanBuilder TglTagihan(DateTime tglTagihan)
        {
            _aggregate.TagihanDate = tglTagihan;
            return this;    
        }

        public ITagihanBuilder Sales(ISalesPersonKey salesKey)
        {
            var sales = _salesDal.GetData(salesKey)
                        ?? throw new KeyNotFoundException("Sales not found");
            _aggregate.SalesPersonId = sales.SalesPersonId;
            _aggregate.SalesPersonName = sales.SalesPersonName;
            return this;
        }

        public ITagihanBuilder AddFaktur(IFakturKey fakturKey, decimal sisaTagihan)
        {
            var faktur = _fakturDal.GetData(fakturKey)
                ?? throw new KeyNotFoundException("Faktur not found"); 
            var noUrut = _aggregate.ListFaktur.DefaultIfEmpty(new TagihanFakturModel { NoUrut = 0 }).Max(x => x.NoUrut) + 1;
            var tagihanFaktur = new TagihanFakturModel
            {
                FakturId = faktur.FakturId,
                NoUrut = noUrut,
                FakturCode = faktur.FakturCode,
                CustomerId = faktur.CustomerId,
                CustomerName = faktur.CustomerName,
                Alamat = faktur.Address,
                Nilai = sisaTagihan
            };
            _aggregate.TotalTagihan += sisaTagihan;
            _aggregate.ListFaktur.Add(tagihanFaktur);
            return this;
        }

        public ITagihanBuilder RemoveFaktur(IFakturKey fakturKey)
        {
            // remove faktur from list faktur
            _aggregate.ListFaktur.RemoveAll(x => x.FakturId == fakturKey.FakturId);
            _aggregate.TotalTagihan = _aggregate.ListFaktur.Sum(x => x.Nilai);
            return this;
        }
    }
}