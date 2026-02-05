using j07_btrade_sync.Model;
using j07_btrade_sync.Shared;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class CustomerUploadService
    {
        private readonly RegistryHelper _registryHelper;

        public CustomerUploadService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool, string)> UploadCustomer(IEnumerable<CustomerType> enumCustomer)
        {
            //  BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["btrade-cloud-base-url"];
                var endpoint = $"{baseUrl}/api/Customer";
            var client = new RestClient(endpoint);
            //  serialize object cmd to json using System.Text.Json
            var listCustomer = enumCustomer.ToList();
            foreach (var item in listCustomer)
                item.ServerId = serverTargetId;

            var requestBody = System.Text.Json.JsonSerializer.Serialize(new CustomerUploadCommand(listCustomer, serverTargetId));
            var req = new RestRequest()
                .AddJsonBody(requestBody)
                .AddHeader("Content-Type", "application/json"); 
            //  EXECUTE
            var response = await client.ExecutePostAsync(req);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return (false, response.ErrorMessage);
            }
            else
            {
                return (true, "");
            }
        }
    }

    public class CustomerUploadCommand
    {
        public CustomerUploadCommand(IEnumerable<CustomerType> listCustomer, string serverId)
        {
            ListCustomer = listCustomer.Select(x => CustomerDto.Create(x))?.ToList()
                ?? new List<CustomerDto>();
            ServerId = serverId;
        } 

        public List<CustomerDto> ListCustomer { get; set; }
        public string ServerId { get; set; }
    }
}
