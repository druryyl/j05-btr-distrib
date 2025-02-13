namespace btr.distrib.InventoryContext.MutasiAgg
{
    public class MutasiItemDto
    {
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; private set; }

        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public decimal HppBesar { get; private set; }
        public int Conversion { get; private set; }

        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HppKecil { get; private set; }

        public int Qty { get; private set; }
        public string Sat { get; private set; }

        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; private set; }

        public decimal Hpp { get; private set; }

        public string StokDetilStr { get; private  set; }
        public string HppDetilStr { get; private set; }
        
        public decimal NilaiSediaan { get; set; } 

    }
}
