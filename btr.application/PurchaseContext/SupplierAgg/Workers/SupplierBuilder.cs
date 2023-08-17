using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.SupplierAgg.Workers
{
    public interface ISupplierBuilder : INunaBuilder<SupplierModel>
    {
        ISupplierBuilder Create();
        ISupplierBuilder Load(ISupplierKey key);
        ISupplierBuilder Name(string name);
        ISupplierBuilder Address(string addr1, string addr2, string kota);
        ISupplierBuilder Telp(string noTelp, string noFax);
        ISupplierBuilder ContactPerson(string contactPerson);
        ISupplierBuilder INpwp(string nnpwp);
        ISupplierBuilder INoPkp(string noPkp);
    }

    public class SupplierBuilder : ISupplierBuilder
    {
        private SupplierModel _agg = new SupplierModel();
        private readonly ISupplierDal _supplierDal;

        public SupplierBuilder(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
        }

        public SupplierModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public ISupplierBuilder Create()
        {
            _agg = new SupplierModel();
            return this;
        }

        public ISupplierBuilder Load(ISupplierKey key)
        {
            _agg = _supplierDal.GetData(key)
                ?? throw new KeyNotFoundException("SupplierId not found");
            return this;
        }

        public ISupplierBuilder Name(string name)
        {
            _agg.SupplierName = name;
            return this;
        }

        public ISupplierBuilder Address(string addr1, string addr2, string kota)
        {
            _agg.Address1 = addr1;
            _agg.Address2 = addr2;
            _agg.Kota = kota;
            return this;
        }

        public ISupplierBuilder Telp(string noTelp, string noFax)
        {
            _agg.NoTelp = noTelp;
            _agg.NoFax = noFax;
            return this;
        }

        public ISupplierBuilder ContactPerson(string contactPerson)
        {
            _agg.ContactPerson = contactPerson;
            return this;
        }

        public ISupplierBuilder INpwp(string nnpwp)
        {
            _agg.Npwp = nnpwp;
            return this;
        }

        public ISupplierBuilder INoPkp(string noPkp)
        {
            _agg.NoPkp = noPkp;
            return this;
        }
    }
}
