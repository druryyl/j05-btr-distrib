using j07_btrade_sync.Model;
using j07_btrade_sync.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class WilayahSyncService
    {
        private readonly RegistryHelper _registryHelper;
        public WilayahSyncService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool Success, string ErrorMessage)> SyncWilayah(IEnumerable<WilayahType> enumWilayah)
        {
            // BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Wilayah";  
            var client = new RestClient(endpoint);

            // Serialize using System.Text.Json
            var listWilayah = enumWilayah.ToList();
            foreach(var item in listWilayah)
                item.ServerId = serverTargetId;

            var requestBody = JsonSerializer.Serialize(new WilayahSyncCommand(listWilayah, serverTargetId));
            var req = new RestRequest()
                .AddJsonBody(requestBody);

            // EXECUTE
            var response = await client.ExecutePostAsync(req);

            return response.StatusCode == HttpStatusCode.OK
                ? (true, string.Empty)
                : (false, response.ErrorMessage ?? response.Content);
        }
    }

    public class WilayahSyncCommand
    {
        public WilayahSyncCommand(IEnumerable<WilayahType> listWilayah, string serverId)
        {
            ListWilayah = new List<WilayahType>(listWilayah);
            ServerId = serverId;
        }

        public List<WilayahType> ListWilayah { get; set; }
        public string ServerId { get; set; }
    }
}
