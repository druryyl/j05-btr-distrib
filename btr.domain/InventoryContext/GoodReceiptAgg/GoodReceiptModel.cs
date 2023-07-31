using System;

namespace btr.domain.InventoryContext.GoodReceiptAgg
{
    public class GoodReceiptModel : IGoodReceiptKey
    {
        public string GoodReceiptId { get; set; }
        public DateTime GoodReceiptDate { get; set; }
    }
}