using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.KartuStokRpt
{
    public interface IKartuStokSummaryDal :
        IListData<KartuStokSummaryDto, Periode>
    {
    }
    public class KartuStokSummaryDto
    {
        public string BrgId { get; set; }
        public string Warehouse { get; set; }
        public string Supplier { get; set; }
        public string Kategori { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public decimal Invoice { get; set; }
        public decimal Faktur { get; set; }
        public decimal Retur { get; set; }
        public decimal Mutasi { get; set; }
        public decimal Opname { get; set; }
        public decimal QtyInPcs { get; set; }
        //public decimal QtyBesar { get; set; }
        //public string SatBesar { get; set; }
        //public decimal QtyKecil { get; set; }
        //public decimal Conversion { get; set; }
        //public string Satuan { get; set; }

    }
}
