using btr.domain.BrgContext.BrgAgg;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.MutasiAgg
{
    public class MutasiItemModel : IMutasiKey, IBrgKey
    {
        public string MutasiId { get; set; }
        public string MutasiItemId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        public string QtyInputStr { get; set; }
        
        public int QtyBesar { get; set; }
        public string  SatBesar { get; set; }
        public int Conversion { get; set; }
        public decimal HppBesar { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HppKecil { get; set; }
        
        public int StokBesar { get; set; }
        public int StokKecil { get; set; }
        
        public int Qty { get; set; }
        public string Sat { get; set; }
        public decimal Hpp { get; set; }
        
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; set; }
        public decimal DiscRp { get; set; }

        public string QtyDetilStr { get; set; }
        public string StokDetilStr { get; set; }
        public string HppDetilStr { get; set; }
        
        public decimal NilaiSediaan { get; set; } 
        public List<MutasiDiscModel> ListDisc { get; set; }
    }
}