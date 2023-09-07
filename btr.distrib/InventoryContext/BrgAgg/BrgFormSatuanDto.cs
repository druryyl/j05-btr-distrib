namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormSatuanDto
    {
        public BrgFormSatuanDto()
        {
        }

        public BrgFormSatuanDto(string id, int conversion, string satuanPrint) => (Satuan, Conversion, SatuanPrint) = (id, conversion, satuanPrint);
        public string Satuan { get; set; }
        public int Conversion { get; set; }
        public string SatuanPrint { get; set; }
    }
}
