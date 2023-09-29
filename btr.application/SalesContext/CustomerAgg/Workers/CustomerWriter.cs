using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.CustomerAgg.Workers
{
    public interface ICustomerWriter : INunaWriter<CustomerModel>
    {
    }

    public class CustomerWriter : ICustomerWriter
    {
        private readonly ICustomerDal _customerDal;
        private readonly INunaCounterBL _counter;

        public CustomerWriter(ICustomerDal customerDal, 
            INunaCounterBL counter)
        {
            _customerDal = customerDal;
            _counter = counter;
        }

        public void Save(ref CustomerModel model)
        {
            if (model.SupplierId.IsNullOrEmpty())
                model.SupplierId = _counter.Generate("CS", IDFormatEnum.PFnnnn);

            var custDb = _customerDal.GetData(model);
            if (custDb is null)
                _customerDal.Insert(model);
            else
                _customerDal.Update(model);

        }
    }
}
