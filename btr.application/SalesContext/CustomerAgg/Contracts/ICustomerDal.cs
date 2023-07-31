using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.CustomerAgg.Contracts
{
    public interface ICustomerDal :
        IInsert<CustomerModel>,
        IUpdate<CustomerModel>,
        IDelete<ICustomerKey>,
        IGetData<CustomerModel, ICustomerKey>,
        IListData<CustomerModel>
    {
    }
}