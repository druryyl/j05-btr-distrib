using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.CustomerAgg.Contracts
{
    public interface ICustomerLocHistDal :
        IInsert<CustomerLocHistModel>,
        IUpdate<CustomerLocHistModel>,
        IDelete<ICustomerLocHistKey>,
        IGetData<CustomerLocHistModel, ICustomerLocHistKey>,
        IListData<CustomerLocHistModel, ICustomerKey>
    {
    }
}
