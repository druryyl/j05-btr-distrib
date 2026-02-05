using j07_btrade_sync.Model;
using j07_btrade_sync.Shared;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class SalesPersonSyncService
    {
        private readonly RegistryHelper _registryHelper;

        public SalesPersonSyncService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool, string)> SyncSalesPerson(IEnumerable<Model.SalesPersonType> enumSalesPerson)
        {
            //  BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/SalesPerson";
            RestClient client = new RestSharp.RestClient(endpoint);

            //  serialize object cmd to json using System.Text.Json
            var listSalesPerson = enumSalesPerson.ToList();
            foreach(var item in listSalesPerson)
                item.ServerId = serverTargetId;

            var requestBody = System.Text.Json.JsonSerializer.Serialize(new SalesPersonSyncCommand(listSalesPerson, serverTargetId));
            var req = new RestSharp.RestRequest()
                .AddJsonBody(requestBody);
            
            //  EXECUTE
            var response = await client.ExecutePostAsync(req);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return (false, response.ErrorMessage);
            }
            else
            {
                return (true, "");
            }
        }
    }

    public class SalesPersonSyncCommand
    {
        public SalesPersonSyncCommand(IEnumerable<SalesPersonType> listSalesPerson, string serverId)
        {
            ListSalesPerson = new List<SalesPersonType>(listSalesPerson);
            ServerId = serverId;
        }
        public List<SalesPersonType> ListSalesPerson { get; set; }
        public string ServerId { get; set; }
    }
}
