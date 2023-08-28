namespace btr.domain.PurchaseContext.SupplierAgg
{
    public class SupplierModel : ISupplierKey
    {
        public SupplierModel()
        {
        }
        public SupplierModel(string id) => SupplierId = id;
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Kota { get; set; }
        public string KodePos { get; set; }
        
        public string NoTelp { get; set; }
        public string NoFax { get; set; }
        public string ContactPerson { get; set; }
        
        public string Npwp { get; set; }
        public string NoPkp { get; set; }
    }
}