using btr.domain.SalesContext.FakturAgg;
using System;

namespace btr.domain.InventoryContext.PackingOrderFeature
{
    public class FakturReff : IFakturKey
    {
        public FakturReff(string fakturId, string fakturCode, DateTime fakturDate, string adminName)
        {
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            AdminName = adminName;
        }

        public static FakturReff Default => new FakturReff(
            "-", "-", new DateTime(3000, 1, 1), string.Empty);

        public static IFakturKey Key(string id)
        {
            var result = Default;
            result.FakturId = id;
            return result;
        }

        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string AdminName { get; private set; }
    }
}

