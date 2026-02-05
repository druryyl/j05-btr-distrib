using j07_btrade_sync.Shared;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class CustomerClearUpdateFlagService
    {
        private readonly RegistryHelper _registryHelper;
        public CustomerClearUpdateFlagService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool, string)> Execute()
        {
            //  BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var bodyReq = new CustomerClearUpdateFlagReq() { serverId = serverTargetId };
            var baseUrl = ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Customer/ClearUpdatedFlag";
            var client = new RestClient(endpoint);
            var req = new RestRequest();
            req.AddHeader("Content-Type", "application/json");
            req.AddJsonBody(bodyReq); 

            //  EXECUTE
            var response = await client.ExecutePatchAsync(req);
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

    public class CustomerClearUpdateFlagReq
    {
        public string serverId { get; set; }
    }
}
