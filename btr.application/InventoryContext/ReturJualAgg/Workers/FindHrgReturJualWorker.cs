using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Polly;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public class FindHrgReturJualRequest : ICustomerKey, IBrgKey
    {
        public FindHrgReturJualRequest(string brgId, 
            string customerId)
        {
            BrgId = brgId;
            CustomerId = customerId;
        }

        public string BrgId { get; set; }
        public string CustomerId { get; set; }
    }
    
    public interface IFindHrgReturJualHandler : INunaService<BrgHargaModel, FindHrgReturJualRequest>
    {
    }
    
    public class FindHrgReturJualHandler : IFindHrgReturJualHandler
    {
        private readonly IBrgBuilder _brgBuilder;
        private readonly ICustomerBuilder _customerBuilder;
        private readonly IFakturItemViewDal _fakturItemViewDal;
        private readonly ITglJamDal _tglJamDal;
        private const string DEFAULT_HARGA_TYPE_ID = "MT";

        public FindHrgReturJualHandler(IBrgBuilder brgBuilder, 
            ICustomerBuilder customerBuilder, 
            IFakturItemViewDal fakturItemViewDal, 
            ITglJamDal tglJamDal)
        {
            _brgBuilder = brgBuilder;
            _customerBuilder = customerBuilder;
            _fakturItemViewDal = fakturItemViewDal;
            _tglJamDal = tglJamDal;
        }

        public BrgHargaModel Execute(FindHrgReturJualRequest req)
        {
            req.RemoveNull();
            var fallbackBrg = Policy<BrgModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new BrgModel
                {
                    BrgId = string.Empty,
                    ListHarga = new List<BrgHargaModel>
                    {
                        new BrgHargaModel
                        {
                            BrgId = string.Empty,
                            Harga = 0,
                            HargaTypeId = DEFAULT_HARGA_TYPE_ID
                        }
                    }
                });
            var brg = fallbackBrg.Execute(() => _brgBuilder.Load(req).Build());
            
            var fallbackCustomer = Policy<CustomerModel>
                .Handle<KeyNotFoundException>()
                .Fallback(new CustomerModel
                {
                    CustomerId = string.Empty,
                    HargaTypeId = DEFAULT_HARGA_TYPE_ID
                });
            var customer = fallbackCustomer
                .Execute(() => _customerBuilder.Load(req).Build());
            
            //  cari dari history pembelian
            var periode = new Periode(
                _tglJamDal.Now.AddYears(-1), 
                _tglJamDal.Now);
            var listFakturItem = _fakturItemViewDal.ListData(customer, brg, periode)?.ToList();
            
            var result = listFakturItem != null ?
                GetHargaFromHistoryJual(listFakturItem, customer) :
                GetHargaFromMasterBrg(brg, customer);

            return result;
        }

        private static BrgHargaModel GetHargaFromMasterBrg(BrgModel brg, IHargaTypeKey hrgType)
            => brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == hrgType.HargaTypeId) 
                     ?? new BrgHargaModel
                     {
                         BrgId = brg.BrgId,
                         Harga = 0,
                         HargaTypeId = hrgType.HargaTypeId
                     };

        private static BrgHargaModel GetHargaFromHistoryJual(IEnumerable<FakturItemView> listFakturItem, IHargaTypeKey hrgType)
            => listFakturItem
                .OrderByDescending(x => x.FakturDate)
                .Select(x => new BrgHargaModel
                {
                    BrgId = x.BrgId,
                    Harga = x.HrgSat,
                    HargaTypeId = hrgType.HargaTypeId
                })
                .First();
    }
}