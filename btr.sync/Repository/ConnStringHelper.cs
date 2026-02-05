using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public static class ConnStringHelper
    {
        private static string server = string.Empty;
        private static string database = string.Empty;
        public static string Get()
        {
            const string uid = "btrLogin";
            const string pass = "btr123!";

            //  read from registry first
            (server, database) = ReadFromRegistry();


            SaveToRegistry(server, database);

            var result = $"Server={server};Database={database};User Id={uid};Password={pass};TrustServerCertificate=True";
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
                string database = key.GetValue("Database") as string;

                // Close the key to release the resource
                key.Close();

                return (server ?? string.Empty, database ?? string.Empty);
            }

            return (string.Empty, string.Empty);
        }

    }
}
