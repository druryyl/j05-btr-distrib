namespace btr.distrib.SalesContext.SalesPersonAgg
{
    public class  CustomerViewDto
    {
        public CustomerViewDto(string id, string code, string name, 
            string address, string wilayah)
        {
            CustomerId = id;
            CustomerCode = code;
            CustomerName = name;
            Address = address;
            Wilayah = wilayah;
            Hari = string.Empty;
        }
        public string CustomerId { get; }
        public string Wilayah { get; }
        public string CustomerCode { get; }
        public string CustomerName { get; }
        public string Address { get; }
        public string Hari { get; private set; }
        public void SetHari(string hari)
        {
            Hari = hari;
        } 
    }

}
