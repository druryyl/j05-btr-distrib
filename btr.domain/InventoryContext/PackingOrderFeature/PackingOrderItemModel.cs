namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class PackingOrderItemModel
    {
        public PackingOrderItemModel(int noUrut, BrgReff brg, QtyType qtyBesar, QtyType qtyKecil)
        {
            NoUrut = noUrut;
            Brg = brg;
            QtyBesar = qtyBesar;
            QtyKecil = qtyKecil;
        }
        public int NoUrut { get; }
        public BrgReff Brg { get; }
        public QtyType QtyBesar { get; }
        public QtyType QtyKecil { get; }
    }
}
