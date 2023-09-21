namespace btr.distrib.InventoryContext.OpnameAgg
{
    public class BrgOpnameFormDto
    {
        public BrgOpnameFormDto(string id, string code, string name)
        {
            BrgId = id;
            Code = code;
            Name = name;
        }
        public string BrgId { get; }
        public string Code { get; }
        public string Name { get; }
    }
}
