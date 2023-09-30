using System;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.MutasiAgg
{
    public class MutasiModel
    {
        public string MutasiId { get; set; }
        public JenisMutasiEnum JenisMutasi { get; set; }
        
        public string Keterangan { get; set; }
        public DateTime MutasiDate { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        
        public string UserId { get; set; }
        public List<MutasiItemModel> ListItem { get; set; }
    }

    public class MutasiItemModel
    {
        public string MutasiId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        public string QtyInputStr { get; set; }
        
        public int QtyBesar { get; set; }
        public string  SatBesar { get; set; }
        public decimal HppBesar { get; set; }
        public int Conversion { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HppKecil { get; set; }
        
        public int StokBesar { get; set; }
        public int StokKecil { get; set; }
        
        public int Qty { get; set; }
        public string Sat { get; set; }
        public decimal Hpp { get; set; }
        
        public string QtyDetilStr { get; set; }
        public string StokDetilStr { get; set; }
        public string HppDetilStr { get; set; }
        
        public decimal NilaiSediaan { get; set; } 
    }
    
    public enum JenisMutasiEnum
    {
        MutasiKeluar, MutasiMasuk
    }
}