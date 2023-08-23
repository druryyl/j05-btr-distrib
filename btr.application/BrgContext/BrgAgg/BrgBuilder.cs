using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.JenisBrgAgg;
using btr.application.BrgContext.KategoriAgg.Contracts;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.BrgContext.JenisBrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgBuilder : INunaBuilder<BrgModel>
    {
        IBrgBuilder Create();
        IBrgBuilder Load(IBrgKey brgKey);
        IBrgBuilder Attach(BrgModel brg);
        IBrgBuilder Activate();
        IBrgBuilder Deactivate();
        IBrgBuilder BrgId(string id);
        IBrgBuilder Name(string name);
        IBrgBuilder Supplier(ISupplierKey supplierKey);
        IBrgBuilder Kategori(IKategoriKey kategoriKey);
        IBrgBuilder JenisBrg(IJenisBrgKey jenisBrgKey);
        
        IBrgBuilder Hpp(decimal hpp);
        IBrgBuilder AddSatuan(string satuan, int conversion);
        IBrgBuilder RemoveSatuan(string satuan);
        IBrgBuilder AddHarga(IHargaTypeKey hargaTypeKey, decimal harga);

    }

    public class BrgBuilder : IBrgBuilder
    {
        private BrgModel _aggRoot = new BrgModel();
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanDal;
        private readonly IBrgHargaDal _brgHargaDal;
        private readonly ISupplierDal _supplierDal;
        private readonly IKategoriDal _kategoriDal;
        private readonly IJenisBrgDal _jenisBrgDal;
        private readonly DateTimeProvider _datetTime;

        public BrgBuilder(IBrgDal brgDal,
            IBrgSatuanDal brgSatuanDal,
            IBrgHargaDal brgHargaDal,
            ISupplierDal supplierDal,
            IKategoriDal kategoriDal,
            DateTimeProvider datetTime, IJenisBrgDal jenisBrgDal)
        {
            _brgDal = brgDal;
            _brgSatuanDal = brgSatuanDal;
            _brgHargaDal = brgHargaDal;
            _supplierDal = supplierDal;
            _kategoriDal = kategoriDal;
            _datetTime = datetTime;
            _jenisBrgDal = jenisBrgDal;
        }

        public BrgModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IBrgBuilder Create()
        {
            _aggRoot = new BrgModel
            {
                ListSatuan = new List<BrgSatuanModel>(),
                ListHarga = new List<BrgHargaModel>()
            };
            return this;
        }

        public IBrgBuilder Load(IBrgKey brgKey)
        {
            _aggRoot = _brgDal.GetData(brgKey)
                       ?? throw new KeyNotFoundException($"BrgId not found ({brgKey.BrgId})");
            _aggRoot.ListSatuan = _brgSatuanDal.ListData(brgKey)?.ToList()
                        ?? new List<BrgSatuanModel>();
            _aggRoot.ListHarga = _brgHargaDal.ListData(brgKey)?.ToList()
                        ?? new List<BrgHargaModel>();

            return this;
        }

        public IBrgBuilder Attach(BrgModel brg)
        {
            _aggRoot = brg;
            return this;
        }

        public IBrgBuilder Activate()
        {
            _aggRoot.IsAktif = true;
            return this;
        }

        public IBrgBuilder Deactivate()
        {
            _aggRoot.IsAktif = false;
            return this;
        }

        public IBrgBuilder BrgId(string id)
        {
            _aggRoot.BrgId = id;
            return this;
        }

        public IBrgBuilder Name(string name)
        {
            _aggRoot.BrgName = name;
            return this;
        }

        public IBrgBuilder Supplier(ISupplierKey supplierKey)
        {
            var supplier = _supplierDal.GetData(supplierKey)
                           ?? throw new KeyNotFoundException($"SupplierID not found ({supplierKey.SupplierId})");
            _aggRoot.SupplierId = supplier.SupplierId;
            _aggRoot.SupplierName = supplier.SupplierName;
            return this;
        }

        public IBrgBuilder Kategori(IKategoriKey kategoriKey)
        {
            var katogeri = _kategoriDal.GetData(kategoriKey)
                           ?? throw new KeyNotFoundException($"SupplierID not found ({kategoriKey.KategoriId})");
            _aggRoot.SupplierId = katogeri.KategoriId;
            _aggRoot.SupplierName = katogeri.KategoriName;
            return this;
        }

        public IBrgBuilder JenisBrg(IJenisBrgKey jenisBrgKey)
        {
            var jenisBrg = _jenisBrgDal.GetData(jenisBrgKey)
                           ?? throw new KeyNotFoundException("JenisBrg invalid");
            _aggRoot.JenisBrgId = jenisBrg.JenisBrgId;
            _aggRoot.JenisBrgName = jenisBrg.JenisBrgName;
            return this;
        }

        public IBrgBuilder Hpp(decimal hpp)
        {
            if (hpp <= 0)
                throw new ArgumentException("Hpp can not less than 0");
            _aggRoot.Hpp = hpp;
            _aggRoot.HppTimestamp = _datetTime.Now;
            return this;
        }

        public IBrgBuilder AddSatuan(string satuan, int conversion)
        {
            if (_aggRoot.ListSatuan.Any(x => x.Satuan == satuan))
                throw new ArgumentException($"Satuan already exist ({satuan})");
            _aggRoot.ListSatuan.Add(new BrgSatuanModel(_aggRoot.BrgId, satuan, conversion));
            return this;
        }

        public IBrgBuilder RemoveSatuan(string satuan)
        {
            _aggRoot.ListSatuan.RemoveAll(x => x.Satuan == satuan);
            return this;
        }

        public IBrgBuilder AddHarga(IHargaTypeKey hargaTypeKey, decimal harga)
        {
            var newItem = new BrgHargaModel
            {
                HargaTypeId = hargaTypeKey.HargaTypeId,
                Harga = harga,
                HargaTimestamp = _datetTime.Now
            };
            _aggRoot.ListHarga.Add(newItem);
            return this;
        }
    }
}