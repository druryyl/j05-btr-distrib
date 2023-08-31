namespace btr.domain.SalesContext.KlasifikasiAgg
{
    public class KlasifikasiModel : IKlasifikasiKey
    {
        public KlasifikasiModel(string id) => KlasifikasiId = id;
        public KlasifikasiModel()
        {
        }

        public string KlasifikasiId { get; set; }
        public string KlasifikasiName { get; set; }
    }
}