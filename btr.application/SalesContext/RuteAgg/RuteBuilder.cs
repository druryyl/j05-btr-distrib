using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.RuteAgg
{
    public interface IRuteBuilder : INunaBuilder<RuteModel>
    {
        IRuteBuilder Load(IRuteKey key);
        IRuteBuilder AddItem(ICustomerKey key);
    }

    public class RuteBuilder : IRuteBuilder
    {
        private readonly IRuteDal _ruteDal;
        private readonly IRuteItemDal _ruteItemDal;
        private RuteModel _agg;

        public RuteBuilder(IRuteDal ruteDal, IRuteItemDal ruteItemDal)
        {
            _ruteDal = ruteDal;
            _ruteItemDal = ruteItemDal;
            _agg = new RuteModel();
        }


        public RuteModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IRuteBuilder Load(IRuteKey key)
        {
            _agg = _ruteDal.GetData(key) 
                ?? throw new KeyNotFoundException($"Rute not found ({key})");
            _agg.ListCustomer = _ruteItemDal.ListData(key)?.ToList()
                ?? new List<RuteItemModel>();
            return this;    
        }

        public IRuteBuilder AddItem(ICustomerKey key)
        {
            var newItem = new RuteItemModel
            {
                RuteId = _agg.RuteId,
                CustomerId = key.CustomerId
            };
            _agg.ListCustomer.Add(newItem);
            return this;
        }
    }
}
