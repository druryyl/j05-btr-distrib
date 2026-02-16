using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class BrgReff : IBrgKey
    {
        public BrgReff(string brgId, string brgCode, string brgName, string kategoriName, string supplierName)
        {
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
            KategoriName = kategoriName;
            SupplierName = supplierName;
        }

        public static BrgReff Default => new BrgReff(
            "-", "-", "-", "-", "-");

        public static IBrgKey Key(string id)
        {
            var result = Default;
            result.BrgId = id;
            return result;
        }

        public string BrgId { get; private set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string KategoriName { get; private set; }
        public string SupplierName { get; private set; }
        }
}
