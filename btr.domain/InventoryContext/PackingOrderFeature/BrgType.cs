namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class BrgType : IBrgKey
    {
        public BrgType(string brgId, string brgCode, string brgName)
        {
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
        }

        public static BrgType Default => new BrgType(
            "-", "-", "-");

        public static IBrgKey Key(string id)
        {
            var result = Default;
            result.BrgId = id;
            return result;
        }

        public string BrgId { get; private set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
    }

    public interface IBrgKey
    {
        string BrgId { get; }
    }
}
