namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormBrgDto
    {
        public BrgFormBrgDto(string id, string name, string cat) 
            => (Id, BrgName, Kategori) = (id, name, cat);
        
        public string Id { get; private set; }
        public string BrgName { get; private set; }
        public string Kategori { get; private set; }
    }
}