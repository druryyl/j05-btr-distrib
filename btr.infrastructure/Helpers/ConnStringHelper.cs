using Microsoft.Extensions.Options;
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
                _connString = Generate(options.ServerName, options.DbName, options.IsTest);

            return _connString;
        }
        
        public static IOptions<DatabaseOptions> GetTestOptions()
        {
            return Options.Create(new DatabaseOptions
            {
                ServerName = "JUDE7",
                DbName = "devTest",
                IsTest = true
            });
        }

        private static string Generate(string server, string db, bool isTest)
        {
            const string uid = "btrLogin";
            const string pass = "btr123!";

            string resolvedServer;
            string resolvedDatabase;

            if (isTest)
            {
                // for test use provided values directly
                resolvedServer = server ?? string.Empty;
                resolvedDatabase = db ?? string.Empty;
            }
            else
            {
                // try to read saved values from registry; fall back to provided values
                var (regServer, regDb) = ReadFromRegistry();
                resolvedServer = string.IsNullOrEmpty(regServer) ? (server ?? string.Empty) : regServer;
                resolvedDatabase = string.IsNullOrEmpty(regDb) ? (db ?? string.Empty) : regDb;

                // persist resolved values so next run can reuse them
                SaveToRegistry(resolvedServer, resolvedDatabase);
            }

            Server = resolvedServer;
            Database = resolvedDatabase;

            return $"Server={Server};Database={Database};User Id={uid};Password={pass};TrustServerCertificate=True";
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