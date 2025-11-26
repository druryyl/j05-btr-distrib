using btr.domain.SalesContext.CustomerAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;

namespace btr.application.SalesContext.CustomerAgg.Contracts
{
    public interface ICustomerDal :
        IInsert<CustomerModel>,
        IUpdate<CustomerModel>,
        IDelete<ICustomerKey>,
        IGetData<CustomerModel, ICustomerKey>,
        IListData<CustomerModel>
    {
        IEnumerable<CustomerLocationView> ListLocation();
    }

    public class CustomerLocationView
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string WilayahId { get; set; }
        public string WilayahName { get; set; }
        public string KlasifikasiId { get; set; }
        public string KlasifikasiName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public bool HasCoordinate { get; set; }
        public DateTime CoordinateTimestamp { get; set; }
        public string CoordinateUser { get; set; }
    }

}