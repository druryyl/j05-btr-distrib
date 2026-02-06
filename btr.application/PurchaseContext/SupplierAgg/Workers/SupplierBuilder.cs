using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System.Collections.Generic;

namespace btr.application.PurchaseContext.SupplierAgg.Workers
{
    public interface ISupplierBuilder : INunaBuilder<SupplierModel>
    {
        ISupplierBuilder Create();
        ISupplierBuilder Load(ISupplierKey key);
        ISupplierBuilder Attach(SupplierModel model);
        ISupplierBuilder Code(string code);
        ISupplierBuilder Name(string name);
        ISupplierBuilder Address(string addr1, string addr2, string kota);
        ISupplierBuilder KodePos(string kodePos);
        ISupplierBuilder NoTelp(string noTelp);
        ISupplierBuilder NoFax(string noFax);
        ISupplierBuilder ContactPerson(string contactPerson);
        ISupplierBuilder Npwp(string nnpwp);
        ISupplierBuilder NoPkp(string noPkp);
        ISupplierBuilder Keyword(string keyword);
        ISupplierBuilder Depo(string depoId);
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

        public ISupplierBuilder NoTelp(string noTelp)
        {
            _agg.NoTelp = noTelp;
            return this;
        }

        public ISupplierBuilder NoFax(string noFax)
        {
            _agg.NoFax = noFax;
            return this;
        }
        public ISupplierBuilder ContactPerson(string contactPerson)
        {
            _agg.ContactPerson = contactPerson;
            return this;
        }

        public ISupplierBuilder Npwp(string nnpwp)
        {
            _agg.Npwp = nnpwp;
            return this;
        }

        public ISupplierBuilder NoPkp(string noPkp)
        {
            _agg.NoPkp = noPkp;
            return this;
        }

        public ISupplierBuilder Attach(SupplierModel model)
        {
            _agg = model;
            return this;
        }

        public ISupplierBuilder Code(string code)
        {
            _agg.SupplierCode = code;
            return this;
        }

        public ISupplierBuilder KodePos(string kodePos)
        {
            _agg.KodePos = kodePos;
            return this;
        }

        public ISupplierBuilder Keyword(string keyword)
        {
            _agg.Keyword = keyword;
            return this;
        }

        public ISupplierBuilder Depo(string depoId)
        {
            _agg.DepoId = depoId;
            return this;
        }
    }
}
