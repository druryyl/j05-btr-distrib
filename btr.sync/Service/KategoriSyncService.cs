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
    public class KategoriSyncService
    {
        private readonly RegistryHelper _registryHelper;

        public KategoriSyncService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool Success, string ErrorMessage)> SyncKategori(IEnumerable<KategoriType> enumKategori)
        {
            // BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Kategori";
            var client = new RestClient(endpoint);

            // Serialize using System.Text.Json
            var listKategori = enumKategori.ToList();
            foreach (var item in listKategori)
                item.ServerId = serverTargetId;

            var requestBody = JsonSerializer.Serialize(new KategoriSyncCommand(listKategori, serverTargetId));
            var req = new RestRequest()
                .AddJsonBody(requestBody);

            // EXECUTE
            var response = await client.ExecutePostAsync(req);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return (false, response.ErrorMessage ?? response.Content);
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }

    public class KategoriSyncCommand
    {
        public KategoriSyncCommand(IEnumerable<KategoriType> listKategori, string serverId)
        {
            ListKategori = new List<KategoriType>(listKategori);
            ServerId = serverId;
        }

        public List<KategoriType> ListKategori { get; set; }
        public string ServerId { get; set; }
    }
}
