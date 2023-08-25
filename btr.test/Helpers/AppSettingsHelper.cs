using System;
using System.IO;
using btr.infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace btr.test.Helpers
{
    public static class AppSettingsHelper
    {
        public static DatabaseOptions GetDatabaseOptions()
        {
            var json = LoadJson();
            var appSettings = JObject.Parse(json);
            var database = appSettings.GetValue("Database");
            var result = database?.ToObject<DatabaseOptions>();
            return result ?? new DatabaseOptions();
        }

        private static string LoadJson()
        {
            string result;
            try
            {
                result = LoadByMachine();
            }
            catch (Exception)
            {
                result = LoadDefault();
            }
            return result;
        }

        private static string LoadByMachine()
        {
            var filename = $"appsettings.{Environment.MachineName}.json";
            using (var r = new StreamReader(filename))
            {
                var result = r.ReadToEnd();
                return result;
            }
        }
        private static string LoadDefault()
        {
            var filename = $"appsettings.json";
            using (var r = new StreamReader(filename))
            {
                var result = r.ReadToEnd();
                return result;
            }
        }
    }}