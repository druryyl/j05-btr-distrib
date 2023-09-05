using System;

namespace btr.domain.InventoryContext.OpnameAgg
{
    public class OpnameModel : IOpnameKey
    {
        public string OpnameId { get; set; }
        public DateTime OpnameDate { get; set; }
        public string UserId { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        
        public int Qty1Awal {get;set;}
        public int Qty1Opname {get;set;}
        public int Qty1Adjust {get;set;}
        public string Satuan1 {get;set;}
        public int Conversion1 { get; set; }
        public int Qty2Awal {get;set;}
        public int Qty2Opname {get;set;}
        public int Qty2Adjust {get;set;}
        public string Satuan2 {get;set;}
        public int Conversion2 { get; set; }
        
        public decimal Nilai {get;set;}
    }
}
