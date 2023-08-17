using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace btr.infrastructure.PurchaseContext.SupplierAgg
{
    public class SupplierDal : ISupplierDal
    {
        private readonly DatabaseOptions _opt;

        public SupplierDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(SupplierModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Supplier
                ";
        }

        public void Update(SupplierModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(ISupplierKey key)
        {
            throw new NotImplementedException();
        }

        public SupplierModel GetData(ISupplierKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SupplierModel> ListData()
        {
            throw new NotImplementedException();
        }
    }
}
