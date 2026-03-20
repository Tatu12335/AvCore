using AvCore.Application.DTOs;
using AvCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AvCore.Infrastructure.Repositories
{
    public class AbuseApiClient : IAbuseClient
    {
        
        private readonly HttpClient _httpClient;
        public AbuseApiClient( HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task <Response>GetAbuseChClient(string hashvalue)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("query","get_info"),
                new KeyValuePair<string, string>("hash",hashvalue)
            });
            try
            {
                var response = await _httpClient.PostAsync("https://mb-api.abuse.ch/api/v1/",content);

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
