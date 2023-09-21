using btr.domain.InventoryContext.OpnameAgg;
using System;

namespace btr.distrib.InventoryContext.OpnameAgg
{
    public class ReportOpnameFormDto
    {
        public ReportOpnameFormDto(OpnameModel opname)
        {
            if (opname is null)
                return;
            TglJam = opname.OpnameDate;
            BrgId = opname.BrgId;
            Code = opname.BrgCode;
            BrgName = opname.BrgName;
            Qty_Awal = $"{opname.Qty2Awal:N0} {opname.Satuan2}";
            Qty_Awal_ = $"{opname.Qty1Awal:N0} {opname.Satuan1}";
            Qty_Op = $"{opname.Qty2Opname:N0} {opname.Satuan2}";
            Qty_Op_ = $"{opname.Qty1Opname:N0} {opname.Satuan1}";

        }
        public DateTime TglJam { get; }
        public string BrgId { get; }
        public string Code { get; }
        public string BrgName { get; }
        public string A {get;set;}
        public string Qty_Awal { get; }
        public string Qty_Awal_ { get; }
        public string B { get; set; }
        public string Qty_Op { get; }
        public string Qty_Op_ { get; }
    }
}
