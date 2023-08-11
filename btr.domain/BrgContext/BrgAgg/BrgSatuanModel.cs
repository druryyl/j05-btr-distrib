namespace btr.domain.BrgContext.BrgAgg
{
    public class BrgSatuanModel : IBrgKey
    {
        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
    }
}