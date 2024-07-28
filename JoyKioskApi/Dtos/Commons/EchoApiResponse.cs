using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Commons
{
    public class EchoApiRequestBody<T>
    {
        public T? Body { get; set; }
    }

    public class EchoApiResponse
    {
        [JsonPropertyName("success")] public bool? Success { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("data")] public object? Data { get; set; }
    }
}
