using btr.application.InventoryContext.PackingOrderFeature;
using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using BtrGudang.Infrastructure.PackingOrderFeature;
using System.Linq;

namespace btr.infrastructure.InventoryContext.PackingOrderFeature
{
    public class PackingOrderRepo : IPackingOrderRepo
    {
        private readonly IPackingOrderDal _packingOrderDal;
        private readonly IPackingOrderItemDal _packingOrderItemDal;
        private readonly IPackingOrderDepoDal _packingOrderDepoDal;

        public PackingOrderRepo(IPackingOrderDal packingOrderDal,
            IPackingOrderItemDal packingOrderItemDal,
            IPackingOrderDepoDal packingOrdeDepoDal)
        {
            _packingOrderDal = packingOrderDal;
            _packingOrderItemDal = packingOrderItemDal;
            _packingOrderDepoDal = packingOrdeDepoDal;
        }


        public void SaveChanges(PackingOrderModel model)
        {
            LoadEntity(model)
                .Match(
                    onSome: _ => _packingOrderDal.Update(PackingOrderDto.FromModel(model)),
                    onNone: () => _packingOrderDal.Insert(PackingOrderDto.FromModel(model)));

            _packingOrderItemDal.Delete(model);
            _packingOrderItemDal.Insert(model.ListItem
                .Select(x => PackingOrderItemDto.FromModel(x, model.PackingOrderId))
                .ToList());

            _packingOrderDepoDal.Delete(model);
            _packingOrderDepoDal.Insert(model.ListDepo
                .Select(x => PackingOrderDepoDto.FromModel(x, model.PackingOrderId))
                .ToList());
        }

        public void DeleteEntity(IPackingOrderKey key)
        {
            _packingOrderDal.Delete(key);
            _packingOrderItemDal.Delete(key);
            _packingOrderDepoDal.Delete(key);
        }

        public MayBe<PackingOrderModel> LoadEntity(IPackingOrderKey key)
        {
            var hdr = _packingOrderDal.GetData(key);
            var listDtl = _packingOrderItemDal.ListData(key).SafeToList();
            var listDtlModel = listDtl
                .Select(x => x.ToModel())
                .ToList();

            var listDepo = _packingOrderDepoDal.ListData(key).SafeToList();
            var listDepoModel = listDepo
                .Select(x => x.ToModel())
                .ToList();

            var model = hdr?.ToModel(listDtlModel, listDepoModel);
            return MayBe.From(model);
        }

        public MayBe<PackingOrderModel> LoadEntity(IFakturKey key)
        {
            var hdr = _packingOrderDal.GetData(key);
            var packingOrderKey = PackingOrderModel.Key(hdr?.PackingOrderId ?? string.Empty);
            return LoadEntity(packingOrderKey);
        }

    }
}
