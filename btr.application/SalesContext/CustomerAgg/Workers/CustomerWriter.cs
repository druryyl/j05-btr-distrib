using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (model.CustomerId.IsNullOrEmpty())
                model.CustomerId = _counter.Generate("CS", IDFormatEnum.PFnnnn);

            var custDb = _customerDal.GetData(model);
            if (custDb is null)
                _customerDal.Insert(model);
            else
                _customerDal.Update(model);

        }
    }
}
