namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class QtyType
    {
        public QtyType(decimal qty, string satuan)
        {
            Qty = qty;
            Satuan = satuan;
        }

        public static QtyType Default => new QtyType(
            0, "-");

        public decimal Qty { get; private set; }
        public string Satuan { get; private set; }
    }
}
