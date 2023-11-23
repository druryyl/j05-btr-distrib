using System;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.FakturAgg.Contracts
{
    public interface IFakturItemDal :
        IInsertBulk<FakturItemModel>,
        IDelete<IFakturKey>,
        IListData<FakturItemModel, IFakturKey>
    {
    }

    public interface IFakturItemViewDal :
        IListData<FakturItemView, ICustomerKey, IBrgKey, Periode>
    {
    }
    
    public class FakturItemView
    {
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string BrgId { get; set; }
        public decimal HrgSatuan { get; set; }
    }
}