using j07_btrade_sync.Model;
using j07_btrade_sync.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace j07_btrade_sync.Service
{
    public class CustomerDownloadUpdatedService
    {
        private readonly RegistryHelper _registryHelper;

        public CustomerDownloadUpdatedService()
        {
            _registryHelper = new RegistryHelper();
        }
        public async Task<(bool, string, IEnumerable<CustomerType>)> Execute()
        {
            //  BUILD
            var serverTargetId = _registryHelper.ReadString("ServerTargetID");
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["btrade-cloud-base-url"];
            var endpoint = $"{baseUrl}/api/Customer/Updated/{serverTargetId}";
            var client = new RestClient(endpoint);
            var req = new RestRequest();
            //  EXECUTE
            var response = await client.ExecuteGetAsync<CustomerType>(req);
            
            //  RESULT
            if (!response.IsSuccessful)
                return (false, response.ErrorMessage ?? response.StatusDescription, null);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<CustomerDto>>>(response.Content, options);

            if (apiResponse == null)
                return (false, "Failed to deserialize API response", null);

            if (apiResponse.Status?.ToLower() != "success")
                return (false, $"API returned non-success status: {apiResponse.Status}", null);
            var listDto = apiResponse.Data ?? new List<CustomerDto>();
            var result = listDto.Select(x => x.ToModel());
            return (true, "", result);
        }
    }

    public class CustomerDto
    {
        public string CustomerId {get;set;} 
        public string CustomerCode {get;set;}
        public string CustomerName {get;set;} 
        public string Alamat {get;set;} 
        public string Wilayah {get;set;}
        public double Latitude {get;set;} 
        public double Longitude {get;set;} 
        public double Accuracy {get;set;}
        public long CoordinateTimeStamp {get;set;} 
        public string CoordinateUser {get;set;} 
        public bool IsUpdated { get; set; }
        public string ServerId { get; set; }

        public CustomerType ToModel()
        {
            var coordinateTimeStampe = EpochConverter.FromMilliseconds(CoordinateTimeStamp);
            var result = new CustomerType(CustomerId, CustomerCode, CustomerName,
                Alamat, Wilayah, Latitude, Longitude, Accuracy,
                coordinateTimeStampe, CoordinateUser, ServerId);
            return result;
        }
        public static CustomerDto Create(CustomerType model)
        {
            var coordinateTimeStamp = EpochConverter.ToMilliseconds(model.CoordinateTimeStamp);
            var result = new CustomerDto{
                CustomerId = model.CustomerId,
                CustomerCode = model.CustomerCode,
                CustomerName = model.CustomerName,
                Alamat = model.Alamat,
                Wilayah =model.Wilayah,
                Latitude =model.Latitude, 
                Longitude = model.Longitude, 
                Accuracy = model.Accuracy,
                CoordinateTimeStamp = coordinateTimeStamp,
                CoordinateUser = model.CoordinateUser,
                ServerId = model.ServerId};
            return result;
        }
    }
    public static class EpochConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts milliseconds since Unix epoch to DateTime
        /// </summary>
        /// <param name="milliseconds">Milliseconds since 1970-01-01</param>
        /// <returns>DateTime in UTC</returns>
        public static DateTime FromMilliseconds(long milliseconds)
        {
            return Epoch.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Converts DateTime to milliseconds since Unix epoch
        /// </summary>
        /// <param name="dateTime">DateTime to convert</param>
        /// <returns>Milliseconds since 1970-01-01</returns>
        public static long ToMilliseconds(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - Epoch).TotalMilliseconds;
        }
    }

}
