using btr.domain.InventoryContext.WarehouseAgg;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class PackingOrderItemModel
    {
        public PackingOrderItemModel(int noUrut, BrgReff brg, QtyType qtyBesar, QtyType qtyKecil,
            string depoId, string depoName)
        {
            NoUrut = noUrut;
            Brg = brg;
            QtyBesar = qtyBesar;
            QtyKecil = qtyKecil;
            DepoId = depoId;
            DepoName = depoName;
        }
        public int NoUrut { get; }
        public BrgReff Brg { get; }
        public QtyType QtyBesar { get; }
        public QtyType QtyKecil { get; }
        public string DepoId { get; }
        public string DepoName { get; }
    }
}
