namespace btr.distrib.InventoryContext.PackingAgg
{
    internal class Packing2FakturBrgDto
    {
        public Packing2FakturBrgDto(string id, string name, string code,
            int qtyBesar, string satBesar, int qtyKecil, string satKecil,
            decimal hargaJual)
        {
            BrgId = id;
            BrgName = name;
            BrgCode = code;
            QtyBesar = qtyBesar;
            SatBesar = satBesar;
            QtyKecil = qtyKecil;
            SatKecil = satKecil;
            HargaJual = hargaJual;
        }

        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HargaJual { get; private set; }
    }
}