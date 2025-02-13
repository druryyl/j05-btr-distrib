using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.InventoryContext.MutasiAgg
{
    public class MutasiDiscModel : IMutasiKey, IBrgKey
    {
        public MutasiDiscModel()
        {
        }

        public MutasiDiscModel(int no, string brgId, decimal discProsen, decimal discRp)
        {
            NoUrut = no;
            BrgId = brgId;
            DiscProsen = discProsen;
            DiscRp = discRp;
        }

        public string MutasiId { get; set; }
        public string MutasiItemId { get; set; }
        public string MutasiDiscId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public decimal DiscProsen { get; set; }
        public decimal DiscRp { get; set; }
    }
}