using j07_btrade_sync.Model;
using j07_btrade_sync.Shared;
using Nuna.Lib.ValidationHelper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class OrderIncrementalDownloadService
    {
        private readonly RegistryHelper _registryHelper;

        public OrderIncrementalDownloadService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool, string, List<OrderModel>)> Execute(Periode periode)
        {
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Order/incremental/{{tgl1}}/{{tgl2}}/{{serverId}}";
            var client = new RestClient(endpoint);

            var request = new RestRequest()
                .AddUrlSegment("tgl1", periode.Tgl1.ToString("yyyy-MM-dd"))
                .AddUrlSegment("tgl2", periode.Tgl2.ToString("yyyy-MM-dd"))
                .AddUrlSegment("serverId", serverTargetId);
            var response = await client.ExecuteGetAsync(request);

            if (!response.IsSuccessful)
            {
                return (false, response.ErrorMessage ?? response.StatusDescription, null);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderModel>>>(response.Content, options);

            if (apiResponse == null)
            {
                return (false, "Failed to deserialize API response", null);
            }

            if (apiResponse.Status?.ToLower() != "success")
            {
                return (false, $"API returned non-success status: {apiResponse.Status}", null);
            }

            return (true, "", apiResponse.Data ?? new List<OrderModel>());
        }
    }
}
public class ApiResponse<T>
{
    public string Status { get; set; }
    public string Code { get; set; }
    public T Data { get; set; }
}