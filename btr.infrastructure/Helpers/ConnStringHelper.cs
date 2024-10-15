using Microsoft.Win32;

namespace btr.infrastructure.Helpers
{
    public static class ConnStringHelper
    {
        private static string _connString = string.Empty;
        public static string Server { get; private set; }
        public static string Database { get; private set; }

        public static string Get(DatabaseOptions options)
        {
            if (_connString.Length == 0)
                _connString = Generate(options.ServerName, options.DbName);

            return _connString;
        }

        private static string Generate(string server, string db)
        {
            const string uid = "btrLogin";
            const string pass = "btr123!";

            //  read from registry first
            (Server, Database) = ReadFromRegistry();

            if (Server == string.Empty)
                Server = server;
            if (Database == string.Empty)
                Database = db;

            SaveToRegistry(Server, Database);

            var result = $"Server={Server};Database={Database};User Id={uid};Password={pass};TrustServerCertificate=True";
            return result;
        }

        private static void SaveToRegistry(string server, string db)
        {
            // Open or create a subkey in HKEY_CURRENT_USER
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"DrurySoftware\BTRApp");

            // Save a value
            key.SetValue("Server", server);
            key.SetValue("Database", db);

            // Close the key to release the resource
            key.Close();
        }

        public static (string, string) ReadFromRegistry()
        {
            // Open the subkey in read mode
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"DrurySoftware\BTRApp");

            if (key != null)
            {
                // Read the value; if it doesn't exist, return null
                string server = key.GetValue("Server") as string;
                string database= key.GetValue("Database") as string;

                // Close the key to release the resource
                key.Close();

                return (server??string.Empty, database??string.Empty);
            }

            return (string.Empty, string.Empty);
        }
    }
}