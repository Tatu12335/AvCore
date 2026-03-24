using AvCore.Application.DTOs;
using AvCore.Application.Interfaces;
using System.Text.Json;

namespace AvCore.Infrastructure.Services
{
    public class AbuseApiClient : IAbuseClient
    {

        private readonly HttpClient _httpClient;
        public AbuseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public string ApiKey { get; set; }
        public async Task<Response> GetAbuseChClient(string hashvalue)
        {
            ApiKey = Environment.GetEnvironmentVariable("Malware_Bazaar_Key");

            if (ApiKey == null)
            {
                throw new Exception("abuse.ch key is null, have you configured it");
            }

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Auth-Key", ApiKey);


            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("query","get_info"),
                new KeyValuePair<string, string>("hash",hashvalue)
            });
            try
            {
                var response = await _httpClient.PostAsync("https://mb-api.abuse.ch/api/v1/", content);

                string responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Response>(responseContent);

                if (result == null) return null;

                return result;

            }
            catch (HttpRequestException hrex)
            {
                throw new Exception(hrex.Message);
            }
        }
    }
}
