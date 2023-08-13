namespace btr.domain.BrgContext.BrgAgg
{
    public class BrgSatuanModel : IBrgKey
    {
        public BrgSatuanModel(string brgId, string satuan, int conversion) =>
            (BrgId, Satuan, Conversion) = (brgId, satuan, conversion);

        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
    }
}