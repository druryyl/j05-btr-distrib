using System;
using System.Collections.Generic;
using btr.application.BrgContext.HargaTypeAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.KlasifikasiAgg;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.KlasifikasiAgg;
using btr.domain.SalesContext.WilayahAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.CustomerAgg.Workers
{
    public interface ICustomerBuilder : INunaBuilder<CustomerModel>
    {
        ICustomerBuilder CreateNew();
        ICustomerBuilder Load(ICustomerKey customerKey);
        ICustomerBuilder Attach(CustomerModel customer);
        
        ICustomerBuilder Name(string name);
        ICustomerBuilder Code(string code);
        ICustomerBuilder Klasifikasi(IKlasifikasiKey klasifikasiKey);
        ICustomerBuilder HargaType(IHargaTypeKey hargaTypeKey);
        ICustomerBuilder Plafond(decimal plafond);
        ICustomerBuilder CreditBalance(decimal creditBalance);
        ICustomerBuilder Wilayah(IWilayahKey wilayahKey);
        ICustomerBuilder Address(string address1, string address2, string kota);
        ICustomerBuilder KodePos(string kodePos);
        ICustomerBuilder NoTelp(string noTelp);
        ICustomerBuilder NoFax(string noFax);
        ICustomerBuilder Email(string email);
        ICustomerBuilder Nitku(string nitku);
        ICustomerBuilder IsKenaPajak(bool kenaPajak);
        ICustomerBuilder Npwp(string npwp);
        ICustomerBuilder Nppkp(string nppkp);
        ICustomerBuilder NamaWp(string namaWp);
        ICustomerBuilder AddressWp(string address1, string address2);
        ICustomerBuilder JenisIdentitasPajak(string jenisIdentitasPajak);
    }

    public class CustomerBuilder : ICustomerBuilder
    {
        private CustomerModel _aggRoot = new CustomerModel();
        private readonly ICustomerDal _customerDal;
        private readonly IKlasifikasiDal _klasifikasiDal;
        private readonly IHargaTypeDal _hargaTypeDal;
        private readonly IWilayahDal _wilayahDal;

        public CustomerBuilder(ICustomerDal customerDal,
            IKlasifikasiDal klasifikasiDal,
            IHargaTypeDal hargaTypeDal,
            IWilayahDal wilayahDal)
        {
            _customerDal = customerDal;
            _klasifikasiDal = klasifikasiDal;
            _hargaTypeDal = hargaTypeDal;
            _wilayahDal = wilayahDal;
        }

        public CustomerModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public ICustomerBuilder CreateNew()
        {
            _aggRoot = new CustomerModel
            {
                CustomerName = string.Empty,
                CustomerId = string.Empty
            };
            return this;
        }

        public ICustomerBuilder Load(ICustomerKey customerKey)
        {
            _aggRoot = _customerDal.GetData(customerKey)
                       ?? throw new KeyNotFoundException($"CustomerId not found ({customerKey.CustomerId})");
            return this;
        }

        public ICustomerBuilder Name(string name)
        {
            _aggRoot.CustomerName = name;
            return this;
        }

        public ICustomerBuilder Attach(CustomerModel customer)
        {
            _aggRoot = customer;
            return this;
        }

        public ICustomerBuilder Code(string code)
        {
            _aggRoot.CustomerCode = code;
            return this;
        }

        public ICustomerBuilder Klasifikasi(IKlasifikasiKey klasifikasiKey)
        {
            var klasifikasi = _klasifikasiDal.GetData(klasifikasiKey)
                ?? throw new KeyNotFoundException("Klasifikasi invalid");
            _aggRoot.KlasifikasiId = klasifikasi.KlasifikasiId;
            _aggRoot.KlasidikasiName = klasifikasi.KlasifikasiName;
            return this;
        }

        public ICustomerBuilder HargaType(IHargaTypeKey hargaTypeKey)
        {
            var hargaType = _hargaTypeDal.GetData(hargaTypeKey)
                ?? throw new KeyNotFoundException("HargaType invalid");
            _aggRoot.HargaTypeId = hargaType.HargaTypeId;
            _aggRoot.HargaTypeName = hargaType.HargaTypeName;
            return this;
        }

        public ICustomerBuilder Plafond(decimal plafond)
        {
            if (plafond < 0)
                throw new ArgumentException("PLafond invalid");
            _aggRoot.Plafond = plafond;
            return this;
        }

        public ICustomerBuilder CreditBalance(decimal creditBalance)
        {
            _aggRoot.CreditBalance= creditBalance;
            return this;
        }

        public ICustomerBuilder Wilayah(IWilayahKey wilayahKey)
        {
            var wilayah = _wilayahDal.GetData(wilayahKey)
                ?? throw new KeyNotFoundException("WilayahID invalid");
            _aggRoot.WilayahId = wilayah.WilayahId;
            _aggRoot.WilayahName = wilayah.WilayahName;
            return this;
        }

        public ICustomerBuilder Address(string address1, string address2, string kota)
        {
            _aggRoot.Address1 = address1;
            _aggRoot.Address2 = address2;
            _aggRoot.Kota = kota;
            return this;
        }

        public ICustomerBuilder KodePos(string kodePos)
        {
            _aggRoot.KodePos = kodePos;
            return this;
        }

        public ICustomerBuilder NoTelp(string noTelp)
        {
            _aggRoot.NoTelp = noTelp;
            return this;
        }

        public ICustomerBuilder NoFax(string noFax)
        {
            _aggRoot.NoFax = noFax;
            return this;
        }

        public ICustomerBuilder IsKenaPajak(bool kenaPajak)
        {
            _aggRoot.IsKenaPajak = kenaPajak;
            return this;
        }

        public ICustomerBuilder Npwp(string npwp)
        {
            _aggRoot.Npwp = npwp;
            return this;
        }

        public ICustomerBuilder Nppkp(string nppkp)
        {
            _aggRoot.Nppkp = nppkp; ;
            return this;
        }

        public ICustomerBuilder AddressWp(string address1, string address2)
        {
            _aggRoot.AddressWp = address1;
            _aggRoot.AddressWp2 = address2;
            return this;
        }

        public ICustomerBuilder NamaWp(string namaWp)
        {
            _aggRoot.NamaWp = namaWp;
            return this;
        }

        public ICustomerBuilder Email(string email)
        {
            _aggRoot.Email = email;
            return this;
        }

        public ICustomerBuilder Nitku(string nitku)
        {
            _aggRoot.Nitku = nitku;
            return this;
        }

        public ICustomerBuilder JenisIdentitasPajak(string jenisIdentitasPajak)
        {
            _aggRoot.JenisIdentitasPajak = jenisIdentitasPajak;
            return this;
        }
    }
}