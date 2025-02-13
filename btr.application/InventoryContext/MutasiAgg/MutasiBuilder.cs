using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiBuilder : INunaBuilder<MutasiModel>
    {
        IMutasiBuilder CreateNew(IUserKey userKey);
        IMutasiBuilder Load(IMutasiKey mutasiKey);
        IMutasiBuilder Attach(MutasiModel mutasi);
        IMutasiBuilder Warehouse(IWarehouseKey warehouse);
        IMutasiBuilder JenisMutasi(JenisMutasiEnum jenisMutasiEnum);

        IMutasiBuilder AddItem(IBrgKey brgKey, string qtyString, string discInputStr);
        IMutasiBuilder ClearItem();


        IMutasiBuilder Void(IUserKey userKey);
        IMutasiBuilder ReActivate(IUserKey userKey);
        IMutasiBuilder User(IUserKey user);
        IMutasiBuilder CalcTotal();
    }

    public class MutasiBuilder : IMutasiBuilder
    {
        private MutasiModel _aggRoot = new MutasiModel();
        private readonly IMutasiDal _mutasiDal;
        private readonly IMutasiItemDal _mutasiItemDal;

        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly ICreateMutasiItemWorker _createMutasiItemWorker;

        public MutasiBuilder(IMutasiDal mutasiDal,
            IMutasiItemDal mutasiItemDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime,
            ICreateMutasiItemWorker createMutasiItemWorker)
        {
            _mutasiDal = mutasiDal;
            _mutasiItemDal = mutasiItemDal;

            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
            _createMutasiItemWorker = createMutasiItemWorker;
        }

        public MutasiModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        #region INITIATOR
        public IMutasiBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new MutasiModel
            {
                MutasiDate = _dateTime.Now,
                CreateTime = _dateTime.Now,
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                VoidDate = new DateTime(3000, 1, 1),
                UserIdVoid = userKey.UserId,
                ListItem = new List<MutasiItemModel>(),
            };
            return this;
        }

        public IMutasiBuilder Load(IMutasiKey mutasiKey)
        {
            _aggRoot = _mutasiDal.GetData(mutasiKey)
                ?? throw new KeyNotFoundException($"Mutasi not found ({mutasiKey.MutasiId})");
            _aggRoot.ListItem = _mutasiItemDal.ListData(mutasiKey)?.ToList()
                ?? new List<MutasiItemModel>();
            return this;
        }

        public IMutasiBuilder Attach(MutasiModel mutasi)
        {
            _aggRoot = mutasi;
            return this;
        }
        #endregion

        #region HEADER
        public IMutasiBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }
        #endregion

        #region GRID
        public IMutasiBuilder AddItem(IBrgKey brgKey, string qtyInputStr, string discInputStr)
        {
            var item = _createMutasiItemWorker.Execute(
                new CreateMutasiItemRequest(brgKey.BrgId, _aggRoot.WarehouseId, qtyInputStr, discInputStr));

            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new MutasiItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            item.NoUrut = noUrutMax + 1;

            _aggRoot.ListItem.Add(item);
            return this;
        }

        #endregion

        #region STATE
        public IMutasiBuilder Void(IUserKey userKey)
        {
            _aggRoot.VoidDate = _dateTime.Now;
            _aggRoot.UserIdVoid = userKey.UserId;
            return this;
        }

        public IMutasiBuilder ReActivate(IUserKey userKey)
        {
            _aggRoot.VoidDate = new DateTime(3000, 1, 1);
            _aggRoot.UserIdVoid = string.Empty;
            return this;
        }

        public IMutasiBuilder User(IUserKey user)
        {
            _aggRoot.UserId = user.UserId;
            return this;
        }

        public IMutasiBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }
        #endregion

        public IMutasiBuilder CalcTotal()
        {
            _aggRoot.NilaiSediaan = _aggRoot.ListItem.Sum(x => x.NilaiSediaan);
            return this;
        }

        public IMutasiBuilder JenisMutasi(JenisMutasiEnum jenisMutasi)
        {
            _aggRoot.JenisMutasi = jenisMutasi;
            return this;
        }
    }
}


