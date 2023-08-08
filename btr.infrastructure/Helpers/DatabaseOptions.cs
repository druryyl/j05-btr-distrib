namespace btr.infrastructure.Helpers
{
    public class DatabaseOptions
    {
        public const string SECTION_NAME = "Database";

        public DatabaseOptions()
        {
            ServerName = string.Empty;
            DbName = string.Empty;
        }

        public string ServerName { get; set; }
        public string DbName { get; set; }
    }
}