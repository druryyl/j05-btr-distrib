namespace btr.domain.BrgContext.BrgAgg
{
    public class BrgSatuanModel : IBrgKey
    {
        public BrgSatuanModel(string brgId, string satuan, int conversion, string satuanPrint) =>
            (BrgId, Satuan, Conversion, SatuanPrint) = (brgId, satuan, conversion, satuanPrint);
        public BrgSatuanModel()
        { 
        }

        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
        public string SatuanPrint { get; set; }
    }
}