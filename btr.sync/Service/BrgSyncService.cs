using j07_btrade_sync.Shared;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace j07_btrade_sync
{
    public class BrgSyncService
    {
        private readonly RegistryHelper _registryHelper;

        public BrgSyncService()
        {
            _registryHelper = new RegistryHelper();
        }
        //  SYNC BRG
        public async Task<(bool, string)> SyncBrg(IEnumerable<BrgType> enumBrg)
        {
            //  BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Brg";
            var client = new RestClient(endpoint);

            //  serialize object cmd to json using System.Text.Json
            var listBrg = enumBrg.ToList();
            foreach(var item in listBrg)
                item.ServerId = serverTargetId;

            var requestBody = JsonSerializer.Serialize(new BrgSyncCommand(listBrg, serverTargetId));

            var req = new RestRequest()
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

    public class BrgSyncCommand
    {
        public BrgSyncCommand(IEnumerable<BrgType> listBrg, string serverId)
        {
            ListBrg = new List<BrgType>(listBrg);
            ServerId = serverId;
        }
        public List<BrgType> ListBrg { get; set; }
        public string ServerId { get; set; }
    }
}
