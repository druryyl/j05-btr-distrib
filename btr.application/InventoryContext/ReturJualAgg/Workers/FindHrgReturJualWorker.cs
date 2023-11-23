using btr.application.BrgContext.BrgAgg;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;

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

        public FindHrgReturJualHandler(IBrgBuilder brgBuilder, 
            ICustomerBuilder customerBuilder)
        {
            _brgBuilder = brgBuilder;
            _customerBuilder = customerBuilder;
        }

        public BrgHargaModel Execute(FindHrgReturJualRequest req)
        {
            var brg = _brgBuilder.Load(req).Build();
            var customer = _customerBuilder.Load(req).Build();

            var result = brg.ListHarga.Find(x => x.HargaTypeId == customer.HargaTypeId);

            return result;
        }
    }
}