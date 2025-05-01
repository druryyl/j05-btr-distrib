namespace btr.domain.BrgContext.KategoriAgg
{
    public class KategoriModel : IKategoriKey
    {
        public KategoriModel()
        {
        }
        public KategoriModel(string id) => KategoriId = id;

        public string KategoriId { get; set; }
        public string KategoriName { get; set; }
        public string Code { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}