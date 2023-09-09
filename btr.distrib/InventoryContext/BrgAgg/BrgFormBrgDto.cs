namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormBrgDto
    {
        public BrgFormBrgDto(string id, string code, string name, string cat) 
            => (Id, Code, BrgName, Kategori) = (id, code, name, cat);
        
        public string Id { get; private set; }
        public string Code { get; private set; }
        public string BrgName { get; private set; }
        public string Kategori { get; private set; }
    }
}