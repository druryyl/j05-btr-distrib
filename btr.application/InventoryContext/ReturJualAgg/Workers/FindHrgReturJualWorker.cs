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
            var brg = _brgBuilder.Load(req).Build();
            var customer = _customerBuilder.Load(req).Build();
            
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