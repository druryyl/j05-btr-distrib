namespace btr.distrib.SalesContext.AlokasiFpAgg
{
    public class AlokasiFpAlokasiDto
    {
        public AlokasiFpAlokasiDto(string alokasiId, string noSeriFp, int sisa)
        {
            AlokasiId = alokasiId;
            NoSeriFp = noSeriFp;
            Sisa = sisa;
        }
        public string AlokasiId { get; private set; }
        public string NoSeriFp { get; private set; }
        public int Sisa { get; private set; }
    }
}
