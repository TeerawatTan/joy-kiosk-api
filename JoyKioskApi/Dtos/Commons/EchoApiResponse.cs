using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Commons
{
    public class EchoApiRequestBody<T>
    {
        public T? Body { get; set; }
    }

    public class EchoCrmApiResponse<T>
    {
        [JsonPropertyName("success")] public bool? Success { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("data")] public T? Data { get; set; }
    }
    public class EchoApiResponse
    {
        [JsonPropertyName("success")] public bool? Success { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("data")] public object? Data { get; set; }
    }

    public class EchoApiValidateUserResponseot
    {
        [JsonPropertyName("data")] public EchoApiValidateUserDetailResponse? Data { get; set; }

        [JsonPropertyName("auth")] public bool? Auth { get; set; }
    }

    public class EchoApiValidateUserDetailResponse
    {
        [JsonPropertyName("id")] public int? Id { get; set; }

        [JsonPropertyName("firstname")] public string? Firstname { get; set; }

        [JsonPropertyName("lastname")] public string? Lastname { get; set; }

        [JsonPropertyName("mobile_no")] public string? MobileNo { get; set; }

        [JsonPropertyName("username")] public string? Username { get; set; }

        [JsonPropertyName("role_id")] public int? RoleId { get; set; }

        [JsonPropertyName("description")] public string? Description { get; set; }

        [JsonPropertyName("is_active")] public bool? IsActive { get; set; }

        [JsonPropertyName("is_delete")] public int? IsDelete { get; set; }
    }

    
}
