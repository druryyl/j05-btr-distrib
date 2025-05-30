﻿using btr.application.SalesContext.SalesPersonAgg;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.SalesRuteAgg
{
    public interface ISalesRuteBuilder : INunaBuilder<SalesRuteModel>
    {
        ISalesRuteBuilder LoadOrCreate(ISalesPersonKey salesKey, IHariRuteKey hariRute);
        ISalesRuteBuilder Load(ISalesRuteKey salesRuteKey);

    }

    public class SalesRuteBuilder : ISalesRuteBuilder
    {
        private readonly ISalesRuteDal _salesRuteDal;
        private readonly ISalesRuteItemDal _salesRuteItemDal;
        private SalesRuteModel _agg;
        private readonly ISalesPersonDal _salesPersonDal;

        public SalesRuteBuilder(ISalesRuteDal salesRuteDal,
            ISalesRuteItemDal salesRuteItemDal,
            ISalesPersonDal salesPersonDal)
        {
            _salesRuteDal = salesRuteDal;
            _salesRuteItemDal = salesRuteItemDal;
            _salesPersonDal = salesPersonDal;
        }

        public ISalesRuteBuilder LoadOrCreate(ISalesPersonKey salesKey, IHariRuteKey hariRute)
        {
            var listSalesRute = _salesRuteDal.ListData(salesKey)?.ToList() ?? new List<SalesRuteModel> ();
            _agg = listSalesRute.FirstOrDefault(x => x.HariRuteId == hariRute.HariRuteId);
            if (_agg is null)
                _agg = new SalesRuteModel
                {
                    SalesRuteId = string.Empty,
                    SalesPersonId = salesKey.SalesPersonId,
                    SalesPersonName = _salesPersonDal.GetData(salesKey)?.SalesPersonName ?? string.Empty,
                    HariRuteId = hariRute.HariRuteId,
                    ListCustomer = new List<SalesRuteItemModel>()
                };
            if (_agg.SalesRuteId != string.Empty)
                _agg.ListCustomer = _salesRuteItemDal.ListData(_agg)?.ToList() 
                    ?? new List<SalesRuteItemModel> ();
            return this;
        }
        public ISalesRuteBuilder Load(ISalesRuteKey salesRuteKey)
        {
            _agg = _salesRuteDal.GetData(salesRuteKey)
                ?? throw new KeyNotFoundException("SalesRute-ID not found");
            _agg.ListCustomer = _salesRuteItemDal.ListData(salesRuteKey)
                ?.ToList() 
                ?? new List<SalesRuteItemModel>();
            return this;
        }

        public SalesRuteModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

    }
}
