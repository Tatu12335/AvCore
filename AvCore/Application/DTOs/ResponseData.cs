using System.Text.Json.Serialization;

namespace AvCore.Application.DTOs
{
    public class Response
    {
        // query-status ,data , signature and filename.
        [JsonPropertyName("query-status")] public string QueryStatus { get; set; }
        [JsonPropertyName("data")] public List<MalwareData> Data { get; set; }

    }
}
